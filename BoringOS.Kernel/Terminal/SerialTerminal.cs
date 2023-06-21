using System;
using BoringOS.Terminal;
using Cosmos.HAL;

namespace BoringOS.Kernel.Terminal;

public class SerialTerminal : ITerminal
{
    public ConsoleKeyInfo ReadKey()
    {
        char c = (char)SerialPort.Receive();
        if (c == (char)0x1b) return this.HandleVt100Code();

        return FromChar(c);
    }

    private ConsoleKeyInfo HandleVt100Code()
    {
        char c = (char)SerialPort.Receive();
        if (c != '[') return FromChar(c);

        ConsoleKey keyCode = SerialPort.Receive() switch
        {
            0x41 => ConsoleKey.UpArrow,
            0x42 => ConsoleKey.DownArrow,
            0x43 => ConsoleKey.RightArrow,
            0x44 => ConsoleKey.LeftArrow,
            _ => ConsoleKey.NoName,
        };
        
        return FromKey(keyCode);
    }

    private ConsoleKeyInfo FromChar(char c)
    {
        ConsoleKey key = c switch
        {
            '\r' => ConsoleKey.Enter,
            '\b' => ConsoleKey.Backspace,
            _ => ConsoleKey.NoName
        };

        return new ConsoleKeyInfo(c, key, char.IsUpper(c), false, false);
    }
    
    private ConsoleKeyInfo FromKey(ConsoleKey key)
    {
        return new ConsoleKeyInfo('\0', key, false, false, false);
    }
    
    private int _cursorY;
    private int _cursorX;

    public int CursorX
    {
        get => this._cursorX;
        set
        {
            this._cursorX = value;
            this.WriteVt100Position();
        }
    }

    public int CursorY
    {
        get => this._cursorY;
        set
        {
            this._cursorY = value;
            this.WriteVt100Position();
        }
    }

    public int Width => 80;
    public int Height => 60;
    public void SetCursorPosition(int x, int y)
    {
        this._cursorX = x;
        this._cursorY = y;
        
        this.WriteVt100Position();
    }

    private void WriteVt100Header(bool secondPart = true)
    {
        this.WriteChar((char)0x1b);
        if(secondPart) this.WriteChar('[');
    }

    private void WriteVt100Code(string code)
    {
        this.WriteVt100Header();
        this.WriteString(code);
    }
    
    private void WriteVt100Code(char c, bool secondPart = true)
    {
        this.WriteVt100Header(secondPart);
        this.WriteChar(c);
    }

    private void WriteVt100Position()
    {
        this.WriteVt100Code($"{this._cursorY + 1};{this._cursorX + 1}H");
    }

    public void WriteChar(char c)
    {
        if (c == '\n')
        {
            this._cursorY++;
            this.WriteChar('\r');
        }
        else if (c == '\r')
        {
            this._cursorX = 0;
        }
        else
        {
            this._cursorX++;
        }
        
        SerialPort.Send(c);
    }

    public void WriteString(string str)
    {
        foreach (char c in str.ReplaceLineEndings("\n"))
        {
            this.WriteChar(c);
        }
    }

    public void ClearLine(int skip = 0)
    {
        this.CursorX = 0;
        this.WriteVt100Code("2K");
    }

    public void ClearScreen()
    {
        // this.WriteVt100Code("2J");
        this.WriteVt100Code('c', false);
        this._cursorX = 0;
        this._cursorY = 0;
    }
    
}