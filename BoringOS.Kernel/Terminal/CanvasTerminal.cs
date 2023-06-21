using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using BoringOS.Terminal;
using Cosmos.Core.Multiboot;
using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using Console = System.Console;

namespace BoringOS.Kernel.Terminal;

public class CanvasTerminal : ITerminal
{
    private readonly Canvas _canvas;

    private const string Font = "HAAcABwAHAAcABwAHAAcABwAHAA+AD4AHAAAAAAAAAB/AH8AfwB/AH4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+AP4A/gP+A/4B/AH8A/4D/gH8A/gD+AAAAAAAAAAAAPwB/gH8AfAB+AH+AH4AfgH+Af4B/AH4AHAAcAAAAAAB7gP+A/wD/AP4AHgA8AD+Af4B/gP+A/4DgAAAAAAAAAD4AfwB/AH8APgB8AP+A74DvgP+A/4B/gAAAAAAAAAAAHAAcABwAHAAcAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAfADwAPAA4ADgAOAA4ADgAOAA4ADwAPAAfAA8AAAAAADwAHgAeAA4ADwAPAA8ADwAPAA4AHgAeADwAOAAAAAAAAAAAAH8AfwA+AP+A/4A+AH8AfwAAAAAAAAAAAAAAAAAAAAAAHAAcAH8AfwAcABwAHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB4AHgAeADwAPAA8ADgAAAAAAAAAAAAAAAAAfwB/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAcAD4APgAAAAAAAAAAAAeAB4APAA8AHgAeADwAeAB4APAA8ADgAMAAAAAAAAAAPgB/APeA/4D/gP+A/4D/gP+A94B/AD4AAAAAAAAAAAAcAD4AfgB8ABwAHAAcABwAHAAcAP+A/4AAAAAAAAAAAD4AfwD/gPeAB4AfAD4AfADwAPAA/4D/gAAAAAAAAAAAPgB/AH+Ad4A/AD8AH4AHgPeA/4B/AD4AAAAAAAAAAAAPAB8AHwA/AH8A9wD/gP+ABwAHAAcABwAAAAAAAAAAAP+A/4DwAP8A/4D/gHeAB4DngP+A/wA+AAAAAAAAAAAAPwB8AHAAfwB/gH+Ad4BzgHeAf4B/AD4AAAAAAAAAAAD/gP+AB4APAA8AHgAeADwAPAB4AHgAcAAAAAAAAAAAAD4AfwD3gPeAfwB/AP+A94DjgPeA/wA+AAAAAAAAAAAAPgB/AP+A94DzgPeAf4B/gAeADwB/AHwAAAAAAAAAAAAAAAAAAAAAAB4AHgAcAAAAHgAeABwAAAAAAAAAAAAAAAAAAAAAAAAAHgAeAB4AAAAAABwAPAA8ADwAPAA4AAAAA4APgB+APgB8APgA+AB8AD4AHwAPgAeAAAAAAAAAAAAAAAAAAAB/AH8AAAB/AH8AAAAAAAAAAAAAAAAAAAAAAPAA+AB8AD4AHwAPgAeAH4A+AHwA+ADwAAAAAAAAAAAAPgB/AH+Ad4APgB8AHgAcABwAPAA+ADwAAAAAAAAAAAA+AH8Af4DzgP+A/4D/gP+A/4D/gPOA94B/AD4AAAAAAA4AHwAfAD8APwB/AH+A/4D3gPOA48DjwAAAAAAAAAAA/gD/AO8A5wDvAP8A/wDvgOeA/4D/APwAAAAAAAAAAAAfgD+Af4B7gPAA8ADgAOAA84D/gP+APgAAAAAAAAAAAPgA/gD/AO+A54DjgOOA48DjgPeA/4D/AAAAAAAAAAAA/4D/gPAA4ADgAP+A/4DgAOAA4AD/gP+AAAAAAAAAAAD/gP+A8ADgAP8A/4DgAOAA4ADgAOAA4AAAAAAAAAAAAB8AP4B/gHgA8AD/gP+A/4D3gP8A/wA8AAAAAAAAAAAA44D3gPeA94D3gP+A/4D3gPeA94D3gOeAAAAAAAAAAAD/gP+AHAAcABwAHAAcABwAHAAcAP+A/4AAAAAAAAAAAP+A/4AOAA4ADgAOAA4A7gD+AP4AfgA8AAAAAAAAAAAA5wDvAP8A/gD8APgA+AD8AP4A7wDngOOAAAAAAAAAAADwAPAA8ADwAPAA8ADwAPAA8ADzgP+A/4AAAAAAAAAAAHeA/4D/gP+A/4D/gP+A/4D/gP+A/8D/gAAAAAAAAAAA84DzgPuA+4D/gP+A/4D/gO+A74DngOeAAAAAAAAAAAA+AH8A/4D3gPOA84DjgPeA94D/gH8APgAAAAAAAAAAAP4A/wD3gPeA94D3gP8A/gDwAPAA8ADwAAAAAAAAAAAAPgB/AP+A94DzgOOA44D/gP+A/4B/AD8AB4AHgAOAAAD8AP8A/4D3gPeA/4D/AP4A/wD/gPeA84AAAAAAAAAAAD+Af4D4APAA/gB/gA+AA4DngP+A/wB+AAAAAAAAAAAA/4D/gBwAHAAcABwAHAAcAB4AHgAeABwAAAAAAAAAAADjgOOA44DjgOOA44DjgOOA94D/gH8APgAAAAAAAAAAAOOA48D3gPeA94B3AH8AfwA+AD4APgAcAAAAAAAAAAAA/4D/gP+A/4D/gP+A/4D/gP+A/4D/gHcAAAAAAAAAAADjgPeA/4B/AD8APgA+AD4AfwD3gPeA44AAAAAAAAAAAOOA44D3gPeAfwB/AD4AHgA8ADwAeAB4ADAAAAAAAAAA/4D/gA+ADwAeAD4APAB4AHgA8AD/gP+AAAAAAAAAAAA4ADgAOAA4ADgAOAA4ADgAOAA4ADgAPwA/AAAAAAAAAPAA8AB4AHgAPAA8AB4ADgAPAAeAB4AHgAEAAAAAAAAABwAHAAcABwAHAAcABwAHAAcABwAHAD8APwAAAAAAAAB/gHeAY4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD/gP+AAAAAAAAAPgAeAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/AH+Af4D3gPeA94D/gH+AP4AAAAAAAAAAAHAAcABwAH8Af4B3gHeAc4B3gHeAfwB+AAAAAAAAAAAAAAAAAAAAHgB/AH+A94DwAPOA/4B/gD4AAAAAAAAAAAADgAeAB4A/gH+A/4D3gPeA94D/gH+AP4AAAAAAAAAAAAAAAAAAAD4AfwB/gP+A/wD7APeAf4A/AAAAAAAAAAAAP4A+ADgA/wD/gDgAOAA4ADwAPAD/AP8AAAAAAAAAAAAAAAAAAAAAAH+Af4B3gHeAd4B/gH+AP4AHgAeAbwB/APAA8ADwAP8A/wD/gPeA94D3gPeA94D3gAAAAAAAAAAAHAA+AD4AfAB+AB4AHgAeAB4AHgD/gP+AAAAAAAAAAAAAAA8ADwAAAH8AfwAPAA8ADwAPAA8ADwAPAA8APwB+APAA8ADwAPeA/wD+APwA/gD/AP+A94DzgAAAAAAAAAAAfgAcABwAHAAcABwAHAAcABwAHAD/gP+AAAAAAAAAAAAAAAAAAAD/gP+A/4D/gP+A/4D/gP+A/4AAAAAAAAAAAAAAAAAAAH8A/wD/gPeA94D3gPeA94DzgAAAAAAAAAAAAAAAAAAAPgB/APeA94D3gPeA/4B/AD4AAAAAAAAAAAAAAAAAAAB+AH8Af4B3gHeAd4B3gH8AfgBwAHAAcABwAAAAAAAAAAAAf4B/gHeAd4B3gH+Af4A/gAeAB4AHgAeAAAAAAAAAfwB/gH+A/4DwAPAA8ABwAHAAAAAAAAAAAAAAAAAAAAAfAH8Af4B/AH+AZ4D/gP8AfgAAAAAAAAAAAHgAeAB4AP8A/wB4AHgAeAB4AHsAP4AfAAAAAAAAAAAAAAAAAAAA94D3gPeA94D3gPeA/4B/gD+AAAAAAAAAAAAAAAAAAADjgPeA94B/gH8AfwA+AD4AHAAAAAAAAAAAAAAAAAAAAP+A/8D/gP+A/4D/gP+AfwB3AAAAAAAAAAAAAAAAAAAAY4D3gP8AfwA+AD4AfwD3gOeAAAAAAAAAAAAAAAAAAAAAAHOAd4B3gH8AfwA+AD4APAA8AHgAeABwAAAAAAAAAP+A/4APAB8APgB8AHgA/4D/gAAAAAAAAAAAPgA8ABwAHgB+AHwAfgAeABwAPAA8AB+AD4AAAAAAAAAMAAwADAAMAAwADAAMAAwADAAMAAwADAAMAAwADAAAAB4AHgAcADwAP4AfgD8APAAeAB4AHgB8AHgAAAAAAAAAAAAAAAAAe4D/gP+A/4DvAAAAAAAAAAAAAAAAAAAAAAA=";
    private const string FontCharset = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
    private readonly byte[] _font;

