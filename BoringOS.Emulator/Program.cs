using BoringOS.Emulator;

BoringEmulatedKernel kernel = new();

kernel.OnBoot();
kernel.BeforeRun();
while (kernel.KernelIsRunning)
{
    kernel.Run();
}
kernel.AfterRun();
