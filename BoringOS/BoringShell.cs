using System;
using System.Collections.Generic;
using System.Linq;
using BoringOS.Programs;
using BoringOS.Terminal;

namespace BoringOS;

public class BoringShell
{
    private readonly List<string> _history = new();
    private const string Prompt = "$ ";
    
    private readonly ITerminal _terminal;
    private readonly BoringSession _session;
    private readonly List<Program> _programs;

    public BoringShell(ITerminal terminal, BoringSession session, IEnumerable<Program> programs)
    {
        this._terminal = terminal;
        this._session = session;

        this._programs = programs.ToList();
    }

    private void WritePrompt(int skip = 0)
    {
        this._terminal.ClearLine(Prompt.Length + skip);
        this._terminal.WriteString(Prompt);
    }
    
    private string ReadLine()
    {
        string line = "";
        int historyIndex = -1;
        int lineIndex = 0;

        while (true)
        {
            ConsoleKeyInfo key = this._terminal.ReadKey();
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
                line = line.Remove(lineIndex - 1, 1);
                lineIndex--;
                
                WritePrompt(line.Length);
                this._terminal.WriteString(line);
            }
            else
            {
                line = line.Insert(lineIndex, key.KeyChar.ToString());

                this._terminal.CursorX = Prompt.Length + lineIndex;
                this._terminal.WriteString(line.Substring(lineIndex));
                
                lineIndex++;
            }
            
            this._terminal.CursorX = lineIndex + Prompt.Length;
        }

        return line;
    }
    
    private string ShowHistory(int historyIndex)
    {
        historyIndex = Math.Clamp(historyIndex, 0, this._history.Count - 1);
        string historyLine = this._history[historyIndex];
                
        WritePrompt(historyLine.Length);
        this._terminal.WriteString(historyLine);
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
            this._terminal.WriteChar('\n');
            return;
        }
        
        this._terminal.WriteChar('\n');
        
        ProcessLine(line.Split(' '));
    }

    private void ProcessLine(string[] args)
    {
        if (args.Length == 0) return;
        string programName = args[0];

        Program? program = this._programs.FirstOrDefault(p => p.Name == programName);
        if (program == null)
        {
            this._session.Terminal.WriteString($"{programName}: Program not found\n");
            return;
        }

        program.Invoke(args.Skip(1).ToArray(), this._session);
    }
}