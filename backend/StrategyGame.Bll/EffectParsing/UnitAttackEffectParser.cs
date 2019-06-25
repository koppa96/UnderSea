namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increase the attack of the units of a country.
    /// </summary>
    public class UnitAttackEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitAttackEffectParser"/>.
        /// </summary>
        public UnitAttackEffectParser()
            : base(KnownValues.UnitAttackModifier, (effect, country, context, builder, doApply) 
                  => builder.AttackModifier += effect.Value)
        { }
    }
}