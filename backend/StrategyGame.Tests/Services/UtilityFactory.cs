using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Extensions;
using StrategyGame.Bll.Mapping;
using StrategyGame.Dal;
using System.Threading.Tasks;

namespace StrategyGame.Tests.Services
{
    /// <summary>
    /// Creates dependencies for the tested services.
    /// </summary>
    public class UtilityFactory
    {
        private static bool mapperInitialized = false;

        /// <summary>
        /// Creates an in-memory UnderSeaDbContext and fills it with seed data.
        /// </summary>
        /// <returns>The DbContext</returns>
        public static async Task<UnderSeaDatabaseContext> CreateContextAsync()
        {
            var options = new DbContextOptionsBuilder<UnderSeaDatabaseContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            var context = new UnderSeaDatabaseContext(options);
            context.Database.EnsureCreated();
            await context.FillWithDefaultAsync();
            await context.AddTestUsersAsync();

            return context;
        }

        /// <summary>
        /// Returns the instance of the mapper, initializes if uninitialized.
        /// </summary>
        /// <returns>The mapper instance</returns>
        public static IMapper CreateMapper()
        {
            if (!mapperInitialized)
            {
                var mapperConfiguration = new MapperConfigurationExpression();

                // The profiles are in the same assembly
                mapperConfiguration.AddProfiles(typeof(BuildingMapper).Assembly);

                Mapper.Initialize(mapperConfiguration);
                mapperInitialized = true;
            }

            return Mapper.Instance;
        }
    }
}
