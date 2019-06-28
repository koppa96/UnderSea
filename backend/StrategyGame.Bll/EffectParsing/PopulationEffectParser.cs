namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increase the population of a country.
    /// </summary>
    public class PopulationEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopulationEffectParser"/>.
        /// </summary>
        public PopulationEffectParser()
            : base(KnownValues.PopulationIncrease, (effect, country, context, builder, doApply) =>
            {
                builder.Population += (int)effect.Value;
                var resources = effect.Parameter.Split(";");

                foreach (var res in resources)
                {
                    var temp = res.Split(":");
                    var resId = int.Parse(temp[0]);
                    var amount = long.Parse(temp[1]);

                    if (builder.ResourceProductions.ContainsKey(resId))
                    {
                        builder.ResourceProductions[resId] += amount;
                    }
                    else
                    {
                        builder.ResourceProductions.Add(resId, amount);
                    }
                }
            })
        { }
    }
}