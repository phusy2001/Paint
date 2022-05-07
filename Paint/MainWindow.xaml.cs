using IContract;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // State
        bool _isDrawing = false;
        string _currentType = "";
         IShapeEntity _preview = null;
        Point _start;
        List<IShapeEntity> _drawnShapes = new List<IShapeEntity>();

        //Support zooming 
        private Double zoomMax = 5;
        private Double zoomMin = 0.5;
        private Double zoomSpeed = 0.001;
        private Double zoom = 1;

        // Cấu hình
        Dictionary<string, IPaintBusiness> _painterPrototypes = new Dictionary<string, IPaintBusiness>();
        Dictionary<string, IShapeEntity> _shapesPrototypes = new Dictionary<string, IShapeEntity>();

        Stack<IShapeEntity> _shapesStack = new Stack<IShapeEntity>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /* Nạp tất cả dll, tìm kiếm entity và business */
            var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            var folderInfo = new DirectoryInfo(exeFolder);
            var dllFiles = folderInfo.GetFiles("*.dll");

            foreach (var dll in dllFiles)
            {
                Assembly assembly = Assembly.LoadFrom(dll.FullName);

                 Type[] types = assembly.GetTypes();

                // Giả định: 1 dll chỉ có 1 entity và 1 business tương ứng
                IShapeEntity? entity = null;
                IPaintBusiness? business = null;

                foreach (Type type in types)
                {
                    if (type.IsClass)
                    {
                        if (typeof(IShapeEntity).IsAssignableFrom(type))
                        {
                            entity = (Activator.CreateInstance(type) as IShapeEntity)!;
                        }

                        if (typeof(IPaintBusiness).IsAssignableFrom(type))
                        {
                            business = (Activator.CreateInstance(type) as IPaintBusiness)!;
                        }
                    }
                }

                //var img = new Bitmap
                if (entity != null)
                {
                    _shapesPrototypes.Add(entity!.Name, entity);
                    _painterPrototypes.Add(entity!.Name, business!);
                }
            }

            Title = $"Tìm thấy {_shapesPrototypes.Count} hình";

            // Tạo ra các nút bấm tương ứng
            foreach (var (name, entity) in _shapesPrototypes)
            {
                var button = new Button();
                //if (name == "Image")
                //{
                //    button.Content = name;
                //    button.Tag = entity;
                //    button.Width = 80;
                //    button.Height = 35;
                //    button.Click += Button_OpenFile_Click;
                //}
                if (name != "Image")
                {
                    button.Content = name;
                    button.Tag = entity;
                    button.Width = 80;
                    button.Height = 35;
                    button.Click += Button_Click;
                    actionsStackPanel.Children.Add(button);
                }

                //TODO: thêm các nút bấm vào giao diện
            }

            if (_shapesPrototypes.Count > 0)
            {
                //Lựa chọn nút bấm đầu tiên
                var (key, shape) = _shapesPrototypes.ElementAt(0);
                _currentType = key;
                _preview = (shape.Clone() as IShapeEntity)!;
            }

        }

        // Đổi lựa chọn
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var entity = button!.Tag as IShapeEntity;

            _currentType = entity!.Name;
            _preview = (_shapesPrototypes[entity.Name].Clone() as IShapeEntity)!;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = true;
            _start = e.GetPosition(canvas);

            _preview.HandleStart(_start);
            _preview.SetThickness((int)ThicknessSlider.Value);
            _preview.SetStrokeColor(ColorPicker.SelectedColor.ToString());
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                var end = e.GetPosition(canvas);
                _preview.HandleEnd(end);


                ReDraw();

                var previewPainter = _painterPrototypes[_preview.Name];
                var previewElement = previewPainter.Draw(_preview);
                canvas.Children.Add(previewElement);
            }
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false;

            var end = e.GetPosition(canvas); // Điểm kết thúc

            _preview.HandleEnd(end);

            _drawnShapes.Add(_preview.Clone() as IShapeEntity);
        }

        FileStream myfileStream = null;
        OpenFileDialog openFileDialog1 = new OpenFileDialog
        {
            InitialDirectory = Directory.GetCurrentDirectory(),
            Filter = "All files (*.*)|*.*|PNG (*.png)|*.png",
            FilterIndex = 2,
            RestoreDirectory = true
        };

        SaveFileDialog saveFileDialog1 = new SaveFileDialog
        {
            InitialDirectory = Directory.GetCurrentDirectory(),
            Filter = "JPEG (*.jpg)|*.jpg|BMP (*.bmp*)|*.bmp|PNG (*.png)|*.png|GIF (*.gif)|*.gif",
            FilterIndex = 2,
            RestoreDirectory = true
        };

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas.RenderSize.Width,
            (int)canvas.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvas);

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var fs = System.IO.File.OpenWrite("filebuff.bmp")) 
            {
                pngEncoder.Save(fs);
                MessageBox.Show("Picture was successfully saved in default directory with name 'filebuff.bmp'!");
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == true)
            {
                ImageBrush brush = new ImageBrush();
                //string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                //brush.ImageSource = new BitmapImage(new Uri(directoryPath, UriKind.RelativeOrAbsolute));
                brush.ImageSource = new BitmapImage(new Uri(openFileDialog1.FileName, UriKind.Relative));
                canvas.Background = brush;

                //using (var fileStream = new FileStream(directoryPath, FileMode.Create))
                //{
                //    ImageBrush brush = new ImageBrush();
                //    brush.ImageSource = new BitmapImage(new Uri(directoryPath, UriKind.RelativeOrAbsolute));
                //    canvas.Background = brush;
                //}
            }
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {

            if (saveFileDialog1.ShowDialog() == true)
            {
                try
                {
                    myfileStream = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                    RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96d, 96d, PixelFormats.Default);
                    rtb.Render(canvas);

                    PngBitmapEncoder pngEnc = new PngBitmapEncoder();
                    pngEnc.Frames.Add(BitmapFrame.Create(rtb));
                    pngEnc.Save(myfileStream);
                    MessageBox.Show("Picture was successfully exported!");
                }
                catch
                {
                    MessageBox.Show("Unable to export file. Check the path.");
                }
                finally
                {
                    myfileStream.Close();
                }
            }
        }

        private void PortableColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            //SelectedColor dependency property stores the current color as System.Windows.Media.Color
            //ColorChanged: An event that fires on SelectedColor change.
            //ColorState dependency property contains all info about the current state of the control.Use this property to bind controls together.

           curColor.Text = ColorPicker.SelectedColor.ToString();
        }

        private void DeleteAllbtn_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            _drawnShapes.Clear();
        }


        private void border_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            zoom += zoomSpeed * e.Delta; // Ajust zooming speed (e.Delta = Mouse spin value )
            if (zoom < zoomMin) { zoom = zoomMin; } // Limit Min Scale
            if (zoom > zoomMax) { zoom = zoomMax; } // Limit Max Scale

            Point mousePos = e.GetPosition(canvas);

            if (zoom > 1)
            {
                // transform Canvas size from mouse position
                canvas.RenderTransform = new ScaleTransform(zoom, zoom, mousePos.X, mousePos.Y);
            }
            else
            {
                // transform Canvas size
                canvas.RenderTransform = new ScaleTransform(zoom, zoom);
            }
        }

        private void Button_OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var button = sender as Button;
                var entity = button!.Tag as IShapeEntity;

                _currentType = entity!.Name;
                _preview = (_shapesPrototypes[entity.Name].Clone() as IShapeEntity)!;
                _preview.SetImageLink(openFileDialog.FileName);
            }

        }

        private void ReDraw()
        {
            canvas.Children.Clear(); // Xóa đi toàn bộ

            // Vẽ lại những hình đã vẽ trước đó
            foreach (var item in _drawnShapes)
            {
                var painter = _painterPrototypes[item.Name];
                var shape = painter.Draw(item);

                canvas.Children.Add(shape);
            }
        }
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if(_drawnShapes.Count > 0)
            {
                var lastItem = _drawnShapes.Last();
                _shapesStack.Push(lastItem);
                _drawnShapes.Remove(lastItem);
                ReDraw();
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if(_shapesStack.Count > 0)
            {
                var topStack = _shapesStack.Pop();
                _drawnShapes.Add(topStack);
                ReDraw();
            } 
        }
    }
}
