using IContract;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageEntity
{
    public class ImageEntity : IShapeEntity, ICloneable
    {
        public Point TopLeft { get; set; }
        public Point RightBottom { get; set; }

        public string ImageUrl { get; set; }
        public string Name => "Image";
        public string Color { get; set; }
        public int Thickness { get; set; }
        public DoubleCollection DashArray { get; set; }
        public BitmapImage Icon => new BitmapImage(new Uri(ImageUrl, UriKind.Relative));
        public BitmapImage Picture;
        public object Clone()
        {
            return MemberwiseClone();
        }

        public void SetImageLink(String link)
        {
            ImageUrl = link;
        }

        public void HandleEnd(Point point)
        {
            TopLeft = point;
        }

        public void HandleStart(Point point)
        {
            RightBottom = point;
        }
        public void SetDashArray(DoubleCollection dasharray)
        {
            DashArray = dasharray;
        }
        public void SetThickness(int thickness)
        {
            Thickness = thickness;
        }
        public void SetStrokeColor(string color)
        {
            Color = color;
        }
    }
}
