using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities.Reports;
using StrategyGame.Model.Entities.Units;

namespace StrategyGame.Bll.Extensions
{
    public static class ReportExtensions
    {
        public static CombatInfo ToAttackerCombatInfo(this CombatReport report, IMapper mapper)
        {
            var combatInfo = mapper.Map<CombatReport, CombatInfo>(report);

            combatInfo.IsAttack = true;
            combatInfo.IsWon = report.DidAttackerWin;
            combatInfo.IsSeen = report.IsSeenByAttacker;
            combatInfo.EnemyCountryId = report.Defender.Id;
            combatInfo.EnemyCountryName = report.Defender.Name;

            combatInfo.YourUnits = report.Attackers.Select(d => mapper.Map<Division, BriefUnitInfo>(d));
            combatInfo.YourLostUnits = report.AttackerLosses.Select(d => mapper.Map<Division, BriefUnitInfo>(d))
        }

        public static CombatInfo ToDefenderCombatInfo(this CombatReport report, IMapper mapper)
        {
            var combatInfo = mapper.Map<CombatReport, CombatInfo>(report);
        }

        public static IEnumerable<BriefUnitInfo> ToBriefUnitInfos(this IEnumerable<Division> divisions, IMapper mapper)
        {

        }
    }
}
