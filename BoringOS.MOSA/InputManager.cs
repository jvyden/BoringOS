using System.Collections.Generic;
using Mosa.DeviceDriver.ScanCodeMap;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.Service;
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
        
        foreach (Device device in keyboards)
        {
            if (device.DeviceDriver is not IKeyboardDevice keyboardDriver) continue;
            
            Keyboards.Add(new Keyboard(keyboardDriver, keymap));
        }
    }
}