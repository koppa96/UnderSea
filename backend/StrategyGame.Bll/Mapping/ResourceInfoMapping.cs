using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities.Resources;

namespace StrategyGame.Bll.Mapping
{
    public class ResourceInfoMapping : Profile
    {
        public ResourceInfoMapping()
        {
            CreateMap<ReportResource, ResourceInfo>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Child.Content.Name))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Child.Content.ImageUrl));
        }
    }
}
