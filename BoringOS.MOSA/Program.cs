using System;
using BoringOS.MOSA.Hardware;
using Mosa.DeviceDriver;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.Service;
using Mosa.Kernel.x86;
using Mosa.Runtime.x86;

namespace BoringOS.MOSA;

// [UsedImplicitly]
public static class Program
{
    private static ServiceManager _serviceManager;
    private static DeviceService _deviceService;
    private static HardwareAbstractionLayer _hal;
    private static BoringMosaKernel _kernel = null!;
    
    public static void Setup()
    {
        try
        {
            _serviceManager = new ServiceManager();
            _deviceService = new DeviceService();

            _serviceManager.AddService(_deviceService);

            _hal = new HardwareAbstractionLayer();
            Screen.WriteLine("Initializing hardware...");
            Mosa.DeviceSystem.Setup.Initialize(_hal, _deviceService.ProcessInterrupt);
            _deviceService.RegisterDeviceDriver(Mosa.DeviceDriver.Setup.GetDeviceDriverRegistryEntries());
            _deviceService.Initialize(new X86System(), null);
            
            Screen.WriteLine("Initializing input manager...");
            InputManager.Initialize(_deviceService);

            Screen.WriteLine("Setup complete, jumping to BoringMosaKernel");
            _kernel = new BoringMosaKernel();
            _kernel.OnBoot();
            _kernel.BeforeRun();
        }
        catch (Exception e)
        {
            PrintException(e);
            while (e.InnerException != null)
            {
                e = e.InnerException;
                PrintException(e);
            }
            
            while (true)
            {
                Native.Hlt();
            }
        }
    }

    private static void PrintException(Exception e)
    {
        Screen.Write(e.GetType().ToString());
        Screen.Write(": ");
        Screen.WriteLine(e.Message);
    }

    public static void Loop()
    {
        while (_kernel.KernelIsRunning)
        {
            _kernel.Run();
        }

        _kernel.Halt();
    }
}