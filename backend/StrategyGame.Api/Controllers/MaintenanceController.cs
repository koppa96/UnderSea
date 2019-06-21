using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll;
using StrategyGame.Bll.Services.TurnHandling;
using StrategyGame.Dal;
using System;
using System.Threading.Tasks;

namespace StrategyGame.Api.Controllers
{
    /// <summary>
    /// API endpoint for maintenance work.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        protected ITurnHandlingService Handler { get; }

        protected UnderSeaDatabaseContext Database { get; }

        public MaintenanceController(ITurnHandlingService handler, UnderSeaDatabaseContext database)
        {
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
            Database = database ?? throw new ArgumentNullException(nameof(database));
        }

        [HttpGet]
        [Route("endturn")]
        [Authorize]
        public async Task<ActionResult> EndTurnAsync()
        {
            await Handler.EndTurnAsync(Database);
            return Ok();
        }
    }
}