using Microsoft.Extensions.Logging;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Logger
{
    public class ExceptionLogger : IExceptionLogger
    {
        private readonly ILogger _logger;
        private readonly UnderSeaDatabaseContext _context;

        public ExceptionLogger(ILogger<ExceptionLogger> logger, UnderSeaDatabaseContext context)
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
    }
}