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

        public async Task<CommandInfo> AttackTargetAsync(string username, int countryId, CommandDetails details)
        {
            var country = await _context.Countries
                .Include(c => c.Commands)
                .ThenInclude(c => c.Divisions)
                    .ThenInclude(d => d.Unit)
                        .ThenInclude(u => u.Content)
                .Include(c => c.ParentUser)
                .SingleOrDefaultAsync(c => c.Id == countryId);

            if (country == null)
            {
                throw new ArgumentOutOfRangeException(nameof(countryId), "No parent country found by the provided ID.");
            }

            if (country.ParentUser.UserName != username)
            {
                throw new UnauthorizedAccessException("Can't access country not owned by the user.");
            }

            var targetCountry = await _context.Countries.FindAsync(details.TargetCountryId);
            if (targetCountry == null)
            {
                throw new ArgumentOutOfRangeException(nameof(details.TargetCountryId), "No target country found by the provided ID.");
            }

            var defendingCommand = country.GetAllDefending();
            var attackingCommand = new Command
            {
                TargetCountry = targetCountry,
                ParentCountry = country
            };

            foreach (var detail in details.Units)
            {
                var defendingDivisions = defendingCommand.Divisions.Where(d => d.Id == detail.UnitId).ToList();
                if (defendingDivisions.Sum(d => d.Count) < detail.Amount)
                {
                    throw new ArgumentException("Not enough units.");
                }

                var newDivisions = new List<Division>();
                while (newDivisions.Sum(d => d.Count) + defendingDivisions[newDivisions.Count].Count < detail.Amount)
                {
                    var division = defendingDivisions[newDivisions.Count];
                    division.ParentCommand = attackingCommand;
                    newDivisions.Add(division);                    
                }

                if (newDivisions.Sum(d => d.Count) < detail.Amount)
                {
                    var division = defendingDivisions[newDivisions.Count];
                    var newDivision = new Division
                    {
                        ParentCommand = attackingCommand,
                        Unit = division.Unit,
                        BattleCount = division.BattleCount,
                        Count = newDivisions.Sum(d => d.Count) - detail.Amount
                    };
                    division.Count -= newDivision.Count;
                    _context.Divisions.Add(newDivision);
                }
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
            var command = await _context.Commands
                .Include(c => c.Divisions)
                    .ThenInclude(d => d.Unit)
                .Include(c => c.TargetCountry)
                .Include(c => c.ParentCountry)
                    .ThenInclude(c => c.ParentUser)
                .SingleOrDefaultAsync(c => c.Id == commandId);

            if (command == null)
            {
                throw new ArgumentOutOfRangeException(nameof(commandId), "No command found by the provided ID.");
            }

            if (command.ParentCountry.ParentUser.UserName != username)
            {
                throw new UnauthorizedAccessException("Can't modify commands of other users.");
            }

            if (command.ParentCountry.Id == command.TargetCountry.Id)
            {
                throw new ArgumentException("Can't modify the defending command directly.");
            }

            var defendingCommand = command.ParentCountry.GetAllDefending();
            foreach (var division in command.Divisions)
            {
                var defendingDivision = defendingCommand.Divisions.SingleOrDefault(d => d.Unit.Id == division.Unit.Id);
                if (defendingDivision == null)
                {
                    defendingDivision = new Division
                    {
                        ParentCommand = defendingCommand,
                        Unit = division.Unit,
                        Count = 0
                    };

                    _context.Divisions.Add(defendingDivision);
                }

                defendingDivision.Count += division.Count;
                _context.Divisions.Remove(division);
            }

            _context.Commands.Remove(command);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommandInfo>> GetCommandsAsync(string username, int countryId)
        {
            var country = await _context.Countries
               .Include(c => c.Commands)
                    .ThenInclude(comm => comm.TargetCountry)
                .Include(c => c.Commands)
                    .ThenInclude(c => c.Divisions)
                        .ThenInclude(d => d.Unit)
                            .ThenInclude(u => u.Content)
               .Include(c => c.ParentUser)
               .SingleOrDefaultAsync(c => c.Id == countryId);

            if (country == null)
            {
                throw new ArgumentOutOfRangeException(nameof(countryId), "No country found by the provided ID.");
            }

            if (country.ParentUser.UserName != username)
            {
                throw new UnauthorizedAccessException("Can't access country not owned by the user.");
            }

            var commandInfos = country.Commands.Select(c => ToCommandInfo(c, _mapper));
            return commandInfos;
        }

        public async Task<CommandInfo> UpdateCommandAsync(string username, int commandId, CommandDetails details)
        {
            var command = await _context.Commands
                .Include(c => c.Divisions)
                    .ThenInclude(d => d.Unit)
                .Include(c => c.TargetCountry)
                .Include(c => c.ParentCountry)
                    .ThenInclude(c => c.ParentUser)
                .SingleOrDefaultAsync(c => c.Id == commandId);

            if (command == null)
            {
                throw new ArgumentOutOfRangeException(nameof(commandId), "No command found by the provided ID.");
            }

            if (command.ParentCountry.ParentUser.UserName != username)
            {
                throw new UnauthorizedAccessException("Can't modify commands of other users.");
            }

            if (command.ParentCountry.Id == command.TargetCountry.Id)
            {
                throw new ArgumentException("Can't modify the defending command directly.");
            }

            var targetCountry = await _context.Countries.FindAsync(details.TargetCountryId);
            if (targetCountry == null)
            {
                throw new ArgumentOutOfRangeException(nameof(details.TargetCountryId), "No target country found by the provided ID.");
            }

            var defendingCommand = command.ParentCountry.GetAllDefending();
            command.TargetCountry = targetCountry;

            foreach (var detail in details.Units)
            {
                
            }

            if (!command.Divisions.Any(d => d.Unit is LeaderType && d.Count > 0))
            {
                throw new InvalidOperationException("Every attack must contain a leader.");
            }

            await _context.SaveChangesAsync();
            return ToCommandInfo(command, _mapper);
        }

        public CommandInfo ToCommandInfo(Command command, IMapper mapper)
        {
            var commandInfo = mapper.Map<Command, CommandInfo>(command);
            commandInfo.Units = command.Divisions.Select(d =>
            {
                var unitInfo = mapper.Map<UnitType, UnitInfo>(d.Unit);
                unitInfo.Count = d.Count;
                return unitInfo;
            });

            return commandInfo;
        }
    }
}