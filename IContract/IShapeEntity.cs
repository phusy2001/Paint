using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace IContract
{
    public interface IShapeEntity : ICloneable
    {
        string Name { get; }
        BitmapImage Icon { get; }
        public int Thickness { get; set; }
        public string Color { get; set; }
        void SetImageLink(String link);
        void HandleStart(Point point);
        void HandleEnd(Point point);
        void SetThickness(int thickness);
        void SetStrokeColor(string color);
    }
}
