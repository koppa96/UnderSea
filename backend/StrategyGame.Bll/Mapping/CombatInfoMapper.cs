using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities;

namespace StrategyGame.Bll.Mapping
{
    public class CombatInfoMapper : Profile
    {
        public CombatInfoMapper()
        {
            CreateMap<CombatReport, CombatInfo>();
        }
    }
}