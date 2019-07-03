using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Reports;

namespace StrategyGame.Bll.Mapping
{
    /// <summary>
    /// Provides mappings between <see cref="CombatReport"/> and <see cref="CombatInfo"/>.
    /// </summary>
    public class CombatInfoMapper : Profile
    {
        public CombatInfoMapper()
        {
            CreateMap<CombatReport, CombatInfo>();
        }
    }
}