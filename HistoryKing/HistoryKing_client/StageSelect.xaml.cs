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
    /// Interaction logic for StageSelect.xaml
    /// </summary>
    
    public partial class StageSelect : Page
    {
        public MainMenu mainmenu;
        public StageSelect()

        {
            InitializeComponent();
            Stag_Init();
        }

        int Stage;

        public StageSelect(MainMenu mainmenu)
        {
			this.mainmenu = mainmenu;
            InitializeComponent();
            Stag_Init();
        }

        private void SamGook_Click(object sender, RoutedEventArgs e)
        {
            SamGookStageSelect samGookStageSelect = new SamGookStageSelect();
            MainMenu.getInstance().frame.Navigate(samGookStageSelect);
        }

        private void Stag_Init()
        {
            MemberDataContext mem = new MemberDataContext();

            var innerJoinQuery = from Members in mem.Member
                                 join Information in mem.MemberGameInformation on Members.MemberName equals Information.MemberName
                                 where Information.MemberName == Player.getInstance().getID()
                                 select Information;

            //DB의 유저의 쫄 카드 정보 받아와서 Card객체 만들고 DeckList에 삽입
            foreach (var item in innerJoinQuery)
            {
                Stage = (int)item.GameStage;
            }
            MessageBox.Show("저장된 현재 사용자 스테이지 " + Stage);
        }
    }
}
