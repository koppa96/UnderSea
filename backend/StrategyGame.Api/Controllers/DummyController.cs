using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;

namespace StrategyGame.Api.Controllers
{
    [Route("api/[controller]")]
    public class DummyController : ControllerBase
    {
        IMapper mapper;
        UnderSeaDatabaseContext db;

        public DummyController(IMapper mapper, UnderSeaDatabaseContext db)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        [Route("map")]
        public IActionResult Index()
        {
            var unitInfo = db.UnitTypes.Include(u => u.Content).Select(unit => mapper.Map<UnitType, UnitInfo>(unit)).ToList();
            return Ok();
        }
    }
}