using System.Drawing;

namespace TowersOfHanoi.Views
{
    public interface IDisc
    {
        int Weight { get; set; }
        int Order { get; set; }
        void UpdateDisc(Rectangle rectangle);
    }
}