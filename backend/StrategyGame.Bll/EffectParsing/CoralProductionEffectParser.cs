namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increases the coral production of a country.
    /// </summary>
    public class CoralProductionEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoralProductionEffectParser"/>.
        /// </summary>
        public CoralProductionEffectParser()
            : base(KnownValues.CoralProductionIncrease, (effect, country, context, builder, doApply)
                  => builder.CoralProduction += (int)effect.Value)
        { }
    }
}