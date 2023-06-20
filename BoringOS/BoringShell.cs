using System;
using System.Collections.Generic;

namespace BoringOS;

public class BoringShell
{
    private readonly List<string> _history = new();
    private const string Prompt = "$ ";

    private string ReadLine()
    {
        string line = "";
        int historyIndex = -1;
        int lineIndex = 0;

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
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
                Console.Write(line);
            }
            else
            {
                line = line.Insert(lineIndex, key.KeyChar.ToString());
                lineIndex++;
                
                WritePrompt(line.Length);
                Console.Write(line);
            }
            
            Console.SetCursorPosition(lineIndex + Prompt.Length, Console.CursorTop);
        }

        return line;
    }

    private string ShowHistory(int historyIndex)
    {
        historyIndex = Math.Clamp(historyIndex, 0, _history.Count - 1);
        string historyLine = _history[historyIndex];
                
        WritePrompt(historyLine.Length);
        Console.Write(historyLine);
        return historyLine;
    }

    private void ClearLine(int skip = 0)
    {
        int y = Console.CursorTop;
        Console.SetCursorPosition(skip, y);
        for (int i = 0; i < Math.Clamp(Console.WindowWidth - skip, 0, Console.WindowWidth); i++)
        {
            Console.Write(' ');
        }
        
        Console.SetCursorPosition(0, y);
    }

    private void WritePrompt(int skip = 0)
    {
        ClearLine(Prompt.Length + skip);
        Console.Write(Prompt);
    }
    
    public void TakeInput()
    {
        WritePrompt();
        string? line = ReadLine();
        if (line == null) throw new Exception("Received a null line from console");

        // If the line has content, push it into the front of the shell's history
        if (!string.IsNullOrWhiteSpace(line))
        {
            this._history.Insert(0, line);
        }
        
        Console.Write('\n');
        
        ProcessLine(line.Split(' '));
    }

    private static void ProcessLine(string[] args)
    {
        
    }
}