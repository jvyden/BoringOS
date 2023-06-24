#if DEBUGMOSA

// ReSharper disable once CheckNamespace
namespace System;

public readonly struct ConsoleKeyInfo
{
    public ConsoleKeyInfo(char keyChar, ConsoleKey key, bool shift, bool alt, bool control)
    {
        this.KeyChar = keyChar;
        this.Key = key;
        
        this.Shift = shift;
        this.Alt = alt;
        this.Control = control;
    }

    public readonly char KeyChar;
    public readonly ConsoleKey Key;
    public readonly bool Shift;
    public readonly bool Alt;
    public readonly bool Control;
    
}

#endif