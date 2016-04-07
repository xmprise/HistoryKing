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
    /// ChosenStageSelect.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChosenStageSelect : Page
    {
        public ChosenStageSelect()
        {
            InitializeComponent();
        }

        private void Backward_Click(object sender, RoutedEventArgs e)
        {
            GoruelStageSelect goruelStageSelect = new GoruelStageSelect();
            MainMenu.getInstance().frame.Navigate(goruelStageSelect);
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            DarkageStageSelect darkageStageSelect = new DarkageStageSelect();
            MainMenu.getInstance().frame.Navigate(darkageStageSelect);
        }

        private void click1(object sender, RoutedEventArgs e)
        {
            String heroName = "문익점 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage3/Stage3-1.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage3_Click2(object sender, RoutedEventArgs e)
        {
            String heroName = "장영실 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage3/Stage3-2.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage3_Click3(object sender, RoutedEventArgs e)
        {
            String heroName = "이이.png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage3/Stage3-3.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage3_Click4(object sender, RoutedEventArgs e)
        {
            String heroName = "신사임당 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage3/Stage3-4.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage3_Click5(object sender, RoutedEventArgs e)
        {
            String heroName = "이순신 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage3/Stage3-5.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage3_Click6(object sender, RoutedEventArgs e)
        {
            String heroName = "권율 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage3/Stage3-6.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage3_Click7(object sender, RoutedEventArgs e)
        {
            String heroName = "임꺽정 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage3/Stage3-7.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage3_Click8(object sender, RoutedEventArgs e)
        {
            String heroName = "박문수 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage3/Stage3-8.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage3_Click9(object sender, RoutedEventArgs e)
        {
            String heroName = "김홍도 (2).png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage3/Stage3-9.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

        private void stage3_Click10(object sender, RoutedEventArgs e)
        {
            String heroName = "정조.png";//불러올 위인이미지를 적어준다.
            String storyName = "./Story/Stage3/Stage3-10.txt";//불러올 Stage의 파일을 적어준다.
            StageAnimation1_1 ani = new StageAnimation1_1(heroName, storyName);
            MainMenu.getInstance().frame.Navigate(ani);
        }

    }
}