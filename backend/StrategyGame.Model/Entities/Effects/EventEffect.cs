namespace StrategyGame.Model.Entities.Effects
{
    /// <summary>
    /// Represents the linking table between <see cref="RandomEvent"/>s and effects.
    /// </summary>
    public class EventEffect : AbstractConnector<RandomEvent, Effect>
    { }
}