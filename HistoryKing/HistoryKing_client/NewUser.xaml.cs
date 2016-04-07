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

namespace HistoryKing_client
{
    /// <summary>
    /// NewUser.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NewUser : Window
    {
        public NewUser()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MyMessageBox message = new MyMessageBox();
            if (txtID.Text == "" || txtPassword.Text == "" || txtConfrim.Text == "" || txtPassword2.Text == "")
            {
                message.SetText = ("빈칸이 없도록 입력해 주세요.");
                message.ShowDialog();
            }

            if (txtID.Text != "" && txtPassword.Text != "" && txtConfrim.Text != "" && txtPassword2.Text != "")
            {
                if (txtPassword.Text == txtConfrim.Text)
                {
                    message.SetText = ("새로운 사용자를 등록 합니다.");
                    message.ShowDialog();
                    db_Insert();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    message.SetText = ("패스워드를 확인해 주세요.");
                    message.ShowDialog();
                }
            }
            
        }

        private void db_Insert()
        {
            MemberDataContext memContext = new MemberDataContext();
            CardDataContext card = new CardDataContext();

            Member mem = new Member();
            mem.MemberName = txtID.Text;
            mem.MemberPassword = txtPassword.Text;
            mem.MemberPassword2 = txtPassword2.Text;

            memContext.Member.InsertOnSubmit(mem);
            try
            {
                memContext.SubmitChanges();
                card.Game_CardDeck(txtID.Text);
            }
            catch
            {
                MyMessageBox error = new MyMessageBox();
                error.SetText = "같은 아이디가 있습니다.";
                error.ShowDialog();
            }
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
