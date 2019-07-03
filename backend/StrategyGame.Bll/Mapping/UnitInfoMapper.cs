using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Units;
using System.Linq;

namespace StrategyGame.Bll.Mapping
{
    /// <summary>
    /// Provides a mappings between <see cref="UnitType"/> and <see cref="UnitInfo"/> and <see cref="BriefUnitInfo"/>, along with
    /// <see cref="Division"/> and <see cref="UnitInfo"/>.
    /// </summary>
    public class UnitInfoMapper : Profile
    {
        public UnitInfoMapper()
        {
            CreateMap<UnitType, UnitInfo>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Content.Name))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl))
                .ForMember(dest => dest.Cost, conf => conf.MapFrom(src => src.Cost))
                .ForMember(dest => dest.Cost, conf => conf.MapFrom(src => src.Cost.Select(c => new ResourceInfo
                {
                    Name = c.Child.Content.Name,
                    Amount = (int)c.Amount,
                    ImageUrl = c.Child.Content.ImageUrl
                })));

            CreateMap<UnitType, BriefUnitInfo>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Content.Name))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Content.ImageUrl));

            CreateMap<Division, BriefUnitInfo>()
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Unit.Id))
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Unit.Content.Name))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Unit.Content.ImageUrl))
                .ForMember(dest => dest.TotalCount, conf => conf.MapFrom(src => src.Count));;

            CreateMap<Division, UnitInfo>()
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Unit.Id))
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Unit.Content.Name))
                .ForMember(dest => dest.ImageUrl, conf => conf.MapFrom(src => src.Unit.Content.ImageUrl))
                .ForMember(dest => dest.AttackPower, conf => conf.MapFrom(src => src.Unit.AttackPower))
                .ForMember(dest => dest.DefensePower, conf => conf.MapFrom(src => src.Unit.DefensePower))
                .ForMember(dest => dest.Cost, conf => conf.MapFrom(src => src.Unit.Cost.Select(c => new ResourceInfo
                {
                    Name = c.Child.Content.Name,
                    Amount = (int)c.Amount,
                    ImageUrl = c.Child.Content.ImageUrl
                })));
        }
    }
}