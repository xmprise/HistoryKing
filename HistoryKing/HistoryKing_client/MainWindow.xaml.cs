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

namespace HistoryKing_client
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
		private static Player player;
        private static MainMenu maimenu;
        public MainWindow()
        {
            player = Player.getInstance();
            InitializeComponent();
        }

     
        MemberDataContext mem = new MemberDataContext();
        public string UserID;
        private void button2_Click(object sender, RoutedEventArgs e)
        {          
            var queryMember = from cust in mem.Member
                              where cust.MemberName == textBox2.Text
                              select cust;

            foreach (var item in queryMember)
            {
                if (item.MemberPassword == textBox1.Text)
                {
                    Test test = new Test();
                    test.SetText = "확인되었습니다. ";
                    test.SetText = "게임을 시작합니다.";
                    test.ShowDialog();
                    //MessageBox.Show("확인되었습니다. 게임을 시작 합니다.");
                    if (test.DialogResult == true)
                    {
                        maimenu = MainMenu.getInstance();
                        Menu menu = new Menu();
                        maimenu.Activate();

                        this.Close();
                        if (maimenu.ShowActivated == true)
                        {
                            UserID = textBox2.Text;
                            player.setID(UserID);
                            maimenu.UserID = UserID;
                            maimenu.Show();
                        }
                    }

                }
                else
                {
                    MyMessageBox message = new MyMessageBox();
                    message.SetText = "ID나 비밀번호를 다시 확인해 주세요.";
                    message.ShowDialog();
                }
                    
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            this.Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            NewUser newUser = new NewUser();
            newUser.Show();
            this.Close();
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

      
    }
}
