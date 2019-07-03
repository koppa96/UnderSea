using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    var globals = context.GlobalValues.Single();

                    
                }
            })
        { }
    }
}