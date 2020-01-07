using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TowersOfHanoi.Controllers;

namespace TowersOfHanoi.Views
{
    public partial class TowerUserControl : UserControl, ITowerUserControl
    {
        private const int TowerWidth = 10;

        public TowerUserControl()
        {
            DiscController = new DiscController(this);
            TowerDragAndDropController = new TowerDragAndDropController(this);
            InitializeComponent();
        }

        [Browsable(true)]
        [DefaultValue(0)]
        [Category("Towers")]
        public int Id { get; set; } = 0;

        public IDiscController DiscController { get; }

        public TowerDragAndDropController TowerDragAndDropController { get; }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <remarks>
        ///     Moved the <c>Dispose</c> method to here to be able to dispose myself :^)
        /// </remarks>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                (DiscController as IDisposable)?.Dispose();
                TowerDragAndDropController.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            DrawTower(e.Graphics);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            Invalidate(ClientRectangle);
            foreach (var disc in DiscController.GetDiscs())
            {
                disc.UpdateDisc(ClientRectangle);
            }
        }

        private void DrawTower(Graphics graphics)
        {
            var middle = (ClientRectangle.Width - TowerWidth) / 2;
            graphics.FillRectangle(Brushes.Chocolate, middle, 0, TowerWidth, ClientRectangle.Height);
        }
    }
}