using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DirectShowLib;
using System.Threading;
using System.Windows.Threading;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace HistoryKing_client
{
    /// <summary>
    /// Webcam.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Webcam : Window
    {
        Camera MyWebCam = new Camera();
        Canvas MyCanvas = new Canvas();
        Button capture = new Button();
        ISampleGrabber fSampleGrabber = null;

        public CreatedCharactor mCreat;

        const int CAMWIDTH = 260;
        const int CAMHIGHT = 200;

        public Webcam(CreatedCharactor a)
        {
            InitializeComponent();
            InitUI();
            InitCamera();
            mCreat = a;
        }

        private void InitUI()
        {
            fSampleGrabber = (ISampleGrabber)new SampleGrabber();

            this.Width = 400;
            this.Height = 420;

            MyCanvas.Width = 320;
            MyCanvas.Height = 240;

            capture.Width = 70;
            capture.Height = 30;
            capture.Content = "얼굴찍기";
            capture.Click += new RoutedEventHandler(capture_Click);
            
            capture.Margin = new Thickness(CAMWIDTH/2, CAMHIGHT + 50, (CAMWIDTH * 2), 0);
            MyCanvas.Children.Add(capture);

            //webcam_Form.Closed += new EventHandler(webcam_Form_Closed);

            Content = MyCanvas;
        }

        void capture_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("헐");
            face_Capture();
            //throw new NotImplementedException();
        }

        public void InitCamera()
        {
            //카메라 셋팅 
            MyWebCam = new Camera();

            FilterInfoCollection MyDevices = MyWebCam.MyCameraDevices;

            if (MyDevices.Count == 0)
            {
                //MessageBox.Show("현재 PC에 연결된 Cam이 없습니다.");
                return;
            }


            VideoCaptureDevice MyCaptureDevice = new VideoCaptureDevice(MyDevices[0].MonikerString);
            MyCaptureDevice.DesiredFrameSize = new System.Drawing.Size(CAMWIDTH, CAMHIGHT);
            MyCaptureDevice.DesiredFrameRate = 30;

            MyWebCam.SetCamera(MyCaptureDevice);
            
            MyWebCam.NewFrame += new EventHandler(MyWebCam_NewFrame);

            MyWebCam.Start();
        }

        private void face_Capture()
        {
            //MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory);
            System.Drawing.Bitmap MyBitMap; //실제 캠에서 들어 오는 Bitmap을 받기 위해서 Drawing쪽에 Bitmap을 가지고 온다. 
            MyBitMap = MyWebCam.LastFrame;  //웹캠의 데이터를 받는다.

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "face.jpg"))
            {

                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "face.jpg");
                    MyBitMap.Save("face.jpg", ImageFormat.Jpeg);
            }
            else
            {
                MyBitMap.Save("face.jpg", ImageFormat.Jpeg);
            }





            //System.Drawing.Image img = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "face.jpg");
            //img.Save(System.IO.Path.GetTempFileName());
            
                //System.Drawing.Image img = (System.Drawing.Image)MyBitMap;
                ////img.Save(AppDomain.CurrentDomain.BaseDirectory + "face.jpg");
                //img.Save(System.IO.Path.GetTempFileName());
            




            //mCreat = new CreatedCharactor();
            //mCreat.showImage(AppDomain.CurrentDomain.BaseDirectory + "face.jpg");
            //MyBitMap.Save("face.jpg", ImageFormat.Jpeg);
            CheckFace check = new CheckFace(AppDomain.CurrentDomain.BaseDirectory, this);
            //MyWebCam.Stop();
            MyBitMap.Dispose();
            //MyBitMap = null;
            check.Show();
        }


        delegate void Delegate_FillBitmapImage(ImageBrush ib, Canvas TargetCanvas);

        //캠에서 신호가 들어올때 마다 발생하는 이벤트 핸들러 
        void MyWebCam_NewFrame(object sender, EventArgs e)
        {
            try
            {
                ImageBrush ib_Origin = new ImageBrush();//캠에서 들어오는 원본 

                System.Drawing.Bitmap MyBitMap; //실제 캠에서 들어 오는 Bitmap을 받기 위해서 Drawing쪽에 Bitmap을 가지고 온다. 
                MyBitMap = MyWebCam.LastFrame; //웹캠에서 데이터를 받는다. 

                //캠에서 들어 오는 원본 소스 
                BitmapSource OriginBitmap;
                OriginBitmap = Get_Convert_BitmapSource(MyBitMap);
                ib_Origin.ImageSource = OriginBitmap;

                ib_Origin.Freeze();

                Delegate_FillBitmapImage df1 = new Delegate_FillBitmapImage(Delegate_FillRectangleBitmapImage_Method);
                Dispatcher.Invoke(DispatcherPriority.Normal, df1, ib_Origin, MyCanvas);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //화면에 그린다. 
        public void Delegate_FillRectangleBitmapImage_Method(ImageBrush ib, Canvas TargetCanvas)
        {
            try
            {
                TargetCanvas.Background = ib;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //DeleteObject를 쓰기 위해 외부 Dll Import
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// System.Drawing.Bitmap 파일을 Windows.media.image(WPF 포맷으로 적합한 파일로 변환 하기 위한 함수)
        /// </summary>
        /// <param name="source">System.Drawing.Bitmap 형식의 Bitmap</param>
        /// <returns>BitmapSource</returns>
        public BitmapSource Get_Convert_BitmapSource(System.Drawing.Bitmap source)
        {

            IntPtr hBitmap = source.GetHbitmap();

            BitmapSource retval = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty,
                     System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(hBitmap); //요걸 안해주면 비디오 메모리가 질질 샌다.^^

            return retval;
        }
    }
}
