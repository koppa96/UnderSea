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
            : base(KnownValues.PopulationChange, (effect, country, context, builder, doApply) =>
            {
                var resources = effect.Parameter.Split(";");
                var pop = int.Parse(resources[0]);
                builder.Population += pop;

                for (int iii = 1; iii < resources.Length; iii++)
                {
                    var temp = resources[iii].Split(":");
                    var resId = int.Parse(temp[0]);
                    var amount = long.Parse(temp[1]);

                    if (builder.ResourceProductions.ContainsKey(resId))
                    {
                        builder.ResourceProductions[resId] += pop * amount;
                    }
                    else
                    {
                        builder.ResourceProductions.Add(resId, pop * amount);
                    }
                }
            })
        { }
    }
}