namespace StrategyGame.Model.Entities
{
    public class AbstractConnector<TParent, TChild> : AbstractEntity<AbstractConnectorWithAmount<TParent, TChild>>
        where TParent : AbstractEntity<TParent> where TChild : AbstractEntity<TChild>
    {
        public TParent Parent { get; set; }
        public TChild Child { get; set; }
    }
}