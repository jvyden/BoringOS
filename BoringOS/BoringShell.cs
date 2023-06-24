using BoringOS.Programs;

namespace BoringOS;

public class BoringShell
{
    private readonly List<string> _history = new();
    private const string Prompt = "$ ";
    
    private readonly BoringSession _session;

    public BoringShell(BoringSession session)
    {
        this._session = session;
    }

    private void WritePrompt(int skip = 0)
    {
        this._session.Terminal.ClearLine(Prompt.Length + skip);
        this._session.Terminal.WriteString(Prompt);
    }
    
    private string ReadLine()
    {
        string line = "";
        int historyIndex = -1;
        int lineIndex = 0;

        while (true)
        {
            ConsoleKeyInfo key = this._session.Terminal.ReadKey();
            if (key.Key == ConsoleKey.Enter) break;

            if (key.Key == ConsoleKey.UpArrow)
            {
                historyIndex++;
                line = ShowHistory(historyIndex);
                lineIndex = line.Length;
                
                continue;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                historyIndex--;
                line = ShowHistory(historyIndex);
                lineIndex = line.Length;
                
                continue;
            }
            
            // Text editing
            if (key.Key == ConsoleKey.LeftArrow)
                lineIndex = Math.Clamp(lineIndex - 1, 0, line.Length);
            else if (key.Key == ConsoleKey.RightArrow)
                lineIndex = Math.Clamp(lineIndex + 1, 0, line.Length);
            else if (key.Key == ConsoleKey.Backspace)
            {
                if(lineIndex == 0) continue;
                line = line.Remove(lineIndex - 1, 1);
                lineIndex--;
                
                WritePrompt(line.Length);
                this._session.Terminal.WriteString(line);
            }
            else
            {
                line = line.Insert(lineIndex, key.KeyChar.ToString());

                this._session.Terminal.CursorX = Prompt.Length + lineIndex;
                this._session.Terminal.WriteString(line.Substring(lineIndex));
                
                lineIndex++;
            }
            
            this._session.Terminal.CursorX = lineIndex + Prompt.Length;
        }

        return line;
    }
    
    private string ShowHistory(int historyIndex)
    {
        historyIndex = Math.Clamp(historyIndex, 0, this._history.Count - 1);
        string historyLine = this._history[historyIndex];
                
        WritePrompt(historyLine.Length);
        this._session.Terminal.WriteString(historyLine);
        return historyLine;
    }
    
    public void InputCycle()
    {
        WritePrompt();
        string? line = ReadLine();
        if (line == null) throw new Exception("Received a null line from console");

        // If the line has content, push it into the front of the shell's history
        if (!string.IsNullOrWhiteSpace(line))
        {
            this._history.Insert(0, line);
        }
        else
        {
            this._session.Terminal.WriteChar('\n');
            return;
        }
        
        this._session.Terminal.WriteChar('\n');
        
        ProcessLine(line.Split(' '));
    }

    private void ProcessLine(IReadOnlyList<string> args)
    {
        if (args.Count == 0) return;
        string programName = args[0];

        Program? program = null;
        foreach (Program? p in this._session.Programs)
        {
            if (p.Name != programName) continue;
            
            program = p;
            break;
        }

        if (program == null)
        {
            this._session.Terminal.WriteString($"{programName}: Program not found\n");
            return;
        }

        string[] argsNoBase = new string [args.Count - 1];
        for (int i = 1; i < args.Count; i++)
        {
            argsNoBase[i - 1] = args[i];
        }

        program.Invoke(argsNoBase, this._session);
    }
}