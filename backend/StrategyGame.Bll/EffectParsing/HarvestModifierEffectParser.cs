namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increase the harvest rates (coral production) of a country.
    /// </summary>
    public class HarvestModifierEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitAttackEffectParser"/>.
        /// </summary>
        public HarvestModifierEffectParser()
            : base(KnownValues.HarvestModifier, (effect, country, context, builder, doApply)
                  => builder.HarvestModifier += effect.Value)
        { }
    }
}