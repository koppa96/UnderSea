namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increase the barrack space (maximum unit limit) of a country.
    /// </summary>
    public class BarrackSpaceEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BarrackSpaceEffectParser"/>.
        /// </summary>
        public BarrackSpaceEffectParser()
            : base(KnownValues.BarrackSpaceChange, (effect, country, context, builder, doApply)
                  => builder.BarrackSpace += (int)effect.Value)
        { }
    }
}