namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that add any kind of resource to the country.
    /// </summary>
    public class ResourceProductionEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceProductionEffectParser"/>.
        /// </summary>
        public ResourceProductionEffectParser()
            : base(KnownValues.ResourceProductionChange, (effect, country, context, builder, doApply) =>
            {
                long value = (long)effect.Value;
                var id = int.Parse(effect.Parameter);

                if (builder.ResourceProductions.ContainsKey(id))
                {
                    builder.ResourceProductions[id] += value;
                }
                else
                {
                    builder.ResourceProductions.Add(id, value);
                }
            })
        { }
    }
}