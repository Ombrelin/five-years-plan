using System;
using Avalonia;
using Serilog;

namespace NodeEditorDemo;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Application Crashed");
        }
    }

    static Program()
    {
        App.EnableInputOutput = true;
        App.EnableMainMenu = true;
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseSkia();
}