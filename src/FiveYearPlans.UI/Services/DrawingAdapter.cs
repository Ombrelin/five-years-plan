using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FiveYearPlans.ViewModels;
using FiveYearPlans.ViewModels.Buildings;
using FiveYearPlans.ViewModels.Buildings.Abstractions;
using Microsoft.Extensions.Logging;
using NodeEditor.Model;
using NodeEditor.Mvvm;

namespace NodeEditorDemo.Services;

internal class DrawingAdapter : IBuildingContextProvider
{
    public IDrawingNode Drawing { get; }
    private readonly ILogger<DrawingAdapter>? logger;

    public DrawingAdapter(ILogger<DrawingAdapter>? logger)
    {
        this.logger = logger;
        Drawing = CreateDemoDrawing();
    }

    private IDrawingNode CreateDemoDrawing()
    {
        var connectors = new ObservableCollection<IConnector>();
        var drawing = new DrawingNodeViewModel
        {
            X = 0,
            Y = 0,
            Width = 1280,
            Height = 720,
            Nodes = new ObservableCollection<INode>(),
            Connectors = connectors,
            Settings = new DrawingNodeSettingsViewModel()
            {
                EnableMultiplePinConnections = false,
                EnableSnap = true,
                SnapX = 15.0,
                SnapY = 15.0,
                EnableGrid = true,
                GridCellWidth = 15.0,
                GridCellHeight = 15.0
            }
        };

        connectors.CollectionChanged += (e, a) =>
        {
            try
            {
                if (a.OldItems?.Count > 0 && a.OldItems[0] is ConnectorViewModel deletedConnector)
                {
                    Disconnect(deletedConnector);
                    return;
                }

                if (a.NewItems is null)
                {
                    return;
                }

                if (a.NewItems[0] is ConnectorViewModel newConnector)
                {
                    newConnector.PropertyChanged += (t, v) =>
                    {
                        if (v.PropertyName is not "End")
                        {
                            return;
                        }

                        Connect(newConnector);
                    };
                }
            }
            catch (Exception? exception)
            {
                logger.LogError(exception, "Action error");
            }
        };

        return drawing;
    }

    private void Disconnect(ConnectorViewModel newConnector)
    {
        if (newConnector.Start?.Parent?.Content is OutputBuilding outputBuilding &&
            newConnector.End?.Parent?.Content is InputBuilding inputBuilding)
        {
            new BuildingConnector(this).DisconnectBuilding(
                ComputeOutputIndex(newConnector),
                ComputeInputIndex(newConnector),
                inputBuilding,
                outputBuilding
            );
        }
    }

    private static bool IsDisconnect(PropertyChangedEventArgs v, ConnectorViewModel newConnector) =>
        v.PropertyName is "Start" or "End" && (newConnector.Start is null || newConnector.End is null);

    private void Connect(ConnectorViewModel newConnector)
    {
        if (newConnector.Start?.Parent?.Content is OutputBuilding outputBuilding &&
            newConnector.End?.Parent?.Content is InputBuilding inputBuilding)
        {
            new BuildingConnector(this).ConnectBuildings(
                ComputeOutputIndex(newConnector),
                ComputeInputIndex(newConnector),
                inputBuilding,
                outputBuilding
            );
        }
    }


    private static uint ComputeInputIndex(ConnectorViewModel newConnector) =>
        (uint)newConnector
            .End
            .Parent
            .Pins
            .Where(pin => pin.Alignment is PinAlignment.Left)
            .ToList()
            .IndexOf(newConnector.End);

    private static uint ComputeOutputIndex(ConnectorViewModel newConnector)
    {
        var index = newConnector
            .Start
            .Parent
            .Pins
            .Where(pin => pin.Alignment is PinAlignment.Right)
            .ToList()
            .IndexOf(newConnector.Start);

        if (index == -1)
        {
            throw new InvalidOperationException($"Ouput index for {newConnector.Start.Parent.Name} was not found");
        }
        
        return (uint)index;
    }

    public IReadOnlyDictionary<uint, Building?> GetOutputConnectionState(Guid id)
    {
        var pins = Drawing
            .Nodes
            .First(node => (node.Content as Building).Id == id)
            .Pins;
        return GetOutputConnectionState(pins);
    }
    
    public IReadOnlyDictionary<uint, Building?> GetInputConnectionState(Guid id)=>GetInputConnectionState(Drawing
            .Nodes
            .First(node => (node.Content as Building).Id == id)
            .Pins);

    public IReadOnlyDictionary<uint, Building?> GetInputConnectionState(IEnumerable<IPin> pins)=>
        pins
            .Where(pin => pin.Alignment is PinAlignment.Left)
            .Select((pin, index) => (Index: (uint)index, Building: GetInputBuilding(pin)))
            .ToDictionary(kvp => kvp.Item1, kvp => kvp.Item2);
    

    private IReadOnlyDictionary<uint, Building?> GetOutputConnectionState(IEnumerable<IPin> pins) =>
        pins
            .Where(pin => pin.Alignment is PinAlignment.Right)
            .Select((pin, index) => (Index: (uint)index, Building: GetOutputBuilding(pin)))
            .ToDictionary(kvp => kvp.Item1, kvp => kvp.Item2);

    private Building? GetOutputBuilding(IPin pin)
    {
        IConnector? firstOrDefault = Drawing.Connectors.FirstOrDefault(connector => connector.Start == pin);

        if (firstOrDefault is null)
        {
            return null;
        }

        return firstOrDefault.End.Parent
            .Content as Building;
    }
    
    private Building? GetInputBuilding(IPin pin)
    {
        IConnector? firstOrDefault = Drawing.Connectors.FirstOrDefault(connector => connector.End == pin);

        if (firstOrDefault is null)
        {
            return null;
        }

        return firstOrDefault.Start.Parent
            .Content as Building;
    }
}