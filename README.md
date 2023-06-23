# BoringOS
A boring abstracted kernel for use with COSMOS and the .NET runtime.

![BoringOS running in QEMU](https://github.com/jvyden/BoringOS/assets/40577357/5e33a443-50dd-4b72-bc8c-02c70553f7a0)

![BoringOS running in the .NET runtime](https://github.com/jvyden/BoringOS/assets/40577357/26bc1d8e-3aa5-4197-86e8-fcc9cb93815c)


## BoringOS.Emulator

A version of the kernel in the .NET runtime, intended for quick, iterative development/debugging of applications.

## BoringOS.Kernel

An actual implementation of the kernel on bare metal. Run with `run.sh` on UNIX or through the COSMOS Visual Studio extension.

### Startup keys

When BoringOS starts up, you can hold a key down to trigger different behaviours in the kernel, similar to Apple computers.

`s`: Load a serial VT100 terminal.
`c`: Boot a SVGAII/VGA graphical terminal (default in VBE builds)

![BoringOS running in serial mode](https://github.com/jvyden/BoringOS/assets/40577357/ea243350-6a04-45be-b173-c46c2d84cdec)
