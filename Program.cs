using Avalonia;
using Avalonia.LinuxFramebuffer;
using System;
using System.Linq;

namespace AnypocApp;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var builder = BuildAvaloniaApp();

        // Check for DRM mode (for QEMU virtio-gpu and modern displays)
        if (args.Contains("--drm"))
        {
            builder.StartLinuxDrm(args, card: "/dev/dri/card0", scaling: 1.0);
        }
        // Check if we should use framebuffer mode (for real hardware with /dev/fb0)
        else if (args.Contains("--framebuffer") || Environment.GetEnvironmentVariable("AVALONIA_USE_FRAMEBUFFER") == "1")
        {
            builder.StartLinuxFbDev(args);
        }
        else
        {
            builder.StartWithClassicDesktopLifetime(args);
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
