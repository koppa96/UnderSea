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
                var temp = effect.Parameter.Split(";");
                var id = int.Parse(temp[0]);

                if (builder.ResourceProductions.ContainsKey(id))
                {
                    builder.ResourceProductions[id] += long.Parse(temp[1]);
                }
                else
                {
                    builder.ResourceProductions.Add(id, long.Parse(temp[1]));
                }
            })
        { }
    }
}