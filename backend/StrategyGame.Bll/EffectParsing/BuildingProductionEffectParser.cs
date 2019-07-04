using System.Linq;

namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increases the coral production of a country for every building of a type.
    /// </summary>
    public class BuildingProductionEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingProductionEffectParser"/>.
        /// </summary>
        public BuildingProductionEffectParser()
            : base(KnownValues.BuildingProductionChange, (effect, country, context, builder, doApply) =>
            {
                var split = effect.Parameter.Split(";");
                int buildingId = int.Parse(split[0]);
                int resourceId = int.Parse(split[1]);
                var resourceAmount = country.Buildings.Count(b => b.Child.Id == buildingId) * int.Parse(split[2]);

                if (builder.ResourceProductions.ContainsKey(resourceId))
                {
                    builder.ResourceProductions[resourceId] += resourceAmount;
                }
                else
                {
                    builder.ResourceProductions.Add(resourceId, resourceAmount);
                }
            })
        { }
    }
}