using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FiveYearPlans.ViewModels;
using FiveYearPlans.ViewModels.Buildings.ViewModels;
using FiveYearPlans.ViewModels.Recipes;
using FiveYearPlans.ViewModels.Resources;
using NodeEditor.Model;
using NodeEditor.Mvvm;

namespace NodeEditorDemo.Services;

public class NodeFactory : INodeFactory
{
    internal static INode CreateMiner(double x, double y, double width = 270, double height = 90, double pinSize = 10,
        string name = "MINER")
    {
        var node = new NodeViewModel
        {
            Name = name,
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new MinerViewModel()
        };

        node.AddPin(width, height / 2, pinSize, pinSize, PinAlignment.Right, $"R-{Guid.NewGuid()}");

        return node;
    }

    internal static INode CreateConstructor(double x, double y, double width = 270, double height = 90, double pinSize = 10,
        string name = "CONSTRUCTOR")
    {
        var node = new NodeViewModel
        {
            Name = name,
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new ConstructorViewModel()
        };

        node.AddPin(width, height / 2, pinSize, pinSize, PinAlignment.Right, $"R-{Guid.NewGuid()}");
        node.AddPin(0, height / 2, pinSize, pinSize, PinAlignment.Left, $"L-{Guid.NewGuid()}");

        return node;
    }
    
    private static INode CreateSmelter(double x, double y, double width = 270, double height = 90, double pinSize = 10,
        string name = "SMELTER")
    {
        var node = new NodeViewModel
        {
            Name = name,
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new SmelterViewModel()
        };

        node.AddPin(width, height / 2, pinSize, pinSize, PinAlignment.Right, $"R-{Guid.NewGuid()}");
        node.AddPin(0, height / 2, pinSize, pinSize, PinAlignment.Left, $"L-{Guid.NewGuid()}");

        return node;
    }

    internal static INode CreateSplitter(double x, double y, double width = 270, double height = 90,
        double pinSize = 10, string name = "SPLITTER")
    {
        var node = new NodeViewModel
        {
            Name = name,
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new SplitterViewModel()
        };

        node.AddPin(width, height / 2, pinSize, pinSize, PinAlignment.Right, $"R-{Guid.NewGuid()}");
        node.AddPin(width, height / 2 + 8, pinSize, pinSize, PinAlignment.Right, $"R-{Guid.NewGuid()}");
        node.AddPin(width, height / 2 + 16, pinSize, pinSize, PinAlignment.Right, $"R-{Guid.NewGuid()}");
        node.AddPin(0, height / 2, pinSize, pinSize, PinAlignment.Left, $"L-{Guid.NewGuid()}");

        return node;
    }
    
    internal static INode CreateMerger(double x, double y, double width = 270, double height = 90,
        double pinSize = 10, string name = "MERGER")
    {
        var node = new NodeViewModel
        {
            Name = name,
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = new MergerViewModel()
        };

        node.AddPin(0, height / 2, pinSize, pinSize, PinAlignment.Left, $"L-{Guid.NewGuid()}");
        node.AddPin(0, height / 2 + 8, pinSize, pinSize, PinAlignment.Left, $"L-{Guid.NewGuid()}");
        node.AddPin(0, height / 2 + 16, pinSize, pinSize, PinAlignment.Left, $"L-{Guid.NewGuid()}");
        node.AddPin(width, height / 2, pinSize, pinSize, PinAlignment.Right, $"R-{Guid.NewGuid()}");

        return node;
    }

    public IDrawingNode CreateDrawing(string? name = null)
    {
        var drawing = new DrawingNodeViewModel
        {
            Name = name,
            X = 0,
            Y = 0,
            Width = 900,
            Height = 600,
            Nodes = new ObservableCollection<INode>(),
            Connectors = new ObservableCollection<IConnector>(),
            Settings = new DrawingNodeSettingsViewModel() 
            {
                EnableMultiplePinConnections = false,
                EnableSnap = true,
                SnapX = 15.0,
                SnapY = 15.0,
                EnableGrid = true,
                GridCellWidth = 15.0,
                GridCellHeight = 15.0,
            }
        };

        return drawing;
    }

    public IList<INodeTemplate> CreateTemplates()
    {
        return new ObservableCollection<INodeTemplate>
        {
            new NodeTemplateViewModel
            {
                Title = "Constructor",
                Template = CreateConstructor(0, 0),
                Preview = CreateConstructor(0, 0)
            },
            new NodeTemplateViewModel
            {
                Title = "Smelter",
                Template = CreateSmelter(0, 0),
                Preview = CreateSmelter(0, 0)
            },
            new NodeTemplateViewModel
            {
                Title = "Splitter",
                Template = CreateSplitter(0, 0),
                Preview = CreateSplitter(0, 0)
            },
            new NodeTemplateViewModel
            {
                Title = "Merger",
                Template = CreateMerger(0, 0),
                Preview = CreateMerger(0, 0)
            },
            new NodeTemplateViewModel
            {
                Title = "Miner",
                Template = CreateMiner(0, 0),
                Preview = CreateMiner(0, 0)
            }
        };
    }

  
}