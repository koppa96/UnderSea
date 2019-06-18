using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Model.Entities.Effects
{
    public class DummyEffect : AbstractEffect
    {
        public override void Apply(Country target)
        {
            throw new NotImplementedException();
        }
    }
}
