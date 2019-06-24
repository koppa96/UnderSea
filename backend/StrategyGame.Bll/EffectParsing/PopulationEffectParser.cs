namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increase the population of a country.
    /// </summary>
    public class PopulationEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopulationEffectParser"/>.
        /// </summary>
        public PopulationEffectParser()
            : base(KnownValues.PopulationIncrease, (effect, builder) => builder.Population += (int)effect.Value)
        { }
    }
}