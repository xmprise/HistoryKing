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
    public class Player
    {
        private static Player _instance=null;

        private int hp;
        private string id;
        private int stage;
    

        public Player(){}

        public static Player getInstance()
        {
            if (_instance == null)
            {
                _instance = new Player();
            }
            return _instance;
        }

        public int getHP()
        {
            return hp;
        }
        public void setHP(int hp)
        {
            this.hp = hp;
        }
        public string getID()
        {
            return id;
        }
        public void setID(string id)
        {
            this.id = id;
        }
        public int getStage()
        {
            return stage;
        }

        public void setStage(int stage)
        {
            this.stage = stage;
        }
    }
}