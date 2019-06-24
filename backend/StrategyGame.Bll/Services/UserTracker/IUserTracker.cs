using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Bll.Services.UserTracker
{
    public interface IUserTracker
    {
        IEnumerable<string> GetConnectedUsers();
        void AddUser(string username);
        void RemoveUser(string username);
    }
}
