namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that modify unit attack by a set amount.
    /// </summary>
    public class IncreaseUnitAttackEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncreaseUnitAttackEffectParser"/>.
        /// </summary>
        public IncreaseUnitAttackEffectParser()
            : base(KnownValues.AddBuildingEffect, (effect, country, context, builder, doApply) =>
            builder.AttackIncrease += (int)effect.Value)
        { }
    }
}