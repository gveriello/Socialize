using System.Linq;
using System.Windows.Media;

namespace UnifyMe.Core.Classes
{
    public class ColorInfo
    {
        public string ColorName { get; set; }
        public Color Color { get; set; }

        public SolidColorBrush SampleBrush
        {
            get { return new SolidColorBrush(Color); }
        }
        public string HexValue
        {
            get { return Color.ToString(); }
        }
        public ColorInfo() { }
        public ColorInfo(string color_name, Color color)
        {
            ColorName = color_name;
            if (color_name == null)
                ColorName = this.GetNameOfColor(color);
            Color = color;
        }
        public string GetNameOfColor(Color color)
        {
            var colorProperty = typeof(Colors).GetProperties().FirstOrDefault(p =>
                (Color)(p.GetValue(p, null)) == color);
            return (colorProperty != null) ? colorProperty.Name : color.ToString();
        }
    }
}
