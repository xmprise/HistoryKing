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
    /// MyMessageBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MyMessageBox : Window
    {
        public MyMessageBox()
        {
            InitializeComponent();
        }

        private string Text;

        public string SetText
        {
            get { return Text; }
            set { Text = value; }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBlock1.Text = Text;
            
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
    }
}
