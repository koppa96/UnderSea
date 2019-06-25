using StrategyGame.Model.Entities;
using System.Diagnostics;
using System.Linq;

namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that add a building to the country.
    /// </summary>
    public class AddBuildingEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoralProductionEffectParser"/>.
        /// </summary>
        public AddBuildingEffectParser()
            : base(KnownValues.AddBuildingEffect, (effect, country, context, builder, doApply) =>
            {
                if (doApply)
                {
                    int count = (int)effect.Value;

                    if (count > 0)
                    {
                        var existing = country.Buildings.SingleOrDefault(b => b.Building.Id == effect.TargetId);

                        if (existing == null)
                        {
                            country.Buildings.Add(new CountryBuilding
                            {
                                // TODO async effect parsing?
                                Building = context.BuildingTypes.Find(effect.TargetId),
                                Count = (int)effect.Value,
                            });
                        }
                        else
                        {
                            existing.Count += (int)effect.Value;
                        }
                    }
                    else if (count < 0)
                    {
                        if (country.Buildings.Any(b => b.Building.Id == effect.TargetId && b.Count > 0))
                        {
                            country.Buildings.Single(b => b.Building.Id == effect.TargetId).Count--;
                        }
                        else
                        {
                            builder.WasEventIgnored = true;
                            Debug.WriteLine("Effect with name {0} could not be applied to country {1} because the pre-requisites are not met.",
                                effect.Name, country.Name);
                        }
                    }
                }
            })
        { }
    }
}