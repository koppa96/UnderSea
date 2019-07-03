namespace StrategyGame.Bll.Dto.Sent.Country
{
    public class EventInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FlavorText { get; set; }
        public string ImageUrl { get; set; }
        public ulong Round { get; set; }
        public bool IsSeen { get; set; }
    }
}