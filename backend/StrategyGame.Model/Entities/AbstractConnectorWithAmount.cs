namespace StrategyGame.Model.Entities
{
    public class AbstractConnectorWithAmount<TParent, TChild> : AbstractConnector<TParent, TChild>
        where TParent : AbstractEntity<TParent> where TChild : AbstractEntity<TChild>
    {
        public long Amount { get; set; }
    }
}