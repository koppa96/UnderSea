using AutoMapper;
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
            CreateMap<CountryBuilding, BriefCreationInfo>()
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Building.Content.ImageUrl))
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Building.Id));
            CreateMap<CountryResearch, BriefCreationInfo>()
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Research.Content.ImageUrl))
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Research.Id));
        }
    }
}
