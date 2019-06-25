using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Bll.Dto.Received.UserManagement
{
    public class PasswordChangeData
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
