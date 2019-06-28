using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities;
using System.Linq;

namespace StrategyGame.Bll.Mapping
{
    /// <summary>
    /// Provides mappings from <see cref="BuildingType"/> to <see cref="CreationInfo"/> and <see cref="BriefCreationInfo"/>.
    /// </summary>
    public class BuildingMapper : Profile
    {
        public BuildingMapper()
        {
            CreateMap<BuildingType, CreationInfo>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Content.Name))
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Content.Description))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl))
                .ForMember(dest => dest.Cost, conf => conf.MapFrom(src => src.Cost.Select(c => new ResourceInfo
                {
                    Name = c.ResourceType.Content.Name,
                    Amount = (int)c.Amount,
                    ImageUrl = c.ResourceType.Content.ImageUrl
                })))
                .ForMember(dest => dest.IconImageUrl, conf => conf.MapFrom(src => src.Content.IconImageUrl))

            CreateMap<BuildingType, BriefCreationInfo>()
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl))
                .ForMember(dest => dest.IconImageUrl, conf => conf.MapFrom(src => src.Content.IconImageUrl))
                .ForMember(dest => dest.Count, conf => conf.MapFrom(src => 0));
        }
    }
}