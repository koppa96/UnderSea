using System;

namespace StrategyGame.Model.Entities.Frontend
{
    /// <summary>
    /// Represents the content for a unit object.
    /// </summary>
    public class UnitContent : AbstractFrontendContent<UnitType, UnitContent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitContent"/>.
        /// </summary>
        public UnitContent()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitType"/>.
        /// </summary>
        /// <param name="parent">The unit the content belongs to.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public UnitContent(UnitType parent)
            : base(parent)
        { }
    }
}