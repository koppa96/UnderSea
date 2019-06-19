namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increase the taxation (pearl production) of a country.
    /// </summary>
    public class TaxModifierEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaxModifierEffectParser"/>.
        /// </summary>
        public TaxModifierEffectParser()
            : base("taxation-modifier", (effect, builder) => builder.TaxModifier += effect.Value)
        { }
    }
}