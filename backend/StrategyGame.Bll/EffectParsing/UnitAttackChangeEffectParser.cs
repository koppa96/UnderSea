namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that modify unit attack by a set amount.
    /// </summary>
    public class UnitAttackChangeEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitAttackChangeEffectParser"/>.
        /// </summary>
        public UnitAttackChangeEffectParser()
            : base(KnownValues.UnitAttackChange, (effect, country, context, builder, doApply) =>
            builder.AttackIncrease += (int)effect.Value)
        { }
    }
}