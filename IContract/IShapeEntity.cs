using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IContract
{
    public interface IShapeEntity : ICloneable
    {
        string Name { get; }

        List<Point> ControlPoints { get; set; }
        BitmapImage Icon { get; }
        public int Thickness { get; set; }
        public string Color { get; set; }
        void SetImageLink(String link);
        void HandleStart(Point point);
        void HandleEnd(Point point);
        void SetThickness(int thickness);
        void SetDashArray(DoubleCollection dasharray);
        void SetStrokeColor(string color);

        bool CheckNear(Point currPoint);
        void SetControlPoints();
    }
}
