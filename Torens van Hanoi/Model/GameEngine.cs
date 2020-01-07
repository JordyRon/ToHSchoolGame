using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using TowersOfHanoi.Properties;
using TowersOfHanoi.Rules;
using TowersOfHanoi.Views;

namespace TowersOfHanoi.Models
{
    public sealed class GameEngine : IValidatableObject
    {
        private IList<Disc>[] _endTowers;
        private IList<Disc> _beginTower;
      
        private readonly Subject<(int Tower, Disc Disc)> _discAddedSubject = new Subject<(int Tower, Disc Disc)>();
        private readonly Subject<(int Tower, Disc Disc)> _discRemovedSubject = new Subject<(int Tower, Disc Disc)>();
        private readonly Subject<(int From, int To)> _discMovedSubject = new Subject<(int From, int To)>();
        private readonly Subject<ValidationResult> _validationAddedSubject = new Subject<ValidationResult>();

        private GameEngine() => Reset();

        public static GameEngine Instance { get; } = new GameEngine();

        public int AmountOfDiscs => Towers.Sum(t => t.Count);

        public int Moves { get; private set; }

        public IList<IList<Disc>> Towers { get; private set; }

        public IObservable<(int Tower, Disc Disc)> DiscAdded => _discAddedSubject.AsObservable();

        public IObservable<(int Tower, Disc Disc)> DiscRemoved => _discRemovedSubject.AsObservable();

        public IObservable<(int From, int To)> DiscMoved => _discMovedSubject.AsObservable();

        public IObservable<ValidationResult> ValidationAdded => _validationAddedSubject.AsObservable();

        public bool HasWon()
        {
            if (!new OnlyOneTowerIsInitializedRule(_endTowers).Evaluate())
            {
                return false;
            }

            var tower = _endTowers.FirstOrDefault(t => t.Count == AmountOfDiscs);
            if (tower == null)
            {
                return false;
            }

            if (!new TowerIsConeShapedLevelRule(tower).Evaluate())
            {
                return false;
            }

            return !_beginTower.Any();
        }

        public void MoveDisc(int fromTowerIndex, int toTowerIndex)
        {
            if (fromTowerIndex == toTowerIndex)
            {
                _validationAddedSubject.OnNext(new ValidationResult(Resources.TowerIndexesCannotBeTheSame));
                return;
            }

            if (fromTowerIndex < 0 || fromTowerIndex > 2)
            {
                var errorMessage = string.Format(Resources.TowerIndexOutOfRange, nameof(fromTowerIndex));
                var validationResult = new ValidationResult(errorMessage);
                _validationAddedSubject.OnNext(validationResult);
                return;
            }

            if (toTowerIndex < 0 || toTowerIndex > 2)
            {
                var errorMessage = string.Format(Resources.TowerIndexOutOfRange, nameof(toTowerIndex));
                var validationResult = new ValidationResult(errorMessage);
                _validationAddedSubject.OnNext(validationResult);
                return;
            }

            var fromTower = Towers[fromTowerIndex];
            var toTower = Towers[toTowerIndex];
            var fromDisc = fromTower.LastOrDefault();
            var toDisc = toTower.LastOrDefault();
            var largerDiscCannotBeOnTopOfSmallerDiscRule = new LargerDiscCannotBeOnTopOfSmallerDiscRule(fromDisc, toDisc);
            if (largerDiscCannotBeOnTopOfSmallerDiscRule.Evaluate())
            {
                fromTower.Remove(fromDisc);
                toTower.Add(fromDisc);

                Moves++;
                _discRemovedSubject.OnNext((fromTowerIndex, fromDisc));
                _discAddedSubject.OnNext((toTowerIndex, fromDisc));
                _discMovedSubject.OnNext((fromTowerIndex, toTowerIndex));
            }
            else
            {
                var validationResult = new ValidationResult(Resources.LargerDiscCannotBePutOnTopOfSmallerDisc);
                _validationAddedSubject.OnNext(validationResult);
            }

        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!new OnlyOneTowerIsInitializedRule(Towers).Evaluate())
            {
                yield return new ValidationResult(Resources.MoreThanOneTowerIsInitializedWithDiscs);
            }

            if (!new TowerIsConeShapedLevelRule(_beginTower).Evaluate())
            {
                yield return new ValidationResult(Resources.TowerIsNotConeShaped);
            }

            yield return ValidationResult.Success;
        }

        public void AddDisc(int tower, int level, Color color)
        {
            if (tower < 0 || tower >= Towers.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(tower));
            }

            var disc = new Disc(level, color);
            Towers[tower].Add(disc);
            _discAddedSubject.OnNext((tower, disc));
        }

        public void Reset()
        {
            Moves = 0;
            Towers = new IList<Disc>[]
            {
                new List<Disc>(),
                new List<Disc>(),
                new List<Disc>(),
            };
            _beginTower = Towers.First();
            _endTowers = Towers.Skip(1).ToArray();
        }
    }
}