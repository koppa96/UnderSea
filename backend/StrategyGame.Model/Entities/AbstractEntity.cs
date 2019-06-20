using System;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents an abstract base entity in the UnderSea database.
    /// </summary>
    /// <typeparam name="T">The type of the entity used for type safe equality checks.</typeparam>
    /// <remarks>
    /// The generic type is used to provide type safe IEquatable support,
    /// and inheritors should specify their own type as the generic type.
    /// </remarks>
    public class AbstractEntity<T> : IEquatable<AbstractEntity<T>> where T : AbstractEntity<T>
    {
        /// <summary>
        /// Gets or sets the ID of the entity.
        /// </summary>
        public int Id { get; protected internal set; }

        /// <summary>
        /// Gets the hashcode of this <see cref="INingyoEntity{T}"/>.
        /// </summary>
        /// <returns>The hashcode.</returns>
        public override int GetHashCode() => Id.GetHashCode();

        /// <summary>
        /// Decides if this instance of <see cref="INingyoEntity{T}"/> is equal to another.
        /// </summary>
        /// <param name="Other">The other <see cref="object"/>.</param>
        /// <returns>If they were equal.</returns>
        public override bool Equals(object Other) => Other is T entity ? Equals(entity) : false;

        /// <summary>
        /// Decides if this instance of <see cref="AbstractEntity{T}"/> is equal to another.
        /// </summary>
        /// <param name="Other">The other <see cref="AbstractEntity{T}"/>.</param>
        /// <returns>If they were equal.</returns>
        public bool Equals(AbstractEntity<T> Other) => Other is null ? false : Id.Equals(Other.Id);
    }
}