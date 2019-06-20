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

        public int LooterId { get; }

        public int LootedId { get; }



        public CountryLootedEventArgs(long lootedCorals, long lootedPearls, int looterId, int lootedId)
        {
            LootedCorals = lootedCorals;
            LootedPearls = lootedPearls;
            LooterId = looterId;
            LootedId = lootedId;
        }
    }
}
