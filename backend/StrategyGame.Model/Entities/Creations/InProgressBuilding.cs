namespace StrategyGame.Model.Entities.Creations
{
    /// <summary>
    /// Represents a building that is currently being built in a country within the UnderSea database.
    /// </summary>
    public class InProgressBuilding : AbstractConnectorWithProgress<Country, BuildingType>
    { }
}