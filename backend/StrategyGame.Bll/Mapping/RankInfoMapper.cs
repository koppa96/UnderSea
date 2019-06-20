using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities;

namespace StrategyGame.Bll.Mapping
{
    /// <summary>
    /// Maps a <see cref="Country"/> into a <see cref="RankInfo"/>.
    /// </summary>
    public class RankInfoMapper : Profile
    {
        public RankInfoMapper()
        {
            CreateMap<Country, RankInfo>();
        }
    }
}