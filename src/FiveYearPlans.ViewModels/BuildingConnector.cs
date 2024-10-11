using FiveYearPlans.ViewModels.Buildings;
using FiveYearPlans.ViewModels.Buildings.Abstractions;
using FiveYearPlans.ViewModels.Buildings.Exceptions;
using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels;

public class BuildingConnector
{
    private readonly IBuildingContextProvider buildingContextProvider;

    public BuildingConnector(IBuildingContextProvider buildingContextProvider)
    {
        this.buildingContextProvider = buildingContextProvider;
    }

    public void ConnectBuildings(uint outputIndex, uint inputIndex, InputBuilding inputBuilding,
        OutputBuilding outputBuilding)
    {
        FillProvider(inputBuilding, outputBuilding);

        if (outputBuilding is DynamicFlowBuilding dynamicFlowsOutputBuilding)
        {
            dynamicFlowsOutputBuilding.RecomputeOutput(buildingContextProvider);
        }

        inputBuilding.InputResourceFlows[inputIndex] =
            outputBuilding.OutPutResourceFlows[outputIndex];
        if (inputBuilding is OneToOneMappingBuilding oneToOneMappingInputBuilding)
        {
            if (oneToOneMappingInputBuilding.Recipe?.IngredientFlow.Resource !=
                outputBuilding.OutPutResourceFlows[outputIndex].Resource)
            {
                throw new InvalidConnectionException(outputBuilding.OutPutResourceFlows[outputIndex].Resource, oneToOneMappingInputBuilding.Recipe?.IngredientFlow.Resource);
            }
        }

        if (outputBuilding is DynamicFlowBuilding otherDynamicFlowsOutputBuilding)
        {
            otherDynamicFlowsOutputBuilding.RecomputeOutput(buildingContextProvider);
        }
        else if (outputBuilding is Building simpleOutputBuilding)
        {
            simpleOutputBuilding.RecomputeChildren(
                buildingContextProvider,
                buildingContextProvider.GetOutputConnectionState(simpleOutputBuilding.Id).ToArray());
        }

        if (inputBuilding is DynamicFlowBuilding dynamicFlowsInputBuilding)
        {
            dynamicFlowsInputBuilding.RecomputeOutput(buildingContextProvider);
        }
        else if (inputBuilding is Building simpleInputBuilding)
        {
            simpleInputBuilding.RecomputeChildren(
                buildingContextProvider,
                buildingContextProvider.GetOutputConnectionState(simpleInputBuilding.Id).ToArray());
        }
    }

    private void FillProvider(InputBuilding inputBuilding, OutputBuilding outputBuilding)
    {
        if (inputBuilding is Building simpleInputBuilding)
        {
            simpleInputBuilding.buildingsProvider = buildingContextProvider;
        }

        if (outputBuilding is Building simpleOutputBuilding)
        {
            simpleOutputBuilding.buildingsProvider = buildingContextProvider;
        }
    }

    public void DisconnectBuilding(uint outputIndex, uint inputIndex, InputBuilding inputBuilding,
        OutputBuilding outputBuilding)
    {
        inputBuilding.InputResourceFlows.Remove(inputIndex);

        if (outputBuilding is DynamicFlowBuilding otherDynamicFlowsOutputBuilding)
        {
            otherDynamicFlowsOutputBuilding.RecomputeOutput(buildingContextProvider);
        }

        if (inputBuilding is DynamicFlowBuilding dynamicFlowsInputBuilding)
        {
            dynamicFlowsInputBuilding.RecomputeOutput(buildingContextProvider);
        }
    }
}