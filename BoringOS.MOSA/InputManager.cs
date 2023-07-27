using System;
using System.Collections.Generic;
using Mosa.DeviceDriver.ScanCodeMap;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.Service;
using Mosa.Kernel.x86;
using KernelKeyboard = Mosa.Kernel.x86.Keyboard;
using Keyboard = Mosa.DeviceSystem.Keyboard;

namespace BoringOS.MOSA;

public static class InputManager
{
    public static readonly List<Keyboard> Keyboards = new();

    public static void Initialize(DeviceService service)
    {
        // Get StandardKeyboard
        List<Device> keyboards = service.GetDevices("StandardKeyboard");
        IScanCodeMap keymap = new US();
        
        // foreach (var device in service.GetAllDevices())
        // {
        //     Screen.WriteLine(device.Name);
        //     Screen.WriteLine(device.Status.ToString());
        //     Screen.WriteLine(device.DeviceDriver.ToString());
        // }

        Screen.WriteLine(keyboards.Count.ToString());
        foreach (Device device in keyboards)
        {
            if (device.DeviceDriver is not IKeyboardDevice keyboardDriver) continue;
            
            Keyboards.Add(new Keyboard(keyboardDriver, keymap));
        }
    }
}