#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.DeviceDriver.ISA;
using Mosa.DeviceDriver.ScanCodeMap;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.Service;
using Keyboard = Mosa.DeviceSystem.Keyboard;

namespace BoringOS.MOSA.Hardware;

public static class InputManager
{
    public static readonly List<Keyboard> Keyboards = new();
    private static readonly Queue<ConsoleKeyInfo> KeyQueue = new();
    private static readonly object KeyReadLock = new();

    private static ConsoleKeyInfo? ConvertFromMosa(Key? key)
    {
        if (key == null) return null;
        return new ConsoleKeyInfo(key.Character, ConsoleKey.None, key.Shift, key.Alt, key.Control);
    }

    public static ConsoleKeyInfo WaitForKey()
    {
        lock (KeyReadLock)
        {
            ConsoleKeyInfo result;
            while (!KeyQueue.TryDequeue(out result))
            {}
            Console.Write("KEY: ");
            Console.WriteLine(result.KeyChar.ToString());

            return result;
        }
    }

    internal static void HandleKeyboardInterrupt()
    {
        foreach (Keyboard device in Keyboards)
        {
            Key? key = null;
            const byte tries = 2;// # of tries to get a key
            for (int i = 0; i < tries; i++)
            {
                key = device.GetKeyPressed();
                if (key != null) break;
                
                Console.WriteLine("Key was null");
            }

            if (key == null)
            {
                Console.WriteLine("Couldn't get a key");
                continue;
            }

            ConsoleKeyInfo? consoleKey = ConvertFromMosa(key);
            Debug.Assert(consoleKey != null);
            
            // ReSharper disable once InconsistentlySynchronizedField (don't lock for writes)
            KeyQueue.Enqueue(consoleKey!.Value);
        }
    }

    public static void Initialize(DeviceService service)
    {
        // Get StandardKeyboard
        List<Device> keyboards = service.GetDevices("StandardKeyboard");
        IScanCodeMap keymap = new US();
        
        foreach (Device device in keyboards)
        {
            if (device.DeviceDriver is not StandardKeyboard keyboard) continue;
            Keyboards.Add(new Keyboard(keyboard, keymap));
        }
    }
}