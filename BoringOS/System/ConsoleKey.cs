#if DEBUGMOSA

// ReSharper disable once CheckNamespace
namespace System;


/// <summary>Specifies the standard keys on a console.</summary>
public enum ConsoleKey
{
    /// <summary>The BACKSPACE key.</summary>
    Backspace = 8,

    /// <summary>The TAB key.</summary>
    Tab = 9,

    /// <summary>The CLEAR key.</summary>
    Clear = 12, // 0x0000000C

    /// <summary>The ENTER key.</summary>
    Enter = 13, // 0x0000000D

    /// <summary>The PAUSE key.</summary>
    Pause = 19, // 0x00000013

    /// <summary>The ESC (ESCAPE) key.</summary>
    Escape = 27, // 0x0000001B

    /// <summary>The SPACEBAR key.</summary>
    Spacebar = 32, // 0x00000020

    /// <summary>The PAGE UP key.</summary>
    PageUp = 33, // 0x00000021

    /// <summary>The PAGE DOWN key.</summary>
    PageDown = 34, // 0x00000022

    /// <summary>The END key.</summary>
    End = 35, // 0x00000023

    /// <summary>The HOME key.</summary>
    Home = 36, // 0x00000024

    /// <summary>The LEFT ARROW key.</summary>
    LeftArrow = 37, // 0x00000025

    /// <summary>The UP ARROW key.</summary>
    UpArrow = 38, // 0x00000026

    /// <summary>The RIGHT ARROW key.</summary>
    RightArrow = 39, // 0x00000027

    /// <summary>The DOWN ARROW key.</summary>
    DownArrow = 40, // 0x00000028

    /// <summary>The SELECT key.</summary>
    Select = 41, // 0x00000029

    /// <summary>The PRINT key.</summary>
    Print = 42, // 0x0000002A

    /// <summary>The EXECUTE key.</summary>
    Execute = 43, // 0x0000002B

    /// <summary>The PRINT SCREEN key.</summary>
    PrintScreen = 44, // 0x0000002C

    /// <summary>The INS (INSERT) key.</summary>
    Insert = 45, // 0x0000002D

    /// <summary>The DEL (DELETE) key.</summary>
    Delete = 46, // 0x0000002E

    /// <summary>The HELP key.</summary>
    Help = 47, // 0x0000002F

    /// <summary>The 0 key.</summary>
    D0 = 48, // 0x00000030

    /// <summary>The 1 key.</summary>
    D1 = 49, // 0x00000031

    /// <summary>The 2 key.</summary>
    D2 = 50, // 0x00000032

    /// <summary>The 3 key.</summary>
    D3 = 51, // 0x00000033

    /// <summary>The 4 key.</summary>
    D4 = 52, // 0x00000034

    /// <summary>The 5 key.</summary>
    D5 = 53, // 0x00000035

    /// <summary>The 6 key.</summary>
    D6 = 54, // 0x00000036

    /// <summary>The 7 key.</summary>
    D7 = 55, // 0x00000037

    /// <summary>The 8 key.</summary>
    D8 = 56, // 0x00000038

    /// <summary>The 9 key.</summary>
    D9 = 57, // 0x00000039

    /// <summary>The A key.</summary>
    A = 65, // 0x00000041

    /// <summary>The B key.</summary>
    B = 66, // 0x00000042

    /// <summary>The C key.</summary>
    C = 67, // 0x00000043

    /// <summary>The D key.</summary>
    D = 68, // 0x00000044

    /// <summary>The E key.</summary>
    E = 69, // 0x00000045

    /// <summary>The F key.</summary>
    F = 70, // 0x00000046

    /// <summary>The G key.</summary>
    G = 71, // 0x00000047

    /// <summary>The H key.</summary>
    H = 72, // 0x00000048

    /// <summary>The I key.</summary>
    I = 73, // 0x00000049

    /// <summary>The J key.</summary>
    J = 74, // 0x0000004A

    /// <summary>The K key.</summary>
    K = 75, // 0x0000004B

    /// <summary>The L key.</summary>
    L = 76, // 0x0000004C

    /// <summary>The M key.</summary>
    M = 77, // 0x0000004D

    /// <summary>The N key.</summary>
    N = 78, // 0x0000004E

    /// <summary>The O key.</summary>
    O = 79, // 0x0000004F

    /// <summary>The P key.</summary>
    P = 80, // 0x00000050

    /// <summary>The Q key.</summary>
    Q = 81, // 0x00000051

    /// <summary>The R key.</summary>
    R = 82, // 0x00000052

    /// <summary>The S key.</summary>
    S = 83, // 0x00000053

    /// <summary>The T key.</summary>
    T = 84, // 0x00000054

    /// <summary>The U key.</summary>
    U = 85, // 0x00000055

