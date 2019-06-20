namespace StrategyGame.Api.Controllers
{
    public static class ErrorMessages
    {
        public const string NotFound = "Not Found";
        public const string BadRequest = "Bad Request";
        public const string Unauthorized = "Unauthorized";

        public const string NotEnoughMoney = "Not enough money.";
        public const string NotEnoughUnits = "Not enough units.";

        public const string NoSuchCommand = "Command not found.";
        public const string NoSuchBuilding = "Building type not found.";
        public const string NoSuchResearch = "Research type not found.";
        public const string NoSuchUnit = "Unit type not found.";
        public const string InvalidAmount = "Invalid amount.";
        public const string LimitReached = "The limit is reached.";
        public const string InProgress = "Can not start new progress until the last one is finished.";
        public const string CannotChangeCommand = "Can not delete or modify command of others.";
    }
}