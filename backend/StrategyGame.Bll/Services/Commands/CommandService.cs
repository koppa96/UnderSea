using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Received;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Extensions;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Commands
{
    public class CommandService : ICommandService
    {
        private readonly UnderSeaDatabaseContext _context;
        private readonly IMapper _mapper;

        public CommandService(UnderSeaDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommandInfo> AttackTargetAsync(string username, CommandDetails details)
        {
            var country = await _context.Countries.Include(c => c.Commands)
                .ThenInclude(c => c.Divisions)
                    .ThenInclude(d => d.Unit)
                        .ThenInclude(u => u.Content)
                .SingleAsync(c => c.ParentUser.UserName == username);

            var targetCountry = await _context.Countries.FindAsync(details.TargetCountryId);
            if (targetCountry == null)
            {
                throw new ArgumentOutOfRangeException(nameof(details.TargetCountryId), "Invalid country id.");
            }

            var defendingCommand = country.GetAllDefending();
            var attackingCommand = new Command
            {
                TargetCountry = targetCountry,
                ParentCountry = country
            };

            foreach (var detail in details.Units)
            {
                var defendingDivision = defendingCommand.Divisions.SingleOrDefault(d => d.Unit.Id == detail.UnitId);
                if (defendingDivision == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(detail.UnitId), "Invalid unit id.");
                }

                if (defendingDivision.Count < detail.Amount)
                {
                    throw new ArgumentException("Not enough units.");
                }

                defendingDivision.Count -= detail.Amount;
                var attackingDivision = new Division
                {
                    Unit = defendingDivision.Unit,
                    ParentCommand = attackingCommand,
                    Count = detail.Amount
                };

                _context.Divisions.Add(attackingDivision);
            }

            if (!attackingCommand.Divisions.Any(d => d.Unit is LeaderType && d.Count > 0))
            {
                throw new InvalidOperationException("Every attack must contain a leader.");
            }

            _context.Commands.Add(attackingCommand);
            await _context.SaveChangesAsync();
            return ToCommandInfo(attackingCommand, _mapper);
        }

        public async Task DeleteCommandAsync(string username, int commandId)
        {
            var country = await _context.Countries.Include(c => c.Commands)
                .ThenInclude(c => c.Divisions)
                    .ThenInclude(d => d.Unit)
                .Include(c => c.Commands)
                    .ThenInclude(c => c.TargetCountry)
                .Include(c => c.ParentUser)
                .SingleAsync(c => c.ParentUser.UserName == username);

            var command = country.Commands.SingleOrDefault(c => c.Id == commandId);
            if (command == null || command.ParentCountry.Id == command.TargetCountry.Id)
            {
                if (await _context.Commands.AnyAsync(c => c.Id == commandId))
                {
                    throw new UnauthorizedAccessException("Can not modify commands of others.");
                }

                throw new ArgumentOutOfRangeException("Invalid command id.");
            }

            command.MergeInto(country.GetAllDefending(), _context);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommandInfo>> GetCommandsAsync(string username)
        {
            var commands = await _context.Commands
                .Include(c => c.Divisions)
                    .ThenInclude(d => d.Unit)
                        .ThenInclude(u => u.Content)
                .Include(c => c.TargetCountry)
                .Where(c => c.ParentCountry.ParentUser.UserName == username)
                .ToListAsync();

            var commandInfos = commands.Select(c => ToCommandInfo(c, _mapper));
            return commandInfos;
        }

        public async Task<CommandInfo> UpdateCommandAsync(string username, int commandId, CommandDetails details)
        {
            var country = await _context.Countries.Include(c => c.Commands)
                    .ThenInclude(c => c.TargetCountry)
                .Include(c => c.Commands)
                    .ThenInclude(c => c.Divisions)
                        .ThenInclude(d => d.Unit)
                            .ThenInclude(u => u.Content)
                .SingleAsync(c => c.ParentUser.UserName == username);

            var attackingCommand = country.Commands.SingleOrDefault(c => c.Id == commandId);
            if (attackingCommand == null)
            {
                if (await _context.Commands.AnyAsync(c => c.Id == commandId))
                {
                    throw new UnauthorizedAccessException("Can not modify commands of others.");
                }

                throw new ArgumentOutOfRangeException(nameof(commandId), "Invalid command id.");
            }

            var targetCountry = await _context.Countries.FindAsync(details.TargetCountryId);
            if (targetCountry == null)
            {
                throw new ArgumentOutOfRangeException(nameof(details.TargetCountryId), "Invalid country id.");
            }

            var defendingCommand = country.GetAllDefending();
            attackingCommand.TargetCountry = targetCountry;

            foreach (var detail in details.Units)
            {
                var division = attackingCommand.Divisions.SingleOrDefault(d => d.Unit.Id == detail.UnitId);
                var defendingDivision = defendingCommand.Divisions.SingleOrDefault(d => d.Unit.Id == detail.UnitId);
                if (division == null)
                {
                    var unit = defendingDivision?.Unit;
                    if (unit == null)
                    {
                        if (await _context.UnitTypes.AnyAsync(u => u.Id == detail.UnitId))
                        {
                            throw new ArgumentException("Not enough units.");
                        }

                        throw new ArgumentOutOfRangeException("Invalid unit id.");
                    }

                    division = new Division
                    {
                        ParentCommand = attackingCommand,
                        Unit = unit,
                        Count = 0
                    };

                    _context.Divisions.Add(division);
                }

                if (detail.Amount - division.Count > defendingDivision.Count)
                {
                    throw new ArgumentException("Not enough units.");
                }

                defendingDivision.Count -= detail.Amount - division.Count;
                division.Count = detail.Amount;
            }

            if (!attackingCommand.Divisions.Any(d => d.Unit is LeaderType && d.Count > 0))
            {
                throw new InvalidOperationException("Every attack must contain a leader.");
            }

            await _context.SaveChangesAsync();
            return ToCommandInfo(attackingCommand, _mapper);
        }

        public CommandInfo ToCommandInfo(Command command, IMapper mapper)
        {
            var commandInfo = mapper.Map<Command, CommandInfo>(command);
            commandInfo.Units = command.Divisions.Select(d =>
            {
                var unitInfo = mapper.Map<UnitType, BriefUnitInfo>(d.Unit);
                unitInfo.Count = d.Count;
                return unitInfo;
            });

            return commandInfo;
        }
    }
}