    /// <summary>The V key.</summary>
    V = 86, // 0x00000056

    /// <summary>The W key.</summary>
    W = 87, // 0x00000057

    /// <summary>The X key.</summary>
    X = 88, // 0x00000058

    /// <summary>The Y key.</summary>
    Y = 89, // 0x00000059

    /// <summary>The Z key.</summary>
    Z = 90, // 0x0000005A

    /// <summary>The left Windows logo key (Microsoft Natural Keyboard).</summary>
    LeftWindows = 91, // 0x0000005B

    /// <summary>The right Windows logo key (Microsoft Natural Keyboard).</summary>
    RightWindows = 92, // 0x0000005C

    /// <summary>The Application key (Microsoft Natural Keyboard).</summary>
    Applications = 93, // 0x0000005D

    /// <summary>The Computer Sleep key.</summary>
    Sleep = 95, // 0x0000005F

    /// <summary>The 0 key on the numeric keypad.</summary>
    NumPad0 = 96, // 0x00000060

    /// <summary>The 1 key on the numeric keypad.</summary>
    NumPad1 = 97, // 0x00000061

    /// <summary>The 2 key on the numeric keypad.</summary>
    NumPad2 = 98, // 0x00000062

    /// <summary>The 3 key on the numeric keypad.</summary>
    NumPad3 = 99, // 0x00000063

    /// <summary>The 4 key on the numeric keypad.</summary>
    NumPad4 = 100, // 0x00000064

    /// <summary>The 5 key on the numeric keypad.</summary>
    NumPad5 = 101, // 0x00000065

    /// <summary>The 6 key on the numeric keypad.</summary>
    NumPad6 = 102, // 0x00000066

    /// <summary>The 7 key on the numeric keypad.</summary>
    NumPad7 = 103, // 0x00000067

    /// <summary>The 8 key on the numeric keypad.</summary>
    NumPad8 = 104, // 0x00000068

    /// <summary>The 9 key on the numeric keypad.</summary>
    NumPad9 = 105, // 0x00000069

    /// <summary>The Multiply key (the multiplication key on the numeric keypad).</summary>
    Multiply = 106, // 0x0000006A

    /// <summary>The Add key (the addition key on the numeric keypad).</summary>
    Add = 107, // 0x0000006B

    /// <summary>The Separator key.</summary>
    Separator = 108, // 0x0000006C

    /// <summary>The Subtract key (the subtraction key on the numeric keypad).</summary>
    Subtract = 109, // 0x0000006D

    /// <summary>The Decimal key (the decimal key on the numeric keypad).</summary>
    Decimal = 110, // 0x0000006E

    /// <summary>The Divide key (the division key on the numeric keypad).</summary>
    Divide = 111, // 0x0000006F

    /// <summary>The F1 key.</summary>
    F1 = 112, // 0x00000070

    /// <summary>The F2 key.</summary>
    F2 = 113, // 0x00000071

    /// <summary>The F3 key.</summary>
    F3 = 114, // 0x00000072

    /// <summary>The F4 key.</summary>
    F4 = 115, // 0x00000073

    /// <summary>The F5 key.</summary>
    F5 = 116, // 0x00000074

    /// <summary>The F6 key.</summary>
    F6 = 117, // 0x00000075

    /// <summary>The F7 key.</summary>
    F7 = 118, // 0x00000076

    /// <summary>The F8 key.</summary>
    F8 = 119, // 0x00000077

    /// <summary>The F9 key.</summary>
    F9 = 120, // 0x00000078

    /// <summary>The F10 key.</summary>
    F10 = 121, // 0x00000079

    /// <summary>The F11 key.</summary>
    F11 = 122, // 0x0000007A

    /// <summary>The F12 key.</summary>
    F12 = 123, // 0x0000007B

    /// <summary>The F13 key.</summary>
    F13 = 124, // 0x0000007C

    /// <summary>The F14 key.</summary>
    F14 = 125, // 0x0000007D

    /// <summary>The F15 key.</summary>
    F15 = 126, // 0x0000007E

    /// <summary>The F16 key.</summary>
    F16 = 127, // 0x0000007F

    /// <summary>The F17 key.</summary>
    F17 = 128, // 0x00000080

    /// <summary>The F18 key.</summary>
    F18 = 129, // 0x00000081

    /// <summary>The F19 key.</summary>
    F19 = 130, // 0x00000082

    /// <summary>The F20 key.</summary>
    F20 = 131, // 0x00000083

    /// <summary>The F21 key.</summary>
    F21 = 132, // 0x00000084

    /// <summary>The F22 key.</summary>
    F22 = 133, // 0x00000085

    /// <summary>The F23 key.</summary>
    F23 = 134, // 0x00000086

    /// <summary>The F24 key.</summary>
    F24 = 135, // 0x00000087

