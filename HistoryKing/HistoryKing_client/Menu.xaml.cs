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
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    { 
        MainMenu mainmenu;
        int Stage;
        int Phase;

        public Menu()
        {
            InitializeComponent();
        }

        public Menu(MainMenu mainmenu)
        {
            this.mainmenu = mainmenu;
            InitializeComponent();
        }
        public string UserID;

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            /*StageAnimation1_1 st = new StageAnimation1_1();
            MainMenu.getInstance().frame.Navigate(st);*/
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mWindow = new MainWindow();
            
            mWindow.Show();
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            MemberDataContext memContext = new MemberDataContext();
            Test test = new Test();
            MyMessageBox myMessageBox = new MyMessageBox();
            //DB의 Mem.MemberCharacter의 Character 필드가 null일때 CreatedCharactor를 호출
            var queryMember = from cust in memContext.MemberCharacter
                              where cust.MemberName == UserID
                              select cust;

            foreach (var item in queryMember)
            {
                if (item.CharacterImage == null)
                {
                    //MessageBox.Show(UserID + "님은 현재 캐릭터가 없습니다.");
                    myMessageBox.SetText = UserID + "님은 현재 캐릭터가 없습니다.";
                    myMessageBox.ShowDialog();
                    CreatedCharactor mCreate = new CreatedCharactor(mainmenu);
                    mCreate.Show();
                    //MainMenu main = (MainMenu)this.Parent;
                    //main.Hide();
                    // 페이지 숨기기
                }
                else
                {
                    //MessageBox.Show(UserID + "님 게임을 시작 합니다.");
                    myMessageBox.SetText = UserID + "님 게임을 시작 합니다.";
                    //CreatedCharactor mCreate = new CreatedCharactor(mainmenu);
                    //mCreate.Show();
                    //StageSelect stageSel = new StageSelect(mainmenu);
                    //MainMenu.getInstance().frame.Content = stageSel;
                    
                    myMessageBox.ShowDialog();
                    if (myMessageBox.DialogResult == true)
                    {
                        Stag_Select();
                    }
                }
                //게임으로 화면 전환하기
            }
        }

        private void Stag_Select()
        {
            MemberDataContext mem = new MemberDataContext();
            MyMessageBox myMessageBox = new MyMessageBox();

            var innerJoinQuery = from Members in mem.Member
                                 join Information in mem.MemberGameInformation on Members.MemberName equals Information.MemberName
                                 where Information.MemberName == Player.getInstance().getID()
                                 select Information;

            //DB의 유저의 쫄 카드 정보 받아와서 Card객체 만들고 DeckList에 삽입
            foreach (var item in innerJoinQuery)
            {
                Stage = (int)item.GameStage;
                Phase = (int)item.GamePhase;
            }

            //MessageBox.Show("현재 "+ UserID + " 님의 게임 진행 상황은 Stage " + Stage + " - " + Phase + " 를 진행 중 입니다.");
            myMessageBox.SetText = "현재 " + UserID + " 님의 게임 진행 상황은 Stage " + Stage + " - " + Phase + " 를 \n진행 중 입니다.";
            myMessageBox.ShowDialog();

            SamGookStageSelect samgookStageSelect = new SamGookStageSelect();
            
            switch (Stage)
            {
                case 1:
                    
                    MainMenu.getInstance().frame.Navigate(samgookStageSelect);
                    break;
                case 2:
                    GoruelStageSelect goruelStageSelect = new GoruelStageSelect();
                    MainMenu.getInstance().frame.Navigate(goruelStageSelect);
                    break;
                case 3:
                    ChosenStageSelect chosenStageSelect = new ChosenStageSelect();
                    MainMenu.getInstance().frame.Navigate(chosenStageSelect);
                    break;
                case 4:
                    DarkageStageSelect darkageStageSelect = new DarkageStageSelect();
                    MainMenu.getInstance().frame.Navigate(darkageStageSelect);
                    break;
                default:
                    MainMenu.getInstance().frame.Navigate(samgookStageSelect);
                    break;
            }
        }

        private void option_Click(object sender, RoutedEventArgs e)
        {
           TestWindow tw = new TestWindow();
           tw.Show(); 
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UserID = Convert.ToString(label2.Content);
        }

        private void network_Click(object sender, RoutedEventArgs e)
        {
            WaittingRoom waittingRoom = new WaittingRoom();
            MainMenu.getInstance().frame.Content = waittingRoom;
        }
    }
}
