using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Frontend;
using StrategyGame.Model.Entities.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Dal
{
    /// <summary>
    /// Represents a  database context towards a UnderSea database.
    /// </summary>
    /// <remarks>
    public class UnderSeaDatabaseContext : IdentityDbContext<User>
    {
        #region DbSets

        /// <summary>
        /// Gets the collection of <see cref="Country"/> in the database.
        /// </summary>
        public DbSet<Country> Countries { get; }

        /// <summary>
        /// Gets the collection of <see cref="RandomEvent"/> in the database.
        /// </summary>
        public DbSet<RandomEvent> RandomEvents { get; }

        /// <summary>
        /// Gets the collection of <see cref="BuildingType"/> in the database.
        /// </summary>
        public DbSet<BuildingType> BuildingTypes { get; }

        /// <summary>
        /// Gets the collection of <see cref="ResearchType"/> in the database.
        /// </summary>
        public DbSet<ResearchType> ResearchTypes { get; }

        /// <summary>
        /// Gets the collection of <see cref="UnitType"/> in the database.
        /// </summary>
        public DbSet<UnitType> UnitTypes { get; }

        /// <summary>
        /// Gets the collection of <see cref="Command"/> in the database.
        /// </summary>
        public DbSet<Command> Commands { get; }

        /// <summary>
        /// Gets the collection of <see cref="Division"/> in the database.
        /// </summary>
        public DbSet<Division> Divisions { get; }

        /// <summary>
        /// Gets the collection of <see cref="CountryResearch"/> in the database.
        /// </summary>
        public DbSet<CountryResearch> CountryResearches { get; }

        /// <summary>
        /// Gets the collection of <see cref="CountryBuilding"/> in the database.
        /// </summary>
        public DbSet<CountryBuilding> CountryBuildings { get; }

        /// <summary>
        /// Gets the collection of <see cref="InProgressBuilding"/> in the database.
        /// </summary>
        public DbSet<InProgressBuilding> InProgressBuildings { get; }

        /// <summary>
        /// Gets the collection of <see cref="InProgressResearch"/> in the database.
        /// </summary>
        public DbSet<InProgressResearch> InProgressResearches { get; }

        /// <summary>
        /// Gets the collection of <see cref="BuildingEffect"/> in the database.
        /// </summary>
        public DbSet<BuildingEffect> BuildingEffects { get; }

        /// <summary>
        /// Gets the collection of <see cref="ResearchEffect"/> in the database.
        /// </summary>
        public DbSet<ResearchEffect> ResearchEffects { get; }

        /// <summary>
        /// Gets the collection of <see cref="EventEffect"/> in the database.
        /// </summary>
        public DbSet<EventEffect> EventEffects { get; }

        /// <summary>
        /// Gets the collection of <see cref="Effect"/> in the database.
        /// </summary>
        public DbSet<Effect> Effects { get; }

        /// <summary>
        /// Gets the collection of <see cref="Effect"/> in the database.
        /// </summary>
        public DbSet<CombatReport> Reports { get; }

        /// <summary>
        /// Gets the collection of <see cref="GlobalValue"/> in the database.
        /// </summary>
        public DbSet<GlobalValue> GlobalValues { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="BuildingContent"/> in the database.
        /// </summary>
        public DbSet<BuildingContent> BuildingContents { get; }

        /// <summary>
        /// Gets the collection of <see cref="ResearchContent"/> in the database.
        /// </summary>
        public DbSet<ResearchContent> ResearchContents { get; }

        /// <summary>
        /// Gets the collection of <see cref="UnitContent"/> in the database.
        /// </summary>
        public DbSet<UnitContent> UnitContents { get; }

        /// <summary>
        /// Gets the collection of <see cref="EventContent"/> in the database.
        /// </summary>
        public DbSet<EventContent> EventContents { get; }

        public DbSet<ExceptionLog> ExceptionLogs { get; set; }

        public DbSet<RequestLog> RequestLogs { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="UnderSeaDatabaseContext"/>.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions{UnderSeaDatabaseContext}"/> for the database.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public UnderSeaDatabaseContext(DbContextOptions<UnderSeaDatabaseContext> options)
            : base(options)
        {
            Countries = Set<Country>();
            BuildingTypes = Set<BuildingType>();
            ResearchTypes = Set<ResearchType>();
            UnitTypes = Set<UnitType>();
            Commands = Set<Command>();
            Divisions = Set<Division>();
            CountryResearches = Set<CountryResearch>();
            CountryBuildings = Set<CountryBuilding>();
            InProgressBuildings = Set<InProgressBuilding>();
            InProgressResearches = Set<InProgressResearch>();
            BuildingEffects = Set<BuildingEffect>();
            ResearchEffects = Set<ResearchEffect>();
            Effects = Set<Effect>();
            RandomEvents = Set<RandomEvent>();
            EventEffects = Set<EventEffect>();
            Reports = Set<CombatReport>();

            BuildingContents = Set<BuildingContent>();
            ResearchContents = Set<ResearchContent>();
            UnitContents = Set<UnitContent>();
            EventContents = Set<EventContent>();
            GlobalValues = Set<GlobalValue>();
            ExceptionLogs = Set<ExceptionLog>();
            RequestLogs = Set<RequestLog>();
        }

        /// <summary>
        /// Configures the model of the database.
        /// </summary>
        /// <param name="Builder">The <see cref="ModelBuilder"/> to use.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Combat report
            builder.Entity<CombatReport>()
                .HasOne(c => c.Attacker)
                .WithMany(c => c.Attacks);

            builder.Entity<CombatReport>()
                .HasOne(c => c.Defender)
                .WithMany(c => c.Defenses);

            // Effect
            builder.Entity<Effect>().Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Entity<Effect>().Property(e => e.TargetId).IsRequired(false);

            //Building - BuildingEffect - Effect
            builder.Entity<BuildingEffect>()
                .HasOne(be => be.Building)
                .WithMany(b => b.Effects);

            builder.Entity<BuildingEffect>()
                .HasOne(be => be.Effect)
                .WithMany(e => e.AffectedBuildings);

            //Research - ResearchEffect - Effect
            builder.Entity<ResearchEffect>()
                .HasOne(re => re.Research)
                .WithMany(r => r.Effects);

            builder.Entity<ResearchEffect>()
                .HasOne(re => re.Effect)
                .WithMany(e => e.AffectedResearches);

            // RandomEvent - EventEffect - Effect
            builder.Entity<EventEffect>()
                .HasOne(ee => ee.Event)
                .WithMany(e => e.Effects);

            builder.Entity<EventEffect>()
                .HasOne(ee => ee.Effect)
                .WithMany(e => e.AffectedEvents);

            // Country            
            builder.Entity<Country>().HasMany(x => x.Researches).WithOne(x => x.ParentCountry);
            builder.Entity<Country>().HasMany(x => x.Commands).WithOne(x => x.ParentCountry);

            builder.Entity<Country>()
                .HasOne(c => c.ParentUser)
                .WithMany(u => u.RuledCountries);

            builder.Entity<Country>()
                .HasOne(c => c.CurrentEvent)
                .WithMany(e => e.ParentCountries);

            // BuildingType - BuildingContent
            builder.Entity<BuildingType>()
                .HasOne(b => b.Content)
                .WithMany(c => c.Parents);
            
            // ResearchType - ResearchContent
            builder.Entity<ResearchType>()
                .HasOne(r => r.Content)
                .WithMany(c => c.Parents);
            
            // UnitType - UnitContent
            builder.Entity<UnitType>()
                .HasOne(u => u.Content)
                .WithMany(c => c.Parents);

            // Commands
            builder.Entity<Command>()
                .HasOne(c => c.ParentCountry)
                .WithMany(c => c.Commands);

            builder.Entity<Command>()
                .HasOne(c => c.TargetCountry)
                .WithMany(c => c.IncomingAttacks);

            builder.Entity<Command>()
                .HasMany(c => c.Divisions)
                .WithOne(d => d.ParentCommand);

            // Divisions
            builder.Entity<Division>()
                .HasOne(d => d.Unit)
                .WithMany(u => u.ContainingDivisions);

            //Country - CountryBuilding - BuildingType
            builder.Entity<CountryBuilding>()
                .HasOne(cb => cb.ParentCountry)
                .WithMany(c => c.Buildings);

            builder.Entity<CountryBuilding>()
                .HasOne(cb => cb.Building)
                .WithMany(b => b.CompletedBuildings);

            //Country - CountryResearch - ResearchType
            builder.Entity<CountryResearch>()
                .HasOne(cr => cr.ParentCountry)
                .WithMany(c => c.Researches);

            builder.Entity<CountryResearch>()
                .HasOne(cr => cr.Research)
                .WithMany(r => r.CompletedResearches);

            //Country - InProgressBuilding - BuildingType
            builder.Entity<InProgressBuilding>()
                .HasOne(ib => ib.ParentCountry)
                .WithMany(c => c.InProgressBuildings);

            builder.Entity<InProgressBuilding>()
                .HasOne(ib => ib.Building)
                .WithMany(b => b.InProgressBuildings);

            //Country - InProgressResearch - ResearchType
            builder.Entity<InProgressResearch>()
                .HasOne(ir => ir.ParentCountry)
                .WithMany(c => c.InProgressResearches);

            builder.Entity<InProgressResearch>()
                .HasOne(ir => ir.Research)
                .WithMany(r => r.InProgressResearches);

            // Leader
            builder.Entity<LeaderType>()
                .HasBaseType<UnitType>();

            base.OnModelCreating(builder);
        }
    }
}