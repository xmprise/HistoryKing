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
    /// Interaction logic for CardDeck.xaml
    /// </summary>
    public partial class CardDeck : Page
    {
        public CardDeck()
        {
            InitializeComponent();
        }

        private void cardDeck_Loaded(object sender, RoutedEventArgs e)
        {
            RichTextBox richTextBox1 = new RichTextBox();
            richTextBox1.Background = new ImageBrush(new BitmapImage(new Uri("pan.jpg", UriKind.Relative)));
            CardDeckGrid.Children.Add(richTextBox1);
            FlowDocument flowDoc = new FlowDocument();
            flowDoc.Blocks.Add(new Paragraph(new Run("테스트String")));
            richTextBox1.Document = flowDoc;

            Grid.SetRow(richTextBox1, 0);
            Grid.SetColumn(richTextBox1, 0);

        }
    }
}
