using System.Collections.Generic;
using System.Linq;
using TowersOfHanoi.Models;

namespace TowersOfHanoi.Rules
{
    public class LevelsMustBeUniqueRule : IRule
    {
        private readonly IEnumerable<Disc> _discs;

        public LevelsMustBeUniqueRule(IEnumerable<Disc> discs)
        {
            _discs = discs;
        }

        public bool Evaluate() =>
            _discs.ToLookup(x => x.Weight).All(x => x.Count() == 1);
    }
}