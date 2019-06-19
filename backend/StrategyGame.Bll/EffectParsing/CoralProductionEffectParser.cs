namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increase the coral production of a country.
    /// </summary>
    public class CoralProductionEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoralProductionEffectParser"/>.
        /// </summary>
        public CoralProductionEffectParser()
            : base("coral-production", (effect, builder) => builder.CoralProduction += (int)effect.Value)
        { }
    }
}