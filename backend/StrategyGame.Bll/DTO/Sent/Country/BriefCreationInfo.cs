﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Dto.Sent.Country
{
    public class BriefCreationInfo
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int InProgressCount { get; set; }
        public string ImageUrl { get; set; }
    }
}
