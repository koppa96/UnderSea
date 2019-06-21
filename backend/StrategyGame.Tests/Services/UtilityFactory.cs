using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll;
using StrategyGame.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Tests.Services
{
    public class UtilityFactory
    {
        private static bool mapperInitialized = false;

        public static UnderSeaDatabaseContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<UnderSeaDatabaseContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            return new UnderSeaDatabaseContext(options);
        }

        public static IMapper CreateMapper()
        {
            if (!mapperInitialized)
            {
                var mapperConfiguration = new MapperConfigurationExpression();
                mapperConfiguration.AddProfiles(typeof(KnownValues).Assembly);
                Mapper.Initialize(mapperConfiguration);
                mapperInitialized = true;
            }
            
            return Mapper.Instance;
        }
    }
}
