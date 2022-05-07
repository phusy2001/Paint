using IContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageEntity
{
    internal class ImagePainter : IPaintBusiness
    {
        public UIElement Draw(IShapeEntity shape)
        {
            var image = shape as ImageEntity;
            var x = Math.Min(image.RightBottom.X, image.TopLeft.X);
            var y = Math.Min(image.RightBottom.Y, image.TopLeft.Y);

            var width = Math.Max(image.RightBottom.X, image.TopLeft.X) - x;
            var height = Math.Max(image.RightBottom.Y, image.TopLeft.Y) - y;

            Image? element = new Image()
            {
                Width = width,
                Height = height,
                Source = new BitmapImage(new Uri(image.ImageUrl, UriKind.Absolute)),
            };
            Canvas.SetTop(element, y);
            Canvas.SetLeft(element, x);
            return element;
        }
    }
}
