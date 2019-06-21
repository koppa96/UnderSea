using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Bll.Mapping
{
    public class CountryInfoMapper : Profile
    {
        public CountryInfoMapper()
        {
            CreateMap<Country, CountryInfo>();
            CreateMap<Country, RankInfo>();

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
        }
    }
}
