using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using StrategyGame.Bll.Dto.Received;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Extensions;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Commands
{
    public class CommandService : ICommandService
    {
        private readonly UnderSeaDatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly AsyncReaderWriterLock _turnEndLock;

        public CommandService(UnderSeaDatabaseContext context, AsyncReaderWriterLock turnEndLock, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _turnEndLock = turnEndLock;
        }

        public async Task<CommandInfo> AttackTargetAsync(string username, int countryId, CommandDetails details,
            CancellationToken turnEndWaitToken)
        {
            using (var lck = await _turnEndLock.ReaderLockAsync(turnEndWaitToken))
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
                    ParentCountry = country,
                    Divisions = new List<Division>()
                };

                _context.Commands.Add(attackingCommand);

                foreach (var detail in details.Units)
                {
                    TransferUnits(defendingCommand, 
                        attackingCommand, 
                        detail.UnitId, 
                        detail.Amount);
                }

                if (!attackingCommand.Divisions.Any(d => d.Unit is LeaderType && d.Count > 0))
                {
                    throw new InvalidOperationException("Every attack must contain a leader.");
                }

                await _context.SaveChangesAsync();
                return ToCommandInfo(attackingCommand, _mapper);
            }
        }

        public async Task DeleteCommandAsync(string username, int commandId, CancellationToken turnEndWaitToken)
        {
            using (var lck = await _turnEndLock.ReaderLockAsync(turnEndWaitToken))
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
                    if (await _context.Commands.AnyAsync(c => c.Id == commandId))
                    {
                        throw new UnauthorizedAccessException("Can not modify commands of others.");
                    }

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
                command.MergeInto(defendingCommand, _context);
                await _context.SaveChangesAsync();
            }
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

        public async Task<CommandInfo> UpdateCommandAsync(string username, int commandId, CommandDetails details,
            CancellationToken turnEndWaitToken)
        {
            using (var lck = await _turnEndLock.ReaderLockAsync(turnEndWaitToken))
            {
                var command = await _context.Commands
                .Include(c => c.Divisions)
                    .ThenInclude(d => d.Unit)
                .Include(c => c.TargetCountry)
                .Include(c => c.ParentCountry)
                    .ThenInclude(c => c.ParentUser)
                .Include(c => c.ParentCountry)
                    .ThenInclude(c => c.Commands)
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
                    var attackingDivisions = command.Divisions.Where(d => d.Id == detail.UnitId).ToList();

                    TransferUnits(defendingCommand, 
                        command, detail.UnitId, 
                        detail.Amount - attackingDivisions.Sum(d => d.Count));
                }

                if (!command.Divisions.Any(d => d.Unit is LeaderType && d.Count > 0))
                {
                    throw new InvalidOperationException("Every attack must contain a leader.");
                }

                await _context.SaveChangesAsync();
                return ToCommandInfo(command, _mapper);
            }
        }

        private void TransferUnits(Command from, Command to, int unitId, int amount)
        {
            if (amount < 0)
            {
                var tmp = to;
                to = from;
                from = tmp;
            }

            var takenDivisions = from.TakeFrom(unitId, amount, _context);
            takenDivisions.MergeInto(to, _context);
        }

        public CommandInfo ToCommandInfo(Command command, IMapper mapper)
        {
            var commandInfo = mapper.Map<Command, CommandInfo>(command);
            commandInfo.Units = command.GetAllBriefUnitInfo(mapper);
            return commandInfo;
        }
    }
}