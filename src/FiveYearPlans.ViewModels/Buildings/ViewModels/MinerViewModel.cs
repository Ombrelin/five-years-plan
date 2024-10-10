using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using FiveYearPlans.ViewModels.Buildings.Abstractions;
using FiveYearPlans.ViewModels.Recipes;
using FiveYearPlans.ViewModels.Resources;

namespace FiveYearPlans.ViewModels.Buildings.ViewModels;

[ObservableObject]
public partial class MinerViewModel : Building, OutputBuilding
{
    [ObservableProperty] private ResourceFlow outPutResourceFlow;
    [ObservableProperty] private Resource resource;
    [ObservableProperty] private ResourceDepositPurity resourceDepositPurity = ResourceDepositPurity.Impure;
    [ObservableProperty] private MinerTier tier = MinerTier.Mk1;

    public List<Resource> PossibleResources { get; } = Enum
        .GetValues(typeof(Resource))
        .Cast<Resource>()
        .ToList();
    
    public List<ResourceDepositPurity> PossibleResourceDepositPurity { get; } = Enum
        .GetValues(typeof(ResourceDepositPurity))
        .Cast<ResourceDepositPurity>()
        .ToList();
    
    public List<MinerTier> PossibleMinerTiers { get; } = Enum
        .GetValues(typeof(MinerTier))
        .Cast<MinerTier>()
        .ToList();

    [JsonConstructor]
    public MinerViewModel()
    {
        PropertyChanged += OnPropertyChangedEventHandler;
    }

    private void OnPropertyChangedEventHandler(object? _, PropertyChangedEventArgs args)
    {
        this.OutPutResourceFlow = new ResourceFlow(Resource, ResourceOutput);
        if (buildingsProvider is not null)
        {
            RecomputeChildren(buildingsProvider, buildingsProvider.GetOutputConnectionState(Id).ToArray());
        }
    }

    /// <summary>
    /// Source : https://satisfactory.wiki.gg/wiki/Miner#Mining_speed
    /// </summary>
    private decimal ResourceOutput => (PurityModifier) * 1 /*(Overclock percentage) / 100 : not supported yet*/  * (MiningSpeed);

    private decimal PurityModifier => ResourceDepositPurity switch
    {
        ResourceDepositPurity.Impure => .5m,
        ResourceDepositPurity.Normal => 1,
        ResourceDepositPurity.Pure => 2,
        _ => throw new ArgumentOutOfRangeException()
    };

    private uint MiningSpeed => Tier switch
    {
        MinerTier.Mk1 => 60,
        MinerTier.Mk2 => 120,
        MinerTier.Mk3 => 240,
        _ => throw new ArgumentOutOfRangeException()
    };
    
    public override Dictionary<uint, ResourceFlow> OutPutResourceFlows => new()
    {
        [0] = outPutResourceFlow
    };

    public enum MinerTier
    {
        Mk1,
        Mk2,
        Mk3
    }
}