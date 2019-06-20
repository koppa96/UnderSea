using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities;

namespace StrategyGame.Bll.Mapping
{
    public class BuildingMapper : Profile
    {
        public BuildingMapper()
        {
            CreateMap<BuildingType, CreationInfo>()
                .ForMember(ci => ci.Name, conf => conf.MapFrom(b => b.Content.Name))
                .ForMember(ci => ci.Description, conf => conf.MapFrom(b => b.Content.Description))
                .ForMember(ci => ci.ImageUrl, conf => conf.MapFrom(b => b.Content.ImageUrl))
                .ForMember(ci => ci.Cost, conf => conf.MapFrom(b => b.CostPearl))
                .ForMember(ci => ci.Count, conf => conf.MapFrom(b => 0));
        }
    }
}