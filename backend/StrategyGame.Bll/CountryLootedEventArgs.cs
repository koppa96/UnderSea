using System;

namespace StrategyGame.Bll
{
    /// <summary>
    /// Event args class for events that happen when a country is looted by another.
    /// </summary>
    public class CountryLootedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the amount of corals acquired via looting.
        /// </summary>
        public long LootedCorals { get; }

        /// <summary>
        /// Gets the amount of pearls acquired via looting.
        /// </summary>
        public long LootedPearls { get; }

        /// <summary>
        /// Gets the looter country's ID.
        /// </summary>
        public int LooterId { get; }

        /// <summary>
        /// Gets the looted country's ID.
        /// </summary>
        public int LootedId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryLootedEventArgs"/>.
        /// </summary>
        /// <param name="lootedCorals">The amount of corals acquired via looting.</param>
        /// <param name="lootedPearls">The amount of pearls acquired via looting.</param>
        /// <param name="looterId">The looter country's ID.</param>
        /// <param name="lootedId">The looted country's ID.</param>
        public CountryLootedEventArgs(long lootedCorals, long lootedPearls, int looterId, int lootedId)
        {
            LootedCorals = lootedCorals;
            LootedPearls = lootedPearls;
            LooterId = looterId;
            LootedId = lootedId;
        }
    }
}