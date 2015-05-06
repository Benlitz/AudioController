using System;
using System.Windows.Media;

namespace AudioController
{
    public class ColorViewModel : IComparable<ColorViewModel>
    {
        public ColorViewModel(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        public string Name { get; private set; }

        public Color Color { get; private set; }

        public int CompareTo(ColorViewModel other)
        {
            return string.Compare(Name, other.Name, StringComparison.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}