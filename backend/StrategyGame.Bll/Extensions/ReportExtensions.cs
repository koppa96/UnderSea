using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Connectors;
using StrategyGame.Model.Entities.Creations;
using StrategyGame.Model.Entities.Frontend;
using StrategyGame.Model.Entities.Reports;
using StrategyGame.Model.Entities.Units;
using System.Collections.Generic;
using System.Linq;

namespace StrategyGame.Bll.Extensions
{
    public static class ReportExtensions
    {
        public static CombatInfo ToAttackerCombatInfo(this CombatReport report, IMapper mapper)
        {
            var combatInfo = mapper.Map<CombatReport, CombatInfo>(report);
            var spyCount = report.Attackers.Count(d => d.Unit is SpyType);
            var enemySpyCount = report.Defenders.Count(d => d.Unit is SpyType);

            combatInfo.IsAttack = true;
            combatInfo.IsWon = report.DidAttackerWin;
            combatInfo.IsSeen = report.IsSeenByAttacker;
            combatInfo.EnemyCountryId = report.Defender.Id;
            combatInfo.EnemyCountryName = report.Defender.Name;

            combatInfo.YourUnits = report.Attackers.ToBriefUnitInfos(mapper);
            combatInfo.YourLostUnits = report.AttackerLosses.ToBriefUnitInfos(mapper);

            combatInfo.Loot = report.Loot.Select(mapper.Map<ReportResource, ResourceInfo>);

            if (spyCount > enemySpyCount)
            {
                combatInfo.EnemyUnits = report.Defenders.ToBriefUnitInfos(mapper);
                combatInfo.EnemyLostUnits = report.Defenders.ToBriefUnitInfos(mapper);

                if (spyCount > 1.2 * enemySpyCount)
                {
                    combatInfo.RemainingResources = report.Loot.Select(r =>
                    {
                        var resourceInfo = mapper.Map<ReportResource, ResourceInfo>(r);
                        resourceInfo.Amount = r.RemainingAmount;
                        return resourceInfo;
                    });

                    if (spyCount > 1.5 * enemySpyCount)
                    {
                        combatInfo.Buildings = report.DefenderBuildings
                            .Select(b => mapper.Map<BuildingType, BriefCreationInfo>(b.Child));

                        if (spyCount > 2 * enemySpyCount)
                        {
                            combatInfo.Researches = report.DefenderResearches
                                .Select(r => mapper.Map<ResearchType, BriefCreationInfo>(r.Child));
                        }
                    }
                }
            }

            return combatInfo;
        }

        public static CombatInfo ToDefenderCombatInfo(this CombatReport report, IMapper mapper)
        {
            var combatInfo = mapper.Map<CombatReport, CombatInfo>(report);

            combatInfo.IsAttack = false;
            combatInfo.IsWon = !report.DidAttackerWin;
            combatInfo.IsSeen = report.IsSeenByDefender;
            combatInfo.EnemyCountryId = report.Attacker.Id;
            combatInfo.EnemyCountryName = report.Attacker.Name;

            combatInfo.YourUnits = report.Defenders.ToBriefUnitInfos(mapper);
            combatInfo.YourLostUnits = report.DefenderLosses.ToBriefUnitInfos(mapper);
            combatInfo.EnemyUnits = report.Attackers.ToBriefUnitInfos(mapper);
            combatInfo.EnemyLostUnits = report.AttackerLosses.ToBriefUnitInfos(mapper);

            combatInfo.Loot = report.Loot.Select(mapper.Map<ReportResource, ResourceInfo>);
            combatInfo.RemainingResources = report.Loot.Select(r =>
            {
                var resourceInfo = mapper.Map<ReportResource, ResourceInfo>(r);
                resourceInfo.Amount = r.RemainingAmount;
                return resourceInfo;
            });

            combatInfo.Buildings = report.DefenderBuildings.Select(r => r.ToBriefCreationInfo<CombatReport, BuildingType, ConnectorWithAmount<BuildingType, ResourceType>, BuildingContent>());
            combatInfo.Researches = report.DefenderResearches.Select(r => r.ToBriefCreationInfo<CombatReport, ResearchType, ConnectorWithAmount<ResearchType, ResourceType>, ResearchContent>());

            return combatInfo;
        }

        public static IEnumerable<BriefUnitInfo> ToBriefUnitInfos(this IEnumerable<Division> divisions, IMapper mapper)
        {
            return divisions.Select(mapper.Map<Division, BriefUnitInfo>);
        }

        public static BriefCreationInfo ToBriefCreationInfo<TEntity, TCreation, TConnector, TContent>(this ConnectorWithAmount<TEntity, TCreation> connector)
            where TEntity : AbstractEntity<TEntity>
            where TCreation : AbstractCreationType<TCreation, TConnector, TContent>
            where TConnector : ConnectorWithAmount<TCreation, ResourceType>
            where TContent: AbstractFrontendContent<TCreation>
        {
            return new BriefCreationInfo
            {
                Id = connector.Child.Id,
                Count = (int)connector.Amount,
                IconImageUrl = connector.Child.Content.IconImageUrl,
                ImageUrl = connector.Child.Content.ImageUrl,
                InProgressCount = 0
            };
        }
    }
}
