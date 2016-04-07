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
using System.Windows.Media.Animation;

namespace HistoryKing_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TutorialScreen : Window
    {
        private int count;
        private Storyboard s;

        public TutorialScreen()
        {
            InitializeComponent();

            textBox1.Text = "오늘도 평소와 똑같이 학교를 가는 길이었다";
            
            image1.Visibility = Visibility.Collapsed;
            image2.Visibility = Visibility.Collapsed;
            image3.Visibility = Visibility.Collapsed;
            image4.Visibility = Visibility.Collapsed;
            image5.Visibility = Visibility.Collapsed;
            image6.Visibility = Visibility.Collapsed;
            image7.Visibility = Visibility.Collapsed;
            image8.Visibility = Visibility.Collapsed;
            image9.Visibility = Visibility.Collapsed;
            image10.Visibility = Visibility.Collapsed;
            image11.Visibility = Visibility.Collapsed;
            image12.Visibility = Visibility.Collapsed;
            image13.Visibility = Visibility.Collapsed;
            image14.Visibility = Visibility.Collapsed;
        }

        private void textbox_event(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (count < 1)
                {
                    Console.WriteLine("EnterKey 값 입력");
                    
                    s = (Storyboard)this.FindResource("Storyboard");
                    this.BeginStoryboard(s);
                    count++;
                }

                else if(count == 1){
                    textBox1.Text = "저건 뭐지? 으악!";
                    Storyboard s1 = (Storyboard)this.FindResource("personmove");
                    s = (Storyboard)this.FindResource("cyclone");
                    this.BeginStoryboard(s);
                    this.BeginStoryboard(s1);
                    count++;
                }
            }
        }
    }
}