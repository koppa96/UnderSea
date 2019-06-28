using StrategyGame.Model.Entities.Frontend;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Model.Entities.Resources
{
    public class ResourceType : AbstractEntity<ResourceType>
    {
        public ResourceContent Content { get; set; }

        public long StartingAmount { get; set; }

        public long NewCountryCost { get; set; }

        public ICollection<BuildingResource> BuildingResources { get; set; }

        public ICollection<ResearchResource> ResearchResources { get; set; }

        public ICollection<CountryResource> CountryResources { get; set; }

        public ICollection<UnitResource> UnitResources { get; set; }
    }
}
