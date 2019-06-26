using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities;

namespace StrategyGame.Bll.Mapping
{
    /// <summary>
    /// Provides a mapping between <see cref="UnitType"/> and <see cref="UnitInfo"/>. 
    /// The content should be included in the type, and the <see cref="UnitInfo.Count"/> must be set manually.
    /// </summary>
    public class UnitInfoMapper : Profile
    {
        public UnitInfoMapper()
        {
            CreateMap<UnitType, UnitInfo>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Content.Name))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl));

            CreateMap<Division, UnitInfo>()
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Unit.Id))
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Unit.Content.Name))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Unit.Content.ImageUrl))
                .ForMember(dest => dest.AttackPower, conf => conf.MapFrom(src => src.Unit.AttackPower))
                .ForMember(dest => dest.DefensePower, conf => conf.MapFrom(src => src.Unit.DefensePower))
                .ForMember(dest => dest.MaintenanceCoral, conf => conf.MapFrom(src => src.Unit.MaintenanceCoral))
                .ForMember(dest => dest.MaintenancePearl, conf => conf.MapFrom(src => src.Unit.MaintenancePearl))
                .ForMember(dest => dest.CostPearl, conf => conf.MapFrom(src => src.Unit.CostPearl));
        }
    }
}