using System;
using System.Collections.Generic;
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
	public class Card
	{
        public Boolean front = false;
        public int index;

        public int CardHP
        {
            get;
            set;
        }

        public string CardName
        {
            get;
            set;
        }
        public int CardDam
        {
            get;
            set;
        }
        public int CardDef
        {
            get;
            set;
        }
        public int attackUpSkill
        {
            get;
            set;
        }
        public int healthUpSkill
        {
            get;
            set;
        }
        public int turnUpSkill
        {
            get;
            set;
        }

        public string skill1
        {
            get;
            set;
        }

        public string skill2
        {
            get;
            set;
        }
        public string skill3
        {
            get;
            set;
        }
        public int cardNumber
        {
            get;
            set;
        }
        public string HeroCardInfo
        {
            get;
            set;
        }
    }


}