using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Logger
{
    public interface IExceptionLogger
    {
        Task LogExceptionAsync(Exception e);
    }
}
