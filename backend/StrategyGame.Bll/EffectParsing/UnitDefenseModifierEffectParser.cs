using System.Globalization;

namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increase the defense of the units of a country.
    /// </summary>
    public class UnitDefenseModifierEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitDefenseModifierEffectParser"/>.
        /// </summary>
        public UnitDefenseModifierEffectParser()
            : base(KnownValues.UnitDefenseModifier, (effect, country, context, builder, doApply)
                  => builder.DefenseModifier += double.Parse(effect.Parameter, CultureInfo.InvariantCulture))
        { }
    }
}