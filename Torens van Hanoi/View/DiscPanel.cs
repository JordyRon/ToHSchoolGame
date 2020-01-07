using System;
using System.Drawing;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace TowersOfHanoi.Views
{
    public class DiscPanel : Panel, IDisc
    {
        private const int DiscHeight = 25;
        private const int Bumper = 5;
        private static readonly CompositeDisposable Subscriptions = new CompositeDisposable();

        public DiscPanel()
        {
            var mouseMoveObservable = Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
                h => MouseDown += h,
                h => MouseDown -= h);
            mouseMoveObservable
                .Select(e => (Panel)e.Sender)
                .Subscribe(p => p.DoDragDrop(p, DragDropEffects.Copy))
                .DisposeWith(Subscriptions);
        }

        public int Weight { get; set; } = 1;

        public int Order { get; set; } = 1;

        public void UpdateDisc(Rectangle rectangle)
        {
            if (TryCalculateIndent(Weight, out var indent))
            {
                Left = indent;
                Top = rectangle.Height - DiscHeight * Order;
                Width = rectangle.Width - indent * 2;
                Height = DiscHeight;
            }
        }

        private bool TryCalculateIndent(int level, out int indent)
        {
            indent = Bumper * (level - 1);
            return indent >= 0 && indent * 2 <= ClientRectangle.Width;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Subscriptions.Dispose();
        }
    }
}