    private const int CharHeight = 16;
    private const int CharWidth = 8;
    
    // private int CharHeight => this._font.Height;
    // private int CharWidth => this._font.Width;
    
    public CanvasTerminal()
    {
        Mode mode = new(800, 600, ColorDepth.ColorDepth32);

        this._font = Convert.FromBase64String(Font);

        if (VMTools.IsVMWare) this._canvas = new SVGAIICanvas(mode);
        else if (Multiboot2.IsVBEAvailable) this._canvas = new VBECanvas(mode);
        else this._canvas = new VGACanvas();

        this.ClearScreen();
    }
    
    public ConsoleKeyInfo ReadKey()
    {
        return Console.ReadKey(true);
    }
    
    private void DrawChar(char c, Color color, int x, int y)
    {
        int index = FontCharset.IndexOf(c); // TODO: cache chars
        if (index == -1) return;
        
        const int size8 = CharHeight / 8;
        int sizePerFont = CharHeight * size8 * index;

        for (int height = 0; height < CharHeight; height++)
        {
            for (int aw = 0; aw < size8; aw++)
            {
                for (int ww = 0; ww < 8; ww++)
                {
                    if ((this._font[sizePerFont + height * size8 + aw] & (0x80 >> ww)) == 0) continue;
                    int max = aw * 8 + ww;

                    this._canvas.DrawPoint(color, x + max, y + height);
                }
            }
        }
    }

    public int CursorX { get; set; }
    public int CursorY { get; set; }

    public int Width => (int)(this._canvas.Mode.Width / CharWidth);
    public int Height => (int)(this._canvas.Mode.Height / CharHeight);
    
    public void SetCursorPosition(int x, int y)
    {
        this.CursorX = x;
        this.CursorY = y;
    }

    public void WriteChar(char c)
    {
        if (c == '\n')
        {
            this.CursorX = 0;
            this.CursorY++;
            return;
        }

        if (c == ' ')
        {
            this.CursorX++;
            return;
        }
        
        this.DrawChar(c, Color.White, this.CursorX * CharWidth, this.CursorY * CharHeight);
        this.CursorX++;
    }

    public void WriteString(string str)
    {
        foreach (char c in str) this.WriteChar(c);
    }

    public void ClearLine(int skip = 0)
    {
        
    }

    public void ClearScreen()
    {
        this._canvas.Clear();
        this.SetCursorPosition(0, 0);
    }
} 