using System;
using TowersOfHanoi.Models;

namespace TowersOfHanoi.Rules
{
    public class LargerDiscCannotBeOnTopOfSmallerDiscRule : IRule
    {
        private readonly Disc _movedDisc;
        private readonly Disc _belowDisc;

        public LargerDiscCannotBeOnTopOfSmallerDiscRule(Disc topDisc, Disc bottomDisc)
        {
            _movedDisc = topDisc ?? throw new ArgumentNullException(nameof(topDisc));
            _belowDisc = bottomDisc;
        }
        public bool Evaluate()
        {
            if (_belowDisc == null)
            {
                return true;
            }

            return _belowDisc.Weight < _movedDisc.Weight;
        }
    }
}