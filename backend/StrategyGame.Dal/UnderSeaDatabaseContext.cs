using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Connectors;
using StrategyGame.Model.Entities.Creations;
using StrategyGame.Model.Entities.Frontend;
using StrategyGame.Model.Entities.Logging;
using StrategyGame.Model.Entities.Reports;
using StrategyGame.Model.Entities.Units;
using System;

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
        public DbSet<Country> Countries { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="RandomEvent"/> in the database.
        /// </summary>
        public DbSet<RandomEvent> RandomEvents { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="BuildingType"/> in the database.
        /// </summary>
        public DbSet<BuildingType> BuildingTypes { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="ResearchType"/> in the database.
        /// </summary>
        public DbSet<ResearchType> ResearchTypes { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="UnitType"/> in the database.
        /// </summary>
        public DbSet<UnitType> UnitTypes { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="Command"/> in the database.
        /// </summary>
        public DbSet<Command> Commands { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="Division"/> in the database.
        /// </summary>
        public DbSet<Division> Divisions { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="CountryResearch"/> in the database.
        /// </summary>
        public DbSet<ConnectorWithAmount<Country, ResearchType>> CountryResearches { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="CountryBuilding"/> in the database.
        /// </summary>
        public DbSet<ConnectorWithAmount<Country, BuildingType>> CountryBuildings { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="InProgressBuilding"/> in the database.
        /// </summary>
        public DbSet<ConnectorWithProgress<Country, BuildingType>> InProgressBuildings { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="InProgressResearch"/> in the database.
        /// </summary>
        public DbSet<ConnectorWithProgress<Country, ResearchType>> InProgressResearches { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="BuildingEffect"/> in the database.
        /// </summary>
        public DbSet<Connector<BuildingType, Effect>> BuildingEffects { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="ResearchEffect"/> in the database.
        /// </summary>
        public DbSet<Connector<ResearchType, Effect>> ResearchEffects { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="EventEffect"/> in the database.
        /// </summary>
        public DbSet<Connector<RandomEvent, Effect>> EventEffects { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="Effect"/> in the database.
        /// </summary>
        public DbSet<Effect> Effects { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="Effect"/> in the database.
        /// </summary>
        public DbSet<CombatReport> Reports { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="GlobalValue"/> in the database.
        /// </summary>
        public DbSet<GlobalValue> GlobalValues { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="BuildingContent"/> in the database.
        /// </summary>
        public DbSet<BuildingContent> BuildingContents { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="ResearchContent"/> in the database.
        /// </summary>
        public DbSet<ResearchContent> ResearchContents { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="UnitContent"/> in the database.
        /// </summary>
        public DbSet<UnitContent> UnitContents { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="EventContent"/> in the database.
        /// </summary>
        public DbSet<EventContent> EventContents { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="UnitContent"/> in the database.
        /// </summary>
        public DbSet<ResourceContent> ResourceContents { get; set; }

        public DbSet<ExceptionLog> ExceptionLogs { get; set; }

        public DbSet<RequestLog> RequestLogs { get; set; }

        public DbSet<ResourceType> ResourceTypes { get; set; }

        public DbSet<ConnectorWithAmount<Country, ResourceType>> CountryResources { get; set; }

        public DbSet<ConnectorWithAmount<BuildingType, ResourceType>> BuildingResources { get; set; }

        public DbSet<ConnectorWithAmount<ResearchType, ResourceType>> ResearchResources { get; set; }

        public DbSet<UnitResource> UnitResources { get; set; }

        public DbSet<EventReport> EventReports { get; set; }

        public DbSet<ConnectorWithAmount<CombatReport, ResearchType>> ReportResearches { get; set; }

        public DbSet<ConnectorWithAmount<CombatReport, BuildingType>> ReportBuildings { get; set; }

        public DbSet<ReportResource> ReportResources { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="UnderSeaDatabaseContext"/>.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions{UnderSeaDatabaseContext}"/> for the database.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public UnderSeaDatabaseContext(DbContextOptions<UnderSeaDatabaseContext> options)
            : base(options)
        { }

        /// <summary>
        /// Configures the model of the database.
        /// </summary>
        /// <param name="builder">The <see cref="ModelBuilder"/> to use.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ConnectorWithAmount<CombatReport, BuildingType>>()
                .HasOne(b => b.Parent)
                .WithMany(r => r.DefenderBuildings);

            builder.Entity<ConnectorWithAmount<CombatReport, BuildingType>>()
                .HasOne(b => b.Child)
                .WithMany(b => b.ReportBuildings);

            builder.Entity<ConnectorWithAmount<CombatReport, ResearchType>>()
                .HasOne(r => r.Parent)
                .WithMany(r => r.DefenderResearches);

            builder.Entity<ConnectorWithAmount<CombatReport, ResearchType>>()
                .HasOne(r => r.Child)
                .WithMany(r => r.ReportResearches);

            // Event reports
            builder.Entity<EventReport>()
                .HasOne(r => r.Country)
                .WithMany(c => c.EventReports);

            builder.Entity<EventReport>()
                .HasOne(r => r.Event)
                .WithMany(e => e.EventReports);

            // Resources
            builder.Entity<ResourceType>()
                .HasOne(r => r.Content)
                .WithMany(c => c.Parents);

            builder.Entity<ConnectorWithAmount<Country, ResourceType>>()
                .HasOne(cr => cr.Parent)
                .WithMany(c => c.Resources);

            builder.Entity<ConnectorWithAmount<Country, ResourceType>>()
                .HasOne(cr => cr.Child)
                .WithMany(r => r.CountryResources);

            builder.Entity<ConnectorWithAmount<BuildingType, ResourceType>>()
                .HasOne(br => br.Parent)
                .WithMany(b => b.Cost);

            builder.Entity<ConnectorWithAmount<BuildingType, ResourceType>>()
                .HasOne(br => br.Child)
                .WithMany(r => r.BuildingResources);

            builder.Entity<ConnectorWithAmount<ResearchType, ResourceType>>()
                .HasOne(rr => rr.Parent)
                .WithMany(r => r.Cost);

            builder.Entity<ConnectorWithAmount<ResearchType, ResourceType>>()
                .HasOne(rr => rr.Child)
                .WithMany(r => r.ResearchResources);

            builder.Entity<UnitResource>()
                .HasOne(ur => ur.Parent)
                .WithMany(u => u.Cost);

            builder.Entity<UnitResource>()
                .HasOne(ur => ur.Child)
                .WithMany(r => r.UnitResources);

            builder.Entity<ReportResource>()
                .HasOne(r => r.Parent)
                .WithMany(r => r.Loot);

            builder.Entity<ReportResource>()
                .HasOne(r => r.Child)
                .WithMany(r => r.ReportResources);

            // Combat report
            builder.Entity<CombatReport>()
                .HasOne(c => c.Attacker)
                .WithMany(c => c.Attacks);

            builder.Entity<CombatReport>()
                .HasOne(c => c.Defender)
                .WithMany(c => c.Defenses);

            // Effect
            builder.Entity<Effect>().Property(e => e.Name).IsRequired().HasMaxLength(200);

            //Building - BuildingEffect - Effect
            builder.Entity<Connector<BuildingType, Effect>>()
                .HasOne(be => be.Parent)
                .WithMany(b => b.Effects);

            builder.Entity<Connector<BuildingType, Effect>>()
                .HasOne(be => be.Child)
                .WithMany(e => e.AffectedBuildings);

            //Research - ResearchEffect - Effect
            builder.Entity<Connector<ResearchType, Effect>>()
                .HasOne(re => re.Parent)
                .WithMany(r => r.Effects);

            builder.Entity<Connector<ResearchType, Effect>>()
                .HasOne(re => re.Child)
                .WithMany(e => e.AffectedResearches);

            // RandomEvent - EventEffect - Effect
            builder.Entity<Connector<RandomEvent, Effect>>()
                .HasOne(ee => ee.Parent)
                .WithMany(e => e.Effects);

            builder.Entity<Connector<RandomEvent, Effect>>()
                .HasOne(ee => ee.Child)
                .WithMany(e => e.AffectedEvents);

            // Country            
            builder.Entity<Country>()
                .HasMany(x => x.Researches)
                .WithOne(x => x.Parent);

            builder.Entity<Country>()
                .HasMany(x => x.Commands)
                .WithOne(x => x.ParentCountry);

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
            builder.Entity<ConnectorWithAmount<Country, BuildingType>>()
                .HasOne(cb => cb.Parent)
                .WithMany(c => c.Buildings);

            builder.Entity<ConnectorWithAmount<Country, BuildingType>>()
                .HasOne(cb => cb.Child)
                .WithMany(b => b.CompletedBuildings);

            //Country - CountryResearch - ResearchType
            builder.Entity<ConnectorWithAmount<Country, ResearchType>>()
                .HasOne(cr => cr.Parent)
                .WithMany(c => c.Researches);

            builder.Entity<ConnectorWithAmount<Country, ResearchType>>()
                .HasOne(cr => cr.Child)
                .WithMany(r => r.CompletedResearches);

            //Country - InProgressBuilding - BuildingType
            builder.Entity<ConnectorWithProgress<Country, BuildingType>>()
                .HasOne(ib => ib.Parent)
                .WithMany(c => c.InProgressBuildings);

            builder.Entity<ConnectorWithProgress<Country, BuildingType>>()
                .HasOne(ib => ib.Child)
                .WithMany(b => b.InProgressBuildings);

            //Country - InProgressResearch - ResearchType
            builder.Entity<ConnectorWithProgress<Country, ResearchType>>()
                .HasOne(ir => ir.Parent)
                .WithMany(c => c.InProgressResearches);

            builder.Entity<ConnectorWithProgress<Country, ResearchType>>()
                .HasOne(ir => ir.Child)
                .WithMany(r => r.InProgressResearches);

            // Leader
            builder.Entity<LeaderType>()
                .HasBaseType<UnitType>();

            // Spy
            builder.Entity<SpyType>()
                .HasBaseType<UnitType>();

            base.OnModelCreating(builder);
        }
    }
}