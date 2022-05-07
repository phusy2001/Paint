using IContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EllipseEntity
{
    public class IEllipsePainter : IPaintBusiness
    {
        public UIElement Draw(IShapeEntity shape)
        {
            var ellipse = shape as EllipseEntity;

            var x = Math.Min(ellipse.RightBottom.X, ellipse.TopLeft.X);
            var y = Math.Min(ellipse.RightBottom.Y, ellipse.TopLeft.Y);

            var width = Math.Max(ellipse.RightBottom.X, ellipse.TopLeft.X) - x;
            var height = Math.Max(ellipse.RightBottom.Y, ellipse.TopLeft.Y) - y;

            // TODO: chú ý việc đảo lại rightbottom và topleft 
            //double width = rectangle.RightBottom.X - rectangle.TopLeft.X;
            //double height = rectangle.RightBottom.Y - rectangle.TopLeft.Y;

            var element = new Ellipse()
            {
                Width = width,
                Height = height,
                StrokeThickness = ellipse.Thickness,
                //Stroke = new SolidColorBrush(Colors.Red)
                Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom(ellipse.Color),
                StrokeDashArray = ellipse.DashArray
            };
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);

            return element;
        }
    }
}
