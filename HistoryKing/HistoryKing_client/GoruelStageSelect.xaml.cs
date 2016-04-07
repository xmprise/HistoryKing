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
    /// GoruelStageSelect.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GoruelStageSelect : Page
    {
        public GoruelStageSelect()
        {
            InitializeComponent();
        }

        private void Backward_Click(object sender, RoutedEventArgs e)
        {
            SamGookStageSelect samgookStageSelect = new SamGookStageSelect();
            MainMenu.getInstance().frame.Navigate(samgookStageSelect);
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            ChosenStageSelect chosenStageSelect = new ChosenStageSelect();
            MainMenu.getInstance().frame.Navigate(chosenStageSelect);
        }

        private void Phase1_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "강감찬 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage2/Stage2-1.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void Phase2_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "서희 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage2/Stage2-2.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);

        }

        private void Phase3_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "정중부 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage2/Stage2-3.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void Phase4_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "최무선 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage2/Stage2-4.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void Phase6_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "김부식 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage2/Stage2-6.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void Phase7_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "지눌 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage2/Stage2-7.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void Phase8_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "의천 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage2/Stage2-8.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void Phase9_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "이종무 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage2/Stage2-9.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void Phase10_Click(object sender, RoutedEventArgs e)
        {
            String heroName = "이종무 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage2/Stage2-9.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
