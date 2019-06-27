using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Model.Entities;

namespace StrategyGame.Bll.Mapping
{
    public class CountryInfoMapper : Profile
    {
        public CountryInfoMapper()
        {
            CreateMap<Country, CountryInfo>();
            CreateMap<Country, RankInfo>();

            CreateMap<UnitType, BriefUnitInfo>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Content.Name))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl))
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Id));

            CreateMap<CountryBuilding, BriefCreationInfo>()
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Building.Content.ImageUrl))
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Building.Id))
                .ForMember(dest => dest.InProgressCount, conf => conf.MapFrom(src => 0));
            CreateMap<CountryResearch, BriefCreationInfo>()
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Research.Content.ImageUrl))
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Research.Id))
                .ForMember(dest => dest.InProgressCount, conf => conf.MapFrom(src => 0));

            CreateMap<BuildingType, BriefCreationInfo>()
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl))
                .ForMember(dest => dest.Count, conf => conf.MapFrom(src => 0));
            CreateMap<ResearchType, BriefCreationInfo>()
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl))
                .ForMember(dest => dest.Count, conf => conf.MapFrom(src => 0));

            CreateMap<RandomEvent, EventInfo>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Content.Name))
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Content.Description))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl))
                .ForMember(dest => dest.Flavourtext, conf => conf.MapFrom(src => src.Content.FlavourText));
        }
    }
}