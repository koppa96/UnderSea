using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StrategyGame.Dal;
using StrategyGame.Model.Entities.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Logger
{
    public class DbLogger : IDbLogger
    {
        private readonly ILogger _logger;
        private readonly UnderSeaDatabaseContext _context;

        public DbLogger(ILogger<DbLogger> logger, UnderSeaDatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task LogExceptionAsync(Exception e)
        {
            _logger.LogError(e.Message, e);

            var exceptionLog = new ExceptionLog
            {
                ExceptionType = e.GetType().Name,
                Message = e.Message,
                ThrownAt = DateTime.UtcNow,
                StackTrace = e.StackTrace
            };

            _context.ExceptionLogs.Add(exceptionLog);
            await _context.SaveChangesAsync();
        }

        public async Task LogRequestAsync(HttpContext context)
        {
            var requestLog = new RequestLog
            {
                Method = context.Request.Method,
                ResponseStatus = context.Response.StatusCode,
                Url = context.Request.Path.Value,
                Timestamp = DateTime.UtcNow
            };

            _context.RequestLogs.Add(requestLog);
            await _context.SaveChangesAsync();
        }
    }
}