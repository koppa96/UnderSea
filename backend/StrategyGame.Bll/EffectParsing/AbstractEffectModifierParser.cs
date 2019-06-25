using StrategyGame.Bll.Services.TurnHandling;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;

namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Provides a base class for effect parsers that apply modifications to a <see cref="CountryModifierBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Implementors of this class are expected to provide thread safety for parsing operations.
    /// </remarks>
    public abstract class AbstractEffectModifierParser
    {
        /// <summary>
        /// Gets the name of the <see cref="Effect"/> that can be handled by the parser.
        /// </summary>
        public string HandledEffectName { get; }
        
        /// <summary>
        /// Gets the action run by the <see cref="TryParse(Effect, CountryModifierBuilder)"/> when the effect's name matches the <see cref="HandledEffectName"/>.
        /// </summary>
        protected Action<Effect, Country, UnderSeaDatabaseContext, CountryModifierBuilder, bool> OnParse { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractEffectModifierParser"/>.
        /// </summary>
        /// <param name="handledEffectName">The name of the effect that can be handled by the parser.</param>
        /// <param name="onParse">The action run by the <see cref="TryParse(Effect, CountryModifierBuilder)"/> method when the effect's name matches the <paramref name="handledEffectName"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        /// <remarks>
        /// <paramref name="onParse"/> may be null, however in that case the <see cref="TryParse(Effect, CountryModifierBuilder)"/>
        /// method must be overridden in the inheriting class, otherwise a <see cref="NullReferenceException"/> will occur.
        /// </remarks>
        protected AbstractEffectModifierParser(string handledEffectName,
            Action<Effect, Country, UnderSeaDatabaseContext, CountryModifierBuilder, bool> onParse)
        {
            HandledEffectName = handledEffectName ?? throw new ArgumentNullException(nameof(handledEffectName));
            OnParse = onParse;
        }

        /// <summary>
        /// Attempts to parse an <see cref="Effect"/>, and add its modifier to the provided <see cref="CountryModifierBuilder"/>.
        /// </summary>
        /// <param name="effect">The <see cref="Effect"/> to parse.</param>
        /// <param name="country">The country to parse the effect for.</param>
        /// <param name="context">The database to use.</param>
        /// <param name="builder">The <see cref="CountryModifierBuilder"/> to add modifiers to.</param>
        /// <param name="doApplyPermanent">If effects that have permanenet effects should be applied.</param>
        /// <returns>If the effect was parsed by the parser.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public virtual bool TryParse(Effect effect, Country country, UnderSeaDatabaseContext context,
            CountryModifierBuilder builder, bool doApplyPermanent)
        {
            if (effect == null)
            {
                throw new ArgumentNullException(nameof(effect));
            }

            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (effect.Name.Equals(HandledEffectName, StringComparison.OrdinalIgnoreCase))
            {
                OnParse(effect, country, context, builder, doApplyPermanent);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}