    /// <summary>The Browser Back key.</summary>
    BrowserBack = 166, // 0x000000A6

    /// <summary>The Browser Forward key.</summary>
    BrowserForward = 167, // 0x000000A7

    /// <summary>The Browser Refresh key.</summary>
    BrowserRefresh = 168, // 0x000000A8

    /// <summary>The Browser Stop key.</summary>
    BrowserStop = 169, // 0x000000A9

    /// <summary>The Browser Search key.</summary>
    BrowserSearch = 170, // 0x000000AA

    /// <summary>The Browser Favorites key.</summary>
    BrowserFavorites = 171, // 0x000000AB

    /// <summary>The Browser Home key.</summary>
    BrowserHome = 172, // 0x000000AC

    /// <summary>The Volume Mute key (Microsoft Natural Keyboard).</summary>
    VolumeMute = 173, // 0x000000AD

    /// <summary>The Volume Down key (Microsoft Natural Keyboard).</summary>
    VolumeDown = 174, // 0x000000AE

    /// <summary>The Volume Up key (Microsoft Natural Keyboard).</summary>
    VolumeUp = 175, // 0x000000AF

    /// <summary>The Media Next Track key.</summary>
    MediaNext = 176, // 0x000000B0

    /// <summary>The Media Previous Track key.</summary>
    MediaPrevious = 177, // 0x000000B1

    /// <summary>The Media Stop key.</summary>
    MediaStop = 178, // 0x000000B2

    /// <summary>The Media Play/Pause key.</summary>
    MediaPlay = 179, // 0x000000B3

    /// <summary>The Start Mail key (Microsoft Natural Keyboard).</summary>
    LaunchMail = 180, // 0x000000B4

    /// <summary>The Select Media key (Microsoft Natural Keyboard).</summary>
    LaunchMediaSelect = 181, // 0x000000B5

    /// <summary>The Start Application 1 key (Microsoft Natural Keyboard).</summary>
    LaunchApp1 = 182, // 0x000000B6

    /// <summary>The Start Application 2 key (Microsoft Natural Keyboard).</summary>
    LaunchApp2 = 183, // 0x000000B7

    /// <summary>The OEM 1 key (OEM specific).</summary>
    Oem1 = 186, // 0x000000BA

    /// <summary>The OEM Plus key on any country/region keyboard.</summary>
    OemPlus = 187, // 0x000000BB

    /// <summary>The OEM Comma key on any country/region keyboard.</summary>
    OemComma = 188, // 0x000000BC

    /// <summary>The OEM Minus key on any country/region keyboard.</summary>
    OemMinus = 189, // 0x000000BD

    /// <summary>The OEM Period key on any country/region keyboard.</summary>
    OemPeriod = 190, // 0x000000BE

    /// <summary>The OEM 2 key (OEM specific).</summary>
    Oem2 = 191, // 0x000000BF

    /// <summary>The OEM 3 key (OEM specific).</summary>
    Oem3 = 192, // 0x000000C0

    /// <summary>The OEM 4 key (OEM specific).</summary>
    Oem4 = 219, // 0x000000DB

    /// <summary>The OEM 5 (OEM specific).</summary>
    Oem5 = 220, // 0x000000DC

    /// <summary>The OEM 6 key (OEM specific).</summary>
    Oem6 = 221, // 0x000000DD

    /// <summary>The OEM 7 key (OEM specific).</summary>
    Oem7 = 222, // 0x000000DE

    /// <summary>The OEM 8 key (OEM specific).</summary>
    Oem8 = 223, // 0x000000DF

    /// <summary>The OEM 102 key (OEM specific).</summary>
    Oem102 = 226, // 0x000000E2

    /// <summary>The IME PROCESS key.</summary>
    Process = 229, // 0x000000E5

    /// <summary>The PACKET key (used to pass Unicode characters with keystrokes).</summary>
    Packet = 231, // 0x000000E7

    /// <summary>The ATTN key.</summary>
    Attention = 246, // 0x000000F6

    /// <summary>The CRSEL (CURSOR SELECT) key.</summary>
    CrSel = 247, // 0x000000F7

    /// <summary>The EXSEL (EXTEND SELECTION) key.</summary>
    ExSel = 248, // 0x000000F8

    /// <summary>The ERASE EOF key.</summary>
    EraseEndOfFile = 249, // 0x000000F9

    /// <summary>The PLAY key.</summary>
    Play = 250, // 0x000000FA

    /// <summary>The ZOOM key.</summary>
    Zoom = 251, // 0x000000FB

    /// <summary>A constant reserved for future use.</summary>
    NoName = 252, // 0x000000FC

    /// <summary>The PA1 key.</summary>
    Pa1 = 253, // 0x000000FD

    /// <summary>The CLEAR key (OEM specific).</summary>
    OemClear = 254, // 0x000000FE
}

#endif