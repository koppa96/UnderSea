using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities;

namespace StrategyGame.Bll.Mapping
{
    /// <summary>
    /// Provides mappings from <see cref="ResearchType"/> to <see cref="CreationInfo"/> and <see cref="BriefCreationInfo"/>.
    /// </summary>
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

            CreateMap<ResearchType, BriefCreationInfo>()
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl))
                .ForMember(dest => dest.IconImageUrl, conf => conf.MapFrom(src => src.Content.IconImageUrl))
                .ForMember(dest => dest.Count, conf => conf.MapFrom(src => 0));
        }
    }
}