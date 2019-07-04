using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Connectors;
using StrategyGame.Model.Entities.Creations;
using StrategyGame.Model.Entities.Reports;

namespace StrategyGame.Bll.Mapping
{
    /// <summary>
    /// Provides mappings between <see cref="Country"/> and <see cref="CountryInfo"/>, <see cref="Country"/> and <see cref="RankInfo"/>,
    /// <see cref="CountryBuilding"/> and <see cref="BriefCreationInfo"/>, <see cref="CountryResearch"/> and <see cref="BriefCreationInfo"/>,
    /// and <see cref="RandomEvent"/> and <see cref="EventInfo"/>.
    /// </summary>
    public class CountryInfoMapper : Profile
    {
        public CountryInfoMapper()
        {
            CreateMap<Country, CountryInfo>();
            CreateMap<Country, RankInfo>();

            CreateMap<ConnectorWithAmount<Country, BuildingType>, BriefCreationInfo>()
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Child.Content.ImageUrl))
                .ForMember(dest => dest.IconImageUrl, conf => conf.MapFrom(src => src.Child.Content.IconImageUrl))
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Child.Id))
                .ForMember(dest => dest.InProgressCount, conf => conf.MapFrom(src => 0));

            CreateMap<ConnectorWithAmount<Country, ResearchType>, BriefCreationInfo>()
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Child.Content.ImageUrl))
                .ForMember(dest => dest.IconImageUrl, conf => conf.MapFrom(src => src.Child.Content.IconImageUrl))
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Child.Id))
                .ForMember(dest => dest.InProgressCount, conf => conf.MapFrom(src => 0));

            CreateMap<EventReport, EventInfo>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Event.Content.Name))
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Event.Content.Description))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Event.Content.ImageUrl))
                .ForMember(dest => dest.FlavorText, conf => conf.MapFrom(src => src.Event.Content.FlavourText));
            
            CreateMap<ConnectorWithAmount<Country, ResourceType>, ResourceInfo>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Child.Content.Name))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Child.Content.ImageUrl));
        }
    }
}