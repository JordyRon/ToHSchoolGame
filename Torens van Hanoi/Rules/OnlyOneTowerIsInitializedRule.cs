using System.Collections.Generic;
using System.Linq;
using TowersOfHanoi.Models;

namespace TowersOfHanoi.Rules
{
    public class OnlyOneTowerIsInitializedRule : IRule
    {
        private readonly IList<IList<Disc>> _towers;

        public OnlyOneTowerIsInitializedRule(IList<IList<Disc>> towers)
        {
            _towers = towers;
        }
        public bool Evaluate()
        {
            return _towers.Count(t => t.Any()) == 1;
        }
    }
}