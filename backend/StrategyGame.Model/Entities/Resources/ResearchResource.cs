using StrategyGame.Model.Entities.Creations;

namespace StrategyGame.Model.Entities.Resources
{
    /// <summary>
    /// Linking table between a research and a resource.
    /// </summary>
    public class ResearchResource : AbstractConnectorWithAmount<ResearchType, ResourceType>
    { }
}