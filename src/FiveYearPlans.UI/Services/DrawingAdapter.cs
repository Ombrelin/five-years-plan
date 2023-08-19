using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FiveYearPlans.ViewModels;
using FiveYearPlans.ViewModels.Buildings;
using NodeEditor.Model;
using NodeEditor.Mvvm;

namespace NodeEditorDemo.Services;

internal class DrawingAdapter : IBuildingContextProvider
{
    public IDrawingNode Drawing { get; }

    public DrawingAdapter()
    {
        Drawing = CreateDemoDrawing();
    }

    private IDrawingNode CreateDemoDrawing()
    {
        var connectors = new ObservableCollection<IConnector>();
        var drawing = new DrawingNodeViewModel
        {
            X = 0,
            Y = 0,
            Width = 900,
            Height = 600,
            Nodes = new ObservableCollection<INode>(),
            Connectors = connectors,
            EnableMultiplePinConnections = false,
            EnableSnap = true,
            SnapX = 15.0,
            SnapY = 15.0,
            EnableGrid = true,
            GridCellWidth = 15.0,
            GridCellHeight = 15.0,




        };

        connectors.CollectionChanged += (e, a) =>
        {
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
                };
            }
        };

        return drawing;
    }
    
    


    private static uint ComputeInputIndex(ConnectorViewModel newConnector) =>
        (uint)newConnector
            .End
            .Parent
            .Pins
            .Where(pin => pin.Alignment is PinAlignment.Left)
            .ToList()
            .IndexOf(newConnector.End);

    private static uint ComputeOutputIndex(ConnectorViewModel newConnector) =>
        (uint)newConnector
            .Start
            .Parent
            .Pins
            .Where(pin => pin.Alignment is PinAlignment.Right)
            .ToList()
            .IndexOf(newConnector.Start);

    public IReadOnlyDictionary<uint, DynamicFlowBuilding?> GetOutputConnectionState(Guid id)
    {
        var pins = Drawing
            .Nodes
            .First(node => (node.Content as Building).Id == id)
            .Pins;
        return GetOutputConnectionState(pins);
    }

    private IReadOnlyDictionary<uint, DynamicFlowBuilding?> GetOutputConnectionState(IEnumerable<IPin> pins) =>
        pins
            .Where(pin => pin.Alignment is PinAlignment.Right)
            .Select((pin, index) => (Index: (uint)index, Building: GetBuilding(pin)))
            .ToDictionary(kvp => kvp.Item1, kvp => kvp.Item2);
    
    private DynamicFlowBuilding? GetBuilding(IPin pin)
    {
        IConnector? firstOrDefault = Drawing.Connectors.FirstOrDefault(connector => connector.Start == pin);

        if (firstOrDefault is null)
        {
            return null;
        }
        
        return firstOrDefault.End.Parent
            .Content as DynamicFlowBuilding;
    }

}