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
using System.Windows.Navigation;

namespace HistoryKing_client
{
    /// <summary>
    /// MainMenu.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainMenu : Window
    {
        public String UserID;
        private static MainMenu _instance = null;

        public MainMenu()
        {
            InitializeComponent();  
        }

        public static MainMenu getInstance()
        {
            if (_instance == null)
            {
                _instance = new MainMenu();
            }
            return _instance;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.label2.Content = UserID;
            frame.Content = menu;
        }

        private void frame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
