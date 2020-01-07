using System.Drawing;

namespace TowersOfHanoi.Models
{
    public class Disc
    {
        public Disc(int weight, Color color)
        {
            Weight = weight;
            Color = color;
        }

        public int Weight { get; }

        public Color Color { get; }
    }
}