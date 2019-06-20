using System.Collections.Generic;
using System.Threading.Tasks;
using StrategyGame.Bll.Dto.Sent;

namespace StrategyGame.Bll.Services.Researches
{
    public interface IResearchService
    {
         Task<IEnumerable<CreationInfo>> GetResearchesAsync(string username);
         Task StartResearchAsync(string username, int researchId);
    }
}