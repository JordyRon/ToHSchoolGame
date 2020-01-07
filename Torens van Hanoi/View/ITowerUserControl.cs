using System.Drawing;
using System.Windows.Forms;
using TowersOfHanoi.Controllers;

namespace TowersOfHanoi.Views
{
    public interface ITowerUserControl
    {
        int Id { get; }
        Control.ControlCollection Controls { get; }
        Rectangle ClientRectangle { get; }
        IDiscController DiscController { get; }
        void SuspendLayout();
        void ResumeLayout();
        event DragEventHandler DragEnter;
        event DragEventHandler DragDrop;
    }
}