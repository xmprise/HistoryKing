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
using System.Collections;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using WebSocket4Net;
using SuperSocket.ClientEngine;
using Newtonsoft.Json;
using System.IO;
using AudioPlayerLib;
using SuperWebSocket;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using SuperSocket.SocketBase.Config;



namespace HistoryKing_client
{
    /// <summary>
    /// Interaction logic for PlayNetworkGame.xaml
    /// </summary>
    public partial class PlayNetworkGame : Page
    {

        static public List<Card> heroCardDeckList = new List<Card>();
        static public List<Card> playerCardDeckList = new List<Card>();
        static public List<Card> enemyCardDeckList = new List<Card>();

        static public List<Canvas> canvasList = new List<Canvas>();

        static public Queue<Card> playerCardDeckQueue = new Queue<Card>(); // 랜덤하게 섞인 유저 카드덱 큐
        static public Queue<Card> enemyCardDeckQueue = new Queue<Card>(); // 랜덤하게 섞인 적 카드덱 큐
        static public Queue<Card> playerCardDeckQueueSend = new Queue<Card>(); // 컨트롤러에 보낼 복사큐
        ArrayList cardNumber = new ArrayList(); // 큐의 모든 카드 번호를 담을 임시 리스트

        static public Boolean cardLock = false; // 카드를 냈는지 여부
        static public Boolean turnLock = false; // 턴이 끝났는지 여부
        static public Boolean firstTurn = true; // 첫턴인지 여부
        static public Boolean targetCardClick = false; // 적의 공격 타겟카드 지정여부
        static public Boolean attackerCardClick = false; // 유저 자신의 공격카드 지정여부
        static public Canvas targetCanvas; // 타겟 카드의 canvas 객체

        static public Canvas selectCanvas;
        static public Canvas enemySelectCanvas;
        static public Grid destiGrid;

        static public Card targetCard; // 공격타겟 카드
        static public Card attackerCard; // 공격자 카드

        static public Card enemyCard; // 적 카드
        static public Card playerCard; // 유저 카드

        static public Canvas[] playerBackCanvasList = new Canvas[4];
        static public Canvas[] playerFrontCanvasList = new Canvas[4];
        static public Canvas[] enemyBackCanvasList = new Canvas[4];
        static public Canvas[] enemyFrontCanvasList = new Canvas[4];

        static public Canvas playerCanvas;
        static public Canvas enemyHeroCanvas;

        static double targetX;
        static double targetY;

        static Point absoluteTargetPoint;
        static Point absoluteDestiPoint;

        static public int playerActionPoint = 3;
        static public int enemyActionPoint = 3;
        static public bool[] randomIndexs = new bool[4]; //수만큼 bool형 선언

        static public Canvas tempTarget;

        static public int time = 0;
        static public int time2 = 0;
        static public int time3 = 0;
        static public int time5 = 0;
        static public int time6 = 0;
        static public int time7 = 0;
        static public DispatcherTimer timer;
        static public DispatcherTimer timer2;
        static public DispatcherTimer timer3;
        static public DispatcherTimer timer4;
        static public DispatcherTimer timer5;

        static public DispatcherTimer skillTimer;
        static public DispatcherTimer skillTimer2; 

        static public int enemyBackTime;
        static public int playerBackTime;

        string userID = null;
        string enemyID = null;
        static string playerCardDeckQueueJson = null;
        static string palyerCardJson = null;
        static string enemyCardDeckQueueJson = null;
        static string enemyCardJson = null;
        string roomMasterID = null;
        string guestID = null;
        bool myTurn = false;

        readonly AudioPlayer attackSound = new AudioPlayer("MOZUOTO.WAV", false);
        readonly AudioPlayer damagedSound = new AudioPlayer("Damaged.WAV", false);
        readonly AudioPlayer dieSound = new AudioPlayer("Die.WAV", false);

        GifImage slashGif = new GifImage();
        GifImage dieGif = new GifImage();

        GifImage attackSkillGif0 = new GifImage();
        GifImage attackSkillGif1 = new GifImage();
        GifImage attackSkillGif2 = new GifImage();
        GifImage attackSkillGif3 = new GifImage();

        GifImage healthSkillGif0 = new GifImage();
        GifImage healthSkillGif1 = new GifImage();
        GifImage healthSkillGif2 = new GifImage();
        GifImage healthSkillGif3 = new GifImage();

        GifImage turnSkillGif = new GifImage();

        int goldChanceNum = 0;
        int skillEffect = 0;
        Storyboard skillStory;

        static List<WebSocketSession> controlSessionList = new List<WebSocketSession>();
        string enemyFrontCanvasListJson = null;
        string[] controlSendEnemy = new string[4];

        

        public PlayNetworkGame(string userID, string guestID, string roomMasterID)
        {
            InitializeComponent();
            var websocketServer = new WebSocketServer();  // 소켓 생성

            websocketServer.NewDataReceived += new SessionEventHandler<WebSocketSession, byte[]>(websocketServer_NewDataReceived);
            websocketServer.NewMessageReceived += new SessionEventHandler<WebSocketSession, string>(websocketServer_NewMessageReceived);
            websocketServer.NewSessionConnected += new SessionEventHandler<WebSocketSession>(websocketServer_NewSessionConnected);
            websocketServer.SessionClosed += new SessionEventHandler<WebSocketSession, CloseReason>(websocketServer_SessionClosed);
            websocketServer.Setup(new RootConfig(), new ServerConfig
            {
                Port = 1338, // 포트설정
                Ip = "210.118.69.142", // 아이피설정
                MaxConnectionNumber = 100,  // 최대접속 인원
                Mode = SocketMode.Async, // 비동기 모드
                Name = "SuperWebSocket Server",// 네임
                ReceiveBufferSize = 10000,
                SendBufferSize = 10000
            }, SocketServerFactory.Instance); // 싱글톤인가???

            websocketServer.Start(); // 웹서버 시작
            Console.WriteLine("잘 되네????");
            Console.WriteLine("The server is started, press 'Q' to quit the server!"); 

            //웹 소켓 핸들러들 등록하고 ^^
            WaittingRoom.websocket.MessageReceived += new EventHandler<WebSocket4Net.MessageReceivedEventArgs>(websocket_MessageReceived);
            WaittingRoom.websocket.Closed += new EventHandler(websocket_Closed);
            WaittingRoom.websocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);

            // 준비방에서 넘어 온 본인 아이디와 상대방 아이디 셋
            this.userID = userID;
            this.guestID = guestID;
            this.roomMasterID = roomMasterID;            

            // 상대방 캔버스 이름에 상대방 아이디 입력
            if (userID.Equals(guestID))
                enemyID = roomMasterID;
            else if (userID.Equals(roomMasterID))
                enemyID = guestID;

            // 라벨에 아이디 넣어주고
            User_ID.Content = userID;
            Enemy_ID.Content = enemyID;

                //this.enemyID = enemyID;
            
                Console.WriteLine("userID" + userID);

            playerCard = new Card(); // 플레이어카드 객체생성
            //playerCard.CardHP = 2000; // 플레이어카드 HP 셋
            playerCard.CardName = Player.getInstance().getID(); // 플레이어카드 이름 셋
            
            playerLoad();

            slashGif.GifSource = "pack://application:,,,/attack.gif"; // slash 효과 gif 파일 경로 지정
            dieGif.GifSource = "pack://application:,,,/explosion.gif";
            attackSkillGif0.GifSource = "pack://application:,,,/attackSkill.gif";
            attackSkillGif1.GifSource = "pack://application:,,,/attackSkill.gif";
            attackSkillGif2.GifSource = "pack://application:,,,/attackSkill.gif";
            attackSkillGif3.GifSource = "pack://application:,,,/attackSkill.gif";

            attackSkillGif0.Visibility = Visibility.Hidden;
            attackSkillGif1.Visibility = Visibility.Hidden;
            attackSkillGif2.Visibility = Visibility.Hidden;
            attackSkillGif3.Visibility = Visibility.Hidden;

            AttackUpSkill0.Children.Add(attackSkillGif0);
            AttackUpSkill1.Children.Add(attackSkillGif1);
            AttackUpSkill2.Children.Add(attackSkillGif2);
            AttackUpSkill3.Children.Add(attackSkillGif3);

            slashGif.Visibility = Visibility.Hidden;
            dieGif.Visibility = Visibility.Hidden;

            enemyAttackPanel.Children.Add(slashGif);
            enemyDiePanel.Children.Add(dieGif);

            SkillTextBlock.Visibility = Visibility.Hidden;
            myFace();
            

            playerSubCardLoad(); // 유저 카드 정보 DB에서 로드
            playerCardDeckQueue = ShuffleList<Card>(playerCardDeckList); // 유저 카드 랜덤 섞기
            foreach (var item in playerCardDeckQueue)
            {
                cardNumber.Add(item.cardNumber); //하나씩 담고
            }
            
            if (userID.Equals(guestID)) // 내가 손님이면
                WaittingRoom.websocket.Send("GuestStartOK?" + guestID + "?" + roomMasterID); // 서버를 통해 상대방에게 랜덤하게 섞인 상대방 카드 큐정보 요청
            else if (userID.Equals(roomMasterID)) // 내가 방장이면
                WaittingRoom.websocket.Send("RoomMasterOK?" + guestID); // 서버를 통해 상대방에게 랜덤하게 섞인 상대방 카드 큐정보 요청

            //WaittingRoom.websocket.Send("EnemyCardDeckReq?" + guestID); // 서버를 통해 상대방에게 랜덤하게 섞인 상대방 카드 큐정보 요청
            // 여기 까지 진행되면 적의 랜덤하게 섞인 카드 정보 셋팅.....

            skillStory = this.Resources["OnTextInput1"] as Storyboard;
        }

        void websocketServer_SessionClosed(WebSocketSession session, CloseReason e)
        {
            Console.WriteLine("세션 종료");
        }

        void websocketServer_NewSessionConnected(WebSocketSession session)
        {
            Console.WriteLine("컨트롤러 접속");
            controlSessionList.Add(session);
        }

