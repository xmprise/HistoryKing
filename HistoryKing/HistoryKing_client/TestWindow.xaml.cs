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
using System.IO;
using System.Drawing;

namespace HistoryKing_client
{
    /// <summary>
    /// TestWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TestWindow : Window
    {
        public static System.Windows.Media.ImageSource wpfBitmap;

        public TestWindow()
        {
            InitializeComponent();
            loadImagebyte();
        }

        public void loadImagebyte()
        {
            MemberDataContext memContext = new MemberDataContext();
            //System.Drawing.Image returnImage;

            var query = from memCharacter in memContext.MemberCharacter
                        where memCharacter.MemberName == Player.getInstance().getID().ToString()
                        select memCharacter;

            foreach (var item in query)
            {
                byte[] byteimg;
                byteimg = item.CharacterImage.ToArray();
                Console.WriteLine(item.CharacterImage.ToString());
                Console.WriteLine();


                MemoryStream ms = new MemoryStream();
                ms.Write(byteimg, 0, byteimg.Length);
                setImage(System.Drawing.Image.FromStream(ms));

            }
        }

        private void setImage(System.Drawing.Image getimg)
        {
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(getimg);
            IntPtr hBitmap = bmp.GetHbitmap();
            TestWindow.wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            img.Source = wpfBitmap;
            img.Width = 150;
            img.Height = 200;
            img.Stretch = System.Windows.Media.Stretch.Fill;
            grid1.Children.Add(img);
        }
    }
}