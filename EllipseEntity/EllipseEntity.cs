using IContract;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace EllipseEntity
{
    public class EllipseEntity : IShapeEntity, ICloneable
    {
        public Point TopLeft { get; set; }
        public Point RightBottom { get; set; }
        public int Thickness { get; set; }
        public string Color { get; set; }

        public string Name => "Ellipse";

        public BitmapImage Icon => throw new NotImplementedException();

        public void HandleStart(Point point)
        {
            TopLeft = point;
        }
        public void HandleEnd(Point point)
        {
            RightBottom = point;
        }
        public void SetThickness(int thickness)
        {
            Thickness = thickness;
        }
        public void SetStrokeColor(string color)
        {
            Color = color;
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
