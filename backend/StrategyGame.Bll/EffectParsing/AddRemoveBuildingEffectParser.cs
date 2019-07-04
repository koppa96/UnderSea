using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Creations;
using System.Diagnostics;
using System.Linq;

namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that add a building to the country.
    /// </summary>
    public class AddRemoveBuildingEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddRemoveBuildingEffectParser"/>.
        /// </summary>
        public AddRemoveBuildingEffectParser()
            : base(KnownValues.AddRemoveBuildingEffect, (effect, country, context, builder, doApply) =>
            {
                if (doApply)
                {
                    int count = (int)effect.Value;
                    var id = int.Parse(effect.Parameter);

                    if (count > 0)
                    {
                        var existing = country.Buildings.SingleOrDefault(b => b.Child.Id == id);

                        if (existing == null)
                        {
                            var bld = new AbstractConnectorWithAmount<Country, BuildingType>
                            {
                                // TODO async effect parsing?
                                Child = context.BuildingTypes.Find(id),
                                Amount = (int)effect.Value,
                            };
                            country.Buildings.Add(bld);
                            context.CountryBuildings.Add(bld);
                        }
                        else
                        {
                            existing.Amount += (int)effect.Value;
                        }
                    }
                    else if (count < 0)
                    {
                        if (country.Buildings.Any(b => b.Child.Id == id && b.Amount > 0))
                        {
                            country.Buildings.Single(b => b.Child.Id == id).Amount--;
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