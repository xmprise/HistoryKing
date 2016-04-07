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
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace HistoryKing_client
{
    /// <summary>
    /// SetCardDeck.xaml에 대한 상호 작용 논리
    /// </summary>
    
    public partial class SetCardDeck : Page
    {
        private List<Card> playerCardDeckList = new List<Card>();
        private List<Card> gameCardDeckList = new List<Card>();
        
        public DispatcherTimer timer;
        public DispatcherTimer gtimer;
        public Canvas[] playerBackCanvasList = new Canvas[14];
        public Canvas[] gameCardCanvasList = new Canvas[5];

        private double targetX;
        private double targetY;

        private Point absoluteTargetPoint;
        private Point absoluteDestiPoint;

        private Canvas tempTarget;
        private Canvas selectCanvas;

        int time = 0;
        int listCount = 0;

        int gtime = 0;
        int glistCount = 0;

        int selectCardNumber = 0;
        int gameDeckCardList_Index = 0;
        int playerDeckCardList_Index = 0;
        private Boolean gtimeInit = true;
        
        private Boolean UpdateCard = false;

        private Boolean b1 = true;
        private Boolean b2 = true;
        private Boolean b3 = true;
        private Boolean b4 = true;
        private Boolean b5 = true;

        public SetCardDeck()
        {
            InitializeComponent();
        }

        void UserDeck_Loaded(object sender, RoutedEventArgs e)
        {
            init();
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            button3.IsEnabled = false;
            button4.IsEnabled = false;
            button5.IsEnabled = false;
            setPlayerCard();
        }

        public void init()
        {
            MyMessageBox myMessageBox = new MyMessageBox();
            CardDataContext card = new CardDataContext();
            Card cards;
            
            var innerJoinQuery = from Deck in card.MemberCardDeck
                                 join Hero in card.HeroCard on Deck.CardNumberID equals Hero.CardNumberID
                                 where Deck.MemberName == Player.getInstance().getID()
                                 select Hero;
            
            foreach (var item in innerJoinQuery)
            {
                cards = new Card();
                cards.CardName = item.HeroCardName;
                cards.CardHP = item.HeroCardHealthPoint;
                cards.CardDam = item.HeroCardAttackPoint;
                cards.cardNumber = (int)item.CardNumberID;
                cards.HeroCardInfo = item.HeroCardInformation;
                playerCardDeckList.Add(cards);

                var innerJoinQuery2 = from Deck in card.HeroCard
                                      join Magic in card.HeroCardSkill on Deck.HeroCardName equals Magic.HeroCardName
                                      where Deck.HeroCardName == cards.CardName
                                      select Magic;
                int i = 0;
                foreach (var item2 in innerJoinQuery2)
                {
                    if (i == 0)
                    {
                        cards.skill1 = item2.SkillName;
                        cards.attackUpSkill = item2.SkillPoint;
                    }
                    else if (i == 1)
                    {
                        cards.skill2 = item2.SkillName;
                        cards.healthUpSkill = item2.SkillPoint;
                    }
                    else if (i == 2)
                    {
                        cards.skill3 = item2.SkillName;
                        cards.turnUpSkill = item2.SkillPoint;
                    }
                    i++;
                }
            }
        }

        public void initMyDeck()
        {
            CardDataContext card = new CardDataContext();
            Card cards;
            var innerJoinQuery = from Deck in card.GameDeck
                                 join Hero in card.HeroCard on Deck.CardNumberID equals Hero.CardNumberID
                                 where Deck.MemberName == Player.getInstance().getID()
                                 select Hero;
            foreach (var item in innerJoinQuery)
            {
                cards = new Card();
                cards.CardName = item.HeroCardName;
                cards.CardHP = item.HeroCardHealthPoint;
                cards.CardDam = item.HeroCardAttackPoint;
                cards.cardNumber = (int)item.CardNumberID;
                cards.HeroCardInfo = item.HeroCardInformation;
                gameCardDeckList.Add(cards);

                var innerJoinQuery2 = from Deck in card.HeroCard
                                      join Magic in card.HeroCardSkill on Deck.HeroCardName equals Magic.HeroCardName
                                      where Deck.HeroCardName == cards.CardName
                                      select Magic;
                int i = 0;
                foreach (var item2 in innerJoinQuery2)
                {
                    if (i == 0)
                    {
                        cards.skill1 = item2.SkillName;
                        cards.attackUpSkill = item2.SkillPoint;
                    }
                    else if (i == 1)
                    {
                        cards.skill2 = item2.SkillName;
                        cards.healthUpSkill = item2.SkillPoint;
                    }
                    else if (i == 2)
                    {
                        cards.skill3 = item2.SkillName;
                        cards.turnUpSkill = item2.SkillPoint;
                    }
                    i++;
                }
            }
        }

        private void setPlayerCard()
        {
            Deck_Canvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(270); //
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
            }));
        }

        private void setGameDeckCard()
        {
            Deck_Canvas.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                gtimer = new DispatcherTimer();
                gtimer.Interval = TimeSpan.FromMilliseconds(270); //
                gtimer.Tick += new EventHandler(gtimer_Tick);
                gtimer.Start();
            }));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        }

        void gtimer_Tick(Object sender, EventArgs e)
        {
            gtime++;
            
            try
            {
                if (gtime <= 1 || gtime <= 5)
                {
                    int i = gtime - 1;
                    Card card2 = new Card();
                    Canvas myDeckCanvas = new Canvas();
                      
                    card2 = gameCardDeckList[glistCount];
                    
                    myDeckCanvas.Name = card2.CardName;

                    if (card2.CardName == null)
                    {
                        throw new ExceptionError();
                    }
                    myDeckCanvas.Background = new ImageBrush(new BitmapImage(new Uri(card2.CardName + ".png", UriKind.Relative)));
                    TextBlock hp = new TextBlock();
                    TextBlock dam = new TextBlock();
                    TextBlock skill = new TextBlock();
                    TextBlock col = new TextBlock();
                    TextBlock row = new TextBlock();
                    TextBlock skillLine1 = new TextBlock();
                    TextBlock skillLine2 = new TextBlock();
                    TextBlock skillLine3 = new TextBlock();

                    hp.Name = "hp";
                    dam.Name = "dam";
                    skill.Name = "skill";
                    col.Name = "index";
                    row.Name = "gridName";

                    hp.Text = card2.CardHP.ToString();
                    dam.Text = card2.CardDam.ToString();
                    col.Text = "0";
                    row.Text = time.ToString();
                    skillLine1.Text = card2.skill1 + ": 공격력 업";
                    skillLine2.Text = card2.skill2 + ": 체력 업";
                    skillLine3.Text = card2.skill3 + ": 턴 업";

                    hp.FontSize = 10;
                    hp.Foreground = Brushes.Red;
                    hp.FontWeight = FontWeights.Bold;

                    dam.FontSize = 10;
                    dam.Foreground = Brushes.Red;
                    dam.FontWeight = FontWeights.Bold;

                    skillLine1.FontSize = 8;
                    skillLine1.Foreground = Brushes.Blue;
                    skillLine1.FontWeight = FontWeights.Bold;

                    skillLine2.FontSize = 8;
                    skillLine2.Foreground = Brushes.Blue;
                    skillLine2.FontWeight = FontWeights.Bold;

                    skillLine3.FontSize = 8;
                    skillLine3.Foreground = Brushes.Blue;
                    skillLine3.FontWeight = FontWeights.Bold;

                    gameCardCanvasList[i] = myDeckCanvas;

                    Canvas.SetLeft(hp, 65);
                    Canvas.SetBottom(hp, 37);

                    Canvas.SetLeft(dam, 22);
                    Canvas.SetBottom(dam, 36);

                    Canvas.SetLeft(skillLine1, 10);
                    Canvas.SetBottom(skillLine1, 26);

                    Canvas.SetLeft(skillLine2, 10);
                    Canvas.SetBottom(skillLine2, 17);

                    Canvas.SetLeft(skillLine3, 10);
                    Canvas.SetBottom(skillLine3, 8);

                    myDeckCanvas.Children.Add(hp);
                    myDeckCanvas.Children.Add(dam);
                    myDeckCanvas.Children.Add(skill);
                    myDeckCanvas.Children.Add(col);
                    myDeckCanvas.Children.Add(row);
                    myDeckCanvas.Children.Add(skillLine1);
                    myDeckCanvas.Children.Add(skillLine2);
                    myDeckCanvas.Children.Add(skillLine3);

                    myDeckCanvas.Width = 100;
                    myDeckCanvas.Height = 150;

                    Canvas.SetTop(myDeckCanvas, 1000 - 150);
                    Canvas.SetLeft(myDeckCanvas, 1400 - 100);

                    Deck_Canvas.Children.Add(myDeckCanvas);

                    string targetGridName = "u" + i;
                    //mvAni.Invoke(playerSubCanvas, (Grid)Play_Stage.FindName(targetGridName), i);
                    moveAnimation(myDeckCanvas, (Grid)Deck_Canvas.FindName(targetGridName), i);

                    //moveAnimation(playerSubCanvas, Me, i);
                    myDeckCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(gameCard_MouseLeftButtonDown);
                    myDeckCanvas.MouseRightButtonDown += new MouseButtonEventHandler(gameCard_MouseRightButtonDown);

                    ++glistCount;
                    
                    if (glistCount >= gameCardDeckList.Count)
                    {
                        MyMessageBox message = new MyMessageBox();
                        message.SetText = "더이상 카드가 없어요.";
                        message.ShowDialog();
                        glistCount = 0;
                        b4 = false; //버튼4막기
                        b3 = true;
                        gtimer.Stop();
                        gtime = 0;
                        throw new ExceptionError();
                    }
                }
                else
                {
                    gtimer.Stop();
                    gtime = 0;
                    if (b1 == true)
                    {
                        button1.IsEnabled = true;
                    }
                    else
                    {
                        button1.IsEnabled = false;
                        b1 = true;
                    }
                    if (b2 == true)
                    {
                        button2.IsEnabled = true;
                    }
                    else { 
                        button2.IsEnabled = false;
                        b2 = true;
                    }
                    if (b3 == true)
                    {
                        button3.IsEnabled = true;
                    }
                    else
                    {
                        button3.IsEnabled = false;
                        b3 = true;
                    }
                    if (b4 == true)
                    {
                        button4.IsEnabled = true;
                    }
                    else { 
                        button4.IsEnabled = false;
                        b4 = true;
                    }
                    if (b5 == true)
                    {
                        button5.IsEnabled = true;
                    }
                    else
                    {
                        button5.IsEnabled = false;
                        b5 = true;
                    }
                }
            }
            catch (ExceptionError)
            {
                gtimer.Stop();
                gtime = 0;
                if (b1 == true)
                {
                    button1.IsEnabled = true;
                }
                else
                {
                    button1.IsEnabled = false;
                    b1 = true;
                }
                if (b2 == true)
                {
                    button2.IsEnabled = true;
                }
                else
                {
                    button2.IsEnabled = false;
                    b2 = true;
                }
                if (b3 == true)
                {
                    button3.IsEnabled = true;
                }
                else
                {
                    button3.IsEnabled = false;
                    b3 = true;
                }
                if (b4 == true)
                {
                    button4.IsEnabled = true;
                }
                else
                {
                    button4.IsEnabled = false;
                    b4 = true;
                }
                if (b5 == true)
                {
                    button5.IsEnabled = true;
                }
                else
                {
                    button5.IsEnabled = false;
                    b5 = true;
                }
            }
        }

        private void gameCard_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //컨텍스트 메뉴가 열릴 기준이되는 UI엘리먼트의 화면좌상단으로부터 위치(point)를 구합니다.
            selectCanvas = (Canvas)sender;

            Point targetPoint = selectCanvas.PointToScreen(new Point(0, 0));
            ContextMenu contextMenu = new ContextMenu();
            //컨텍스트 메뉴가 popup될 영역과 기준위치를 설정합니다
            contextMenu.PlacementRectangle = new Rect(targetPoint, new Size(selectCanvas.Width, selectCanvas.Height));
            contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.RelativePoint;
            //offset을 주시구요…
            contextMenu.HorizontalOffset = 60;
            contextMenu.VerticalOffset = 0;

            MenuItem item1 = new MenuItem();
            MenuItem item2 = new MenuItem();

            //I have about 10 items
            //...
            item1.Header = "인물 정보 보기";
            item1.Click += new RoutedEventHandler(item1_Click);
            contextMenu.Items.Add(item1);

            item2.Header = "카드덱에 삽입";
            item2.Click += new RoutedEventHandler(item2_Click);
            contextMenu.Items.Add(item2);

           //컨텍스트 메뉴를 여시면 됩니다.
            contextMenu.IsOpen = true;
        }

        void item2_Click(object sender, RoutedEventArgs e)
        {
            MyMessageBox myMessageBox = new MyMessageBox();
            
            for (int i = 0; i < gameCardDeckList.Count; i++)
            {
                if (gameCardDeckList[i].CardName.CompareTo(selectCanvas.Name) == 0)
                {
                    selectCardNumber = gameCardDeckList[i].cardNumber;
                    gameDeckCardList_Index = i;
                    break;
                }
            }

            myMessageBox.SetText = "카드 리스트에서 카드를 선택하세요. \n선택된 카드를 게임 카드덱에 넣습니다.";
            UpdateCard = true;
            myMessageBox.ShowDialog();
           
            
        }

        void item1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(selectCanvas.Name);
        }

        private void gameCard_Info(object sender, RoutedEventArgs e)
        {
            
            selectCanvas = (Canvas)sender;
            MessageBox.Show(selectCanvas.Name);
        }

        private void gameCard_deck(object sender, RoutedEventArgs e)
        {
            selectCanvas = (Canvas)((ContextMenu)((MenuItem)sender).Parent).Parent;
            //selectCanvas = (Canvas)(((MenuItem)sender).Parent);
            MessageBox.Show(selectCanvas.Name);
        }

        private void gameCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectCanvas = (Canvas)sender; // 클릭한놈으로 부터 canvas 연결

            //string col = ((TextBlock)selectCanvas.Children[3]).Text; // 클릭한놈 인덱스
            //string row = ((TextBlock)selectCanvas.Children[4]).Text; // 클릭한놈 이름
            //int index = Convert.ToInt32(row);
            string CardName = selectCanvas.Name.ToString();
            // 리스트에서 카드번호 찾기
            // DB에 카드번호 업데이트
        }
        void timer_Tick(object sender, EventArgs e)
        {
            time++;
           
            try
            {
                if (time <= 1 || time <= 14)
                {
                    int i = time - 1;
                    Card card = new Card();
                    Canvas playerSubCanvas = new Canvas();
                    
                    card = playerCardDeckList[listCount];

                    playerSubCanvas.Name = card.CardName;

                    if (card.CardName == null)
                    {
                        throw new ExceptionError();
                    }
                    
                    playerSubCanvas.Background = new ImageBrush(new BitmapImage(new Uri(card.CardName + ".png", UriKind.Relative)));
                    TextBlock hp = new TextBlock();
                    TextBlock dam = new TextBlock();
                    TextBlock skill = new TextBlock();
                    TextBlock col = new TextBlock();
                    TextBlock row = new TextBlock();
                    TextBlock skillLine1 = new TextBlock();
                    TextBlock skillLine2 = new TextBlock();
                    TextBlock skillLine3 = new TextBlock();

                    hp.Name = "hp";
                    dam.Name = "dam";
                    skill.Name = "skill";
                    col.Name = "index";
                    row.Name = "gridName";

                    hp.Text = card.CardHP.ToString();
                    dam.Text = card.CardDam.ToString();
                    col.Text = "0";
                    row.Text = i.ToString();
                    skillLine1.Text = card.skill1 + ": 공격력 업";
                    skillLine2.Text = card.skill2 + ": 체력 업";
                    skillLine3.Text = card.skill3 + ": 턴 업";

                    hp.FontSize = 10;
                    hp.Foreground = Brushes.Red;
                    hp.FontWeight = FontWeights.Bold;

                    dam.FontSize = 10;
                    dam.Foreground = Brushes.Red;
                    dam.FontWeight = FontWeights.Bold;

                    skillLine1.FontSize = 8;
                    skillLine1.Foreground = Brushes.Blue;
                    skillLine1.FontWeight = FontWeights.Bold;

                    skillLine2.FontSize = 8;
                    skillLine2.Foreground = Brushes.Blue;
                    skillLine2.FontWeight = FontWeights.Bold;

                    skillLine3.FontSize = 8;
                    skillLine3.Foreground = Brushes.Blue;
                    skillLine3.FontWeight = FontWeights.Bold;

                    playerBackCanvasList[i] = playerSubCanvas;

                    Canvas.SetLeft(hp, 65);
                    Canvas.SetBottom(hp, 37);

                    Canvas.SetLeft(dam, 22);
                    Canvas.SetBottom(dam, 36);

                    Canvas.SetLeft(skillLine1, 10);
                    Canvas.SetBottom(skillLine1, 26);

                    Canvas.SetLeft(skillLine2, 10);
                    Canvas.SetBottom(skillLine2, 17);

                    Canvas.SetLeft(skillLine3, 10);
                    Canvas.SetBottom(skillLine3, 8);

                    playerSubCanvas.Children.Add(hp);
                    playerSubCanvas.Children.Add(dam);
                    playerSubCanvas.Children.Add(skill);
                    playerSubCanvas.Children.Add(col);
                    playerSubCanvas.Children.Add(row);
                    playerSubCanvas.Children.Add(skillLine1);
                    playerSubCanvas.Children.Add(skillLine2);
                    playerSubCanvas.Children.Add(skillLine3);

                    playerSubCanvas.Width = 100;
                    playerSubCanvas.Height = 150;

                    Canvas.SetTop(playerSubCanvas, 1000 - 150);
                    Canvas.SetLeft(playerSubCanvas, 1400 - 100);


                    Deck_Canvas.Children.Add(playerSubCanvas);
                    string targetGridName = "Me" + i;
                    //mvAni.Invoke(playerSubCanvas, (Grid)Play_Stage.FindName(targetGridName), i);
                    moveAnimation(playerSubCanvas, (Grid)Deck_Canvas.FindName(targetGridName), i);

                    //moveAnimation(playerSubCanvas, Me, i);
                    playerSubCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(userCard_MouseLeftButtonDown);
                    playerSubCanvas.MouseRightButtonDown += new MouseButtonEventHandler(Card_MouseLeftButtonDown);
                    ++listCount;

                    if (listCount >= playerCardDeckList.Count)
                    {
                        MyMessageBox messagebox = new MyMessageBox();
                        messagebox.SetText = "더이상 카드가 없어요.";
                        messagebox.ShowDialog();
                        //listCount = 0;
                        b2 = false;
                        b1 = true;
                        timer.Stop();
                        time = 0;
                        throw new ExceptionError();
                    }
                }
                else
                {
                    timer.Stop();
                    time = 0;
                    if (gtimeInit == true)
                    {
                        initMyDeck();
                        setGameDeckCard();
                        gtimeInit = false;
                    }
                    if (b1 == true)
                    {
                        button1.IsEnabled = true;
                    }
                    else
                    {
                        button1.IsEnabled = false;
                        b1 = true;
                    }
                    if (b2 == true)
                    {
                        button2.IsEnabled = true;
                    }
                    else
                    {
                        button2.IsEnabled = false;
                        b2 = true;
                    }
                    if (b3 == true)
                    {
                        button3.IsEnabled = true;
                    }
                    else
                    {
                        button3.IsEnabled = false;
                        b3 = true;
                    }
                    if (b4 == true)
                    {
                        button4.IsEnabled = true;
                    }
                    else
                    {
                        button4.IsEnabled = false;
                        b4 = true;
                    }
                    if (b5 == true)
                    {
                        button5.IsEnabled = true;
                    }
                    else
                    {
                        button5.IsEnabled = false;
                        b5 = true;
                    }
                }
            }
            catch (ExceptionError) 
            {
                timer.Stop();
                time = 0;
                button2.IsEnabled = false;
                if (gtimeInit == true)
                {
                    initMyDeck();
                    setGameDeckCard();
                    gtimeInit = false;                 
                }
                if (b1 == true)
                {
                    button1.IsEnabled = true;
                }
                else
                {
                    button1.IsEnabled = false;
                    b1 = true;
                }
                if (b2 == true)
                {
                    button2.IsEnabled = true;
                }
                else
                {
                    button2.IsEnabled = false;
                    b2 = true;
                }
                if (b3 == true)
                {
                    button3.IsEnabled = true;
                }
                else
                {
                    button3.IsEnabled = false;
                    b3 = true;
                }
                if (b4 == true)
                {
                    button4.IsEnabled = true;
                }
                else
                {
                    button4.IsEnabled = false;
                    b4 = true;
                }
                if (b5 == true)
                {
                    button5.IsEnabled = true;
                }
                else
                {
                    button5.IsEnabled = false;
                    b5 = true;
                }
            }
        }

        private void Card_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectCanvas = (Canvas)sender;
            for (int i = 0; i < playerCardDeckList.Count; i++)
            {
                if (playerCardDeckList[i].CardName.CompareTo(selectCanvas.Name) == 0)
                {
                    Charactor_Infor char_infor = new Charactor_Infor();
                    char_infor.SetText = playerCardDeckList[i].HeroCardInfo;
                    char_infor.ShowDialog();
                }
            }
        }

        private void userCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectCanvas = (Canvas)sender; // 클릭한놈으로 부터 canvas 연결
            int selectDeckCard = 0;
           
            try
            {
                if (UpdateCard == true)
                {
                    Test myDialog = new Test();

                    myDialog.SetText = "선택한 카드가 " + selectCanvas.Name + " 이 맞나요?";
                    myDialog.ShowDialog();

                    if (myDialog.DialogResult == true)
                    {
                        // Check
                        for (int i = 0; i < gameCardDeckList.Count; i++)
                        {
                            if (gameCardDeckList[i].CardName.CompareTo(selectCanvas.Name) == 0)
                            {
                                throw new ExceptionError();
                            }
                        }
                        // Update DataBase
                        for (int i = 0; i < playerCardDeckList.Count; i++)
                        {
                            if (playerCardDeckList[i].CardName.CompareTo(selectCanvas.Name) == 0)
                            {
                                selectDeckCard = playerCardDeckList[i].cardNumber;
                                playerDeckCardList_Index = i;
                                break;

                            }
                        }
                        if (selectDeckCard == selectCardNumber)
                        {
                           throw new ExceptionError();
                        }
                        CardDataContext card = new CardDataContext();

                        var query =
                                    from gameDeck in card.GameDeck
                                    where gameDeck.MemberName == Player.getInstance().getID() && gameDeck.CardNumberID == selectCardNumber
                                    select gameDeck;

                        foreach (var item in query)
                        {
                            item.CardNumberID = selectDeckCard;
                        }

                        try
                        {
                            card.SubmitChanges();
                            gameCardDeckList[gameDeckCardList_Index] = playerCardDeckList[playerDeckCardList_Index];
                            MyMessageBox myMessageBox = new MyMessageBox();
                            myMessageBox.SetText = "카드를 변경 하였어요.";
                            myMessageBox.ShowDialog();
                            gameDeckRef();
                        }
                        catch (Exception error)
                        {
                            MyMessageBox myMessageBoxError = new MyMessageBox();
                            myMessageBoxError.SetText = error.ToString();
                            myMessageBoxError.ShowDialog();
                        }

                    }
                    UpdateCard = false;
                    selectCardNumber = 0;
                }
            }
            catch (ExceptionError) 
            {
                MyMessageBox myMessageBoxError = new MyMessageBox();
                myMessageBoxError.SetText = "선택된 카드가 중복 되었네요. 다른 카드를 선택해 주세요.";
                myMessageBoxError.ShowDialog();
            }
        }

        public void moveAnimation(Canvas target, Grid desti, int index)
        {
            // Play_Stage.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //     {

            absoluteDestiPoint = desti.TransformToAncestor(Deck_Canvas).Transform(new Point(0, 0)); //절대좌표 구하는식
            absoluteTargetPoint = target.TransformToAncestor(Deck_Canvas).Transform(new Point(0, 0));

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
        }

        // 애니메이션이 완료후 수행 작업
        public void strCompleted(object sender, EventArgs e)
        {
            Canvas.SetLeft(tempTarget, absoluteDestiPoint.X);
            Canvas.SetTop(tempTarget, absoluteDestiPoint.Y);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            button3.IsEnabled = false;
            button4.IsEnabled = false;
            button5.IsEnabled = false;

            playDeckRef();
        }

        private void playDeckRef()
        {
            for (int i = 0; i < 14; i++)
            {
                Deck_Canvas.Children.Remove(playerBackCanvasList[i]);
            }

            setPlayerCard();   
        }

        private void gameDeckRef()
        {
            for (int i = 0; i < 5; i++)
            {
                Deck_Canvas.Children.Remove(gameCardCanvasList[i]);
            }
            setGameDeckCard();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            button3.IsEnabled = false;
            button4.IsEnabled = false;
            button5.IsEnabled = false;

            gameDeckRef();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            button3.IsEnabled = false;
            button4.IsEnabled = false;
            button5.IsEnabled = false;

            listCount -= 28;
           
                if (listCount < 0)
                {
                    MyMessageBox Error = new MyMessageBox();
                    Error.SetText = "처음 화면 입니다.";
                    Error.ShowDialog();
                    listCount = 0;
                    b1 = false;
                    //버튼1 막기
                }
                playDeckRef();
            
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            button3.IsEnabled = false;
            button4.IsEnabled = false;
            button5.IsEnabled = false;

            glistCount -= 10;
            try
            {
                if (glistCount < 0)
                {
                    MyMessageBox Error = new MyMessageBox();
                    Error.SetText = "처음 화면 입니다.";
                    Error.ShowDialog();
                    glistCount = 0;
                    b3 = false;
                    //버튼3 막기
                    throw new ExceptionError();
                }
                gameDeckRef();
            }
            catch(ExceptionError)
            {
                button1.IsEnabled = true;
                button2.IsEnabled = true;
                button3.IsEnabled = false;
                button4.IsEnabled = true;
                button5.IsEnabled = true;
            }
            
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            SamGookStageSelect stageSelect1 = new SamGookStageSelect();
            MainMenu.getInstance().frame.Navigate(stageSelect1);   
        }

    }
}
