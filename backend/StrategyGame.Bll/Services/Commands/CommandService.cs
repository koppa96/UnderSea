using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Received;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using StrategyGame.Bll.Extensions;

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
                .ThenInclude(c => c.Divisons)
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
                var defendingDivision = defendingCommand.Divisons.SingleOrDefault(d => d.Unit.Id == detail.UnitId);
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

            _context.Commands.Add(attackingCommand);
            await _context.SaveChangesAsync();
            return attackingCommand.ToCommandInfo(_mapper);
        }

        public async Task DeleteCommandAsync(string username, int commandId)
        {
            var country = await _context.Countries.Include(c => c.Commands)
                .ThenInclude(c => c.Divisons)
                    .ThenInclude(d => d.Unit)
                .SingleAsync(c => c.ParentUser.UserName == username);

            var command = country.Commands.SingleOrDefault(c => c.Id == commandId);
            if (command == null || command.ParentCountry.Id == command.TargetCountry.Id)
            {
                throw new ArgumentOutOfRangeException("Invalid command id.");
            }

            if (command.ParentCountry.ParentUser.UserName != username)
            {
                throw new InvalidOperationException("Can not modify commands of others.");
            }

            var defendingCommand = country.GetAllDefending();
            foreach (var division in command.Divisons)
            {
                var defendingDivision = defendingCommand.Divisons.SingleOrDefault(d => d.Unit.Id == division.Unit.Id);
                if (defendingDivision == null)
                {
                    defendingDivision = new Division
                    {
                        ParentCommand = defendingCommand,
                        Unit = division.Unit,
                        Count = 0
                    };
                }

                defendingDivision.Count += division.Count;
                _context.Divisions.Remove(defendingDivision);
            }

            _context.Commands.Remove(command);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommandInfo>> GetCommandsAsync(string username)
        {
            var commands = await _context.Commands.Include(c => c.Divisons)
                    .ThenInclude(d => d.Unit)
                        .ThenInclude(u => u.Content)
                .Where(c => c.ParentCountry.ParentUser.UserName == username)
                .ToListAsync();
            
            var commandInfos = commands.Select(c => c.ToCommandInfo(_mapper));
            return commandInfos;
        }

        public async Task<CommandInfo> UpdateCommandAsync(string username, int commandId, CommandDetails details)
        {
            var country = await _context.Countries.Include(c => c.Commands)
                    .ThenInclude(c => c.TargetCountry)
                .Include(c => c.Commands)
                    .ThenInclude(c => c.Divisons)
                        .ThenInclude(d => d.Unit)
                            .ThenInclude(u => u.Content)
                .SingleAsync(c => c.ParentUser.UserName == username);

            var command = country.Commands.SingleOrDefault(c => c.Id == commandId);
            if (command == null)
            {
                if (await _context.Commands.AnyAsync(c => c.Id == commandId))
                {
                    throw new InvalidOperationException("Can not modify commands of others.");
                }

                throw new ArgumentOutOfRangeException(nameof(commandId), "Invalid command id.");
            }

            var targetCountry = await _context.Countries.FindAsync(details.TargetCountryId);
            if (targetCountry == null)
            {
                throw new ArgumentOutOfRangeException(nameof(details.TargetCountryId), "Invalid country id.");
            }

            var defendingCommand = country.GetAllDefending();
            command.TargetCountry = targetCountry;
            foreach (var detail in details.Units)
            {
                var division = command.Divisons.SingleOrDefault(d => d.Unit.Id == detail.UnitId);
                var defendingDivision = defendingCommand.Divisons.SingleOrDefault(d => d.Unit.Id == detail.UnitId);
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
                        ParentCommand = command,
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

            await _context.SaveChangesAsync();
            return command.ToCommandInfo(_mapper);
        }
    }
}