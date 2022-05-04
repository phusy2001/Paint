using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace IContract
{
    public interface IShapeEntity : ICloneable
    {
        string Name { get; }
        BitmapImage Icon { get; }

        void HandleStart(Point point);
        void HandleEnd(Point point);
    }
}
