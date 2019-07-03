namespace StrategyGame.Model.Entities.Resources
{
    /// <summary>
    /// Linking table between a country and a resource.
    /// </summary>
    public class CountryResource : AbstractConnectorWithAmount<Country, ResourceType>
    { }
}