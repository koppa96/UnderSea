using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StrategyGame.Dal;
using StrategyGame.Model.Entities.Logging;
using System;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Logger
{
    public class DbLogger : IDbLogger
    {
        private readonly ILogger _logger;
        private readonly DbContextOptions<UnderSeaDatabaseContext> _options;

        public DbLogger(ILogger<DbLogger> logger, DbContextOptions<UnderSeaDatabaseContext> options)
        {
            _logger = logger;
            _options = options;
        }

        public async Task LogExceptionAsync(Exception e)
        {
            using (var context = new UnderSeaDatabaseContext(_options))
            {
                _logger.LogError(e.Message, e);

                var exceptionLog = new ExceptionLog
                {
                    ExceptionType = e.GetType().Name,
                    Message = e.Message,
                    ThrownAt = DateTime.UtcNow,
                    StackTrace = e.StackTrace
                };

                context.ExceptionLogs.Add(exceptionLog);
                await context.SaveChangesAsync();
            }
        }

        public async Task LogRequestAsync(HttpContext context)
        {
            using (var dbContext = new UnderSeaDatabaseContext(_options))
            {
                var requestLog = new RequestLog
                {
                    Method = context.Request.Method,
                    ResponseStatus = context.Response.StatusCode,
                    Url = context.Request.Path.Value,
                    Timestamp = DateTime.UtcNow
                };

                dbContext.RequestLogs.Add(requestLog);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}