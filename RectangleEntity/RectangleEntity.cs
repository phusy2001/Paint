using IContract;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace RectangleEntity
{
    public class RectangleEntity : IShapeEntity, ICloneable
    {
        public Point TopLeft { get; set; }
        public Point RightBottom { get; set; }
        public int Thickness { get; set; }
        public DoubleCollection DashArray { get; set; }
        public string Color { get; set; }

        public List<Point> ControlPoints { get; set;}
        public string Name => "Rectangle";

        public BitmapImage Icon => throw new NotImplementedException();

        public void SetControlPoints()
        {
            ControlPoints = new List<Point>();
            ControlPoints.Add(TopLeft);
            ControlPoints.Add(RightBottom);
            ControlPoints.Add(new Point(TopLeft.X, RightBottom.Y));
            ControlPoints.Add(new Point(RightBottom.X, TopLeft.Y));
        }

        public bool CheckNear(Point currPoint)
        {
            for (int i = 0; i < ControlPoints.Count; i++)
            {
                double dist = Math.Pow((ControlPoints[i].X - currPoint.X), 2) + Math.Pow((ControlPoints[i].Y - currPoint.Y), 2);
                int distance = (int)dist;
                if (distance < Math.Pow(10.0f, 2))
                {
                    return true;
                }
            }
            return false;
        }
        
        public void SetImageLink(String link) { throw new NotImplementedException(); }
        public void HandleStart(Point point)
        {
            TopLeft = point;
        }
        public void HandleEnd(Point point)
        {
            RightBottom = point;
        }
        public object Clone()
        {
            return MemberwiseClone();
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
