using StrategyGame.Model.Entities.Creations;

namespace StrategyGame.Model.Entities.Resources
{
    /// <summary>
    /// Linking table between a building and a resource.
    /// </summary>
    public class BuildingResource : AbstractConnectorWithAmount<BuildingType, ResourceType>
    { }
}