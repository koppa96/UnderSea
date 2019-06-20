using Microsoft.AspNetCore.Mvc;
using StrategyGame.Api.Controllers;
using StrategyGame.Bll.DTO.Received;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Api.ControllerExtensions
{
    public static class CommandsControllerExtensions
    {
        public static ActionResult HandleNotFound(this CommandsController controller, CommandDetails command, ArgumentOutOfRangeException e)
        {
            if (e.ParamName == nameof(command.TargetCountryId))
            {
                return controller.NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = "The target country is not found."
                });
            }
            else
            {
                return controller.NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = $"The unit type whit ID {e.ActualValue} is not found."
                });
            }
        }
    }
}
