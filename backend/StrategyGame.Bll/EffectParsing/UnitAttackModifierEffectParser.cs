using System.Globalization;

namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increase the attack of the units of a country.
    /// </summary>
    public class UnitAttackModifierEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitAttackModifierEffectParser"/>.
        /// </summary>
        public UnitAttackModifierEffectParser()
            : base(KnownValues.UnitAttackModifier, (effect, country, context, builder, doApply)
                  => builder.AttackModifier += double.Parse(effect.Parameter, CultureInfo.InvariantCulture))
        { }
    }
}