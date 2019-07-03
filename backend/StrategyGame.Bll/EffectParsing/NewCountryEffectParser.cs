using StrategyGame.Bll.Extensions;
using System.Linq;

namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Represents a parser that can parse effects that add a building to the country.
    /// </summary>
    public class NewCountryEffectParser : AbstractEffectModifierParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewCountryEffectParser"/>.
        /// </summary>
        public NewCountryEffectParser()
            : base(KnownValues.NewCountryEffect, (effect, country, context, builder, doApply) =>
            {
                if (doApply)
                {
                    // TODO: async effect parsing?
                    country.ParentUser.AddNewCountry(country.ParentUser.UserName, context).Wait();
                }
            })
        { }
    }
}