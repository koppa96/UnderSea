using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities;

namespace StrategyGame.Bll.Mapping
{
    /// <summary>
    /// Provides a mapping between <see cref="UnitType"/> and <see cref="UnitInfo"/>. 
    /// The content should be included in the type, and the <see cref="UnitInfo.Count"/> must be set manually.
    /// </summary>
    public class UnitInfoMapper : Profile
    {
        public UnitInfoMapper()
        {
            CreateMap<UnitType, UnitInfo>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Content.Name))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl));
        }
    }
}