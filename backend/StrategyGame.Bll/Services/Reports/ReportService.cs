using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Reports;
using StrategyGame.Model.Entities.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StrategyGame.Bll.Dto.Sent.Country;

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

        public async Task<IEnumerable<CombatInfo>> GetCombatInfoAsync(string username, int countryId)
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
                .Include(c => c.Attacks)
                    .ThenInclude(r => r.Losses)
                        .ThenInclude(d => d.Unit)
                            .ThenInclude(u => u.Content)
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
                .Include(c => c.Defenses)
                    .ThenInclude(r => r.Losses)
                        .ThenInclude(d => d.Unit)
                            .ThenInclude(u => u.Content)
                .Include(c => c.ParentUser)
                .SingleOrDefaultAsync(c => c.Id == countryId);

            if (country == null)
            {
                throw new ArgumentOutOfRangeException(nameof(countryId), "Invalid country id.");
            }

            if (country.ParentUser.UserName != username)
            {
                throw new UnauthorizedAccessException("Can not view reports of others.");
            }

            return country.Attacks.Where(r => !r.IsDeletedByAttacker)
                .Select(r =>
                {
                    var combatInfo = _mapper.Map<CombatReport, CombatInfo>(r);
                    combatInfo.IsAttack = true;
                    combatInfo.IsWon = r.DidAttackerWin;
                    combatInfo.EnemyCountryId = r.Defender.Id;
                    combatInfo.EnemyCountryName = r.Defender.Name;
                    combatInfo.YourUnits = r.Attackers.Select(d => _mapper.Map<Division, BriefUnitInfo>(d));
                    combatInfo.EnemyUnits = r.Defenders.Select(d => _mapper.Map<Division, BriefUnitInfo>(d));
                    combatInfo.LostUnits = r.Losses.Select(d => _mapper.Map<Division, BriefUnitInfo>(d));
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
                        combatInfo.YourUnits = r.Defenders.Select(d => _mapper.Map<Division, BriefUnitInfo>(d));
                        combatInfo.EnemyUnits = r.Attackers.Select(d => _mapper.Map<Division, BriefUnitInfo>(d));
                        combatInfo.LostUnits = r.Losses.Select(d => _mapper.Map<Division, BriefUnitInfo>(d));
                        combatInfo.IsSeen = r.IsSeenByDefender;
                        return combatInfo;
                    }));
        }

        public async Task SetCombatReportSeenAsync(string username, int reportId)
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

        public async Task DeleteCombatReportAsync(string username, int reportId)
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

        public async Task<IEnumerable<EventInfo>> GetEventInfoAsync(string username, int countryId)
        {
            var country = await _context.Countries.Include(c => c.ParentUser)
                .Include(c => c.EventReports)
                    .ThenInclude(r => r.Event)
                        .ThenInclude(e => e.Content)
                .SingleOrDefaultAsync(c => c.Id == countryId);

            if (country == null)
            {
                throw new ArgumentOutOfRangeException(nameof(countryId), "Invalid country id.");
            }

            if (country.ParentUser.UserName != username)
            {
                throw new UnauthorizedAccessException("Cannot view reports of others.");
            }

            return country.EventReports.Select(r => _mapper.Map<EventReport, EventInfo>(r));
        }

        public async Task SetEventReportSeenAsync(string username, int reportId)
        {
            var report = await _context.EventReports.Include(r => r.Country)
                    .ThenInclude(c => c.ParentUser)
                .SingleOrDefaultAsync(r => r.Id == reportId);

            if (report == null)
            {
                throw new ArgumentOutOfRangeException(nameof(reportId), "Invalid report id.");
            }

            if (report.Country.ParentUser.UserName != username)
            {
                throw new UnauthorizedAccessException("Cannot mark others reports as seen.");
            }

            report.IsSeen = true;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventReportAsync(string username, int reportId)
        {
            var report = await _context.EventReports.Include(r => r.Country)
                    .ThenInclude(c => c.ParentUser)
                .SingleOrDefaultAsync(r => r.Id == reportId);

            if (report == null)
            {
                throw new ArgumentOutOfRangeException(nameof(reportId), "Invalid report id.");
            }

            if (report.Country.ParentUser.UserName != username)
            {
                throw new UnauthorizedAccessException("Cannot delete reports of others.");
            }

            _context.EventReports.Remove(report);
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