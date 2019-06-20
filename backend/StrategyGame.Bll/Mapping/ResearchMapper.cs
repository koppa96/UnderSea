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
                .ForMember(ci => ci.Name, conf => conf.MapFrom(r => r.Content.Name))
                .ForMember(ci => ci.Description, conf => conf.MapFrom(r => r.Content.Description))
                .ForMember(ci => ci.ImageUrl, conf => conf.MapFrom(r => r.Content.ImageUrl))
                .ForMember(ci => ci.Cost, conf => conf.MapFrom(r => r.CostPearl))
                .ForMember(ci => ci.Count, conf => conf.MapFrom(r => 0));
        }
    }
}