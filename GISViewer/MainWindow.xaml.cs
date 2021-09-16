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
using System.IO;
using System.Diagnostics;


namespace GISViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //전역 객체 설정
        int m_SHXFileSize = 0;
        int m_SHXRecordSize = 0;


        public MainWindow()
        {
            InitializeComponent();
            var settings = new GLWpfControlSettings
            {
                MajorVersion = 3,
                MinorVersion = 2
            };
            OpenTkControl.Start(settings);

            //readSHX(@"C:\Users\isbaek\source\repos\World\world.shx");
            VerifiShapeFilePath("");
        }

        private void OpenTkControl_OnRender(TimeSpan obj)
        {
            GL.ClearColor(Color4.Blue);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        }

        //shx 검증 
        bool VerifiSHX(string shxPath)
        {
            if(VerifiShapeFilePath(shxPath))
            {
                //파일 크기 확인
                FileInfo info = new FileInfo(shxPath);
                if(info.Length != 0)
                {
                    byte[] tmp = new byte[4];
                    int halfsize;
                    int Filesize;
                    FileStream shxfs = new FileStream(shxPath, FileMode.Open);
                    shxfs.Seek(24, SeekOrigin.Begin);
                    shxfs.Read(tmp, 0, tmp.Length);

                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(tmp);
                    halfsize = BitConverter.ToInt32(tmp, 0);
                    Filesize = halfsize * 2;

                    if (info.Length == Filesize && Filesize != 0 )
                    {
                        //레코드 개수 확인
                        double recordCount = (Filesize - 100) / 8;
                        if(recordCount==(int)recordCount && recordCount > 0)
                        {
                            //전역 개수 파일 크기 넣기
                            m_SHXFileSize = Filesize;
                            m_SHXRecordSize = Convert.ToInt32(recordCount);
                            return true;
                        }
                        else
                        {
                            Trace.WriteLine("레코드 카운트 오류 ");
                            return false;
                        }
                    }
                    else
                    {
                        Trace.WriteLine("shx 파일 길이 오류");
                        return false;
                    }
                }
                else
                {
                    Trace.WriteLine("shx 파일 길이 오류");
                    return false;
                }
            }
            else
            {
                Trace.WriteLine("shx 검증 오류");
                return false;
            }
        }

        // 파일이름 검증
        bool VerifiShapeFilePath(string path)
        {
            string str = @"C:\Users\isbaek\source\repos\World\world.shx";
            //파일 이름 검증
            FileInfo info = new FileInfo(str);
            if(info.Exists)
            {
                string tmp= str.Substring(str.Length - 4);
                if(tmp == ".shx"|| tmp == ".shp"|| tmp == ".dbf")
                    return true;
                else
                {
                    //파일 타입이 다릅니다.
                    Trace.WriteLine("파일 타입이 다릅니다.");
                    return false;
                }
            }
            else
            {
                //폴더 이거나 파일이 없을경우
                Trace.WriteLine("Invalied File ( " + path +" )");
                return false;
            }
        }

        //shxPath

        bool readSHX(string shpFilePath)
        {
            string shxPath = @"C:\Users\isbaek\source\repos\World\world.shx";
            string shpPath = @"C:\Users\isbaek\source\repos\World\world.shp";

            FileInfo info = new FileInfo(shpFilePath);
            if (!info.Exists) return false;

            byte[] tmp = new byte[4];

            FileStream shxfs = new FileStream(shxPath, FileMode.Open);
            shxfs.Seek(24, SeekOrigin.Begin);
            shxfs.Read(tmp, 0, tmp.Length);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(tmp);
            int i = BitConverter.ToInt32(tmp, 0);

            int m_nFilesize = i * 2;
            int recordCount = (m_nFilesize - 100) / 8;

            FileStream shpfs = new FileStream(shpPath, FileMode.Open);
            shpfs.Seek(32, SeekOrigin.Begin);
            shpfs.Read(tmp, 0, tmp.Length);
            int sh = BitConverter.ToInt32(tmp, 0);

            if (sh != 1)
            {
                Debug.WriteLine("Invalied shape type");
                return false;
            }

         



            return true;

        }

      
    }

}

