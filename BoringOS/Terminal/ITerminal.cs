namespace BoringOS.Terminal;

public interface ITerminal
{
    public ConsoleKeyInfo ReadKey();
    
    public int CursorX { get; set; }
    public int CursorY { get; set; }
    
    public int Width { get; }
    public int Height { get; }

    public void SetCursorPosition(int x, int y);

    public void WriteChar(char c);
    public void WriteString(string str);

    public void ClearLine(int skip = 0);
    public void ClearScreen();
}