namespace StrategyGame.Model.Entities.Frontend
{
    /// <summary>
    /// Represents the content for a research object.
    /// </summary>
    public class EventContent : FrontendContent<RandomEvent>
    {
        /// <summary>
        /// Gets or sets the flavour text for the event.
        /// </summary>
        public string FlavourText { get; set; }
    }
}