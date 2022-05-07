using IContract;
using System;
using System.Collections.Generic;
using System.Windows;
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
        public BitmapImage Icon => new BitmapImage(new Uri(ImageUrl, UriKind.Relative));
        public BitmapImage Picture;
        public List<Point> ControlPoints { get; set; }

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
