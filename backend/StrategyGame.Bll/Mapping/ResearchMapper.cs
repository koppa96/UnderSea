using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities;

namespace StrategyGame.Bll.Mapping
{
    public class ResearchMapper : Profile
    {
        public ResearchMapper()
        {
            CreateMap<ResearchType, CreationInfo>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Content.Name))
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Content.Description))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl))
                .ForMember(dest => dest.IconImageUrl, conf => conf.MapFrom(src => src.Content.IconImageUrl))
                .ForMember(dest => dest.Cost, conf => conf.MapFrom(src => src.CostPearl));
        }
    }
}