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
    /// DarkageStageSelect.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DarkageStageSelect : Page
    {
        public DarkageStageSelect()
        {
            InitializeComponent();
        }

        private void Backward_Click(object sender, RoutedEventArgs e)
        {
            ChosenStageSelect chosenStageSelect = new ChosenStageSelect();
            MainMenu.getInstance().frame.Navigate(chosenStageSelect);
        }

        private void stage4_Click1(object sender, RoutedEventArgs e)
        {
            String heroName = "안중근 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage4/Stage4-1.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage4_Click2(object sender, RoutedEventArgs e)
        {
            String heroName = "윤동주 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage4/Stage4-2.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage4_Click3(object sender, RoutedEventArgs e)
        {
            String heroName = "지석영 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage4/Stage4-3.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage4_Click4(object sender, RoutedEventArgs e)
        {
            String heroName = "손병희 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage4/Stage4-4.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage4_Click5(object sender, RoutedEventArgs e)
        {
            String heroName = "유관순 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage4/Stage4-5.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage4_Click6(object sender, RoutedEventArgs e)
        {
            String heroName = "안창호 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage4/Stage4-6.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage4_Click7(object sender, RoutedEventArgs e)
        {
            String heroName = "방정환 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage4/Stage4-7.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage4_Click8(object sender, RoutedEventArgs e)
        {
            String heroName = "김두환.png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage4/Stage4-8.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage4_Click9(object sender, RoutedEventArgs e)
        {
            String heroName = "이중섭.png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage4/Stage4-9.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage4_Click10(object sender, RoutedEventArgs e)
        {
            String heroName = "이완용 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage4/Stage4-10.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }
    }
}
