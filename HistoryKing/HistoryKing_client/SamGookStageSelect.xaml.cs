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
    /// SamGookStageSelect.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SamGookStageSelect : Page
    {
        public SamGookStageSelect()
        {
            InitializeComponent();
        }

        private void Phase1_Click(object sender, RoutedEventArgs e)
        {
            //테스트 코드
            //CardInfo(); 단군

            String heroName = "dangoon.png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage1/Stage1-1.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
            /*SamGookStage1 samgookstage1 = new SamGookStage1();
            MainMenu.getInstance().WindowState = WindowState.Maximized;
            MainMenu.getInstance().frame.Navigate(samgookstage1);*/
       
        }

        private void CardInfo()
        {
            CardDataContext card = new CardDataContext();
            MyMessageBox myMessageBox = new MyMessageBox();
            var innerJoinQuery = from Deck in card.HeroCard
                                 join Magic in card.HeroCardSkill on Deck.HeroCardName equals Magic.HeroCardName
                                 where Deck.HeroCardName == "동명왕"
                                 select Magic;
            String str = "";                     
            foreach (var item in innerJoinQuery)
            {
                str += item.HeroCardName;
                str += " ";
                str += item.SkillName;
                str += item.SkillPoint;
            }

            myMessageBox.SetText = str;
            myMessageBox.ShowDialog();
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            GoruelStageSelect goruelStageSelect = new GoruelStageSelect();
            MainMenu.getInstance().frame.Navigate(goruelStageSelect);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
          
            SetCardDeck setCardDeck = new SetCardDeck();
            MainMenu.getInstance().frame.Navigate(setCardDeck);
        }

        private void Phase2_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "dongmyung.png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage1/Stage1-2.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void Phase3_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "온조왕 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage1/Stage1-3.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void Phase4_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "박혁거세 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage1/Stage1-4.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void Phase5_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "광개토대왕 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage1/Stage1-5.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);

        }

        private void Phase6_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "이사부 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage1/Stage1-6.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);

        }

        private void Phase7_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "백결선생 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage1/Stage1-7.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void Phase8_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "의자왕 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage1/Stage1-8.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void Phase9_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "계백 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage1/Stage1-9.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void Phase10_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "김유신 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage1/Stage1-10.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

    }
}