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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using HistoryKing_client.Interfaces;
using System.Windows.Forms;


namespace HistoryKing_client
{
    /// <summary>
    /// Interaction logic for StageAnimation1_1.xaml
    /// </summary>
    public partial class StageAnimation1_1 : Page
    {
        private int count = 0;
        private Story_divide sd;

        private String[] main_char;
        private String[] stone;
        private String[] hero;

        private String imgFimeName;
        
        private String storyName;//어떤 경로상의 Story파일을 불러온다.

        public StageAnimation1_1(String imgName, String storyName)//첫번째 인자: 위인이미지, 두번째 인자: 시나리오 파일(경로상의 파일)
        {
            //this.Background = 
            this.imgFimeName = imgName;
            this.storyName = storyName;
            
            InitializeComponent();
        }

        private void stage1_Load(object sender, RoutedEventArgs e)
        {

            //String heroNmae = "dangoon.png";
            //sd = new Story_divide(Properties.Resources.Stage1_1, textBox1, textBox2, image1, image2, imgFimeName);
            sd = new Story_divide(storyName, textBox1, textBox2, image1, image2, imgFimeName);
            sd.getDivide();

            //image1.Source = new BitmapImage(new Uri("dangoon.png", UriKind.Relative));

            /* main_char = sd.getMainChar();
             stone = sd.getStone();
             hero = sd.getHero();*/
        }

        private void stage1_down(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                sd.getDivide();
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}