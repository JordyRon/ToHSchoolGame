using System.Collections.Generic;
using System.Linq;
using TowersOfHanoi.Models;

namespace TowersOfHanoi.Rules
{
    public class TowerIsConeShapedLevelRule : IRule
    {
        private readonly IEnumerable<Disc> _discs;
        private readonly LevelsMustBeUniqueRule _rule;

        public TowerIsConeShapedLevelRule(IEnumerable<Disc> discs)
        {
            _discs = discs;
            _rule = new LevelsMustBeUniqueRule(_discs);
        }

        public bool Evaluate()
        {
            var orderedDiscs = _discs.OrderBy(d => d.Weight);
            return _rule.Evaluate() && _discs.SequenceEqual(orderedDiscs);
        }
    }
}