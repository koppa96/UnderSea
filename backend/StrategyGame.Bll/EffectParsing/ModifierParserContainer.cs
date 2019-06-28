using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Provides a container class for multiple <see cref="AbstractEffectModifierParser"/>s.
    /// </summary>
    /// <remarks>
    /// This class is partially thread safe. Running multiple parsing operations is safe as long as all underlying parsers are also thread safe.
    /// Adding a new parsers is not thread safe with neither another new parser addition, or parsing.
    /// </remarks>
    public class ModifierParserContainer
    {
        /// <summary>
        /// Creates a <see cref="ModifierParserContainer"/> with all known parsers.
        /// </summary>
        /// <returns>The created container.</returns>
        public static ModifierParserContainer CreateDefault()
        {
            return new ModifierParserContainer(new AbstractEffectModifierParser[]
                {
                    new BarrackSpaceEffectParser(),
                    new CoralProductionEffectParser(),
                    new PearlProductionEffectParser(),
                    new HarvestModifierEffectParser(),
                    new PopulationEffectParser(),
                    new TaxModifierEffectParser(),
                    new UnitDefenseEffectParser(),
                    new UnitAttackEffectParser(),
                    new AddBuildingEffectParser(),
                    new IncreaseUnitAttackEffectParser(),
                    new BuildingCoralProductionEffectParser()
                });
        }

        /// <summary>
        /// Writeable backing collection for the <see cref="Parsers"/> collection.
        /// </summary>
        protected List<AbstractEffectModifierParser> WriteableParsers { get; }

        /// <summary>
        /// Gets the collection of parsers currently in the <see cref="ModifierParserContainer"/>.
        /// </summary>
        public IReadOnlyCollection<AbstractEffectModifierParser> Parsers => new ReadOnlyCollection<AbstractEffectModifierParser>(WriteableParsers);

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifierParserContainer"/>.
        /// </summary>
        public ModifierParserContainer()
        {
            WriteableParsers = new List<AbstractEffectModifierParser>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifierParserContainer"/>, with parsers specified.
        /// </summary>
        /// <param name="Parsers">The collection of parsers for the container.</param>
        /// <exception cref="ArgumentNullException"> Thrown if an argument was null.</exception>
        public ModifierParserContainer(IEnumerable<AbstractEffectModifierParser> Parsers)
        {
            WriteableParsers = Parsers?.ToList() ?? throw new ArgumentNullException(nameof(Parsers));
        }

        /// <summary>
        /// Adds a new parser to the container.
        /// </summary>
        /// <param name="parser">The parser to add.</param>
        /// <exception cref="ArgumentNullException"> Thrown if an argument was null.</exception>
        public void AddNewParser(AbstractEffectModifierParser parser)
        {
            WriteableParsers.Add(parser ?? throw new ArgumentNullException(nameof(parser)));
        }

        /// <summary>
        /// Tries to parse the provided <see cref="Effect"/> with the parsers in the container.
        /// </summary>
        /// <param name="effect">The <see cref="Effect"/> to parse.</param>
        /// <param name="country">The country to parse the effect for.</param>
        /// <param name="context">The database to use.</param>
        /// <param name="builder">The <see cref="CountryModifierBuilder"/> to store the effect's effects in.</param>
        /// <param name="doApplyPermanent">If effects that have permanenet effects should be applied.</param>
        /// <returns>If the effect was parsed by any parsers.</returns>
        /// <exception cref="ArgumentNullException"> Thrown if an argument was null.</exception>
        /// <remarks>
        /// If multiple parsers match the effect only the first matching parser is executed.
        /// </remarks>
        public bool TryParse(Effect effect, Country country, UnderSeaDatabaseContext context, CountryModifierBuilder builder,
            bool doApplyPermanent)
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

            foreach (var p in Parsers)
            {
                if (p.TryParse(effect, country, context, builder, doApplyPermanent))
                {
                    return true;
                }
            }

            return false;
        }
    }
}