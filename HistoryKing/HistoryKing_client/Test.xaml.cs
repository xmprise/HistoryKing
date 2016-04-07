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
    /// Test.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();
        }
        private String Text;
        private String Text1;

        public String SetText
        {
            get { return Text; }
            set { Text = value; }
        }

        public String SetText1 {
            get { return Text1; }
            set { Text1 = value; }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBlock1.Text = Text;
        }
    }
}
