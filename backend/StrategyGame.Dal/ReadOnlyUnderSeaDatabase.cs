using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Effects;
using StrategyGame.Model.Entities.Frontend;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Dal
{
    /// <summary>
    /// Represents a read-only database context towards a UnderSea database.
    /// </summary>
    /// <remarks>
    /// The read only database does not track changes to data acquired by it,
    /// as such does not support write operations. It also does not use cached data at all.
    /// </remarks>
    public class ReadOnlyUnderSeaDatabase : IdentityDbContext<User>
    {
        /// <summary>
        /// Defines if the database is actually read-only or not.
        /// </summary>
        readonly bool _isActuallyReadOnly;



        /// <summary>
        /// Gets the collection of <see cref="Country"/> in the database.
        /// </summary>
        public DbSet<Country> Countries { get; }

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
        /// Gets the collection of <see cref="AbstractEffect"/> in the database.
        /// </summary>
        public DbSet<AbstractEffect> Effects { get; }

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
        /// Initializes a new instance of the <see cref="ReadOnlyUnderSeaDatabase"/>.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions{ReadOnlyUnderSeaDatabase}"/> for the database.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public ReadOnlyUnderSeaDatabase(DbContextOptions<ReadOnlyUnderSeaDatabase> options)
            : this(options, true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyUnderSeaDatabase"/>.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions"/> for the database.</param>
        /// <param name="isActuallyReadOnly">Defines if the database is actually read-only or not.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        /// <remarks>
        /// Use this constructor in inheriting classes to set the read-only-ness of the class.
        /// </remarks>
        protected ReadOnlyUnderSeaDatabase(DbContextOptions options, bool isActuallyReadOnly)
            : base(options ?? throw new ArgumentNullException(nameof(options)))
        {
            _isActuallyReadOnly = isActuallyReadOnly;

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
            Effects = Set<AbstractEffect>();

            BuildingContents = Set<BuildingContent>();
            ResearchContents = Set<ResearchContent>();
            UnitContents = Set<UnitContent>();
            GlobalValues = Set<GlobalValue>();
        }

        /// <summary>
        /// Configures the model of the database.
        /// </summary>
        /// <param name="Builder">The <see cref="ModelBuilder"/> to use.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DummyEffect>().HasBaseType<AbstractEffect>();

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

            // Country            
            builder.Entity<Country>().Property(x => x.Pearls).IsRequired();
            builder.Entity<Country>().Property(x => x.Corals).IsRequired();
            builder.Entity<Country>().HasMany(x => x.Researches).WithOne(x => x.ParentCountry);
            builder.Entity<Country>().HasMany(x => x.Commands).WithOne(x => x.ParentCountry);

            builder.Entity<Country>()
                .HasOne(c => c.ParentUser)
                .WithOne(u => u.RuledCountry)
                .HasForeignKey<User>(u => u.RuledCountryId);

            // BuildingType
            builder.Entity<BuildingType>().Property(x => x.CostPearl).IsRequired();
            builder.Entity<BuildingType>().Property(x => x.CostCoral).IsRequired();
            builder.Entity<BuildingType>().Property(x => x.BuildTime).IsRequired();
            builder.Entity<BuildingType>().Property(x => x.MaxCount).IsRequired();

            // BuildingType - BuildingContent
            builder.Entity<BuildingType>().HasOne(b => b.Content).WithOne(c => c.Parent)
                .HasForeignKey<BuildingContent>(c => c.ParentId);

            // ResearchType
            builder.Entity<ResearchType>().Property(x => x.CostPearl).IsRequired();
            builder.Entity<ResearchType>().Property(x => x.CostCoral).IsRequired();
            builder.Entity<ResearchType>().Property(x => x.ResearchTime).IsRequired();
            builder.Entity<ResearchType>().Property(x => x.MaxCompletedAmount).IsRequired();

            // BuildingType - BuildingContent
            builder.Entity<ResearchType>().HasOne(r => r.Content).WithOne(c => c.Parent)
                .HasForeignKey<ResearchContent>(c => c.ParentId);

            // UnitType
            builder.Entity<UnitType>().Property(x => x.AttackPower).IsRequired();
            builder.Entity<UnitType>().Property(x => x.DefensePower).IsRequired();
            builder.Entity<UnitType>().Property(x => x.CostPearl).IsRequired();
            builder.Entity<UnitType>().Property(x => x.CostCoral).IsRequired();
            builder.Entity<UnitType>().Property(x => x.MaintenancePearl).IsRequired();
            builder.Entity<UnitType>().Property(x => x.MaintenanceCoral).IsRequired();

            // BuildingType - BuildingContent
            builder.Entity<UnitType>().HasOne(u => u.Content).WithOne(c => c.Parent)
                .HasForeignKey<UnitContent>(c => c.ParentId);

            // Commands
            builder.Entity<Command>()
                .HasOne(c => c.ParentCountry)
                .WithMany(c => c.Commands);

            builder.Entity<Command>()
                .HasOne(c => c.TargetCountry)
                .WithMany(c => c.IncomingAttacks);

            builder.Entity<Command>()
                .HasMany(c => c.Divisons)
                .WithOne(d => d.ParentCommand);

            // Divisions
            builder.Entity<Division>().Property(x => x.Count).IsRequired();

            builder.Entity<Division>()
                .HasOne(d => d.Unit)
                .WithMany(u => u.ContainingDivisions);

            // CountryBuilding
            builder.Entity<CountryBuilding>().Property(x => x.Count).IsRequired();
            
            //Country - CountryBuilding - BuildingType
            builder.Entity<CountryBuilding>()
                .HasOne(cb => cb.ParentCountry)
                .WithMany(c => c.Buildings);

            builder.Entity<CountryBuilding>()
                .HasOne(cb => cb.Building)
                .WithMany(b => b.CompletedBuildings);

            // CountryResearch
            builder.Entity<CountryResearch>().Property(x => x.Count).IsRequired();
            
            //Country - CountryResearch - ResearchType
            builder.Entity<CountryResearch>()
                .HasOne(cr => cr.ParentCountry)
                .WithMany(c => c.Researches);

            builder.Entity<CountryResearch>()
                .HasOne(cr => cr.Research)
                .WithMany(r => r.CompletedResearches);

            // InProgressBuilding
            builder.Entity<InProgressBuilding>().Property(x => x.TimeLeft).IsRequired();
            
            //Country - InProgressBuilding - BuildingType
            builder.Entity<InProgressBuilding>()
                .HasOne(ib => ib.ParentCountry)
                .WithMany(c => c.InProgressBuildings);

            builder.Entity<InProgressBuilding>()
                .HasOne(ib => ib.Building)
                .WithMany(b => b.InProgressBuildings);

            // InProgressResearch
            builder.Entity<InProgressResearch>().Property(x => x.TimeLeft).IsRequired();
           
            //Country - InProgressResearch - ResearchType
            builder.Entity<InProgressResearch>()
                .HasOne(ir => ir.ParentCountry)
                .WithMany(c => c.InProgressResearches);

            builder.Entity<InProgressResearch>()
                .HasOne(ir => ir.Research)
                .WithMany(r => r.InProgressResearches);
            
            base.OnModelCreating(builder);
        }



        /// <summary>
        /// Adds no tracking query behaviour, effectively making the database read-only (and faster).
        /// </summary>
        /// <param name="Options">The options builder.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            if (_isActuallyReadOnly)
            {
                Options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }

            base.OnConfiguring(Options);
        }



        // Override the save methods to ensure a read-only DB can't be saved.

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the database is read-only.</exception>
        /// <returns>The number of state entries written to the database.</returns>
        public override int SaveChanges()
        {
            if (_isActuallyReadOnly)
            {
                throw new InvalidOperationException("The database is read-only and cannot be saved.");
            }
            else
            {
                return base.SaveChanges();
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether <see cref="ChangeTracker.AcceptAllChanges()"/> is called after the changes have been sent successfully to the database.</param>
        /// <exception cref="InvalidOperationException">Thrown if the database is read-only.</exception>
        /// <returns>The number of state entries written to the database.</returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            if (_isActuallyReadOnly)
            {
                throw new InvalidOperationException("The database is read-only and cannot be saved.");
            }
            else
            {
                return base.SaveChanges(acceptAllChangesOnSuccess);
            }
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <exception cref="InvalidOperationException">Thrown if the database is read-only.</exception>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_isActuallyReadOnly)
            {
                throw new InvalidOperationException("The database is read-only and cannot be saved.");
            }
            else
            {
                return base.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether <see cref="ChangeTracker.AcceptAllChanges()"/> is called after the changes have been sent successfully to the database.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <exception cref="InvalidOperationException">Thrown if the database is read-only.</exception>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (_isActuallyReadOnly)
            {
                throw new InvalidOperationException("The database is read-only and cannot be saved.");
            }
            else
            {
                return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
        }
    }
}