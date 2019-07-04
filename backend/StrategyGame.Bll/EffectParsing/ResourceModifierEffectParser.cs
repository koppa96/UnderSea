using System.Globalization;

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
            : base(KnownValues.ResourceProductionModifier, (effect, country, context, builder, doApply) =>
            {
                var temp = effect.Parameter.Split(";");
                var id = int.Parse(temp[0]);

                if (builder.ResourceModifiers.ContainsKey(id))
                {
                    builder.ResourceModifiers[id] += double.Parse(temp[1], CultureInfo.InvariantCulture);
                }
                else
                {
                    builder.ResourceModifiers.Add(id, double.Parse(temp[1], CultureInfo.InvariantCulture));
                }
            })
        { }
    }
}