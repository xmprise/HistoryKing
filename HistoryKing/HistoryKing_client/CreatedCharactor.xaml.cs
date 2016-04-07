using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
// System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using AForge.Video.DirectShow;
using System.Data.Linq;

namespace HistoryKing_client
{
    /// <summary>
    /// CreatedCharactor.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CreatedCharactor : Window
    {
        static Boolean check = false;
        Webcam mWebcam;
        MainMenu mainmenu;
        String[] random_face = new String[] { "얼굴", "얼굴1", "얼굴2" };
        String[] random_eyes = new String[] { "눈1", "눈2", "눈3", "눈4", "눈5", "눈6", "눈7", "눈8", "눈9", "눈10"};
        String[] random_noses = new String[] { "코1", "코2", "코3", "코4", "코5", "코6", "코7", "코8", "코9", "코10"};
        String[] random_mouse = new String[] { "입1", "입2", "입3", "입4", "입5", "입6", "입7", "입8", "입9", "입10", };
        String[] random_hair = new String[] { "머리1", "머리2", "머리3", "머리4", "머리5", "머리6", "머리7", "머리8", "머리9", "머리10"};

        //Bitmap[] random_face = new Bitmap[] { Properties.Resources.얼굴, Properties.Resources.얼굴1, Properties.Resources.얼굴2 };
        Random rNum;

        public CreatedCharactor(MainMenu mainmenu)
        {
            this.mainmenu = mainmenu;
            InitializeComponent();
        }

        private void add_image_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "jpg";        //기본확장자
            openFile.Filter = "이미지 파일(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            rNum = new Random();
            int num = rNum.Next(1, 10);

            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                face.Source = new BitmapImage(new Uri(openFile.FileName, UriKind.Absolute));
                another_face(num);
            }
        }

        private void myface_save() {
            RenderTargetBitmap source = ConverterBitmapImage(grid1);

                FileStream stream = new FileStream("aa.png", FileMode.Create);
                BitmapEncoder encoder = new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(source));
                encoder.Save(stream);
                stream.Close();
            
           save_DB();
        }

        private void save_DB()
        {
            check = true;
            byte[] imgByte;
            //System.Windows.Forms.MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory);

                System.Drawing.Image img = System.Drawing.Image.FromFile("aa.png");
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imgByte = new byte[ms.Length];
                imgByte = ms.ToArray();

                MemberDataContext memContext = new MemberDataContext();

                var query = from memCharacter in memContext.MemberCharacter
                where memCharacter.MemberName == Player.getInstance().getID().ToString()
                select memCharacter;

               foreach (var item in query)
               {
                   item.CharacterImage = imgByte;
                   Console.WriteLine(item.CharacterImage.ToString());
               }

                try
                {
                   memContext.SubmitChanges();
                }
                catch (ChangeConflictException) {
                    memContext.ChangeConflicts.ResolveAll(RefreshMode.KeepChanges);
                }
                
                // System.Windows.Forms.MessageBox.Show(e.ToString());
                ms.Close();
        }

        private void another_face(int num)
        {
            grid1.Children.Clear();
            //얼굴
            System.Windows.Controls.Image img = null;
            img = new System.Windows.Controls.Image();
            //img.Source = new BitmapImage(new Uri("c:\\" + random_face[num] + ".png", UriKind.Absolute));
            img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory+"image/얼굴3.png", UriKind.Absolute));
            //img.Stretch = Stretch.Fill;
            grid1.Children.Add(img);

            //눈s
            System.Windows.Controls.Image img2;
            img2 = new System.Windows.Controls.Image();
            img2.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image/눈"+num+".png", UriKind.Absolute));
            img2.Stretch = Stretch.Fill;
            grid1.Children.Add(img2);

            //코
            System.Windows.Controls.Image img3;
            img3 = new System.Windows.Controls.Image();
            img3.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image/코"+num+".png", UriKind.Absolute));
            img3.Stretch = Stretch.Fill;
            grid1.Children.Add(img3);

            //입
            System.Windows.Controls.Image img4;
            img4 = new System.Windows.Controls.Image();
            img4.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image/입"+num+".png", UriKind.Absolute));
            img4.Stretch = Stretch.Fill;
            grid1.Children.Add(img4);

            //머리
            System.Windows.Controls.Image img5;
            img5 = new System.Windows.Controls.Image();
            img5.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image/머리" + num + ".png", UriKind.Absolute));
            img5.Stretch = Stretch.Fill;
            grid1.Children.Add(img5);

            check = true;
        }

        private static RenderTargetBitmap ConverterBitmapImage(FrameworkElement element)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            drawingContext.DrawRectangle(new VisualBrush(element), null, new Rect(new System.Windows.Point(0, 0), new System.Windows.Point(element.ActualWidth, element.ActualHeight)));
            drawingContext.Close();

            RenderTargetBitmap target = new RenderTargetBitmap((int)element.ActualWidth, (int)element.ActualHeight, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);
            target.Render(drawingVisual);
            return target;
        }

        public void showImage(String path)
        {
            face.Source = new BitmapImage(new Uri(path + "face.jpg", UriKind.Absolute));
        }


        private void webcam_Click(object sender, RoutedEventArgs e)
        {
            Camera MyWebCam = new Camera();
            FilterInfoCollection MyDevices = MyWebCam.MyCameraDevices;
            if (MyDevices.Count == 0)
            {
                MyMessageBox message = new MyMessageBox();
                message.SetText = "현재 PC에 연결된 Cam이 없습니다.";
                message.ShowDialog();
                
            }
            else {

                mWebcam = new Webcam(this);
                mWebcam.Show();
            }

        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            if (check == true)
            {
                myface_save();
                SamGookStageSelect stageSel = new SamGookStageSelect();
                MainMenu.getInstance().frame.Content = stageSel;
                this.Close();

            }
            else {
                MyMessageBox messagebox = new MyMessageBox();
                messagebox.SetText = ("얼굴을 만들어주세요.");
                messagebox.ShowDialog();
            }
         }
    }
}
