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
using WebSocket4Net;
using SuperSocket.ClientEngine;
using System.Threading;
using System.IO;


namespace HistoryKing_client
{
    /// <summary>
    /// Interaction logic for ReadyRoom.xaml
    /// </summary>
    public partial class ReadyRoom : Page
    {
        string msg;
        string roomMasterID;
        string userID;
        string enemyID;

        public ReadyRoom(string userID, string roomMasterID)
        {
            this.roomMasterID = roomMasterID;
            this.userID = userID;
            WaittingRoom.websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            InitializeComponent();
            Console.WriteLine("생성자쪽 방장: " + roomMasterID + "유저아디: " + userID);
        }

        void websocket_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("또?");
        }

        private void ReadyRoom_Loaded(object sender, RoutedEventArgs e)
        {
            Me_ID.Text = Player.getInstance().getID();
            Console.WriteLine(roomMasterID);

            Input.FontSize = 20;
            Output.FontSize = 20;
            myFace();

            if (!roomMasterID.Equals(userID))
            {
                Console.WriteLine(msg);
                msg = "joinRoom?" + userID + "?" + roomMasterID;
                WaittingRoom.websocket.Send(msg);
            }
        }

        private void sendReq(byte[] packet)
        {
            Console.WriteLine("보내기전 데이터 확인  : " + Encoding.Default.GetString(packet, 0, packet.Length));
            Console.WriteLine("sendReq 호출");
            WaittingRoom.websocket.Send(packet, 0, packet.Length);
        }

        private void sendClick(object sender, RoutedEventArgs e)
        {
            WaittingRoom.websocket.Send("ReadyRoomChat?"+Input.Text+"?"+userID+"?"+enemyID);
            Input.Clear();
        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("머가 오긴오나??");
            string[] cmds = e.Message.Split('?');

            switch (cmds[0])
            {
                case "enemyJoin":
                    Enemy_Control.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        //Enemy_Control.Background += msg + "\n";
                        Console.WriteLine("enemyJoin메세지중"+ cmds[1]);
                        Enemy_ID.Text = cmds[1];
                        enemyID = cmds[1];
                        enemyFace();
                    }));

                    break;

                case "enemyReady":

                    break;

                case "joinRoomOK":
                    Enemy_Control.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        //Enemy_Control.Background += msg + "\n";
                        Console.WriteLine("enemyJoin메세지중" + cmds[1]);
                        Enemy_ID.Text = cmds[1];
                        enemyID = cmds[1];
                        enemyFace();
                        ReadyButton.Visibility = Visibility.Visible;
                    }));
                    break;

                case "go":
                    MainMenu.getInstance().Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        Console.WriteLine("userID=" + userID);
                        Console.WriteLine("enemyID=" + enemyID);

                        PlayNetworkGame playNetworkGame = new PlayNetworkGame(userID, cmds[1], cmds[2]);
                        MainMenu.getInstance().WindowState = WindowState.Maximized;
                        MainMenu.getInstance().frame.Content = playNetworkGame;
                    }));
                    break;

                case "startButton":
                    ReadyRoomCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        StartButton.Visibility = Visibility.Visible;
                    }));
                    break;
                case "ReadyRoomChat":
                    Output.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        Output.Text += cmds[2]+": "+cmds[1] + "\n";
                    }));
                    break;
            }
        }

        void websocket_Closed(object sender, EventArgs e)
        {
   
        }

        void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {

        }

        private void startButtonClick(object sender, RoutedEventArgs e)
        {
            WaittingRoom.websocket.Send("start?" + enemyID + "?" + roomMasterID);
        }

        private void readyButtonClick(object sender, RoutedEventArgs e)
        {
            WaittingRoom.websocket.Send("readyButtonClick?" + roomMasterID + "?" + userID);
        }

        void myFace()
        {

            MemberDataContext memContext = new MemberDataContext();
            var query = from memCharacter in memContext.MemberCharacter
                        where memCharacter.MemberName == Player.getInstance().getID().ToString()
                        select memCharacter;

            foreach (var item in query)
            {
                byte[] byteimg;
                byteimg = item.CharacterImage.ToArray();
                //Console.WriteLine(item.CharacterImage.ToString());
                //Console.WriteLine();

                MemoryStream ms = new MemoryStream();
                ms.Write(byteimg, 0, byteimg.Length);
                getImage(System.Drawing.Image.FromStream(ms));
            }

        }

        void enemyFace()
        {
            MemberDataContext memContext = new MemberDataContext();
            var query = from memCharacter in memContext.MemberCharacter
                        where memCharacter.MemberName == enemyID
                        select memCharacter;

            foreach (var item in query)
            {
                byte[] byteimg;
                byteimg = item.CharacterImage.ToArray();
                //Console.WriteLine(item.CharacterImage.ToString());
                //Console.WriteLine();

                MemoryStream ms = new MemoryStream();
                ms.Write(byteimg, 0, byteimg.Length);
                getImage2(System.Drawing.Image.FromStream(ms));
            }
        }
        
        private void getImage(System.Drawing.Image getimg)
        {
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(getimg);
            IntPtr hBitmap = bmp.GetHbitmap();
            System.Windows.Media.ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            imgMe.Source = wpfBitmap;
            // img1.Width = 150;
            // img1.Height = 200;
            imgMe.Stretch = System.Windows.Media.Stretch.Fill;
        }

        private void getImage2(System.Drawing.Image getimg)
        {
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(getimg);
            IntPtr hBitmap = bmp.GetHbitmap();
            System.Windows.Media.ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            imgEnemy.Source = wpfBitmap;
            // img1.Width = 150;
            // img1.Height = 200;
            imgEnemy.Stretch = System.Windows.Media.Stretch.Fill;
        }
    }
}
