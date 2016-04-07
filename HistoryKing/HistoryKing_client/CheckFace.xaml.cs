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
using System.Windows.Forms;
using System.Drawing;

namespace HistoryKing_client
{
    /// <summary>
    /// CheckFace.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CheckFace : Window
    {
        private String imagePath;
        private Webcam mWebcam;
        //private CreatedCharactor mCreat;
        
        public CheckFace(String path, Webcam b)
        {   
            InitializeComponent();
            imagePath = path;
            showImage();
            mWebcam = b;
        }

        private void showImage() {
            check_face.Source = new BitmapImage(new Uri(imagePath + "face.jpg", UriKind.Absolute)); 
        }

        private void Yes_btn_Click(object sender, RoutedEventArgs e)
        {
            //mWebcam = new Webcam();
            //mCreateChar = new CreatedCharactor();
            //mCreateChar.showImage(imagePath + "face.jpg");
            //System.Diagnostics.Process.GetCurrentProcess().Kill();
            this.mWebcam.mCreat.Show();
            this.mWebcam.mCreat.showImage(imagePath);
            //this.mWebcam.mCreat.showImage();
            this.mWebcam.Close();
            checkFace_Form.Close();
            //mWebcam.webcam_Form.Close();
            //mWebcam.closeForm();
        }

        private void No_btn_Click(object sender, RoutedEventArgs e)
        {
            checkFace_Form.Close();
        }
    }
}
