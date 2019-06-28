using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly UnderSeaDatabaseContext _context;
        private readonly IMapper _mapper;

        public ReportService(UnderSeaDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CombatInfo>> GetCombatInfoAsync(string username)
        {
            var country = await _context.Countries.Include(c => c.Attacks)
                    .ThenInclude(r => r.Attackers)
                        .ThenInclude(d => d.Unit)
                            .ThenInclude(u => u.Content)
                .Include(c => c.Attacks)
                    .ThenInclude(r => r.Defenders)
                        .ThenInclude(d => d.Unit)
                            .ThenInclude(u => u.Content)
                .Include(c => c.Attacks)
                    .ThenInclude(r => r.Defender)
                .Include(c => c.Defenses)
                    .ThenInclude(r => r.Attackers)
                        .ThenInclude(d => d.Unit)
                            .ThenInclude(u => u.Content)
                .Include(c => c.Defenses)
                    .ThenInclude(r => r.Defenders)
                        .ThenInclude(d => d.Unit)
                            .ThenInclude(u => u.Content)
                .Include(c => c.Defenses)
                    .ThenInclude(r => r.Attacker)
                .SingleAsync(c => c.ParentUser.UserName == username);

            return country.Attacks.Where(r => !r.IsDeletedByAttacker)
                .Select(r =>
                {
                    var combatInfo = _mapper.Map<CombatReport, CombatInfo>(r);
                    combatInfo.IsAttack = true;
                    combatInfo.IsWon = r.DidAttackerWin;
                    combatInfo.EnemyCountryId = r.Defender.Id;
                    combatInfo.EnemyCountryName = r.Defender.Name;
                    combatInfo.YourUnits = r.Attackers.Select(d => _mapper.Map<Division, UnitInfo>(d));
                    combatInfo.EnemyUnits = r.Defenders.Select(d => _mapper.Map<Division, UnitInfo>(d));
                    combatInfo.LostUnits = r.Losses.Select(d => _mapper.Map<Division, UnitInfo>(d));
                    combatInfo.IsSeen = r.IsSeenByAttacker;
                    return combatInfo;
                }).Concat(country.Defenses.Where(r => !r.IsDeletedByDefender)
                    .Select(r =>
                    {
                        var combatInfo = _mapper.Map<CombatReport, CombatInfo>(r);
                        combatInfo.IsAttack = false;
                        combatInfo.IsWon = !r.DidAttackerWin;
                        combatInfo.EnemyCountryId = r.Attacker.Id;
                        combatInfo.EnemyCountryName = r.Attacker.Name;
                        combatInfo.YourUnits = r.Defenders.Select(d => _mapper.Map<Division, UnitInfo>(d));
                        combatInfo.EnemyUnits = r.Attackers.Select(d => _mapper.Map<Division, UnitInfo>(d));
                        combatInfo.LostUnits = r.Losses.Select(d => _mapper.Map<Division, UnitInfo>(d));
                        combatInfo.IsSeen = r.IsSeenByDefender;
                        return combatInfo;
                    }));
        }

        public async Task SetSeenAsync(string username, int reportId)
        {
            var report = await _context.Reports.Include(r => r.Attacker)
                    .ThenInclude(c => c.ParentUser)
                .Include(r => r.Defender)
                    .ThenInclude(c => c.ParentUser)
                .SingleOrDefaultAsync(r => r.Id == reportId);

            if (report == null)
            {
                throw new ArgumentOutOfRangeException(nameof(reportId), "Invalid report id.");
            }

            if (report.Attacker.ParentUser.UserName == username)
            {
                report.IsSeenByAttacker = true;
            }
            else if (report.Defender.ParentUser.UserName == username)
            {
                report.IsSeenByDefender = true;
            }
            else
            {
                throw new UnauthorizedAccessException("Can not mark reports of other users as seen.");
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string username, int reportId)
        {
            var report = await _context.Reports.Include(r => r.Attacker)
                    .ThenInclude(c => c.ParentUser)
                .Include(r => r.Defender)
                    .ThenInclude(c => c.ParentUser)
                .SingleOrDefaultAsync(r => r.Id == reportId);

            if (report == null)
            {
                throw new ArgumentOutOfRangeException(nameof(reportId), "Invalid report id.");
            }

            if (report.Attacker.ParentUser.UserName == username)
            {
                report.IsDeletedByAttacker = true;
            }
            else if (report.Defender.ParentUser.UserName == username)
            {
                report.IsDeletedByDefender = true;
            }
            else
            {
                throw new UnauthorizedAccessException("Can not delete reports of others.");
            }

            RemoveReportIfBothDeleted(report);
            await _context.SaveChangesAsync();
        }

        private void RemoveReportIfBothDeleted(CombatReport report)
        {
            if (!(report.IsDeletedByAttacker && report.IsDeletedByDefender))
            {
                return;
            }

            _context.Divisions.RemoveRange(report.Attackers);
            _context.Divisions.RemoveRange(report.Defenders);
            _context.Divisions.RemoveRange(report.Losses);
            _context.Reports.Remove(report);
        }
    }
}