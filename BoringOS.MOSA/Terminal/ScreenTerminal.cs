using System;
using BoringOS.Terminal;
using Mosa.DeviceSystem;
using Mosa.Kernel.x86;

namespace BoringOS.MOSA.Terminal;

public class ScreenTerminal : ITerminal
{
    private readonly IKeyboard _keyboard;

    public ScreenTerminal(IKeyboard keyboard)
    {
        _keyboard = keyboard;
    }

    public ConsoleKeyInfo ReadKey()
    {
        while (true)
        {
            Key key = _keyboard.GetKeyPressed();

            if (key == null) continue;
            if (key.KeyType == KeyType.NoKey) continue;

            return new ConsoleKeyInfo(key.Character, ConsoleKey.None, key.Shift, key.Alt, key.Control);
        }
    }

    public int CursorX
    {
        get => (int)Screen.Row;
        set
        {
            Screen.SetCursor((uint)value, Screen.Column);
            Screen.UpdateCursor();
        }
    }

    public int CursorY
    {
        get => (int)Screen.Column;
        set
        {
            Screen.SetCursor(Screen.Row, (uint)value);
            Screen.UpdateCursor();
        }
    }

    public int Width => (int)Screen.Rows;
    public int Height => (int)Screen.Columns;
    public void SetCursorPosition(int x, int y)
    {
        Screen.SetCursor((uint)y, (uint)x);
        Screen.UpdateCursor();
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
        this.CursorY--;
    }

    public void ClearScreen()
    {
        // Screen.Clear();
    }
}