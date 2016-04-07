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
using System.Threading;
using System.Collections;
using WebSocket4Net;
using SuperSocket.ClientEngine;
using Newtonsoft.Json;

namespace HistoryKing_client
{
    public partial class WaittingRoom
    {
        public static WebSocket websocket;
        static Menu menu = new Menu();
        static Thread roomUpdateThread;
        static byte[] packet = new byte[128];
        static string msg;
        Point absoluteDestiPoint;
        int selectRoomNum = 0;
        string userID = Player.getInstance().getID();
        string roomMasterID;
        static UserInfo userInfo;
        static List<UserInfo> userInfoList = new List<UserInfo>();
        static RoomInfo roomInfo;
        static List<RoomInfo> roomInfoList = new List<RoomInfo>();
        
        List<UserInfo> waittingRoomUserInfoList= null;

        public WaittingRoom()
        {
            websocket = new WebSocket("ws://210.118.69.142:2004/", "basic", WebSocketVersion.DraftHybi10);
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.DataReceived += new EventHandler<DataReceivedEventArgs>(websocket_DataReceived);
            websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            roomUpdateThread = new Thread(new ThreadStart(roomUpdate));
            this.InitializeComponent();

            // Insert code required on object creation below this point.
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Input.FontSize = 20;
            Output.FontSize = 20;
            UserList.FontSize = 20;
            msg = "access?" + userID;
            websocket.Open();
        }

