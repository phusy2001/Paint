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

namespace RectangleEntity
{
    public class IRectanglePainter : IPaintBusiness
    {
        public UIElement Draw(IShapeEntity shape)
        {
            var rectangle = shape as RectangleEntity;

            var x = Math.Min(rectangle.RightBottom.X, rectangle.TopLeft.X);
            var y = Math.Min(rectangle.RightBottom.Y, rectangle.TopLeft.Y);

            var width = Math.Max(rectangle.RightBottom.X, rectangle.TopLeft.X) - x;
            var height = Math.Max(rectangle.RightBottom.Y, rectangle.TopLeft.Y) - y;

            // TODO: chú ý việc đảo lại rightbottom và topleft 
            //double width = rectangle.RightBottom.X - rectangle.TopLeft.X;
            //double height = rectangle.RightBottom.Y - rectangle.TopLeft.Y;

            var element = new Rectangle()
            {
                Width = width,
                Height = height,
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Red)
            };
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);

            return element;
        }
    }
}
