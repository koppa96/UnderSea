using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities;

namespace StrategyGame.Bll.Mapping
{
    public class BuildingMapper : Profile
    {
        public BuildingMapper()
        {
            CreateMap<BuildingType, CreationInfo>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Content.Name))
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Content.Description))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl))
                .ForMember(dest => dest.Cost, conf => conf.MapFrom(src => src.CostPearl))
                .ForMember(dest => dest.Count, conf => conf.MapFrom(src => 0));
        }
    }
}