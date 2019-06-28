using System.Collections.Generic;
using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities;

namespace StrategyGame.Bll.Mapping
{
    /// <summary>
    /// Provides mappings between <see cref="Command"/> and <see cref="CommandInfo"/>.
    /// </summary>
    public class CommandInfoMapper : Profile
    {
        public CommandInfoMapper()
        {
            CreateMap<Command, CommandInfo>()
                .ForMember(dest => dest.TargetCountryId, conf => conf.MapFrom(src => src.TargetCountry.Id))
                .ForMember(dest => dest.TargetCountryName, conf => conf.MapFrom(src => src.TargetCountry.Name))
                .ForMember(dest => dest.Units, conf => conf.MapFrom(src => new List<CommandInfo>()));
        }
    }
}