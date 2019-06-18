using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Model.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Dal
{
    /// <summary>
    /// Represents a read-only database context towards a UnderSea database.
    /// </summary>
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
        /// Gets the collection of <see cref="AbstractEffect"/> in the database.
        /// </summary>
        public DbSet<AbstractEffect> Effects { get; }





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
            Effects = Set<AbstractEffect>();
        }



        /// <summary>
        /// Configures the model of the database.
        /// </summary>
        /// <param name="Builder">The <see cref="ModelBuilder"/> to use.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Country
            builder.Entity<Country>().HasKey(x => x.Id);
            builder.Entity<Country>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<Country>().Property(x => x.ParentUser).IsRequired();
            builder.Entity<Country>().Property(x => x.Pearls).IsRequired();
            builder.Entity<Country>().Property(x => x.Corals).IsRequired();
            builder.Entity<Country>().HasMany(x => x.InProgressBuildings);
            builder.Entity<Country>().HasMany(x => x.InProgressResearches);
            builder.Entity<Country>().HasMany(x => x.Buildings);
            builder.Entity<Country>().HasMany(x => x.Researches);
            builder.Entity<Country>().HasMany(x => x.Commands);
            builder.Entity<Country>().HasOne(x => x.ParentUser);

            // BuildingType
            builder.Entity<BuildingType>().HasKey(x => x.Id);
            builder.Entity<BuildingType>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<BuildingType>().Property(x => x.CostPearl).IsRequired();
            builder.Entity<BuildingType>().Property(x => x.CostCoral).IsRequired();
            builder.Entity<BuildingType>().Property(x => x.BuildTime).IsRequired();
            builder.Entity<BuildingType>().Property(x => x.MaxCount).IsRequired();
            builder.Entity<BuildingType>().HasMany(x => x.CompletedBuildings);
            builder.Entity<BuildingType>().HasMany(x => x.InProgressBuildings);
            builder.Entity<BuildingType>().HasMany(x => x.Effects);

            // ResearchType
            builder.Entity<ResearchType>().HasKey(x => x.Id);
            builder.Entity<ResearchType>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<ResearchType>().Property(x => x.CostPearl).IsRequired();
            builder.Entity<ResearchType>().Property(x => x.CostCoral).IsRequired();
            builder.Entity<ResearchType>().Property(x => x.ResearchTime).IsRequired();
            builder.Entity<ResearchType>().Property(x => x.MaxCompletedAmount).IsRequired();
            builder.Entity<ResearchType>().HasMany(x => x.CompletedResearches);
            builder.Entity<ResearchType>().HasMany(x => x.InProgressResearches);
            builder.Entity<ResearchType>().HasMany(x => x.Effects);

            // UnitType
            builder.Entity<UnitType>().HasKey(x => x.Id);
            builder.Entity<UnitType>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<UnitType>().Property(x => x.AttackPower).IsRequired();
            builder.Entity<UnitType>().Property(x => x.DefensePower).IsRequired();
            builder.Entity<UnitType>().Property(x => x.CostPearl).IsRequired();
            builder.Entity<UnitType>().Property(x => x.CostCoral).IsRequired();
            builder.Entity<UnitType>().Property(x => x.MaintenancePearl).IsRequired();
            builder.Entity<UnitType>().Property(x => x.MaintenanceCoral).IsRequired();

            // Commands
            builder.Entity<Command>().HasKey(x => x.Id);
            builder.Entity<Command>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<Command>().HasIndex(nameof(Command.ParentCountry), nameof(Command.TargetCountry));
            builder.Entity<Command>().HasOne(x => x.ParentCountry);
            builder.Entity<Command>().HasOne(x => x.TargetCountry);
            builder.Entity<Command>().HasMany(x => x.Divisons);

            // Divisions
            builder.Entity<Division>().HasKey(x => x.Id);
            builder.Entity<Division>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<Division>().Property(x => x.Count).IsRequired();
            builder.Entity<Division>().HasOne(x => x.ParentCommand);
            builder.Entity<Division>().HasOne(x => x.Unit);

            // CountryBuilding
            builder.Entity<CountryBuilding>().HasKey(x => x.Id);
            builder.Entity<CountryBuilding>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<CountryBuilding>().Property(x => x.ParentCountry).IsRequired();
            builder.Entity<CountryBuilding>().Property(x => x.Building).IsRequired();
            builder.Entity<CountryBuilding>().Property(x => x.Count).IsRequired();
            builder.Entity<CountryBuilding>().HasIndex(nameof(CountryBuilding.ParentCountry), nameof(CountryBuilding.Building));
            builder.Entity<CountryBuilding>().HasOne(x => x.ParentCountry);
            builder.Entity<CountryBuilding>().HasOne(x => x.Building);

            // CountryResearch
            builder.Entity<CountryResearch>().HasKey(x => x.Id);
            builder.Entity<CountryResearch>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<CountryResearch>().Property(x => x.ParentCountry).IsRequired();
            builder.Entity<CountryResearch>().Property(x => x.Research).IsRequired();
            builder.Entity<CountryResearch>().Property(x => x.Count).IsRequired();
            builder.Entity<CountryResearch>().HasIndex(nameof(CountryResearch.ParentCountry), nameof(CountryResearch.Research));
            builder.Entity<CountryResearch>().HasOne(x => x.ParentCountry);
            builder.Entity<CountryResearch>().HasOne(x => x.Research);

            // InProgressBuilding
            builder.Entity<InProgressBuilding>().HasKey(x => x.Id);
            builder.Entity<InProgressBuilding>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<InProgressBuilding>().Property(x => x.ParentCountry).IsRequired();
            builder.Entity<InProgressBuilding>().Property(x => x.Building).IsRequired();
            builder.Entity<InProgressBuilding>().Property(x => x.TimeLeft).IsRequired();
            builder.Entity<InProgressBuilding>().HasIndex(nameof(InProgressBuilding.ParentCountry), nameof(InProgressBuilding.Building));
            builder.Entity<InProgressBuilding>().HasOne(x => x.ParentCountry);
            builder.Entity<InProgressBuilding>().HasOne(x => x.Building);

            // InProgressResearch
            builder.Entity<InProgressResearch>().HasKey(x => x.Id);
            builder.Entity<InProgressResearch>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<InProgressResearch>().Property(x => x.ParentCountry).IsRequired();
            builder.Entity<InProgressResearch>().Property(x => x.Research).IsRequired();
            builder.Entity<InProgressResearch>().Property(x => x.TimeLeft).IsRequired();
            builder.Entity<InProgressResearch>().HasIndex(nameof(InProgressResearch.ParentCountry), nameof(InProgressResearch.Research));
            builder.Entity<InProgressResearch>().HasOne(x => x.ParentCountry);
            builder.Entity<InProgressResearch>().HasOne(x => x.Research);

            // Effect
            builder.Entity<AbstractEffect>().HasKey(x => x.Id);
            builder.Entity<AbstractEffect>().Property(x => x.Id).ValueGeneratedOnAdd();

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