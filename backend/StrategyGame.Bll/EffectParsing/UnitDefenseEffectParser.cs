namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increase the defense of the units of a country.
    /// </summary>
    public class UnitDefenseEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitDefenseEffectParser"/>.
        /// </summary>
        public UnitDefenseEffectParser()
            : base("unit-defense", (effect, builder) => builder.DefenseModifier += effect.Value)
        { }
    }
}