        // string형태로 서버로 바로 쏴주는  함수
        private void sendMsg()
        {
            try
            {
                Input.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    websocket.Send("WaittingChat?"+Input.Text+"?"+userID);
                    Input.Clear();
                }));
            }
            catch (Exception)
            {
                MessageBox.Show("익셉션");
            }
        }

        // byte 형태로 서버로 쏴주는 함수
        private void sendReq(byte[] packet)
        {
            Console.WriteLine("sendReq 호출");
            websocket.Send(packet, 0, packet.Length);
        }


        static void websocket_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("handshake succeded");
            MessageBox.Show("접속 완료!!!");

            // 소켓 최초 연결시에 패킷의 헤더 0번 + userID 담아서 전송
            websocket.Send(msg);

            // 방 정도 없데이트 스레드 시작
           // roomUpdateThread.Start();

        }

        static void websocket_DataReceived(object sender, DataReceivedEventArgs e)
        {

        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            string[] cmds = e.Message.Split('?');  // 수신된 스트링 메시지 ?토큰으로 구분해서 저장

            switch (cmds[0])
            {
                case "updateUserInfo": // access 될때마다 업데이트 된 유저 정보를 서버로 부터 반환  받는다..
                    userInfoList = JsonConvert.DeserializeObject<List<UserInfo>>(cmds[1]);

                    // 여기서 중요한 건 대기방에 있는 사용자와 준비방에 있는 사용자를 구분해서 뿌려줘야 하니까... 분리작업!!
                    
                    waittingRoomUserInfoList = new List<UserInfo>();

                    foreach (var user in userInfoList) // 반환받은 유저리스트에서 준비방에 있지 않는 유저만 따로 분리
                    {
                        if(user.joinRoomNum==0) // 룸번호가 0번 이라는 뜼은 조인한 방이 없다는 뜻이므로 대기방에 있는 사용자
                        {
                            waittingRoomUserInfoList.Add(user);
                        }
                    }

                    roomCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        

                        if (cmds[2] != "") // 생성된 방이 있다면
                        {
                            roomInfoList = JsonConvert.DeserializeObject<List<RoomInfo>>(cmds[2]); // 방 리스트 변환

                            for (int i = 0; i < 4; i++)
                            {
                                string targetGridName = "room" + (i + 1);
                                Grid desti = (Grid)roomCanvas.FindName(targetGridName);
                                desti.Children.Clear();
                            }

                            for (int i = 0; i < roomInfoList.Count; i++) // 방 개수만큼 동적생성
                            {
                                Console.WriteLine("도란나 방생성안되노");
                                string targetGridName = "room" + (i + 1);
                                Grid desti = (Grid)roomCanvas.FindName(targetGridName);

                                Button roomButton = new Button();

                                if(roomInfoList[i].isPlay == false)
                                    roomButton.Background = new ImageBrush(new BitmapImage(new Uri("대기방_준비.png", UriKind.Relative)));
                                else if(roomInfoList[i].isPlay == true)
                                    roomButton.Background = new ImageBrush(new BitmapImage(new Uri("대기방_시작.png", UriKind.Relative)));

                                roomButton.Name = roomInfoList[i].roomMasterID;
                                roomButton.Click += new RoutedEventHandler(roomButton_Click);
                                desti.Children.Add(roomButton);
                            }
                        }
                        Console.WriteLine("유저수는=" + userInfoList.Count);

                        //대기실에 있는 사용자만 골라서 뿌려주기....

                        UserList.Text = null;
                        foreach (UserInfo user in waittingRoomUserInfoList)
                        {
                                UserList.Text += user.userID + "\n";
                        }
                    }));

                    break;

                //클라이언트에서 createRoom 요청 날리고 서버에서 방정보를 업데이트하고 결과를 반환한 newRoom 헤더 메세지
                // 근데 중요한건 대기방에 있는 사용자들에게만 동적 방생성이 적용되도록 해야함..
                // 준비방에 들어가있거나 게임 하고 있는 세션 사용자들에게까지 동적 방생성 코드를 실행하게 하니까... 터짐!!!!
                case "newRoom":
                    roomMasterID = cmds[2]; // 방장아이디 가져옴
                    userInfoList = JsonConvert.DeserializeObject<List<UserInfo>>(cmds[4]);

                    // 여기서 중요한 건 대기방에 있는 사용자와 준비방에 있는 사용자를 구분해서 뿌려줘야 하니까... 분리작업!!
                    
                    waittingRoomUserInfoList = new List<UserInfo>();

                    foreach (var user in userInfoList) // 반환받은 유저리스트에서 준비방에 있지 않는 유저만 따로 분리
                    {
                        if(user.joinRoomNum==0) // 룸번호가 0번 이라는 뜻은 조인한 방이 없다는 뜻이므로 대기방에 있는 사용자
                        {
                            waittingRoomUserInfoList.Add(user);
                        }
                    }

                    // 업데이트된 방 정보를 받는데... 그 방을 생성한 사람이 본인이면 준비룸 생성 후 대기...
                    if (userID.Equals(roomMasterID))
                        MainMenu.getInstance().Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            Console.WriteLine("새방만들기전 마스터아이디 확인 : " + roomMasterID);
                            ReadyRoom readyRoom = new ReadyRoom(userID, roomMasterID);
                            MainMenu.getInstance().frame.Content = readyRoom;
                        }));
                    else
                    {
                        //대기실에 있는 사용자만 골라서 뿌려주기....
                        roomCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            UserList.Text = null;
                            foreach (UserInfo user in waittingRoomUserInfoList)
                            {
                                UserList.Text += user.userID + "\n";
                            }

                            if (cmds[2] != "") // 생성된 방이 있다면
                            {
                                roomInfoList = JsonConvert.DeserializeObject<List<RoomInfo>>(cmds[3]); // 방 리스트 변환

                                for (int i = 0; i < roomInfoList.Count; i++) // 방 개수만큼 동적생성
                                {
                                    Console.WriteLine("도란나 방생성안되노");
                                    string targetGridName = "room" + (i +1);
                                    Grid desti = (Grid)roomCanvas.FindName(targetGridName);

                                    Button roomButton = new Button();
                                    if (roomInfoList[i].isPlay == true)
                                    {
                                        roomButton.Background = new ImageBrush(new BitmapImage(new Uri("대기방_시작.png", UriKind.Relative)));
                                    }
                                    else
                                    {
                                        roomButton.Background = new ImageBrush(new BitmapImage(new Uri("대기방_준비.png", UriKind.Relative)));

                                    }

                                    TextBlock roomMaster = new TextBlock(); 
                                    roomMaster.Text = roomMasterID; // 방장 아이디 보여주고

                                    TextBlock roomUserNum = new TextBlock();
                                    roomUserNum.Text = roomInfoList[i].joinUserNum.ToString()+" / 2"; // 몇명있는지 보여주고
                                    
                                    roomMaster.FontSize = 11;
                                    roomMaster.Foreground = Brushes.Red;
                                    roomMaster.FontWeight = FontWeights.Bold;

                                    roomUserNum.FontSize = 11;
                                    roomUserNum.Foreground = Brushes.Red;
                                    roomUserNum.FontWeight = FontWeights.Bold;

                                    roomButton.Name = roomInfoList[i].roomMasterID;
                                    roomButton.Click += new RoutedEventHandler(roomButton_Click);
                                    
                                    desti.Children.Add(roomMaster);
                                    desti.Children.Add(roomUserNum);
                                    desti.Children.Add(roomButton);
                                }
                            }
                        }));
                    }
                    break;

                case "updateReadyRoomRes":
                    roomInfoList = JsonConvert.DeserializeObject<List<RoomInfo>>(cmds[2]);

                    roomCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            if (roomInfoList != null) // 방이 0개가 아니라면
                            {
                                for (int i = 0; i < roomInfoList.Count; i++) // 방 개수만큼 동적생성
                                {
                                    Console.WriteLine("도란나 방생성안되노");
                                    string targetGridName = "room" + (i + 1);
                                    Grid desti = (Grid)roomCanvas.FindName(targetGridName);

                                    Button roomButton = new Button();
                                    roomButton.Name = roomInfoList[i].roomMasterID;
                                    roomButton.Click += new RoutedEventHandler(roomButton_Click);
                                    desti.Children.Add(roomButton);
                                }
                            }
                        }));
                    break;

                case "guestJoinOK":
                    userInfoList = JsonConvert.DeserializeObject<List<UserInfo>>(cmds[1]);
                    
                    roomCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                     {
                         UserList.Text = null;
                         foreach (var user in userInfoList) // 반환받은 유저리스트에서 준비방에 있지 않는 유저만 따로 분리
                         {
                             if (user.joinRoomNum == 0) // 룸번호가 0번 이라는 뜻은 조인한 방이 없다는 뜻이므로 대기방에 있는 사용자
                             {
                                 UserList.Text += user.userID + "\n";
                             }
                         }
                     }));
                    break;

                case "WaittingChat":
                    Output.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            Output.Text += cmds[2] +": " + cmds[1]+"\n";
                        }));
                    break;
            }
        }

        // 접속한 사용자가 방 생성 버튼 클릭
        private void createRoomClick(object sender, RoutedEventArgs e)
        {
            // 서버로 방생성 요청 메세지 전송
            msg = "createRoom?" + userID;
            websocket.Send(msg);
        }


        // 생성된 방 버튼을 클릭 -> 방 조인!!!
        void roomButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.getInstance().Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                Console.WriteLine("버튼클릭해서 마스터 아이디 넘기기전 확인 : " + ((Button)sender).Name); // 방 마스터 아이디 확인
                ReadyRoom readyRoom = new ReadyRoom(userID, ((Button)sender).Name); // 레디룸 생성 후 조인
                MainMenu.getInstance().frame.Content = readyRoom;
            }));
        }


        private void websocket_Error(object sender, ErrorEventArgs e)
        {
            websocket.Send("socketClose?" + userID);
            MainMenu.getInstance().Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                MainMenu.getInstance().frame.Content = menu;

            }));
            Console.WriteLine("우짜쓰까");
            MessageBox.Show("서버가 닫혀 있습니다.");
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            websocket.Send("socketClose?" + userID);
        }

        private void sendClick(object sender, RoutedEventArgs e)
        {
            sendMsg();
        }

        private void joinRoomClick(object sender, RoutedEventArgs e)
        {

            msg = "joinRoom?" + userID + "?" + roomMasterID;
            websocket.Send(msg);
        }

        private void roomUpdate()
        {
            while (true)
            {
                if (websocket.Handshaked)
                {
                    msg = "updateReadyRoomReq?" + userID;
                    websocket.Send(msg);
                    Thread.Sleep(1000);
                }
            }
        }

        private void readyRoomUpdate(List<RoomInfo> roomInfoList)
        {
            /*
            roomCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                if (roomInfoList.Count > 0)
                {
                    string targetGridName = "room" + roomInfoList.Count;
                    Grid desti = (Grid)roomCanvas.FindName(targetGridName);
                    
                    Console.WriteLine("방개수는??="+ roomInfoList.Count.ToString());

                    Button roomButton = new Button();
                    //roomButton.Name = roomInfoList.Count.ToString();
                    roomButton.Click += new RoutedEventHandler(roomButton_Click);
                    desti.Children.Add(roomButton);
                }
            }));
            */
        }

        private void RoomJoinButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}