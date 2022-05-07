using IContract;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace LineEntity
{
    public class LineEntity : IShapeEntity, ICloneable
    {
        public Point Start { get; set; }
        public Point End { get; set; }
        public string Color { get; set; }
        public int Thickness { get; set; }    
        public DoubleCollection DashArray { get; set; }
        public string Name => "Line";

        public BitmapImage Icon => new BitmapImage(new Uri("", UriKind.Relative));

        public void SetImageLink(String link) { throw new NotImplementedException(); }
        public object Clone()
        {
            return MemberwiseClone();
        }

        public void HandleEnd(Point point)
        {
            End = point;
        }

        public void HandleStart(Point point)
        {
            Start = point;
        }

        public void SetThickness(int thickness)
        {
            Thickness = thickness;
        }

        public void SetDashArray(DoubleCollection dasharray)
        {
            DashArray = dasharray;
        }
        public void SetStrokeColor(string color)
        {
            Color = color;
        }
    }
}
