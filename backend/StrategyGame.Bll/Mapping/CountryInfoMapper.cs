using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Model.Entities;

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

            CreateMap<CountryBuilding, BriefCreationInfo>()
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Building.Content.ImageUrl))
                .ForMember(dest => dest.IconImageUrl, conf => conf.MapFrom(src => src.Building.Content.IconImageUrl))
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Building.Id))
                .ForMember(dest => dest.InProgressCount, conf => conf.MapFrom(src => 0));

            CreateMap<CountryResearch, BriefCreationInfo>()
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Research.Content.ImageUrl))
                .ForMember(dest => dest.IconImageUrl, conf => conf.MapFrom(src => src.Research.Content.IconImageUrl))
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Research.Id))
                .ForMember(dest => dest.InProgressCount, conf => conf.MapFrom(src => 0));

            CreateMap<RandomEvent, EventInfo>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Content.Name))
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Content.Description))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl))
                .ForMember(dest => dest.Flavourtext, conf => conf.MapFrom(src => src.Content.FlavourText));
        }
    }
}