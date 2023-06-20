using System;
using BoringOS.Terminal;
using Cosmos.HAL;

namespace BoringOS.Kernel.Terminal;

public class SerialTerminal : ITerminal
{
    public ConsoleKeyInfo ReadKey()
    {
        char c = (char)SerialPort.Receive();
        ConsoleKey key = c switch
        {
            '\r' => ConsoleKey.Enter,
            '\b' => ConsoleKey.Backspace,
            _ => ConsoleKey.NoName
        };

        return new ConsoleKeyInfo(c, key, char.IsUpper(c), false, false);
    }

    public int CursorX { get; set; }
    public int CursorY { get; set; }
    public int Width => 80;
    public int Height => 60;
    public void SetCursorPosition(int x, int y)
    {
        this.CursorX = x;
        this.CursorY = y;
    }

    public void WriteChar(char c)
    {
        if(c == '\n') SerialPort.Send('\r');
        SerialPort.Send(c);
    }

    public void WriteString(string str)
    {
        SerialPort.SendString(str.ReplaceLineEndings("\r\n"));
    }

    public void ClearLine(int skip = 0)
    {
        this.WriteChar('\n');
    }
}