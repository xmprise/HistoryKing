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
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace HistoryKing_MiniGame
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            init();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.IsEnabled = true;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        
        }
        string ans;
        int time = 0;
        int file;
        DispatcherTimer timer;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void init()
        {
            Random ran = new Random();
            file = ran.Next(1, 13);
            StreamReader reader = new StreamReader((@"C:\HistoryKing\HistoryKing_client\Resource\Quiz\Quiz-" + file + ".txt"), System.Text.Encoding.Default);
            
            string[] quiz = reader.ReadToEnd().Split('\n');
            string[] ans = new string[1];
            for (int i = 0; i < quiz.Length; i++)
            {
                if (quiz[i].StartsWith("답안") == true)
                {
                    ans = quiz[i].Split(':');
                    quiz[i] = "";
                }
                label5.Content += quiz[i];
            }
            
            this.ans = ans[1];            
            
            Duration duration = new Duration(TimeSpan.FromSeconds(10));
            DoubleAnimation animation = new DoubleAnimation(100.0, duration);
            //progressBar1.BeginAnimation(System.Windows.Controls.ProgressBar.ValueProperty, animation);
        }



        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if ((textBox2.Text).CompareTo(ans) == 0)
            {
                MessageBox.Show("정답");
                Environment.Exit(0); // 종료가 아닌 Window만 close 하기.
            }
            else
                MessageBox.Show("오답");
        }

        void timer_Tick(object sender, EventArgs e)
        {
            //label4.Content = "";
           // label4.Content = time.ToString();
            
            time++;

            if (time < 10) {
                number_img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + time + ".png", UriKind.Absolute));
            }else             {
                MessageBox.Show("End"); 
                Environment.Exit(0); // 종료가 아닌 Window만 close 하기.
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0); // 종료가 아닌 Window만 close 하기.
        }
    }
}
