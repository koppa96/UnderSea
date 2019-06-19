using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Bll
{
    public class CountryLootedEventArgs : EventArgs
    {
        public long LootedCorals { get; }

        public long LootedPearls { get; }

        public Country Looter { get; }

        public Country Looted { get; }



        public CountryLootedEventArgs(long lootedCorals, long lootedPearls, Country looter, Country looted)
        {
            LootedCorals = lootedCorals;
            LootedPearls = lootedPearls;
            Looter = looter ?? throw new ArgumentNullException(nameof(looter));
            Looted = looted ?? throw new ArgumentNullException(nameof(looted));
        }
    }
}
