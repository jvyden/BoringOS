namespace BoringOS.Terminal;

public class ConsoleTerminal : ITerminal
{
    public ConsoleKeyInfo ReadKey() => Console.ReadKey(true);
    
    public int CursorX
    {
        get => Console.CursorLeft;
        set => Console.SetCursorPosition(value, CursorY);
    }

    public int CursorY
    {
        get => Console.CursorTop;
        set => Console.SetCursorPosition(CursorX, value);
    }

    public int Width => Console.WindowWidth;
    public int Height => Console.WindowHeight;

    public void SetCursorPosition(int x, int y) => Console.SetCursorPosition(x, y);

    public void WriteChar(char c) => Console.Write(c);
    public void WriteString(string str)
    {
        foreach (char c in str) this.WriteChar(c);
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
        Console.Clear();
    }
}