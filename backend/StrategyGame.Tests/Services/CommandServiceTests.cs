using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.AsyncEx;
using StrategyGame.Bll.Dto.Received;
using StrategyGame.Bll.Services.Commands;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Tests.Services
{
    [TestClass]
    public class CommandServiceTests
    {
        private UnderSeaDatabaseContext context;
        private ICommandService commandService;

        [TestInitialize]
        public async Task Initialize()
        {
            context = await UtilityFactory.CreateContextAsync();
            commandService = new CommandService(context, new AsyncReaderWriterLock(), UtilityFactory.CreateMapper());
        }

        private async Task<CommandDetails> SetUpValidAttackAsync(string attacker, string target)
        {
            var defendingCommand = await context.Commands.Include(c => c.Divisions)
                    .ThenInclude(d => d.Unit)
                .SingleAsync(c => c.ParentCountry.ParentUser.UserName == attacker);

            var targetCountry = await context.Countries.SingleAsync(c => c.ParentUser.UserName == target);

            var normalDivision = defendingCommand.Divisions.First(d => !(d.Unit is LeaderType));
            var leaderDivision = defendingCommand.Divisions.First(d => d.Unit is LeaderType);
            var commandDetails = new CommandDetails
            {
                TargetCountryId = targetCountry.Id,
                Units = new List<UnitDetails>
                {
                    new UnitDetails
                    {
                        UnitId = normalDivision.Unit.Id,
                        Amount = normalDivision.Count
                    },
                    new UnitDetails
                    {
                        UnitId = leaderDivision.Unit.Id,
                        Amount = leaderDivision.Count
                    }
                }
            };

            return commandDetails;
        }

        [TestMethod]
        [DataRow("TheCommander", "TheRich")]
        public async Task TestAttackTarget(string attacker, string target)
        {
            var commandDetails = await SetUpValidAttackAsync(attacker, target);

            var info = await commandService.AttackTargetAsync(attacker,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == attacker)).Id,
                commandDetails);
            Assert.AreEqual(commandDetails.TargetCountryId, info.TargetCountryId);
            Assert.AreEqual(2, info.Units.Count());
            Assert.AreEqual(commandDetails.Units.First().UnitId, info.Units.First().Id);
            Assert.AreEqual(commandDetails.Units.First().Amount, info.Units.First().TotalCount);
        }

        [TestMethod]
        [DataRow("TheCommander", "TheRich")]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestAttackWithoutLeader(string attacker, string target)
        {
            var defendingCommand = await context.Commands.Include(c => c.Divisions)
                    .ThenInclude(d => d.Unit)
                .SingleAsync(c => c.ParentCountry.ParentUser.UserName == attacker);

            var targetCountry = await context.Countries.SingleAsync(c => c.ParentUser.UserName == target);

            var normalDivision = defendingCommand.Divisions.First(d => !(d.Unit is LeaderType));
            var commandDetails = new CommandDetails
            {
                TargetCountryId = targetCountry.Id,
                Units = new List<UnitDetails>
                {
                    new UnitDetails
                    {
                        UnitId = normalDivision.Unit.Id,
                        Amount = normalDivision.Count
                    }
                }
            };

            await commandService.AttackTargetAsync(attacker,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == attacker)).Id,
                commandDetails);
        }

        [TestMethod]
        [DataRow("TheCommander", "TheRich")]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = false)]
        public async Task TestAttackWithNotEnoughUnits(string attacker, string target)
        {
            var defendingCommand = await context.Commands.Include(c => c.Divisions)
                    .ThenInclude(d => d.Unit)
                .SingleAsync(c => c.ParentCountry.ParentUser.UserName == attacker);

            var targetCountry = await context.Countries.SingleAsync(c => c.ParentUser.UserName == target);

            var normalDivision = defendingCommand.Divisions.First(d => !(d.Unit is LeaderType));
            var leaderDivision = defendingCommand.Divisions.First(d => d.Unit is LeaderType);
            var commandDetails = new CommandDetails
            {
                TargetCountryId = targetCountry.Id,
                Units = new List<UnitDetails>
                {
                    new UnitDetails
                    {
                        UnitId = normalDivision.Unit.Id,
                        Amount = normalDivision.Count + 1
                    },
                    new UnitDetails
                    {
                        UnitId = leaderDivision.Unit.Id,
                        Amount = leaderDivision.Count
                    }
                }
            };

            await commandService.AttackTargetAsync(attacker,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == attacker)).Id,
                commandDetails);
        }

        [TestMethod]
        [DataRow("TheCommander", "TheRich")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestAttackWithNotExistingUnits(string attacker, string target)
        {
            var targetCountry = await context.Countries.SingleAsync(c => c.ParentUser.UserName == target);
            var commandDetails = new CommandDetails
            {
                TargetCountryId = targetCountry.Id,
                Units = new List<UnitDetails>
                {
                    new UnitDetails
                    {
                        UnitId = -1,
                        Amount = 5
                    }
                }
            };

            await commandService.AttackTargetAsync(attacker,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == attacker)).Id,
                commandDetails);
        }

        [TestMethod]
        [DataRow("TheCommander")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestAttackNotExistingTarget(string attacker)
        {
            var commandDetails = new CommandDetails
            {
                TargetCountryId = -1,
                Units = new List<UnitDetails>
                {
                    new UnitDetails
                    {
                        UnitId = -1,
                        Amount = 5
                    }
                }
            };

            await commandService.AttackTargetAsync(attacker,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == attacker)).Id,
                commandDetails);
        }

        [TestMethod]
        [DataRow("TheCommander", "TheRich")]
        public async Task TestDeleteAttack(string attacker, string defender)
        {
            var commandDetails = await SetUpValidAttackAsync(attacker, defender);
            var info = await commandService.AttackTargetAsync(attacker,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == attacker)).Id,
                commandDetails);

            await commandService.DeleteCommandAsync(attacker, info.Id);

            Assert.IsFalse(context.Commands.Any(c => c.Id == info.Id));
        }

        [TestMethod]
        [DataRow("TheCommander", "TheRich")]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task TestDeleteOthersAttack(string attacker, string defender)
        {
            var commandDetails = await SetUpValidAttackAsync(attacker, defender);
            var info = await commandService.AttackTargetAsync(attacker,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == attacker)).Id,
                commandDetails);

            await commandService.DeleteCommandAsync(defender, info.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestDeleteNotExistingCommand()
        {
            await commandService.DeleteCommandAsync("TheCommander", -1);
        }

        [TestMethod]
        [DataRow("TheCommander", "TheRich")]
        public async Task TestModifyCommand(string attacker, string defender)
        {
            var commandDetails = await SetUpValidAttackAsync(attacker, defender);
            var info = await commandService.AttackTargetAsync(attacker,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == attacker)).Id,
                commandDetails);

            var newDetails = new CommandDetails
            {
                TargetCountryId = info.TargetCountryId,
                Units = new List<UnitDetails>
                {
                    new UnitDetails
                    {
                        UnitId = info.Units.First().Id,
                        Amount = info.Units.First().TotalCount - 10
                    },
                    new UnitDetails
                    {
                        UnitId = info.Units.Last().Id,
                        Amount = info.Units.Last().TotalCount
                    }
                }
            };

            var newInfo = await commandService.UpdateCommandAsync(attacker, info.Id, newDetails);

            Assert.AreEqual(2, newInfo.Units.Count());
            Assert.AreEqual(newDetails.Units.First().UnitId, newInfo.Units.First().Id);
            Assert.AreEqual(newDetails.Units.First().Amount, newInfo.Units.First().TotalCount);
        }

        [TestMethod]
        [DataRow("TheCommander", "TheRich")]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task TestModifyOtherCommand(string attacker, string defender)
        {
            var commandDetails = await SetUpValidAttackAsync(attacker, defender);
            var info = await commandService.AttackTargetAsync(attacker,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == attacker)).Id,
                commandDetails);

            await commandService.UpdateCommandAsync(defender, info.Id, commandDetails);
        }

        [TestMethod]
        [DataRow("TheCommander", "TheRich")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestModifyCommandNotExistingTarget(string attacker, string defender)
        {
            var commandDetails = await SetUpValidAttackAsync(attacker, defender);
            var info = await commandService.AttackTargetAsync(attacker,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == attacker)).Id,
                commandDetails);

            commandDetails.TargetCountryId = -1;
            await commandService.UpdateCommandAsync(attacker, info.Id, commandDetails);
        }

        [TestMethod]
        [DataRow("TheCommander", "TheRich")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestModifyCommandNotExistingUnit(string attacker, string defender)
        {
            var commandDetails = await SetUpValidAttackAsync(attacker, defender);
            var info = await commandService.AttackTargetAsync(attacker,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == attacker)).Id,
                commandDetails);

            var firstUnit = commandDetails.Units.First();
            firstUnit.UnitId = -1;

            await commandService.UpdateCommandAsync(attacker, info.Id, commandDetails);
        }

        [TestMethod]
        [DataRow("TheCommander", "TheRich")]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TestModifyCommandNotEnoughUnits(string attacker, string defender)
        {
            var commandDetails = await SetUpValidAttackAsync(attacker, defender);
            var info = await commandService.AttackTargetAsync(attacker,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == attacker)).Id,
                commandDetails);

            var firstUnit = commandDetails.Units.First();
            firstUnit.Amount += 1;

            await commandService.UpdateCommandAsync(attacker, info.Id, commandDetails);
        }

        [TestMethod]
        [DataRow("TheCommander", "TheRich")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestModifyNotExistingCommand(string attacker, string defender)
        {
            var commandDetails = await SetUpValidAttackAsync(attacker, defender);

            await commandService.UpdateCommandAsync(attacker, -1, commandDetails);
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
