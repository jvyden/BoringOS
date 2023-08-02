using System;
using BoringOS.MOSA.Hardware;
using BoringOS.Terminal;
using Mosa.DeviceSystem;
using Mosa.Kernel.x86;

namespace BoringOS.MOSA.Terminal;

public class ScreenTerminal : ITerminal
{
    private readonly IKeyboard _keyboard;
    private static readonly ConsoleKeyInfo EmptyChar = new('\0', ConsoleKey.None, false, false, false);

    public ScreenTerminal(IKeyboard keyboard)
    {
        _keyboard = keyboard;
        // this.ClearScreen();
    }

    public ConsoleKeyInfo ReadKey()
    {
        return InputManager.WaitForKey();
    }

    public int CursorX
    {
        get => (int)Screen.Column;
        set
        {
            Screen.Column = (uint)value;
            Screen.UpdateCursor();
        }
    }

    public int CursorY
    {
        get => (int)Screen.Row;
        set
        {
            Screen.Row = (uint)value;
            Screen.UpdateCursor();
        }
    }

    public int Width => (int)Screen.Columns;
    public int Height => (int)Screen.Rows;
    public void SetCursorPosition(int x, int y)
    {
        this.CursorX = x;
        this.CursorY = y;
        // Screen.SetCursor((uint)y, (uint)x);
    }

    public void WriteChar(char c)
    {
        if (c == '\r')
        {
            CursorX = 0;
            return;
        }

        if (c == '\n')
        {
            this.WriteChar('\r');
            CursorY++;
            return;
        }

        byte b = (byte)c;
        if (b < 0x20 || b > 0x7E)
        {
            Screen.Write('?');
            return;
        }
        
        Screen.Write(c);
    }

    public void WriteString(string str)
    {
        foreach (char c in str) WriteChar(c);
    }

    public void ClearLine(int skip = 0)
    {
        this.CursorX = skip;
        for (int i = 0; i < Math.Clamp(this.Width - skip, 0, this.Width); i++)
        { 
            this.WriteChar(' ');
        }

        this.CursorX = 0;
    }

    public void ClearScreen()
    {
        Screen.Clear();
    }
}