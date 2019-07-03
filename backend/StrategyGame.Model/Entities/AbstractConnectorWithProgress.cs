namespace StrategyGame.Model.Entities
{
    public class AbstractConnectorWithProgress<TParent, TChild> : AbstractConnector<TParent, TChild>
        where TParent : AbstractEntity<TParent> where TChild : AbstractEntity<TChild>
    {
        public int TimeLeft { get; set; }
    }
}