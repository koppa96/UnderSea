using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrategyGame.Bll.Services.UserTracker
{
    public class UserTracker : IUserTracker
    {
        private readonly SynchronizedCollection<string> _users;

        public UserTracker()
        {
            _users = new SynchronizedCollection<string>();
        }

        public void AddUser(string username)
        {
            _users.Add(username);
        }

        public IEnumerable<string> GetConnectedUsers()
        {
            return _users.ToList();
        }

        public void RemoveUser(string username)
        {
            _users.Remove(username);
        }
    }
}
