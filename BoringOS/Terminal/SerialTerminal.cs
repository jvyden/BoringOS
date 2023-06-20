using System;
using Cosmos.HAL;

namespace BoringOS.Terminal;

public class SerialTerminal : ITerminal
{
    public ConsoleKeyInfo ReadKey() => new((char)SerialPort.Receive(), ConsoleKey.NoName, false, false, false);

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
        SerialPort.Send(c);
    }

    public void WriteString(string str)
    {
        SerialPort.SendString(str);
    }

    public void ClearLine(int skip = 0)
    {
        this.WriteString("\r\n");
    }
}