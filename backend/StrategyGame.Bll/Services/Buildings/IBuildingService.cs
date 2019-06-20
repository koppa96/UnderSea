using System.Collections.Generic;
using System.Threading.Tasks;
using StrategyGame.Bll.Dto.Sent;

namespace StrategyGame.Bll.Services.Buildings
{
    public interface IBuildingService
    {
         Task<IEnumerable<CreationInfo>> GetBuildingsAsync(string username);
         Task StartBuildingAsync(string username, int buildingId);
    }
}