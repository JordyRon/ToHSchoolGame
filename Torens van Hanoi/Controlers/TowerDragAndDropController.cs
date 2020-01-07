using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;
using TowersOfHanoi.Models;
using TowersOfHanoi.Views;

namespace TowersOfHanoi.Controllers
{
    public sealed class TowerDragAndDropController : IDisposable
    {
        private static readonly CompositeDisposable Subscriptions = new CompositeDisposable();

        public TowerDragAndDropController(ITowerUserControl towerUserControl)
        {
            if (towerUserControl == null)
            {
                throw new ArgumentNullException(nameof(towerUserControl));
            }

            var dragEnterObservable = Observable.FromEventPattern<DragEventHandler, DragEventArgs>(
                h => towerUserControl.DragEnter += h,
                h => towerUserControl.DragEnter -= h);
            dragEnterObservable
                .Select(x => x.EventArgs)
                .Subscribe(e =>
                {
                    var disc = (DiscPanel)e.Data.GetData(typeof(DiscPanel));
                    var lastPanel = towerUserControl.DiscController.GetLastDiscFromDiscParent(disc);
                    e.Effect = Equals(towerUserControl, disc.Parent) || !Equals(lastPanel, disc)
                        ? DragDropEffects.None
                        : DragDropEffects.Copy;
                })
                .DisposeWith(Subscriptions);

            var dragDropObservable = Observable.FromEventPattern<DragEventHandler, DragEventArgs>(
                h => towerUserControl.DragDrop += h,
                h => towerUserControl.DragDrop -= h);
            dragDropObservable
                .Select(x => x.EventArgs)
                .Subscribe(e =>
                {
                    var disc = (DiscPanel)e.Data.GetData(typeof(DiscPanel));
                    var lastPanel = towerUserControl.DiscController.GetLastDiscFromDiscParent(disc);
                    if (Equals(lastPanel, disc))
                    {
                        var fromTower = (ITowerUserControl)disc.Parent;
                        GameEngine.Instance.MoveDisc(fromTower.Id, towerUserControl.Id);
                    }
                })
                .DisposeWith(Subscriptions);
        }

        public void Dispose()
        {
            Subscriptions.Dispose();
        }
    }
}