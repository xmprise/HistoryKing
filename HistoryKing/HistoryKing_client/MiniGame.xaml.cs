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

namespace HistoryKing_client
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MiniGame : Window
    {

        public MiniGame()
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

            if (new FileInfo("Quiz/Quiz-" + file + ".txt").Exists)
            {
                StreamReader reader = new StreamReader(("Quiz/Quiz-" + file + ".txt"), System.Text.Encoding.Default);

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
            }
            //progressBar1.BeginAnimation(System.Windows.Controls.ProgressBar.ValueProperty, animation);
        }



        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MyMessageBox message = new MyMessageBox();
            if ((textBox2.Text).CompareTo(ans) == 0)
            {
                message.SetText = ("정답");
                message.ShowDialog();
                timer.Stop();
                this.Close();
                SamGookStage1.Question = true;
            }
            else
            {
                message.SetText = "틀렸어요!";
                message.ShowDialog();
                timer.Stop();
                this.Close();
                SamGookStage1.Question = true;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            //label4.Content = "";
           // label4.Content = time.ToString();
            
            time++;
            
            if (time < 10) {
                //MessageBox.Show("하");
                //MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory);
                string uri = AppDomain.CurrentDomain.BaseDirectory + time.ToString() + ".png";
                number_img.Source = new BitmapImage(new Uri(uri, UriKind.RelativeOrAbsolute));
            }else
            {
                MyMessageBox message = new MyMessageBox();
                message.SetText = "시간이 다 되었네요.";
                message.ShowDialog();
                timer.Stop();
                this.Close(); // 종료가 아닌 Window만 close 하기.
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // 종료가 아닌 Window만 close 하기.
        }
    }
}
