using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NodeEditor.Mvvm;
using NodeEditorDemo.Services;
using NodeEditorDemo.ViewModels;
using NodeEditorDemo.Views;
using Serilog;

namespace NodeEditorDemo;

public class App : Application
{
    private readonly ServiceProvider services = new ServiceCollection()
        .AddLogging(loggingBuilder => { loggingBuilder.AddSerilog(); })
        .BuildServiceProvider();

    public static bool EnableInputOutput { get; set; } = true;

    public static bool EnableMainMenu { get; set; } = true;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ConfigureSerilog();
        var vm = new MainViewViewModel
        {
            IsToolboxVisible = true
        };

        var editor = new EditorViewModel
        {
            Serializer = new NodeSerializer(typeof(ObservableCollection<>)),
            Factory = new NodeFactory()
        };

        editor.Templates = editor.Factory.CreateTemplates();
        editor.Drawing = new DrawingAdapter(services.GetService<ILogger<DrawingAdapter>>()).Drawing;
        editor.Drawing.SetSerializer(editor.Serializer);

        vm.Editor = editor;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };

            DataContext = vm;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void ConfigureSerilog()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                path: $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.five-years-plan/logs.txt",
                rollingInterval: RollingInterval.Hour)
            .CreateLogger();
    }
}