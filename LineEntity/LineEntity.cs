using IContract;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;


namespace LineEntity
{
    public class LineEntity : IShapeEntity, ICloneable
    {
        public Point Start { get; set; }
        public Point End { get; set; }
        public string Color { get; set; }
        public int Thickness { get; set; }
        public List<Point> ControlPoints { get; set; }
        public string Name => "Line";

        public void SetControlPoints()
        {
            ControlPoints = new List<Point>();
            ControlPoints.Add(Start);
            ControlPoints.Add(End);
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
        public void SetStrokeColor(string color)
        {
            Color = color;
        }
    }
}
