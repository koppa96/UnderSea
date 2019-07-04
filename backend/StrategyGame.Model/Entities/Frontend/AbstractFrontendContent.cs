using System.Collections.Generic;

namespace StrategyGame.Model.Entities.Frontend
{
    /// <summary>
    /// Provides an abstract base for frontend content classes.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity used for type safe equality checks.</typeparam>
    /// <typeparam name="TParent">The type of the parent for the content.</typeparam>
    /// <remarks>
    /// The <typeparamref name="TEntity"/> generic type is used to provide type safe IEquatable support,
    /// and inheritors should specify their own type as it.
    /// </remarks>
    public abstract class AbstractFrontendContent<TParent> : AbstractEntity<AbstractFrontendContent<TParent>>
    {
        /// <summary>
        /// Gets the object the content belongs to.
        /// </summary>
        public ICollection<TParent> Parents { get; set; }

        /// <summary>
        /// Gets or sets the name for the content.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description for the content.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the image URL for the content.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the image url for the icon of the building.
        /// </summary>
        public string IconImageUrl { get; set; }
    }
}