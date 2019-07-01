using Microsoft.AspNetCore.Http;
using StrategyGame.Bll.Services.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Api.Middlewares
{
    public class RequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IDbLogger logger)
        {
            await _next.Invoke(context);
            await logger.LogRequestAsync(context);
        }
    }
}
