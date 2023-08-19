using System;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using FiveYearPlans.ViewModels;
using FiveYearPlans.ViewModels.Buildings.ViewModels;
using NodeEditorDemo.ViewModels;
using NodeEditorDemo.Views.Nodes;

namespace NodeEditorDemo;

public class ViewLocator : IDataTemplate
{
    public Control Build(object? data)
    {
        var name = data?.GetType().FullName?.Replace("ViewModel", "View");
        var type = name is null ? null : Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }
        else
        {
            return data?.GetType().FullName.Split(".").Last() switch
            {
                nameof(BuilderViewModel) => new BuilderView(),
                nameof(SplitterViewModel) => new SplitterView(),
                nameof(MinerViewModel) => new MinerView(),
                _ => new TextBlock { Text = "Not Found: " + name }
            };
        }
    }

    public bool Match(object? data)
    {
        return data is INotifyPropertyChanged;
    }
}
