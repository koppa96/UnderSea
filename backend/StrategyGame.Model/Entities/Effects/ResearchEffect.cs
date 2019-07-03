using StrategyGame.Model.Entities.Creations;

namespace StrategyGame.Model.Entities.Effects
{
    /// <summary>
    /// Represents the linking table between <see cref="ResearchType"/>s and effects.
    /// </summary>
    public class ResearchEffect : AbstractConnector<ResearchType, Effect>
    { }
}