using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Dal.Entities;
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
            this._isActuallyReadOnly = isActuallyReadOnly;


        }



        /// <summary>
        /// Configures the model of the database.
        /// </summary>
        /// <param name="Builder">The <see cref="ModelBuilder"/> to use.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
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