        void websocketServer_NewMessageReceived(WebSocketSession session, string message)
        {
            string[] cmds = message.Split('?'); // 토큰 ? 로 스트링 구분

            switch (cmds[0])
            {
                case "ControlAccess":  // 최초 접속시에 클라이언트에서 보내는 헤더 메세지
                    Console.WriteLine("오 컨트롤러가 접속했어." + cmds[0]);

                    string cardNumberJson = null; // 카드 넘버 json 메세지 담을 변수

                    Console.WriteLine(playerCardDeckQueueSend.Count);
                    cardNumberJson = JsonConvert.SerializeObject(cardNumber, Formatting.Indented);
                    Console.WriteLine("컨트롤러에게 보내기전 QueueJson : " + cardNumberJson);
                    session.SendResponse("CardDeckRes/"+cardNumberJson);
                    break;

                case "BackToFront":
                    GameCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        Console.WriteLine(cmds[1]);
                        Canvas tempCanvas = null;
                        foreach (var item2 in playerBackCanvasList)
                        {
                            if (item2 != null)
                            {
                                if (((TextBlock)item2.Children[9]).Text.Equals(cmds[1])) // 캔버스의 넘버가  컨트롤러에서 넘어온 카드 넘버랑 일치하면
                                {
                                    tempCanvas = item2;
                                    break;
                                }
                            }
                        }

                        playerBackToFront(tempCanvas);
                    }));
                    break;

                case "FinishTurn": // 컨트롤러에서 요청온 메시지...
                    Console.WriteLine("턴 종료 메세지 => " + cmds[1]);
                    //////////
                    if (myTurn == true) // 내턴일 때만 동작하도록 예외 처리
                    {
                        if (playerActionPoint != 0) // 내가 아직 포인트 다 안썼을 경우
                        {
                            MessageBoxResult result = MessageBox.Show("아직 행동포인트가 남았습니다.\n 턴을 종료하시겠습니까?", "행동{", MessageBoxButton.OKCancel);
                            // 카드덱에 카드 있고, 확인이면
                            if (result == MessageBoxResult.Cancel)
                            {
                                return;
                            }
                        }

                        MessageBox.Show("턴을 종료합니다");

                        myTurn = false;
                        // 사용자가 누구인지에 따라 내 턴이 끝났다는 걸 상대방에게 알려줌
                        if (userID.Equals(guestID))
                            WaittingRoom.websocket.Send("FinishTurn?" + roomMasterID);
                        else if (userID.Equals(roomMasterID))
                            WaittingRoom.websocket.Send("FinishTurn?" + guestID);
                        enemyBackTime = checkBackBlankCount(enemyBackCanvasList); // 카드 빈 곳의 개수를 파악해서 타이머 얼마나 돌지 셋

                        //첫턴이 아닌 턴부터 유저의 카드 꺼내놓는 타이머
                        timer3 = new DispatcherTimer();
                        timer3.Interval = TimeSpan.FromSeconds(1);
                        timer3.Tick += new EventHandler(timer3_Tick);
                        timer3.Start();
                    //////////
                    }

                    break;

                case "Attack":
                    Console.WriteLine("컨트롤 어택 메세지 =>"+ cmds[1]+"/////"+cmds[2]);
                    Canvas tempCanvas2=null;
                    int index = Convert.ToInt32(cmds[1]) -1;
                    
                    // 컨트롤러에게서 받은 cmds[2]는 공격자 자신의 카드 번호 임
                    // 카드 번호로 검색하여 Canvas 객체 추출...
                    foreach (var canvas in playerFrontCanvasList)
                    {
                        if (((TextBlock)canvas.Children[9]).Text.Equals(cmds[2]))
                        {
                            tempCanvas2 = canvas;
                            break;
                        }
                    }

                    Console.WriteLine("공격자=>"+tempCanvas2.Name);
                    Console.WriteLine("타겟=>"+enemyFrontCanvasList[Convert.ToInt32(cmds[1])]);

                    enemyAttack(enemyFrontCanvasList[Convert.ToInt32(cmds[1])], tempCanvas2);  
                    break;

            }
        }

        void websocketServer_NewDataReceived(WebSocketSession session, byte[] e)
        {
            
        }



        private void networkGameLoaded(object sender, RoutedEventArgs e)
        {

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
                enemyCardLoad(System.Drawing.Image.FromStream(ms));
            }

        }

        private void getImage(System.Drawing.Image getimg)
        {
            playerCanvas = new Canvas();
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(getimg);
            IntPtr hBitmap = bmp.GetHbitmap();
            System.Windows.Media.ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            img.Source = wpfBitmap;
            img.Width = 150;
            img.Height = 200;
            img.Stretch = System.Windows.Media.Stretch.Fill;

            TextBlock hp = new TextBlock();
            TextBlock dam = new TextBlock();
            TextBlock skill = new TextBlock();

            hp.Name = "hp";
            dam.Name = "dam";
            skill.Name = "skill";

            hp.Text = "2000";
            dam.Text = "0";

            hp.FontSize = 20;
            hp.Foreground = Brushes.Red;
            hp.FontWeight = FontWeights.Bold;

            dam.FontSize = 20;
            dam.Foreground = Brushes.Red;
            dam.FontWeight = FontWeights.Bold;

            Canvas.SetLeft(hp, 170);
            Canvas.SetBottom(hp, 70);

            Canvas.SetLeft(dam, 60);
            Canvas.SetBottom(dam, 70);

            Canvas.SetLeft(img, 50);
            Canvas.SetBottom(img, 80);

            playerCanvas.Children.Add(hp);
            playerCanvas.Children.Add(dam);
            playerCanvas.Children.Add(img);

            playerCanvas.Width = 250;
            playerCanvas.Height = 300;

            //Canvas.SetTop(canvas, 0);
            Canvas.SetLeft(playerCanvas, 30);
            Canvas.SetTop(playerCanvas, 646);
            //Canvas.SetRight(canvas, 1600);

            GameCanvas.Children.Add(playerCanvas);
        }

        void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {

        }

        void websocket_Closed(object sender, EventArgs e)
        {

        }

        void websocket_MessageReceived(object sender, WebSocket4Net.MessageReceivedEventArgs e)
        {
            string[] cmds = e.Message.Split('?'); // 토큰 ? 로 스트링 구분

            switch (cmds[0])
            {
                case "GuestCardDeckReq": // 게스트인 나의 카드 정보에 대한 요청이 왔음.../

                    playerCardDeckQueueJson = JsonConvert.SerializeObject(playerCardDeckQueue, Formatting.Indented);
                    palyerCardJson = JsonConvert.SerializeObject(playerCard, Formatting.Indented);

                    // 게스트인 나의 카드 정보 보내줌
                    WaittingRoom.websocket.Send("GuestCardDeckRes?" + roomMasterID + "?" + palyerCardJson + "?" + playerCardDeckQueueJson);
                    break;

                case "GuestCardDeckRes": // 방장인 나에게 게스트 카드 정보 반환..

                    enemyCard = JsonConvert.DeserializeObject<Card>(cmds[1]);

                    List<Card> enemyCardDeckQueue2 = JsonConvert.DeserializeObject<List<Card>>(cmds[2]); // 큐를 바로 변환 못해서.. 리스트로 변환후

                    enemyCardDeckQueue = new Queue<Card>(enemyCardDeckQueue2); // 다시 큐로 변환

                    playerCardDeckQueueJson = JsonConvert.SerializeObject(playerCardDeckQueue, Formatting.Indented);
                    palyerCardJson = JsonConvert.SerializeObject(playerCard, Formatting.Indented);

                    WaittingRoom.websocket.Send("RoomMasterSetOK?" + guestID + "?" + palyerCardJson + "?" + playerCardDeckQueueJson);

                    GameCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        //enemyCardLoad();
                        enemyFace();
                        GameCanvas.Background = new ImageBrush(new BitmapImage(new Uri("배경1.png", UriKind.Relative)));
                        setCard();
                    }));
                    break;

                case "RoomMasterCardDeckRes":  // 방장 카드 정보 반환...

                    enemyCard = JsonConvert.DeserializeObject<Card>(cmds[1]);
                    List<Card> enemyCardDeckQueue3 = JsonConvert.DeserializeObject<List<Card>>(cmds[2]); // 큐를 바로 변환 못해서.. 리스트로 변환후

                    enemyCardDeckQueue = new Queue<Card>(enemyCardDeckQueue3); // 다시 큐로 변환

                    GameCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        //enemyCardLoad();
                        enemyFace();
                        GameCanvas.Background = new ImageBrush(new BitmapImage(new Uri("배경1.png", UriKind.Relative)));
                        setCard();
                    }));
                    break;

                case "GuestFirstTurnStartRes": // 서버로부터 손님유저에게 첫턴을 먼저 시작하라는 명령
                    MessageBox.Show("첫 턴은 니 차례야~! GO!!!");
                    myTurn = true;
                    break;

                case "backToFront":
                    GameCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        enemyCardStickOut(Convert.ToInt32(cmds[1]), Convert.ToInt32(cmds[2]));
                    }));
                    break;

                case "FinishTurn":
                    playerActionPoint = 3;
                    // 유저 front 카드의 Lock 해제 + 스킬버튼 숨기고
                    GameCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {

                    foreach (var item in playerFrontCanvasList)
                    {
                        if (item != null)
                        {
                            ((TextBlock)item.Children[8]).Text = "0";
                        }
                    }

                    MessageBox.Show("니 차례 입니다.");

                    //턴 끝나자마자 컨트롤러에게 적 front 정보 날리기
                    for (int i = 0; i < 4; i++)
                    {
                        if (enemyFrontCanvasList[i] == null)
                        {
                            controlSendEnemy[i] = "false";
                        }
                        else
                            controlSendEnemy[i] = "true";
                    }

                    //enemyFrontCanvasListJson = JsonConvert.SerializeObject(controlSendEnemy, Formatting.Indented); // 컨트롤에게 보낼 front 정보
                    foreach (var item in controlSessionList)
                    {
                        item.SendResponse("Enemy/" + controlSendEnemy[0] + "/" + controlSendEnemy[1] + "/" + controlSendEnemy[2] + "/" + controlSendEnemy[3]); // 컨트롤에게 전송!
                    }


                    playerBackTime = checkBackBlankCount(playerBackCanvasList); // 빈공간 몇개인지 파악해서 타임 돌아갈 횟수 겟..
                    
                        //첫턴이 아닌 턴부터 유저의 카드 꺼내놓는 타이머
                        timer2 = new DispatcherTimer();
                        timer2.Interval = TimeSpan.FromSeconds(1);
                        timer2.Tick += new EventHandler(timer2_Tick);
                        timer2.Start();
                    }));

                    break;

                case "MeAttack": // 상대방이 나를 공격한다면
                    GameCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                   {
                       userAttack(cmds[1], cmds[2]);
                   }));
                    break;

                case "Skill":
                    GameCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        int index = Convert.ToInt32(cmds[3]);

                        foreach (var item in enemyFrontCanvasList)
                        {
                            if (item != null)
                            {
                                switch (cmds[1])
                                {
                                    case "AttkUP":
                                        int attackPoint = 0;
                                        int selectCanvasAttackPoint = Convert.ToInt32(cmds[2]);
                                        attackPoint = Convert.ToInt32(((TextBlock)item.Children[1]).Text);
                                        attackPoint += selectCanvasAttackPoint;
                                        ((TextBlock)item.Children[1]).Text = attackPoint.ToString();
                                        skillEffect = 2;

                                        SkillTextBlock.Visibility = Visibility.Visible;
                                        SkillTextBlock.Text = cmds[4];
                                        skillStory.Completed +=new EventHandler(skillStory_Completed);
                                        skillStory.Begin();
                                                                                
                                        break;

                                    case "HpUP":
                                        int healthPoint = 0;
                                        int selectCanvasHealthPoint = Convert.ToInt32(cmds[2]);
                                        healthPoint = Convert.ToInt32(((TextBlock)item.Children[0]).Text);
                                        healthPoint += selectCanvasHealthPoint;
                                        ((TextBlock)item.Children[1]).Text = healthPoint.ToString();
                                        
                                        SkillTextBlock.Visibility = Visibility.Visible;
                                        SkillTextBlock.Text = cmds[4];
                                        skillStory.Begin();

                                        break;
                                   /*
                                    case "ActionUP":
                                        int selectCanvasTnPoint = Convert.ToInt32(cmds[2]);
                                         = Convert.ToInt32(((TextBlock)item.Children[1]).Text);
                                        attackPoint += selectCanvasAttackPoint;
                                        ((TextBlock)item.Children[1]).Text = attackPoint.ToString();
                                        
                                        break;*/
                                }
                            }

                            GameCanvas.Children.Remove(enemyFrontCanvasList[index]);
                            enemyFrontCanvasList[index] = null;
                        }
                    }));
                    break;
                /*
                case "YourCardReq":
                    Console.WriteLine("카드덱 반환 요청이 들어왔다.");

                    playerCardDeckQueueJson = JsonConvert.SerializeObject(playerCardDeckQueue, Formatting.Indented);
                    palyerCardJson = JsonConvert.SerializeObject(playerCard, Formatting.Indented);

                    Console.WriteLine("변환된거" + playerCardDeckQueueJson);
                    Console.WriteLine("변환된거" + palyerCardJson);
                    
                    WaittingRoom.websocket.Send("MyCardRes?" + roomMasterID + "?" + palyerCardJson + "?" + playerCardDeckQueueJson);
                    break;

                case "EnemyCardDeckRes":

                    Console.WriteLine("카드덱 요청에 대한 카드덱 정보가 반환됐다.");

                    enemyCard = JsonConvert.DeserializeObject<Card>(cmds[1]);
                    //enemyCardDeckQueue = JsonConvert.DeserializeObject<Queue<Card>>(cmds[2]);
                    List<Card> enemyCardDeckQueue2 = JsonConvert.DeserializeObject<List<Card>>(cmds[2]); // 큐를 바로 변환 못해서.. 리스트로 변환후

                    enemyCardDeckQueue = new Queue<Card>(enemyCardDeckQueue2); // 다시 큐로 변환

                    Console.WriteLine(enemyCardDeckQueue.Count);

                    GameCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                   {
                        enemyCardLoad();
                       
                    }));

                    WaittingRoom.websocket.Send("QueueSetOK");
                    break;

                case "Continue":
                    GameCanvas.Background = new ImageBrush(new BitmapImage(new Uri("배경1.png", UriKind.Relative)));
                     setCard();
                    break;*/
            }
        }

        //첫턴이 아닌 턴부터 유저의 카드 꺼내놓는 타이머
        void timer2_Tick(object sender, EventArgs e)
        {
            time2++;
            Console.WriteLine("틱" + time2);

            checkPlayerBack();

            if (time2 == playerBackTime + 1 || playerCardDeckQueue.Count == 0)
            {
                timer2.Stop();
                time2 = 0;
                myTurn = true;
            }
        }
        private void playerLoad()
        {
            playerCanvas = new Canvas();
            
            TextBlock hp = new TextBlock();
            TextBlock dam = new TextBlock();
            TextBlock skill = new TextBlock();

            hp.Name = "hp";
            dam.Name = "dam";
            skill.Name = "skill";

            hp.Text = "2000";
            dam.Text = playerCard.CardDam.ToString();
            
            hp.FontSize = 20;
            hp.Foreground = Brushes.Red;
            hp.FontWeight = FontWeights.Bold;

            dam.FontSize = 20;
            dam.Foreground = Brushes.Red;
            dam.FontWeight = FontWeights.Bold;

            Canvas.SetLeft(hp, 170);
            Canvas.SetBottom(hp, 70);

            Canvas.SetLeft(dam, 60);
            Canvas.SetBottom(dam, 70);

            playerCanvas.Children.Add(hp);
            playerCanvas.Children.Add(dam);

            //playerCanvas.Children.Add(img);

            playerCanvas.Width = 250;
            playerCanvas.Height = 300;

            //Canvas.SetTop(canvas, 0);
            Canvas.SetLeft(playerCanvas, 30);
            Canvas.SetTop(playerCanvas, 646);
            //Canvas.SetRight(canvas, 1600);

            GameCanvas.Children.Add(playerCanvas);
        }


        //주인공 쫄카드 정보 로드
        private void playerSubCardLoad()
        {
            CardDataContext card = new CardDataContext();
            Card cards;
            var innerJoinQuery = from Deck in card.GameDeck
                                 join Hero in card.HeroCard on Deck.CardNumberID equals Hero.CardNumberID
                                 where Deck.MemberName == Player.getInstance().getID()
                                 select Hero;

            
            //DB의 유저의 쫄 카드 정보 받아와서 Card객체 만들고 DeckList에 삽입
            foreach (var item in innerJoinQuery)
            {
                int i = 1;
                cards = new Card();
                cards.CardName = item.HeroCardName;
                cards.CardHP = item.HeroCardHealthPoint;
                cards.CardDam = item.HeroCardAttackPoint;
                cards.cardNumber = (int)item.CardNumberID;

                playerCardDeckList.Add(cards);


                var innerJoinQuery2 = from Deck in card.HeroCard
                                      join Magic in card.HeroCardSkill on Deck.HeroCardName equals Magic.HeroCardName
                                      where Deck.HeroCardName == cards.CardName
                                      select Magic;

                foreach (var item2 in innerJoinQuery2)
                {
                    if (i == 1)
                    {
                        cards.skill1 = item2.SkillName;
                        cards.attackUpSkill = item2.SkillPoint;
                    }
                    else if (i == 2)
                    {
                        cards.skill2 = item2.SkillName;
                        cards.healthUpSkill = item2.SkillPoint;
                    }
                    else if (i == 3)
                    {
                        cards.skill3 = item2.SkillName;
                        cards.turnUpSkill = item2.SkillPoint;
                    }
                    i++;
                }
            }
        }

        //적 카드 정보 로드
        private void enemyCardLoad(System.Drawing.Image getimg)
        {
            //적 영웅 로드

            enemyHeroCanvas = new Canvas();

            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(getimg);
            IntPtr hBitmap = bmp.GetHbitmap();
            System.Windows.Media.ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            img.Source = wpfBitmap;
            img.Width = 150;
            img.Height = 200;
            img.Stretch = System.Windows.Media.Stretch.Fill;

            // 상대방 캔버스 이름에 상대방 아이디 입력
            if(userID.Equals(guestID))
                enemyHeroCanvas.Name = roomMasterID;
            else if(userID.Equals(roomMasterID))
                enemyHeroCanvas.Name = guestID;

            //enemyHeroCanvas.Background = new ImageBrush(new BitmapImage(new Uri("Test.png", UriKind.Relative)));
            enemyHeroCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(enemyCard_MouseLeftButtonDown);

            TextBlock hp = new TextBlock();
            TextBlock dam = new TextBlock();
            TextBlock skill = new TextBlock();
            TextBlock col = new TextBlock();

            hp.Name = "hp";
            dam.Name = "dam";
            skill.Name = "skill";

            hp.Text ="2000";
            dam.Text = "0";
            col.Text = "0";

            hp.FontSize = 20;
            hp.Foreground = Brushes.Red;
            hp.FontWeight = FontWeights.Bold;

            dam.FontSize = 20;
            dam.Foreground = Brushes.Red;
            dam.FontWeight = FontWeights.Bold;

            Canvas.SetLeft(hp, 170);
            Canvas.SetBottom(hp, 70);

            Canvas.SetLeft(dam, 60);
            Canvas.SetBottom(dam, 70);

            Canvas.SetLeft(img, 50);
            Canvas.SetBottom(img, 80);

            enemyHeroCanvas.Children.Add(hp);
            enemyHeroCanvas.Children.Add(dam);
            enemyHeroCanvas.Children.Add(skill);
            enemyHeroCanvas.Children.Add(col);
            enemyHeroCanvas.Children.Add(img);

            enemyHeroCanvas.Width = 250;
            enemyHeroCanvas.Height = 300;

            Canvas.SetTop(enemyHeroCanvas, 0);
            // Canvas.SetLeft(canvas, 0);
            //Canvas.SetBottom(canvas, 1000);
            Canvas.SetLeft(enemyHeroCanvas, 1400 - 250);

            GameCanvas.Children.Add(enemyHeroCanvas);
            Console.WriteLine("얼굴!!얼굴!!!!");
        }

        //카드 랜덤 섞기
        private Queue<E> ShuffleList<E>(List<E> inputList)
        {
            Queue<E> randomList = new Queue<E>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Enqueue(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }

        private void setCard()
        {
            GameCanvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
           {
               // 디스패처 타이머로 애니메이션 1초마다 실행 되게 설정..
               timer = new DispatcherTimer();
               timer.Interval = TimeSpan.FromSeconds(1);
               timer.Tick += new EventHandler(timer_Tick);
               timer.Start();
           }));
        }

        void timer_Tick(object sender, EventArgs e)
        {
            time++;

            // 타이머가 9초되면 스탑
            if (time == 1 || time == 2 || time == 3 || time == 4)
            {
                Card card = new Card();
                Canvas playerSubCanvas = new Canvas();
                card = playerCardDeckQueue.Dequeue();
                playerSubCanvas.Name = card.CardName;
                int i = time - 1;

                playerSubCanvas.Background = new ImageBrush(new BitmapImage(new Uri(card.CardName + ".png", UriKind.Relative)));
                TextBlock hp = new TextBlock();
                TextBlock dam = new TextBlock();
                TextBlock skill = new TextBlock();
                TextBlock col = new TextBlock();
                TextBlock row = new TextBlock();
                TextBlock skillLine1 = new TextBlock();
                TextBlock skillLine2 = new TextBlock();
                TextBlock skillLine3 = new TextBlock();
                TextBlock action = new TextBlock();
                TextBlock number = new TextBlock();

                hp.Name = "hp";
                dam.Name = "dam";
                skill.Name = "skill";
                col.Name = "index";
                row.Name = "gridName";
             
                hp.Text = card.CardHP.ToString();
                dam.Text = card.CardDam.ToString();
                col.Text = "0";
                row.Text = i.ToString();
                skillLine1.Text = card.skill1 + ": 공격력UP" + card.attackUpSkill;
                skillLine2.Text = card.skill2 + ": 체력UP" + card.healthUpSkill;
                skillLine3.Text = card.skill3 + ": 턴UP" + card.turnUpSkill;
                action.Text = "0";
                number.Text = card.cardNumber.ToString();

                hp.FontSize = 15;
                hp.Foreground = Brushes.Red;
                hp.FontWeight = FontWeights.Bold;

                dam.FontSize = 15;
                dam.Foreground = Brushes.Red;
                dam.FontWeight = FontWeights.Bold;

                skillLine1.FontSize = 11;
                skillLine1.Foreground = Brushes.Blue;
                skillLine1.FontWeight = FontWeights.Bold;

                skillLine2.FontSize = 11;
                skillLine2.Foreground = Brushes.Blue;
                skillLine2.FontWeight = FontWeights.Bold;

                skillLine3.FontSize = 11;
                skillLine3.Foreground = Brushes.Blue;
                skillLine3.FontWeight = FontWeights.Bold;

                playerBackCanvasList[i] = playerSubCanvas;

                Canvas.SetLeft(hp, 100);
                Canvas.SetBottom(hp, 47);

                Canvas.SetLeft(dam, 33);
                Canvas.SetBottom(dam, 47);

                Canvas.SetLeft(skillLine1, 10);
                Canvas.SetBottom(skillLine1, 35);

                Canvas.SetLeft(skillLine2, 10);
                Canvas.SetBottom(skillLine2, 22);

                Canvas.SetLeft(skillLine3, 10);
                Canvas.SetBottom(skillLine3, 11);

                playerSubCanvas.Children.Add(hp);
                playerSubCanvas.Children.Add(dam);
                playerSubCanvas.Children.Add(skill);
                playerSubCanvas.Children.Add(col);
                playerSubCanvas.Children.Add(row);
                playerSubCanvas.Children.Add(skillLine1);
                playerSubCanvas.Children.Add(skillLine2);
                playerSubCanvas.Children.Add(skillLine3);
                playerSubCanvas.Children.Add(action);
                playerSubCanvas.Children.Add(number);

                playerSubCanvas.Width = 150;
                playerSubCanvas.Height = 200;

                Canvas.SetTop(playerSubCanvas, 1000 - 200);
                Canvas.SetLeft(playerSubCanvas, 1400 - 150);


                GameCanvas.Children.Add(playerSubCanvas);
                string targetGridName = "NetMe" + i;
                //mvAni.Invoke(playerSubCanvas, (Grid)Play_Stage.FindName(targetGridName), i);
                moveAnimation(playerSubCanvas, (Grid)GameCanvas.FindName(targetGridName), i);

                //moveAnimation(playerSubCanvas, Me, i);
                playerSubCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(userCard_MouseLeftButtonDown);
            }
            else if (time == 5 || time == 6 || time == 7 || time == 8)
            {
                Card card = new Card();
                Canvas enemySubCanvas = new Canvas();
                card = enemyCardDeckQueue.Dequeue();
                enemySubCanvas.Name = card.CardName;
                //enemySubCanvas.Background = new ImageBrush(new BitmapImage(new Uri(card.CardName + ".png", UriKind.Relative)));
                enemySubCanvas.Background = new ImageBrush(new BitmapImage(new Uri("CardBack.png", UriKind.Relative)));

                Point relativePoint = TranslatePoint(new Point(0.0, 0.0), enemySubCanvas);
                int i = time - 5;

                TextBlock hp = new TextBlock();
                TextBlock dam = new TextBlock();
                TextBlock skill = new TextBlock();
                TextBlock col = new TextBlock();
                TextBlock row = new TextBlock();
                TextBlock skillLine1 = new TextBlock();
                TextBlock skillLine2 = new TextBlock();
                TextBlock skillLine3 = new TextBlock();
                TextBlock action = new TextBlock();
                TextBlock number = new TextBlock();

                hp.Name = "hp";
                dam.Name = "dam";
                skill.Name = "skill";
                col.Name = "index";
                row.Name = "gridName";

                hp.Text = card.CardHP.ToString();
                dam.Text = card.CardDam.ToString();
                col.Text = "0";
                row.Text = i.ToString();
                skillLine1.Text = card.skill1 + ": 공격력UP" + card.attackUpSkill;
                skillLine2.Text = card.skill2 + ": 체력UP" + card.healthUpSkill;
                skillLine3.Text = card.skill3 + ": 턴UP" + card.turnUpSkill;
                action.Text = "0";
                number.Text = card.cardNumber.ToString();

                hp.FontSize = 15;
                hp.Foreground = Brushes.Red;
                hp.FontWeight = FontWeights.Bold;

                dam.FontSize = 15;
                dam.Foreground = Brushes.Red;
                dam.FontWeight = FontWeights.Bold;

                skillLine1.FontSize = 11;
                skillLine1.Foreground = Brushes.Blue;
                skillLine1.FontWeight = FontWeights.Bold;

                skillLine2.FontSize = 11;
                skillLine2.Foreground = Brushes.Blue;
                skillLine2.FontWeight = FontWeights.Bold;

                skillLine3.FontSize = 11;
                skillLine3.Foreground = Brushes.Blue;
                skillLine3.FontWeight = FontWeights.Bold;

                enemyBackCanvasList[i] = enemySubCanvas;

                Canvas.SetLeft(hp, 100);
                Canvas.SetBottom(hp, 47);

                Canvas.SetLeft(dam, 33);
                Canvas.SetBottom(dam, 47);

                Canvas.SetLeft(skillLine1, 10);
                Canvas.SetBottom(skillLine1, 35);

                Canvas.SetLeft(skillLine2, 10);
                Canvas.SetBottom(skillLine2, 22);

                Canvas.SetLeft(skillLine3, 10);
                Canvas.SetBottom(skillLine3, 11);


                enemySubCanvas.Children.Add(hp);
                enemySubCanvas.Children.Add(dam);
                enemySubCanvas.Children.Add(skill);
                enemySubCanvas.Children.Add(col);
                enemySubCanvas.Children.Add(row);
                enemySubCanvas.Children.Add(skillLine1);
                enemySubCanvas.Children.Add(skillLine2);
                enemySubCanvas.Children.Add(skillLine3);
                enemySubCanvas.Children.Add(action);
                enemySubCanvas.Children.Add(number);

                hp.Visibility = Visibility.Hidden;
                dam.Visibility = Visibility.Hidden;
                skillLine1.Visibility = Visibility.Hidden;
                skillLine2.Visibility = Visibility.Hidden;
                skillLine3.Visibility = Visibility.Hidden;

                enemySubCanvas.Width = 150;
                enemySubCanvas.Height = 200;
                Canvas.SetTop(enemySubCanvas, 0);
                Canvas.SetLeft(enemySubCanvas, 0);

                GameCanvas.Children.Add(enemySubCanvas);

                string targetGridName = "NetEnemy" + i;
                //mvAni.Invoke(enemySubCanvas, (Grid)Play_Stage.FindName(targetGridName), i);
                moveAnimation(enemySubCanvas, (Grid)GameCanvas.FindName(targetGridName), i);

                //moveAnimation(enemySubCanvas, EnemySub, i);
                enemySubCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(enemyCard_MouseLeftButtonDown);
            }
            if (time == 8)
            {
                timer.Stop();
                if (userID.Equals(roomMasterID)) // 방장이 카드 셋팅이 끝이나면
                    WaittingRoom.websocket.Send("GuestFirstTurnStartReq?" + guestID); // 손님 먼저 시작하라고 요청
                time = 0;
            }
        }

        void timer5_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        // 카드 낼때 스무스 하게 애니메이션 적용 
        public void moveAnimation(Canvas target, Grid desti, int index)
        {
            absoluteDestiPoint = desti.TransformToAncestor(GameCanvas).Transform(new Point(0, 0)); //절대좌표 구하는식
            absoluteTargetPoint = target.TransformToAncestor(GameCanvas).Transform(new Point(0, 0));

            //Point relativePoint = TranslatePoint(new Point(0.0, 0.0), desti); // 상대좌표 구하는 식.. 
            //Point centerPoint = new Point(desti.ColumnDefinitions[0].ActualWidth, desti.ActualHeight); // 중심점 좌표 구하는식.. 아놔 좌표마스터
            tempTarget = target;

            targetX = Canvas.GetLeft(target);
            targetY = Canvas.GetTop(target);

            Storyboard story = new Storyboard();
            DoubleAnimation daX = new DoubleAnimation();
            DoubleAnimation daY = new DoubleAnimation();
            //랜드트랜스폼 생성
            TransformGroup tg = new TransformGroup();
            tg.Children.Add(new TranslateTransform());
            target.RenderTransform = tg;
            
            //daX.From = targetX;
            daX.To = absoluteDestiPoint.X - Canvas.GetLeft(target);
            //daTemp.To = absoluteDestiPoint.X  - absoluteTargetPoint.X;
            daX.Duration = new Duration(TimeSpan.FromSeconds(0.25));
            daX.FillBehavior = FillBehavior.Stop;
            Storyboard.SetTarget(daX, target);
            Storyboard.SetTargetProperty(daX, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
            story.Children.Add(daX);

            // target.BeginAnimation(Canvas.LeftProperty, daX);
            // target.BeginAnimation(Canvas.LeftProperty, null);
            // Canvas.SetLeft(target, targetX);

            daY = new DoubleAnimation();

            //daY.From = targetY;
            daY.To = absoluteDestiPoint.Y - Canvas.GetTop(target);
            daY.Duration = new Duration(TimeSpan.FromSeconds(0.25));
            daY.FillBehavior = FillBehavior.Stop;
            Storyboard.SetTarget(daY, target);
            Storyboard.SetTargetProperty(daY, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
            story.Children.Add(daY);
            story.Duration = daY.Duration;

            story.Completed += new EventHandler(strCompleted);
            story.Begin();

            //target.BeginAnimation(Canvas.TopProperty, daY);
            //target.BeginAnimation(Canvas.LeftProperty, daX);

            //target.BeginAnimation(Canvas.TopProperty, null);
            //target.BeginAnimation(Canvas.LeftProperty, null);

            //target.BeginAnimation(Canvas.TopProperty, daY);
            //target.BeginAnimation(Canvas.TopProperty, null);
            //Canvas.SetTop(target, targetY);
            //BeginAnimation(Canvas.TopProperty, null);
            //BeginAnimation(Canvas.LeftProperty, null);
            //Console.WriteLine(target.Name);

        }

        // 카드 낼때 스무스 하게 애니메이션 적용 
        public void moveAnimation2(Canvas target, Grid desti, int index)
        {
            absoluteDestiPoint = desti.TransformToAncestor(GameCanvas).Transform(new Point(0, 0)); //절대좌표 구하는식
            absoluteTargetPoint = target.TransformToAncestor(GameCanvas).Transform(new Point(0, 0));

            //Point relativePoint = TranslatePoint(new Point(0.0, 0.0), desti); // 상대좌표 구하는 식.. 
            //Point centerPoint = new Point(desti.ColumnDefinitions[0].ActualWidth, desti.ActualHeight); // 중심점 좌표 구하는식.. 아놔 좌표마스터
            tempTarget = target;

            targetX = Canvas.GetLeft(target);
            targetY = Canvas.GetTop(target);

            Storyboard story = new Storyboard();
            DoubleAnimation daX = new DoubleAnimation();
            DoubleAnimation daY = new DoubleAnimation();
            //랜드트랜스폼 생성
            TransformGroup tg = new TransformGroup();
            tg.Children.Add(new TranslateTransform());
            target.RenderTransform = tg;

            
            //daX.From = targetX;
            daX.To = absoluteDestiPoint.X - Canvas.GetLeft(target);
            //daTemp.To = absoluteDestiPoint.X  - absoluteTargetPoint.X;
            daX.Duration = new Duration(TimeSpan.FromSeconds(0.25));
            daX.FillBehavior = FillBehavior.Stop;
            Storyboard.SetTarget(daX, target);
            Storyboard.SetTargetProperty(daX, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
            story.Children.Add(daX);

            // target.BeginAnimation(Canvas.LeftProperty, daX);
            // target.BeginAnimation(Canvas.LeftProperty, null);
            // Canvas.SetLeft(target, targetX);

            daY = new DoubleAnimation();

            //daY.From = targetY;
            daY.To = absoluteDestiPoint.Y - Canvas.GetTop(target);
            daY.Duration = new Duration(TimeSpan.FromSeconds(0.25));
            daY.FillBehavior = FillBehavior.Stop;
            Storyboard.SetTarget(daY, target);
            Storyboard.SetTargetProperty(daY, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
            story.Children.Add(daY);
            story.Duration = daY.Duration;

            story.Completed += new EventHandler(strCompleted3);
            story.Begin();

            //target.BeginAnimation(Canvas.TopProperty, daY);
            //target.BeginAnimation(Canvas.LeftProperty, daX);

            //target.BeginAnimation(Canvas.TopProperty, null);
            //target.BeginAnimation(Canvas.LeftProperty, null);

            //target.BeginAnimation(Canvas.TopProperty, daY);
            //target.BeginAnimation(Canvas.TopProperty, null);
            //Canvas.SetTop(target, targetY);
            //BeginAnimation(Canvas.TopProperty, null);
            //BeginAnimation(Canvas.LeftProperty, null);
            //Console.WriteLine(target.Name);

        }

        void strCompleted3(object sender, EventArgs e)
        {
            Canvas.SetLeft(tempTarget, absoluteDestiPoint.X);
            Canvas.SetTop(tempTarget, absoluteDestiPoint.Y);

            Console.WriteLine("이름" + tempTarget.Name);
            
        }

        public void FlipLayout(UIElement Src, UIElement Des)
        {

            if (Src == null || Des == null) return;

            Storyboard FlipStoryBoard = new Storyboard();
            Storyboard.SetTargetProperty(FlipStoryBoard, new
            PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));

            Src.RenderTransformOrigin = new Point(0.5, 0.5);
            Des.RenderTransformOrigin = new Point(0.5, 0.5);

            Src.RenderTransform = new ScaleTransform(1, 1);
            Des.RenderTransform = new ScaleTransform(1, 0);

            DoubleAnimation OutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200));
            DoubleAnimation InAnimation = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));
            InAnimation.BeginTime = TimeSpan.FromMilliseconds(200);

            Storyboard.SetTargetName(OutAnimation, (Src as FrameworkElement).Name);
            Storyboard.SetTargetName(InAnimation, (Des as FrameworkElement).Name);

            FlipStoryBoard.Children.Add(OutAnimation);
            FlipStoryBoard.Children.Add(InAnimation);

            FlipStoryBoard.Begin(this);

        }


        public void attackAnimation(Canvas target, string targetName, Canvas desti)
        {

            GameCanvas.Dispatcher.Invoke(new Action(delegate
            {
                targetY = Canvas.GetTop(target);

                Storyboard story = new Storyboard();
                DoubleAnimation daX = new DoubleAnimation();
                DoubleAnimation daY = new DoubleAnimation();
                //랜드트랜스폼 생성
                TransformGroup tg = new TransformGroup();
                tg.Children.Add(new TranslateTransform());

                target.RenderTransform = tg;

                daY = new DoubleAnimation();

                //daY.From = targetY;
                if (targetName.Equals("user"))
                    daY.To = 50;
                else if (targetName.Equals("enemy"))
                    daY.To = -50;

                daY.Duration = new Duration(TimeSpan.FromSeconds(0.25));
                daY.FillBehavior = FillBehavior.Stop;
                Storyboard.SetTarget(daY, target);
                Storyboard.SetTargetProperty(daY, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
                story.Children.Add(daY);
                story.Duration = daY.Duration;

                story.Completed += new EventHandler(strCompleted2);

                story.Begin();
                
                //target.BeginAnimation(Canvas.TopProperty, daY);

                //Canvas.SetTop(target, targetY);
                //BeginAnimation(Canvas.TopProperty, null);
                //BeginAnimation(Canvas.LeftProperty, null);

                //Canvas.SetLeft(playerSubCanvas, absoluteDestiPoint.X);
                //Canvas.SetTop(playerSubCanvas, absoluteDestiPoint.Y);
                //Console.WriteLine(target.Name);
            }));
        }

        public void damagedAnimation(Canvas target)
        {
            GameCanvas.Dispatcher.Invoke(new Action(delegate
            {
                Console.WriteLine("데미지 에니메이션!!");
                Storyboard story = new Storyboard();
                DoubleAnimation daX = new DoubleAnimation();
                DoubleAnimation daX2 = new DoubleAnimation();
                //랜드트랜스폼 생성
                TransformGroup tg = new TransformGroup();
                tg.Children.Add(new TranslateTransform());

                target.RenderTransform = tg;

                //daY.From = targetY;

                daX.To = -10;

                daX.Duration = new Duration(TimeSpan.FromSeconds(0.05));
                daX.RepeatBehavior = RepeatBehavior.Forever;
                daX.AutoReverse = true;
                Storyboard.SetTarget(daX, target);
                Storyboard.SetTargetProperty(daX, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
                story.Children.Add(daX);


                daX2.To = 10;

                daX2.Duration = new Duration(TimeSpan.FromSeconds(0.05));
                daX2.RepeatBehavior = RepeatBehavior.Forever;
                daX2.AutoReverse = true;
                Storyboard.SetTarget(daX2, target);
                Storyboard.SetTargetProperty(daX2, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
                story.Children.Add(daX2);


                story.Duration = new Duration(TimeSpan.FromSeconds(0.5));

                story.Completed += new EventHandler(strCompleted4);

                story.Begin();

                //target.BeginAnimation(Canvas.TopProperty, daY);

                //Canvas.SetTop(target, targetY);
                //BeginAnimation(Canvas.TopProperty, null);
                //BeginAnimation(Canvas.LeftProperty, null);

                //Canvas.SetLeft(playerSubCanvas, absoluteDestiPoint.X);
                //Canvas.SetTop(playerSubCanvas, absoluteDestiPoint.Y);
                //Console.WriteLine(target.Name);
            }));
        }

        void strCompleted4(object sender, EventArgs e)
        {
            //damagedSound.Play();
        }

        void strCompleted2(object sender, EventArgs e)
        {
            
            Canvas.SetLeft(tempTarget, absoluteDestiPoint.X);
            Canvas.SetTop(tempTarget, absoluteDestiPoint.Y);
            //Console.WriteLine(sender.ToString());
            //Canvas.SetLeft(selectCanvas, 1000);
            Console.WriteLine("이름" + tempTarget.Name);
            enemyAttackPanel.Visibility = Visibility.Hidden;
            enemyDiePanel.Visibility = Visibility.Hidden;
        }

        // 애니메이션이 완료후 수행 작업
        public void strCompleted(object sender, EventArgs e)
        {
            Canvas.SetLeft(tempTarget, absoluteDestiPoint.X);
            Canvas.SetTop(tempTarget, absoluteDestiPoint.Y);
            //Console.WriteLine(sender.ToString());
            //Canvas.SetLeft(selectCanvas, 1000);
            Console.WriteLine("이름" + tempTarget.Name);

            /*
            if (enemyActionPoint == 0) ///////////////////////////////////////////////////////////////////// 여기... 나중에 확인 해보자, 
            {
                timer5 = new DispatcherTimer();
                timer5.Interval = TimeSpan.FromSeconds(1);
                timer5.Tick += new EventHandler(timer5_Tick);
                timer5.Start();
            }*/
        }

        private void userCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectCanvas = (Canvas)sender; // 클릭한놈으로 부터 canvas 연결
            playerBackToFront(selectCanvas);
           
        }

        void playerBackToFront(Canvas selectCanvas)
        {
            string col = ((TextBlock)selectCanvas.Children[3]).Text; // 클릭한놈 인덱스
            string row = ((TextBlock)selectCanvas.Children[4]).Text; // 클릭한놈 이름
            int index = Convert.ToInt32(row);  // int로 변환
            int frontIndex = 0; // 나갈 곳 담는 front 인덱스 변수
            string action = ((TextBlock)selectCanvas.Children[8]).Text; // 카드가 액션을 취했는지 판별 여부

            // 초기에는 스킬버튼 다 숨기고
            NetSkillButton.Visibility = Visibility.Hidden;
            NetAttkUpButton.Visibility = Visibility.Hidden;
            NetHpUpButton.Visibility = Visibility.Hidden;
            NetTnUpButton.Visibility = Visibility.Hidden;

            // front놈을 클릭 한거라면 스킬 버튼 보여주고
            if (Convert.ToInt32(col) == 1)
            {
                NetSkillButton.Visibility = Visibility.Visible;
            }

            
            if (col.Equals("0") && playerActionPoint != 0 && myTurn == true && action.Equals("0"))
            {
                // 낼껀지 안낼건지 확인 메세지 박스 띄우기
                MessageBoxResult result = MessageBox.Show("이거 낼꺼임?", "선택", MessageBoxButton.OKCancel);
                // 카드덱에 카드 있고, 확인이면
                if (result == MessageBoxResult.OK && checkCount(playerFrontCanvasList) != 4)
                {
                    MessageBox.Show("가랏!!");
                    // 클릭한 놈의 그리드 위치 column값 반환

                    // 앞에 낼 카드의 위치를 탐색...
                    for (int i = 0; i < 4; i++)
                    {
                        if (playerFrontCanvasList[i] == null)
                        {
                            frontIndex = i;
                            break;
                        }
                    }

                    // 깔린 카드판이고 클릭한 위치에 카드가 있으면
                    if (col.Equals("0") && cardLock == false)
                    {
                        // 내는 카드판에 카드 그림
                        string targetGridName = "NetFront" + frontIndex;
                        moveAnimation(selectCanvas, (Grid)GameCanvas.FindName(targetGridName), frontIndex);

                        // 카드를 냈으면 카드의 col값을 1로 변경
                        ((TextBlock)selectCanvas.Children[3]).Text = "1";
                        ((TextBlock)selectCanvas.Children[4]).Text = frontIndex.ToString();

                        //카드 판 정보 갱신..
                        playerBackCanvasList[index] = null;
                        playerFrontCanvasList[frontIndex] = selectCanvas;
                        playerActionPoint--;


                        // 카드를 낼 때마다 위치 정보 상대편에게 업데이트 해주기 위해 서버로 전송

                        if (userID.Equals(guestID)) // 카드를 꺼내는 내가 손님이라면
                            WaittingRoom.websocket.Send("backToFront?" + roomMasterID + "?" + index.ToString() + "?" + frontIndex.ToString()); // 서버를 통해 방장에 보냄
                        else if (userID.Equals(roomMasterID)) // 내가 방장이라면
                            WaittingRoom.websocket.Send("backToFront?" + guestID + "?" + index.ToString() + "?" + frontIndex.ToString()); // 서버를 통해 손님에게 보냄

                    }
                    // 더이상 카드 못 내게 Lock
                    //cardLock = true;
                    //행동 수치 감소

                }
                // 덱에 남은 카드가 없다면..
                else if (playerCardDeckQueue == null)
                {
                    MessageBox.Show("멍청아 카드 없다. 손가락 빨아라");
                }
            }
            else if (checkCount(playerFrontCanvasList) == 4 && col.Equals("0"))
            {
                MessageBox.Show("더이상 카드를 낼 수 없다!!");
            }
            // 이미 카드 낸거 예외 처리
            else if (col.Equals("0") && playerActionPoint == 0 && myTurn == true)
            {
                MessageBox.Show("욕심도 많으쎠~~");
            }
            // 사용자 턴이 살아있고, 카드를 내민상태이고, 내민 카드를 클릭한거라면 공격!!!
            else if (col.Equals("1") && playerActionPoint != 0 && action.Equals("1") && myTurn == true)
            {
                MessageBox.Show("이 카드는 이미 썼잖아");
            }
            else if (col.Equals("1") && playerActionPoint != 0 && action.Equals("0") && myTurn == true)
            {
                MessageBox.Show("타겟을 고르세요!");

                // 사용자가 적을 공격                
                while (!targetCardClick)
                {
                    // 이거 한 줄 구현하는데 시발 4시간걸림 --;;;
                    // 이게 뭐냐면 while문에서 이벤트 함수 호출 가능하게 해주는거..
                    // 즉 while문 돌때는 딴짓 못하는데... 이놈으로 인해서 마우스 클릭 이벤트를 줄 수 있다~ 이말이셈 ㅋ
                    // 그럼 왜 무한 while문 써서 이지랄 하는건지? -> 공격할 타겟 카드 선택할때까지 딴짓 못하게 할라고!!
                    App.DoEvents();
                }

                string targetIndex = null;
                string selectIndex = null;

                if (targetCanvas.Name.Equals(enemyHeroCanvas.Name))
                {
                    targetIndex = enemyHeroCanvas.Name;
                }
                else
                    targetIndex = ((TextBlock)targetCanvas.Children[4]).Text;

                selectIndex = ((TextBlock)selectCanvas.Children[4]).Text;


                Console.WriteLine("공격전 보내는 인덱스 확인 => targetIndex : " + targetIndex + "selectIndex : " + selectIndex);

                if (userID.Equals(guestID))
                    WaittingRoom.websocket.Send("EnemyAttack?" + roomMasterID + "?" + targetIndex + "?" + selectIndex); // 적을 공격하기 전에 서버를 통해 상대에게 공격자와 타겟 인덱스를 넘겨준다.
                else if (userID.Equals(roomMasterID))
                    WaittingRoom.websocket.Send("EnemyAttack?" + guestID + "?" + targetIndex + "?" + selectIndex); // 적을 공격하기 전에 서버를 통해 상대에게 공격자와 타겟 인덱스를 넘겨준다.

                enemyAttack(targetCanvas, selectCanvas);

                ((TextBlock)selectCanvas.Children[8]).Text = "1"; // 선택 카드의 행동력 Lock
            }
            else if (playerActionPoint == 0 || myTurn == false)
            {
                MessageBox.Show("행동포인트 0!! 니가 할 수 있는건 없당");
            }
        }

        void enemyCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string col = null;

            Canvas tempCanvas = new Canvas();
            tempCanvas = (Canvas)sender; // 클릭한놈으로 부터 canvas 연결
            col = ((TextBlock)tempCanvas.Children[3]).Text; // 클릭한놈 행
            //row = ((TextBlock)tempCanvas.Children[4]).Text; // 클릭한놈 열

            if (tempCanvas.Name.Equals(enemyHeroCanvas.Name) || col.Equals("1"))
            {
                targetCanvas = tempCanvas;
                targetCardClick = true;
            }
        }


        // 적 카드 한장씩 Front로 꺼내고 애니적용
        public void enemyCardStickOut(int backRow, int frontRow)
        {
            string targetGridName = "NetE_Front" + frontRow;
            Canvas targetCanvas = enemyBackCanvasList[backRow]; // 타겟 캔버스 저장

            moveAnimation(enemyBackCanvasList[backRow], (Grid)GameCanvas.FindName(targetGridName), frontRow); // 무브 애니메이션

            targetCanvas.Background = new ImageBrush(new BitmapImage(new Uri(targetCanvas.Name + ".png", UriKind.Relative))); // 이미지 꺼내주고

            targetCanvas.Children[0].Visibility = Visibility.Visible;
            targetCanvas.Children[1].Visibility = Visibility.Visible;
            targetCanvas.Children[5].Visibility = Visibility.Visible;
            targetCanvas.Children[6].Visibility = Visibility.Visible;
            targetCanvas.Children[7].Visibility = Visibility.Visible;

            ((TextBlock)enemyBackCanvasList[backRow].Children[3]).Text = "1"; // 이거는 네트워크에서는 필요 없어도 되겠지?? -> 개소리 ㅠㅠ 있어야 되네 큰일날뻔
            ((TextBlock)enemyBackCanvasList[backRow].Children[4]).Text = frontRow.ToString();

            enemyFrontCanvasList[frontRow] = enemyBackCanvasList[backRow];
            enemyBackCanvasList[backRow] = null;
            enemyActionPoint--;
        }

        void enemyAttack(Canvas targetCanvas1, Canvas attackerCanvas)
        {
            attackSound.Play();
            attackAnimation(attackerCanvas, "enemy", targetCanvas1);
            damagedAnimation(targetCanvas1);
            loadGif(targetCanvas1, slashGif);
            
            int targetCanvasHp = Convert.ToInt32(((TextBlock)targetCanvas1.Children[0]).Text);
            int attackerCanvasDam = Convert.ToInt32(((TextBlock)attackerCanvas.Children[1]).Text);
            
            if (targetCanvas.Name.Equals(enemyCard.CardName)) // 적 영웅 공격하는 거면
            {
                targetCanvasHp = targetCanvasHp - attackerCanvasDam; // 공격력 만큼 피 빼주고
                if (targetCanvasHp <= 0)
                {
                    loadGif2(targetCanvas1, dieGif);
                    //dieSound.Play(); // 죽는 효과음
                }
                else
                {
                   
                   // damagedSound.Play();// 맞는 효과음
                    ((TextBlock)targetCanvas1.Children[0]).Text = targetCanvasHp.ToString();
                }
                MessageBox.Show("공격성공!");
            }
            else
            {
                int index = Convert.ToInt32(((TextBlock)targetCanvas1.Children[4]).Text);
                targetCanvasHp = targetCanvasHp - attackerCanvasDam;

                if (targetCanvasHp <= 0)
                {
                    loadGif2(targetCanvas1, dieGif);
                    //dieSound.Play(); // 죽는 효과음
                    enemyFrontCanvasList[index] = null;
                    GameCanvas.Children.Remove(targetCanvas1);
                }
                else
                {
                    //damagedSound.Play(); // 맞는 효과음
                    ((TextBlock)targetCanvas1.Children[0]).Text = targetCanvasHp.ToString();
                }
                MessageBox.Show("공격성공!");
            }

            // 공격 할 때마다 행동포인트 감소
            playerActionPoint--;
            targetCardClick = false;
            cardLock = false;

           checkVictory();

        }

        void userAttack(string targetIndex, string selectIndex)
        {
            Canvas attackerCanvas = enemyFrontCanvasList[Convert.ToInt32(selectIndex)]; // 적 공격 카드를 전송받은 인덱스로 찾아서 담고
            int playerHp = Convert.ToInt32(((TextBlock)playerCanvas.Children[0]).Text); // 유저 피 가져오고
            int attackerCanvasDam = Convert.ToInt32(((TextBlock)attackerCanvas.Children[1]).Text); // 공격자의 공격력 가져오고

            if(targetIndex.Equals(userID)) // 상대방이 선택한 타겟이 나를 직접 공격한다면
            {
                attackAnimation(attackerCanvas, "user", playerCanvas); // 어택 애니메이션 적용
                loadGif(playerCanvas, slashGif);
                damagedAnimation(playerCanvas);
                playerHp = playerHp - attackerCanvasDam;

                if (playerHp <= 0)
                {
                    loadGif2(playerCanvas, dieGif);
                    //dieSound.Play(); // 죽는 효과음 재생
                    checkVictory();
                }
                else
                {
                    //damagedSound.Play(); // 맞는 효과음 재생
                    ((TextBlock)playerCanvas.Children[0]).Text = playerHp.ToString();
                }

            }
            else // 상대방이 선택한 타겟이 쫄따구 카드면
            {
                Canvas targetCanvas = playerFrontCanvasList[Convert.ToInt32(targetIndex)]; // 나의 카드중 타겟이 된 카드를 전송받은 인덱스로 찾아서 담고
                int targetCanvasHP = Convert.ToInt32(((TextBlock)targetCanvas.Children[0]).Text); // 타겟의 체력 가져오고
                int index = Convert.ToInt32(((TextBlock)targetCanvas.Children[4]).Text); // 타겟의 현재 인덱스 정보 가져오고
                attackAnimation(attackerCanvas, "user", targetCanvas); // 어택 애니메이션 적용 

                loadGif(targetCanvas, slashGif);
                damagedAnimation(targetCanvas);

                targetCanvasHP = targetCanvasHP - attackerCanvasDam;  // 공격력만큼 체력 빼고

                if (targetCanvasHP <= 0)
                {
                    //dieSound.Play();
                    loadGif2(targetCanvas, dieGif);
                    playerFrontCanvasList[index] = null; //★★ 가장 중요한 부분들.. 위치 갱신
                    GameCanvas.Children.Remove(targetCanvas); // 캔버스 제거
                }
                else
                {
                    //damagedSound.Play(); // 맞는 효과음 재생
                    ((TextBlock)targetCanvas.Children[0]).Text = targetCanvasHP.ToString(); // 캔버스에 체력 업뎃
                }

                checkVictory();
            }

        }

        private void checkPlayerBack()
        {
            Console.WriteLine("플레이어 깔기");
            //int i = time5 - 1;
            int i = checkIndex(playerBackCanvasList);

            if (playerBackCanvasList[i] == null && playerCardDeckQueue != null)
            {
                Card card = new Card();
                Canvas playerSubCanvas = new Canvas();
                card = playerCardDeckQueue.Dequeue();
                playerSubCanvas.Name = card.CardName;
                playerSubCanvas.Background = new ImageBrush(new BitmapImage(new Uri(card.CardName + ".png", UriKind.Relative)));
                TextBlock hp = new TextBlock();
                TextBlock dam = new TextBlock();
                TextBlock skill = new TextBlock();
                TextBlock col = new TextBlock();
                TextBlock row = new TextBlock();
                TextBlock skillLine1 = new TextBlock();
                TextBlock skillLine2 = new TextBlock();
                TextBlock skillLine3 = new TextBlock();
                TextBlock action = new TextBlock();
                TextBlock number = new TextBlock();

                hp.Name = "hp";
                dam.Name = "dam";
                skill.Name = "skill";
                col.Name = "index";
                row.Name = "gridName";

                hp.Text = card.CardHP.ToString();
                dam.Text = card.CardDam.ToString();
                col.Text = "0";
                row.Text = i.ToString();
                skillLine1.Text = card.skill1 + ": 공격력UP" + card.attackUpSkill;
                skillLine2.Text = card.skill2 + ": 체력UP" + card.healthUpSkill;
                skillLine3.Text = card.skill3 + ": 턴UP" + card.turnUpSkill;
                action.Text = "0";
                number.Text = card.cardNumber.ToString();

                hp.FontSize = 15;
                hp.Foreground = Brushes.Red;
                hp.FontWeight = FontWeights.Bold;

                dam.FontSize = 15;
                dam.Foreground = Brushes.Red;
                dam.FontWeight = FontWeights.Bold;

                skillLine1.FontSize = 11;
                skillLine1.Foreground = Brushes.Blue;
                skillLine1.FontWeight = FontWeights.Bold;

                skillLine2.FontSize = 11;
                skillLine2.Foreground = Brushes.Blue;
                skillLine2.FontWeight = FontWeights.Bold;

                skillLine3.FontSize = 11;
                skillLine3.Foreground = Brushes.Blue;
                skillLine3.FontWeight = FontWeights.Bold;

                playerBackCanvasList[i] = playerSubCanvas;

                Canvas.SetLeft(hp, 100);
                Canvas.SetBottom(hp, 47);

                Canvas.SetLeft(dam, 33);
                Canvas.SetBottom(dam, 47);

                Canvas.SetLeft(skillLine1, 10);
                Canvas.SetBottom(skillLine1, 35);

                Canvas.SetLeft(skillLine2, 10);
                Canvas.SetBottom(skillLine2, 22);

                Canvas.SetLeft(skillLine3, 10);
                Canvas.SetBottom(skillLine3, 11);

                playerSubCanvas.Children.Add(hp);
                playerSubCanvas.Children.Add(dam);
                playerSubCanvas.Children.Add(skill);
                playerSubCanvas.Children.Add(col);
                playerSubCanvas.Children.Add(row);
                playerSubCanvas.Children.Add(skillLine1);
                playerSubCanvas.Children.Add(skillLine2);
                playerSubCanvas.Children.Add(skillLine3);
                playerSubCanvas.Children.Add(action);
                playerSubCanvas.Children.Add(number);

                playerSubCanvas.Width = 150;
                playerSubCanvas.Height = 200;

                Canvas.SetTop(playerSubCanvas, 1000 - 200);
                Canvas.SetLeft(playerSubCanvas, 1400 - 150);


                GameCanvas.Children.Add(playerSubCanvas);
                string targetGridName = "NetMe" + i;

                moveAnimation(playerSubCanvas, (Grid)GameCanvas.FindName(targetGridName), i);
                playerSubCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(userCard_MouseLeftButtonDown);
            }
        }
        

        private void checkEnemyBack()
        {
            Console.WriteLine("시발좀 되라");
            //int i = time3 - 1;
            int i = checkIndex(enemyBackCanvasList);  // 최초로 비어 있는 인덱스 반환 하겠지?? 맞겠지 ㅠㅠ???

            // 적 back에 카드가 없거나, 적 카드 큐가 비어 있지 않으면 카드 깔기
            if (enemyBackCanvasList[i] == null && enemyCardDeckQueue.Count != 0)
            {
                Card card = new Card();
                Canvas enemySubCanvas = new Canvas();
                card = enemyCardDeckQueue.Dequeue();
                enemySubCanvas.Name = card.CardName;
                enemySubCanvas.Background = new ImageBrush(new BitmapImage(new Uri("CardBack.png", UriKind.Relative)));

                //Point relativePoint = TranslatePoint(new Point(0.0, 0.0), enemySubCanvas);

                TextBlock hp = new TextBlock();
                TextBlock dam = new TextBlock();
                TextBlock skill = new TextBlock();
                TextBlock col = new TextBlock();
                TextBlock row = new TextBlock();
                TextBlock skillLine1 = new TextBlock();
                TextBlock skillLine2 = new TextBlock();
                TextBlock skillLine3 = new TextBlock();
                TextBlock action = new TextBlock();
                TextBlock number = new TextBlock();

                hp.Name = "hp";
                dam.Name = "dam";
                skill.Name = "skill";
                col.Name = "index";
                row.Name = "gridName";

                hp.Text = card.CardHP.ToString();
                dam.Text = card.CardDam.ToString();
                col.Text = "0";
                row.Text = i.ToString();
                skillLine1.Text = card.skill1 + ": 공격력UP" + card.attackUpSkill;
                skillLine2.Text = card.skill2 + ": 체력UP" + card.healthUpSkill;
                skillLine3.Text = card.skill3 + ": 턴UP" + card.turnUpSkill;
                action.Text = "0";
                number.Text = card.cardNumber.ToString();

                hp.FontSize = 15;
                hp.Foreground = Brushes.Red;
                hp.FontWeight = FontWeights.Bold;

                dam.FontSize = 15;
                dam.Foreground = Brushes.Red;
                dam.FontWeight = FontWeights.Bold;

                skillLine1.FontSize = 11;
                skillLine1.Foreground = Brushes.Blue;
                skillLine1.FontWeight = FontWeights.Bold;

                skillLine2.FontSize = 11;
                skillLine2.Foreground = Brushes.Blue;
                skillLine2.FontWeight = FontWeights.Bold;

                skillLine3.FontSize = 11;
                skillLine3.Foreground = Brushes.Blue;
                skillLine3.FontWeight = FontWeights.Bold;

                enemyBackCanvasList[i] = enemySubCanvas;


                Canvas.SetLeft(hp, 100);
                Canvas.SetBottom(hp, 47);

                Canvas.SetLeft(dam, 33);
                Canvas.SetBottom(dam, 47);

                Canvas.SetLeft(skillLine1, 10);
                Canvas.SetBottom(skillLine1, 35);

                Canvas.SetLeft(skillLine2, 10);
                Canvas.SetBottom(skillLine2, 22);

                Canvas.SetLeft(skillLine3, 10);
                Canvas.SetBottom(skillLine3, 11);

                enemySubCanvas.Children.Add(hp);
                enemySubCanvas.Children.Add(dam);
                enemySubCanvas.Children.Add(skill);
                enemySubCanvas.Children.Add(col);
                enemySubCanvas.Children.Add(row);
                enemySubCanvas.Children.Add(skillLine1);
                enemySubCanvas.Children.Add(skillLine2);
                enemySubCanvas.Children.Add(skillLine3);
                enemySubCanvas.Children.Add(action);
                enemySubCanvas.Children.Add(number);

                hp.Visibility = Visibility.Hidden;
                dam.Visibility = Visibility.Hidden;
                skillLine1.Visibility = Visibility.Hidden;
                skillLine2.Visibility = Visibility.Hidden;
                skillLine3.Visibility = Visibility.Hidden;

                enemySubCanvas.Width = 150;
                enemySubCanvas.Height = 200;
                Canvas.SetTop(enemySubCanvas, 0);
                Canvas.SetLeft(enemySubCanvas, 0);

                GameCanvas.Children.Add(enemySubCanvas);

                string targetGridName = "NetEnemy" + i;
                moveAnimation(enemySubCanvas, (Grid)GameCanvas.FindName(targetGridName), i);

                enemySubCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(enemyCard_MouseLeftButtonDown);
            }

        }

        private void skillButtonClick(object sender, RoutedEventArgs e)
        {
            if (((TextBlock)selectCanvas.Children[8]).Text.Equals("1"))
            {
                MessageBox.Show("이미 행동 한 카드!!!");
            }
            else if (playerActionPoint == 0)
            {
                MessageBox.Show("행동 포인트 0!! ");
            }
            else
            {
                NetAttkUpButton.Visibility = Visibility.Visible;
                NetHpUpButton.Visibility = Visibility.Visible;
                NetTnUpButton.Visibility = Visibility.Visible;
            }
        }

        private void attkUpButtonClick(object sender, RoutedEventArgs e)
        {
            int attackPoint = 0;
            string[] getPoint = ((TextBlock)selectCanvas.Children[5]).Text.Split('P');
            string[] getSkillName = ((TextBlock)selectCanvas.Children[5]).Text.Split(':');

            skillEffect = 1;
            SkillTextBlock.Text = getSkillName[0];
            SkillTextBlock.Visibility = Visibility.Visible;

            skillStory.Completed += new EventHandler(skillStory_Completed);
            skillStory.Begin();
           
          
            int selectCanvasAttackPoint = Convert.ToInt32(getPoint[1]);
            int index = Convert.ToInt32(((TextBlock)selectCanvas.Children[4]).Text);

            foreach (var item in playerFrontCanvasList)
            {
                if (item != null)
                {
                    attackPoint = Convert.ToInt32(((TextBlock)item.Children[1]).Text);
                    attackPoint += selectCanvasAttackPoint;
                    ((TextBlock)item.Children[1]).Text = attackPoint.ToString();
                }
            }

            // 상대방에게 스킬 발동 알림
            if (userID.Equals(guestID))
                WaittingRoom.websocket.Send("Skill?AttkUP?" + roomMasterID + "?" + getPoint[1] + "?" + index.ToString()+"?"+getSkillName[0]);
            else if (userID.Equals(roomMasterID))
                WaittingRoom.websocket.Send("Skill?AttkUP?" + guestID + "?" + getPoint[1] + "?" + index.ToString() + "?" + getSkillName[0]);

            playerFrontCanvasList[index] = null;
            GameCanvas.Children.Remove(selectCanvas);

            playerActionPoint--;
            NetSkillButton.Visibility = Visibility.Hidden;
            NetAttkUpButton.Visibility = Visibility.Hidden;
            NetHpUpButton.Visibility = Visibility.Hidden;
            NetTnUpButton.Visibility = Visibility.Hidden;
        }

        void skillStory_Completed(object sender, EventArgs e) // 스킬 사용 후 
        {
            if(skillEffect == 1)
                loadAttackGif(attackSkillGif0, attackSkillGif1, attackSkillGif2, attackSkillGif3);
            else if(skillEffect == 2)
                loadAttackGif2(attackSkillGif0, attackSkillGif1, attackSkillGif2, attackSkillGif3);

            skillTimer = new DispatcherTimer();
            skillTimer.Interval = TimeSpan.FromSeconds(1);
            skillTimer.Tick += new EventHandler(skillTimer_Tick);
            skillTimer.Start();
            //loadGif(attackSkillGif1);
            //loadGif(attackSkillGif2);
            //loadGif(attackSkillGif3);
        }

        void skillTimer_Tick(object sender, EventArgs e)
        {
            time6++;

            if (time6 == 3)
            {
                skillTimer.Stop();
                time6 = 0;
                AttackUpSkill0.Visibility = Visibility.Hidden;
                AttackUpSkill1.Visibility = Visibility.Hidden;
                AttackUpSkill2.Visibility = Visibility.Hidden;
                AttackUpSkill3.Visibility = Visibility.Hidden;
            }
        }

        private void hpUpButtonClick(object sender, RoutedEventArgs e)
        {
            int healthPoint = 0;
            string[] getPoint = ((TextBlock)selectCanvas.Children[6]).Text.Split('P');
            string[] getSkillName = ((TextBlock)selectCanvas.Children[6]).Text.Split(':');

            SkillTextBlock.Text = getSkillName[0];
            SkillTextBlock.Visibility = Visibility.Visible;
            
            skillStory.Begin();
            
           

            int selectCanvasHealthPoint = Convert.ToInt32(getPoint[1]);
            int index = Convert.ToInt32(((TextBlock)selectCanvas.Children[4]).Text);

            foreach (var item in playerFrontCanvasList)
            {
                if (item != null)
                {
                    healthPoint = Convert.ToInt32(((TextBlock)item.Children[0]).Text);
                    healthPoint += selectCanvasHealthPoint;
                    ((TextBlock)item.Children[0]).Text = healthPoint.ToString();
                }
            }
            // 상대방에게 스킬 발동 알림
            if (userID.Equals(guestID))
                WaittingRoom.websocket.Send("Skill?HpUP?" + roomMasterID + "?" + getPoint[1] + "?" + index.ToString() + "?" + getSkillName[0]);
            else if (userID.Equals(roomMasterID))
                WaittingRoom.websocket.Send("Skill?HpUP?" + guestID + "?" + getPoint[1] + "?" + index.ToString() + "?" + getSkillName[0]);

            playerFrontCanvasList[index] = null;
            GameCanvas.Children.Remove(selectCanvas);
            playerActionPoint--;
            NetSkillButton.Visibility = Visibility.Hidden;
            NetAttkUpButton.Visibility = Visibility.Hidden;
            NetHpUpButton.Visibility = Visibility.Hidden;
            NetTnUpButton.Visibility = Visibility.Hidden;
        }

        private void tnUpButtonClick(object sender, RoutedEventArgs e)
        {
            string[] getPoint = ((TextBlock)selectCanvas.Children[7]).Text.Split('P');
            string[] getSkillName = ((TextBlock)selectCanvas.Children[7]).Text.Split(':');

            SkillTextBlock.Visibility = Visibility.Visible;
            SkillTextBlock.Text = getSkillName[0];
            skillStory.Begin();

            int selectCanvasTnPoint = Convert.ToInt32(getPoint[1]);
            int index = Convert.ToInt32(((TextBlock)selectCanvas.Children[4]).Text);

            playerActionPoint += selectCanvasTnPoint;

            // 상대방에게 스킬 발동 알림
            if (userID.Equals(guestID))
                WaittingRoom.websocket.Send("Skill?ActionUP?" + roomMasterID + "?" + getPoint[1] + "?" + index.ToString() + "?" + getSkillName[0]);
            else if (userID.Equals(roomMasterID))
                WaittingRoom.websocket.Send("Skill?ActionUP?" + guestID + "?" + getPoint[1] + "?" + index.ToString() + "?" + getSkillName[0]);

            playerFrontCanvasList[index] = null;
            GameCanvas.Children.Remove(selectCanvas);
            playerActionPoint--;
            NetSkillButton.Visibility = Visibility.Hidden;
            NetAttkUpButton.Visibility = Visibility.Hidden;
            NetHpUpButton.Visibility = Visibility.Hidden;
            NetTnUpButton.Visibility = Visibility.Hidden;
        }

        // 싱글이랑 비교해서 차후 코드 검수작업
        private void finishTurn(object sender, RoutedEventArgs e)
        {
            if (myTurn == true) // 내턴일 때만 동작하도록 예외 처리
            {
                if (playerActionPoint != 0) // 내가 아직 포인트 다 안썼을 경우
                {
                    MessageBoxResult result = MessageBox.Show("아직 행동포인트가 남았습니다.\n 턴을 종료하시겠습니까?", "행동{", MessageBoxButton.OKCancel);
                    // 카드덱에 카드 있고, 확인이면
                    if (result == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }

                MessageBox.Show("턴을 종료합니다");

                myTurn = false;
                // 사용자가 누구인지에 따라 내 턴이 끝났다는 걸 상대방에게 알려줌
                if (userID.Equals(guestID))
                    WaittingRoom.websocket.Send("FinishTurn?" + roomMasterID);
                else if (userID.Equals(roomMasterID))
                    WaittingRoom.websocket.Send("FinishTurn?" + guestID);
                enemyBackTime = checkBackBlankCount(enemyBackCanvasList); // 카드 빈 곳의 개수를 파악해서 타이머 얼마나 돌지 셋

                //첫턴이 아닌 턴부터 유저의 카드 꺼내놓는 타이머
                timer3 = new DispatcherTimer();
                timer3.Interval = TimeSpan.FromSeconds(1);
                timer3.Tick += new EventHandler(timer3_Tick);
                timer3.Start();

            }
        }
        void timer3_Tick(object sender, EventArgs e)
        {
            time3++;
            Console.WriteLine("틱" + time3);

            checkEnemyBack();

            if (time3 == enemyBackTime + 1 || enemyCardDeckQueue.Count == 0)
            {
                timer3.Stop();
                time3 = 0;
            }
        }

        private void checkVictory() // 게임의 승패를 확인
        {
            int playerHp = Convert.ToInt32(((TextBlock)playerCanvas.Children[0]).Text);
            int enemyHp = Convert.ToInt32(((TextBlock)enemyHeroCanvas.Children[0]).Text);
            int enemyFrontCanvasListCount = checkCount(enemyFrontCanvasList);
            int playerFrontCanvasListCount = checkCount(playerFrontCanvasList);
            int enemyBackCanvasListCount = checkCount(enemyBackCanvasList);
            int playerBackCanvasListCount = checkCount(playerBackCanvasList);

            if (enemyCardDeckQueue.Count == 0 && enemyFrontCanvasListCount == 0 && enemyBackCanvasListCount == 0)
            {
                MessageBox.Show("게임에서 승리 하였습니다.");
                Environment.Exit(1);
            }
            else if (enemyHp <= 0)
            {
                MessageBox.Show("게임에서 승리 하였습니다.");
                Environment.Exit(1);
            }
            else if (playerCardDeckQueue.Count == 0 && playerFrontCanvasListCount == 0 && playerBackCanvasListCount == 0)
            {
                MessageBox.Show("게임에서 패배 하였습니다.");
                Environment.Exit(1);
            }
            else if (playerHp <= 0)
            {
                MessageBox.Show("게임에서 패배 하였습니다.");
                Environment.Exit(1);
            }
        }

        // 비어 있지 않는 곳 개수 검사
        private int checkCount(Canvas[] array)
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                if (array[i] != null)
                    count++;
            }
            return count;
        }

        //비어 있는 곳 인덱스 반환
        private int checkIndex(Canvas[] array)
        {
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                if (array[i] == null)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        //비어 있는 곳 개수 검사
        private int checkBackBlankCount(Canvas[] array)
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                if (array[i] == null)
                    count++;
            }
            return count;
        }


        void loadGif(Canvas targetCanvas, GifImage gif)
        {
            
            gif.Visibility = Visibility.Visible;
            enemyAttackPanel.Visibility = Visibility.Visible;
            Console.WriteLine(targetCanvas.Margin);

            Console.WriteLine(targetCanvas.Name);
            Canvas.SetLeft(enemyAttackPanel, Canvas.GetLeft(targetCanvas));
            Canvas.SetTop(enemyAttackPanel, Canvas.GetTop(targetCanvas));
            
            gif.GifSourceChanged("pack://application:,,,/attack.gif");
        }


        void loadGif2(Canvas targetCanvas, GifImage gif)
        {

            gif.Visibility = Visibility.Visible;
            enemyDiePanel.Visibility = Visibility.Visible;
            Console.WriteLine(targetCanvas.Margin);

            Console.WriteLine(targetCanvas.Name);
            Canvas.SetLeft(enemyDiePanel, Canvas.GetLeft(targetCanvas));
            Canvas.SetTop(enemyDiePanel, Canvas.GetTop(targetCanvas));

            gif.GifSourceChanged("pack://application:,,,/explosion.gif");
        }

        void loadAttackGif(GifImage gif, GifImage gif2, GifImage gif3, GifImage gif4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (playerFrontCanvasList[i] != null)
                {
                    if (i == 0)
                    {
                        Canvas.SetLeft(AttackUpSkill0, Canvas.GetLeft(playerFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill0, Canvas.GetTop(playerFrontCanvasList[i]) - 20);
                        gif.Visibility = Visibility.Visible;
                        AttackUpSkill0.Visibility = Visibility.Visible;
                        gif.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                    else if (i == 1)
                    {
                        Canvas.SetLeft(AttackUpSkill1, Canvas.GetLeft(playerFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill1, Canvas.GetTop(playerFrontCanvasList[i]) - 20);

                        gif2.Visibility = Visibility.Visible;
                        AttackUpSkill1.Visibility = Visibility.Visible;
                        gif2.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                    else if (i == 2)
                    {
                        Canvas.SetLeft(AttackUpSkill2, Canvas.GetLeft(playerFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill2, Canvas.GetTop(playerFrontCanvasList[i]) - 20);
                        gif3.Visibility = Visibility.Visible;
                        AttackUpSkill2.Visibility = Visibility.Visible;
                        gif3.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                    else if (i == 3)
                    {
                        Canvas.SetLeft(AttackUpSkill3, Canvas.GetLeft(playerFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill3, Canvas.GetTop(playerFrontCanvasList[i]) - 20);
                        gif4.Visibility = Visibility.Visible;
                        AttackUpSkill3.Visibility = Visibility.Visible;
                        gif4.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                }

            }
        }


        void loadAttackGif2(GifImage gif, GifImage gif2, GifImage gif3, GifImage gif4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (enemyFrontCanvasList[i] != null)
                {
                    if (i == 0)
                    {
                        Canvas.SetLeft(AttackUpSkill0, Canvas.GetLeft(enemyFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill0, Canvas.GetTop(enemyFrontCanvasList[i]) - 20);
                        gif.Visibility = Visibility.Visible;
                        AttackUpSkill0.Visibility = Visibility.Visible;
                        gif.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                    else if (i == 1)
                    {
                        Canvas.SetLeft(AttackUpSkill1, Canvas.GetLeft(enemyFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill1, Canvas.GetTop(enemyFrontCanvasList[i]) - 20);
                        gif2.Visibility = Visibility.Visible;
                        AttackUpSkill1.Visibility = Visibility.Visible;
                        gif2.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                    else if (i == 2)
                    {
                        Canvas.SetLeft(AttackUpSkill2, Canvas.GetLeft(enemyFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill2, Canvas.GetTop(enemyFrontCanvasList[i]) - 20);
                        gif3.Visibility = Visibility.Visible;
                        AttackUpSkill2.Visibility = Visibility.Visible;
                        gif3.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                    else if (i == 3)
                    {
                        Canvas.SetLeft(AttackUpSkill3, Canvas.GetLeft(enemyFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill3, Canvas.GetTop(enemyFrontCanvasList[i]) - 20);
                        gif4.Visibility = Visibility.Visible;
                        AttackUpSkill3.Visibility = Visibility.Visible;
                        gif4.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                }

            }
        }

        private void goldChanceClick(object sender, RoutedEventArgs e)
        {
            if (myTurn==true && goldChanceNum < 2) // 내턴일때만 되도록, 찬스가 2번만 쓸수 있도록
            {
                MiniGame miniGame = new MiniGame();
                miniGame.Show();
                goldChanceNum++;
            }
        }
    }
}
