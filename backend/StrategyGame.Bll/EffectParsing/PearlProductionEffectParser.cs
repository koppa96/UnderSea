namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increases the pearl production of a country.
    /// </summary>
    public class PearlProductionEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PearlProductionEffectParser"/>.
        /// </summary>
        public PearlProductionEffectParser()
            : base(KnownValues.PearlProductionIncrease, (effect, country, context, builder, doApply)
                  => builder.PearlProduction += (int)effect.Value)
        { }
    }
}
