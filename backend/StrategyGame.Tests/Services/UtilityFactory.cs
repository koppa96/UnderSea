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
        public static UnderSeaDatabaseContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<UnderSeaDatabaseContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            return new UnderSeaDatabaseContext(options);
        }

        public static IMapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfigurationExpression();
            mapperConfiguration.AddProfiles(typeof(KnownValues).Assembly);
            Mapper.Initialize(mapperConfiguration);
            return Mapper.Instance;
        }
    }
}
