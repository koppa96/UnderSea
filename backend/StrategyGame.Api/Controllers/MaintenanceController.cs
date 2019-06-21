using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll;
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
        protected GlobalTurnHandler Handler { get; }

        protected UnderSeaDatabaseContext Database { get; }

        public MaintenanceController(GlobalTurnHandler handler, UnderSeaDatabaseContext database)
        {
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
            Database = database ?? throw new ArgumentNullException(nameof(database));
        }

        [HttpGet]
        [Route("endturn")]
        [Authorize]
        public async Task<ActionResult> EndTurnAsync()
        {
            await Handler.EndTurnAsync(Database).ConfigureAwait(false);
            return Ok();
        }
    }
}