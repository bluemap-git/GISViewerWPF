using System;
using System.Collections.Generic;
using System.Linq;
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
using OpenTK.Graphics.Wgl;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Wpf;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;


namespace GISViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var settings = new GLWpfControlSettings
            {
                MajorVersion = 3,
                MinorVersion = 2
            };
            OpenTkControl.Start(settings);
        }

        private void OpenTkControl_OnRender(TimeSpan obj)
        {
            GL.ClearColor(Color4.Blue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        }
    }
}
