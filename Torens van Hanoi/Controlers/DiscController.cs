using System;
using System.Drawing;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using TowersOfHanoi.Models;
using TowersOfHanoi.Views;

namespace TowersOfHanoi.Controllers
{
    public sealed class DiscController : IDiscController, IDisposable
    {
        private static readonly CompositeDisposable Subscriptions = new CompositeDisposable();
        private readonly ITowerUserControl _towerUserControl;

        public DiscController(ITowerUserControl towerUserControl)
        {
            _towerUserControl = towerUserControl ?? throw new ArgumentNullException(nameof(towerUserControl));
            GameEngine.Instance.DiscAdded
                .Merge(GameEngine.Instance.DiscRemoved)
                .Where(x => x.Tower == _towerUserControl.Id)
                .Subscribe(x => DrawTower(x.Tower))
                .DisposeWith(Subscriptions);
        }

        public DiscPanel[] GetDiscs()
        {
            return _towerUserControl.Controls.OfType<DiscPanel>().ToArray();
        }

        public DiscPanel GetLastDiscFromDiscParent(DiscPanel disc)
        {
            var parent = disc.Parent as ITowerUserControl;
            var lastPanel = parent?.DiscController.GetDiscs().LastOrDefault();
            return lastPanel;
        }

        public void Dispose()
        {
            Subscriptions.Dispose();
        }

        private void AddDisc(Color color, int order, int weight = 1)
        {
            var disc = new DiscPanel
            {
                Weight = weight,
                Order = order,
                BackColor = color
            };
            disc.UpdateDisc(_towerUserControl.ClientRectangle);

            _towerUserControl.Controls.Add(disc);
        }

        private void DrawTower(int towerIndex)
        {
            try
            {
                _towerUserControl.SuspendLayout();
                _towerUserControl.Controls.Clear();
                var tower = GameEngine.Instance.Towers[towerIndex];
                for (var i = 0; i < tower.Count; i++)
                {
                    var disc = tower[i];
                    AddDisc(disc.Color, i + 1, disc.Weight);
                }
            }
            finally
            {
                _towerUserControl.ResumeLayout();
            }
        }
    }
}