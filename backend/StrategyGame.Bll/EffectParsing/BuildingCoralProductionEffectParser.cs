using System.Linq;

namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that increases the coral production of a country for every building of a type.
    /// </summary>
    public class BuildingCoralProductionEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingCoralProductionEffectParser"/>.
        /// </summary>
        public BuildingCoralProductionEffectParser()
            : base(KnownValues.BuildingProductionIncrease, (effect, country, context, builder, doApply) =>
                builder.CoralProduction +=
                    country.Buildings.Count(b => b.Building.Id == effect.TargetId) * (int)effect.Value)
        { }
    }
}