using Microsoft.EntityFrameworkCore;
using System;

namespace StrategyGame.Dal
{
    /// <summary>
    /// Represents a (writeable) database context towards a UnderSea database.
    /// </summary>
    public class UnderSeaDatabase : ReadOnlyUnderSeaDatabase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnderSeaDatabase"/>.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions{NingyoDatabase}"/> for the database.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public UnderSeaDatabase(DbContextOptions<UnderSeaDatabase> options)
            : base(options, false)
        { }
    }
}