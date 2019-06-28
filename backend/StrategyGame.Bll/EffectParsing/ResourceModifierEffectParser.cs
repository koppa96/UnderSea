namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increase the taxation (pearl production) of a country.
    /// </summary>
    public class ResourceModifierEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceModifierEffectParser"/>.
        /// </summary>
        public ResourceModifierEffectParser()
            : base(KnownValues.TaxationModifier, (effect, country, context, builder, doApply) =>
            {
                var id = int.Parse(effect.Parameter);

                if (builder.ResourceModifiers.ContainsKey(id))
                {
                    builder.ResourceModifiers[id] += effect.Value;
                }
                else
                {
                    builder.ResourceModifiers.Add(id, effect.Value);
                }
            })
        { }
    }
}