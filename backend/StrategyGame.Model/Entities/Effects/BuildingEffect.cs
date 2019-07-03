using StrategyGame.Model.Entities.Creations;

namespace StrategyGame.Model.Entities.Effects
{
    /// <summary>
    /// Represents the linking table between <see cref="BuildingType"/>s and effects.
    /// </summary>
    public class BuildingEffect : AbstractConnector<BuildingType, Effect>
    { }
}