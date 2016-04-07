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
using System.IO;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using AudioPlayerLib;

namespace HistoryKing_client
{
    /// <summary>
    /// Interaction logic for SamGookStage1.xaml
    /// </summary>
    /// Bug 
    /// 1438 EnemyHP 다시 만듬
    /// 1442 TargetCanvasHP가 0보다 작으면 승리 체크
    /// 1039 EnemyHP 가 0보다 작으면 적 영웅의 피가 0보다 작음 승리조건 만족
    public partial class SamGookStage1 : Page
    {
        static public List<Card> heroCardDeckList = new List<Card>();
        static public List<Card> playerCardDeckList = new List<Card>();
        static public List<Card> enemyCardDeckList = new List<Card>();

        static public List<Canvas> canvasList = new List<Canvas>();

        static public Queue<Card> playerCardDeckQueue = new Queue<Card>(); // 랜덤하게 섞인 유저 카드덱 큐
        static public Queue<Card> enemyCardDeckQueue = new Queue<Card>(); // 랜덤하게 섞인 적 카드덱 큐

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
        static public int time3 = 0;
        static public int time5 = 0;
        static public int time6 =0;
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
        int skillEffect = 0;

        int goldChanceNum = 0;
        
        Storyboard skillStory;

        static Canvas PlayStage;
        static bool myTurn = false;
        static bool enemyTurn = false;
        public static bool Question = false;
        string HeroName = null;

        int EnemyHeroHP;
        // 생성자
        public SamGookStage1(string Hero)
        {
            InitializeComponent();
            playerCard = new Card(); // 플레이어카드 객체생성
            playerCard.CardHP = Player.getInstance().getHP(); // 플레이어카드 HP 셋
            playerCard.CardName = Player.getInstance().getID(); // 플레이어카드 이름 셋

            this.HeroName = Hero;

            myFace();
            playerSubCardLoad();
            enemyCardLoad();
            PlayStage = Play_Stage;

            slashGif.GifSource = "pack://application:,,,/attack.gif"; // slash 효과 gif 파일 경로 지정
            attackSkillGif0.GifSource = "pack://application:,,,/attackSkill.gif";
            attackSkillGif1.GifSource = "pack://application:,,,/attackSkill.gif";
            attackSkillGif2.GifSource = "pack://application:,,,/attackSkill.gif";
            attackSkillGif3.GifSource = "pack://application:,,,/attackSkill.gif";
            dieGif.GifSource = "pack://application:,,,/explosion.gif";
           // healthSkillGif0.GifSource = "pack://application:,,,/healthSkill.gif";
          //  healthSkillGif1.GifSource = "pack://application:,,,/healthSkill.gif";
           // healthSkillGif2.GifSource = "pack://application:,,,/healthSkill.gif";
           // healthSkillGif3.GifSource = "pack://application:,,,/healthSkill.gif";

          //  turnSkillGif.GifSource = "pack://application:,,,/turnSkill.gif";

            slashGif.Visibility = Visibility.Hidden;
            dieGif.Visibility = Visibility.Hidden;

            AttackUpSkill0.Visibility = Visibility.Hidden;
            AttackUpSkill1.Visibility = Visibility.Hidden;
            AttackUpSkill2.Visibility = Visibility.Hidden;
            AttackUpSkill3.Visibility = Visibility.Hidden;

            HealthUpSkill0.Visibility = Visibility.Hidden;
            HealthUpSkill1.Visibility = Visibility.Hidden;
            HealthUpSkill2.Visibility = Visibility.Hidden;
            HealthUpSkill3.Visibility = Visibility.Hidden;

            TurnUpSkill0.Visibility = Visibility.Hidden;

            enemyAttackPanel.Children.Add(slashGif);
            enemyDiePanel.Children.Add(dieGif);

            AttackUpSkill0.Children.Add(attackSkillGif0);
            AttackUpSkill1.Children.Add(attackSkillGif1);
            AttackUpSkill2.Children.Add(attackSkillGif2);
            AttackUpSkill3.Children.Add(attackSkillGif3);

            HealthUpSkill0.Children.Add(healthSkillGif0);
            HealthUpSkill1.Children.Add(healthSkillGif1);
            HealthUpSkill2.Children.Add(healthSkillGif2);
            HealthUpSkill3.Children.Add(healthSkillGif3);

            TurnUpSkill0.Children.Add(turnSkillGif);
            

            SkillTextBlock.Visibility = Visibility.Hidden;

            skillStory = this.Resources["OnTextInput1"] as Storyboard;

            playerPoint.Text = playerActionPoint.ToString();
            enemyPoint.Text = enemyActionPoint.ToString();
        }

        // 페이지 로딩시 호출
        void stage_Loaded(object sender, RoutedEventArgs e)
        {
            Play_Stage.Background = new ImageBrush(new BitmapImage(new Uri("배경1.png", UriKind.Relative)));
            // 카드덱 List 랜덤 섞기
            playerCardDeckQueue = ShuffleList<Card>(playerCardDeckList);
            enemyCardDeckQueue = ShuffleList<Card>(enemyCardDeckList);

            //카드 배치
            // setPlayerCardDele.Invoke();
            // setEnemyCardDele.Invoke();
            setPlayerCard();
            // setEnemyCard();

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

            Play_Stage.Children.Add(playerCanvas);
        }
        /*
        //대사 처리 함수
        private void dialogueLoad()
        {
            My_Dialogue.Background = new ImageBrush(new BitmapImage(new Uri("dialogue.png", UriKind.Relative)));
            Enemy_Dialogue.Background = new ImageBrush(new BitmapImage(new Uri("dialogue.png", UriKind.Relative)));

            FlowDocument flowDoc = new FlowDocument();
            flowDoc.Blocks.Add(new Paragraph(new Run("시발끄.. 존나 어렵네.. 한다이 하까?")));
            Enemy_Dialogue.Document = flowDoc;
            Enemy_Dialogue.FontSize = 20; // 글자 크기
            Enemy_Dialogue.Foreground = Brushes.Yellow; //글자색상 설정
            My_Dialogue.Visibility = Visibility.Hidden; // 해당 그리드 숨기기
            Enemy_Dialogue.Visibility = Visibility.Hidden;
        }*/


        //주인공 쫄카드 정보 로드
        private void playerSubCardLoad()
        {
            CardDataContext card = new CardDataContext();
            Card cards;
            var innerJoinQuery = from Deck in card.MemberCardDeck
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

        //게임 최초 시작시에 셔플된 큐 카드덱에서 5장 뽑아서 배치 (유저카드)
        private void setPlayerCard()
        {
            Play_Stage.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                // 디스패처 타이머로 애니메이션 1초마다 실행 되게 설정..
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
            }));
        }

        // 타이머가 1초 증가 할 때마다 실행되는 메소드
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

                Canvas.SetLeft(skillLine1, 15);
                Canvas.SetBottom(skillLine1, 35);

                Canvas.SetLeft(skillLine2, 15);
                Canvas.SetBottom(skillLine2, 22);

                Canvas.SetLeft(skillLine3, 15);
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

                playerSubCanvas.Width = 150;
                playerSubCanvas.Height = 200;

                Canvas.SetTop(playerSubCanvas, 1000 - 200);
                Canvas.SetLeft(playerSubCanvas, 1400 - 150);


                Play_Stage.Children.Add(playerSubCanvas);
                string targetGridName = "Me" + i;
                //mvAni.Invoke(playerSubCanvas, (Grid)Play_Stage.FindName(targetGridName), i);
                moveAnimation(playerSubCanvas, (Grid)Play_Stage.FindName(targetGridName), i);

                //moveAnimation(playerSubCanvas, Me, i);
                playerSubCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(userCard_MouseLeftButtonDown);
            }
            else if (time == 5 || time == 6 || time == 7 || time == 8)
            {
                Card card = new Card();
                Canvas enemySubCanvas = new Canvas();
                card = enemyCardDeckQueue.Dequeue();
                enemySubCanvas.Name = card.CardName;
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

                Canvas.SetLeft(skillLine1, 15);
                Canvas.SetBottom(skillLine1, 35);

                Canvas.SetLeft(skillLine2, 15);
                Canvas.SetBottom(skillLine2, 22);

                Canvas.SetLeft(skillLine3, 15);
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

                hp.Visibility = Visibility.Hidden;
                dam.Visibility = Visibility.Hidden;
                skillLine1.Visibility = Visibility.Hidden;
                skillLine2.Visibility = Visibility.Hidden;
                skillLine3.Visibility = Visibility.Hidden;


                enemySubCanvas.Width = 150;
                enemySubCanvas.Height = 200;
                Canvas.SetTop(enemySubCanvas, 0);
                Canvas.SetLeft(enemySubCanvas, 0);


                Play_Stage.Children.Add(enemySubCanvas);

                string targetGridName = "Enemy" + i;
                //mvAni.Invoke(enemySubCanvas, (Grid)Play_Stage.FindName(targetGridName), i);
                moveAnimation(enemySubCanvas, (Grid)Play_Stage.FindName(targetGridName), i);

                //moveAnimation(enemySubCanvas, EnemySub, i);
                enemySubCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(enemyCard_MouseLeftButtonDown);
            }
            if (time == 8)
            {
                timer.Stop();
                time = 0;
                MyMessageBox myMessage = new MyMessageBox();
                myMessage.SetText = "첫 턴!! GO!!!";
                myTurn = true;
            }
        }
        // 게임 최초 시작시에 셔플된 큐 카드덱에서 5장 뽑아서 배치 (적카드)
        private void setEnemyCard()
        {
            Play_Stage.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                for (int i = 0; i < 4; i++)
                {
                }
            }));
        }

        //적 카드 정보 로드
        private void enemyCardLoad()
        {
            CardDataContext card = new CardDataContext();
            Card cards;
            //적 영웅 로드
            var innerJoinQuery = from enemyHero in card.EnemyHero
                                 where enemyHero.EnemyHeroName == this.HeroName //이건 나중에 스테이지별 적 나오게끔 설정
                                 select enemyHero;
            foreach (var item in innerJoinQuery)
            {
                enemyCard = new Card();
                enemyHeroCanvas = new Canvas();

                enemyCard.CardName = item.EnemyHeroName;
                enemyCard.CardHP = item.EnemyHeroHealthPoint;
                enemyHeroCanvas.Name = item.EnemyHeroName;

                EnemyHeroHP = item.EnemyHeroHealthPoint;
                //fieldEnemyCardList.Add(enemyCard);

                enemyHeroCanvas.Background = new ImageBrush(new BitmapImage(new Uri(item.EnemyHeroName + ".png", UriKind.Relative)));
                enemyHeroCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(enemyCard_MouseLeftButtonDown);

                TextBlock hp = new TextBlock();
                TextBlock dam = new TextBlock();
                TextBlock skill = new TextBlock();
                TextBlock col = new TextBlock();

                hp.Name = "hp";
                dam.Name = "dam";
                skill.Name = "skill";

                hp.Text = item.EnemyHeroHealthPoint.ToString();
                dam.Text = item.EnemyHeroAttackPoint.ToString();
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

                enemyHeroCanvas.Children.Add(hp);
                enemyHeroCanvas.Children.Add(dam);
                enemyHeroCanvas.Children.Add(skill);
                enemyHeroCanvas.Children.Add(col);

                enemyHeroCanvas.Width = 250;
                enemyHeroCanvas.Height = 300;

                Canvas.SetTop(enemyHeroCanvas, 0);
                // Canvas.SetLeft(canvas, 0);
                //Canvas.SetBottom(canvas, 1000);
                Canvas.SetLeft(enemyHeroCanvas, 1400 - 250);

                Play_Stage.Children.Add(enemyHeroCanvas);

            }
            //쫄병 로드
            var innerJoinQuery2 = from enemySub in card.EnemyCard
                                  select enemySub;

            foreach (var item in innerJoinQuery2)
            {
                int i = 1;
                cards = new Card();
                cards.CardName = item.EnemyCardName;
                cards.CardHP = item.EnemyCardHealthPoint;
                cards.CardDam = item.EnemyCardAttackPoint;

                var innerJoinQuery3 = from Deck in card.EnemyCard
                                      join Magic in card.EnemyCardSkill on Deck.EnemyCardName equals Magic.EnemyCardName
                                      where Deck.EnemyCardName == cards.CardName
                                      select Magic;

                foreach (var item2 in innerJoinQuery3)
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

                enemyCardDeckList.Add(cards);
            }
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

        private void userCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectCanvas = (Canvas)sender; // 클릭한놈으로 부터 canvas 연결
            string col = ((TextBlock)selectCanvas.Children[3]).Text; // 클릭한놈 인덱스
            string row = ((TextBlock)selectCanvas.Children[4]).Text; // 클릭한놈 이름
            int index = Convert.ToInt32(row);
            int frontIndex = 0;
            string action = ((TextBlock)selectCanvas.Children[8]).Text;

            SkillButton.Visibility = Visibility.Hidden;
            AttkUpButton.Visibility = Visibility.Hidden;
            HpUpButton.Visibility = Visibility.Hidden;
            TnUpButton.Visibility = Visibility.Hidden;

            if (Convert.ToInt32(col) == 1)
            {
                SkillButton.Visibility = Visibility.Visible;
            }
            /*
            if (Convert.ToInt32(col) == 0) // Back인지
            {
                selectGridName = "Me" + row;
                Grid selectGrid = ((Grid)PlayStage.FindName(selectGridName));
                absoluteDestiPoint = selectGrid.TransformToAncestor(PlayStage).Transform(new Point(0, 0));
                
            }
            else if (Convert.ToInt32(col) == 1) // Front인지
            {
                selectGridName = "Front" + row;
                Grid selectGrid = ((Grid)PlayStage.FindName(selectGridName));
                absoluteDestiPoint = selectGrid.TransformToAncestor(PlayStage).Transform(new Point(0, 0));

            }*/

            // 카드리스트에 깔려있는 카드 내는 구문
            if (col.Equals("0") && playerActionPoint != 0 && myTurn == true && action.Equals("0"))
            {
                // 낼껀지 안낼건지 확인 메세지 박스 띄우기
                Test result = new Test();
                result.SetText = "이 카드를 낼 거에요?";
                result.ShowDialog();
                
                // 카드덱에 카드 있고, 확인이면
                if (result.DialogResult == true && checkCount(playerFrontCanvasList) != 4)
                {
                    MyMessageBox myMessage = new MyMessageBox();
                    myMessage.SetText = "가랏!!";
                    myMessage.ShowDialog();
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
                        string targetGridName = "Front" + frontIndex;
                        moveAnimation(selectCanvas, (Grid)PlayStage.FindName(targetGridName), frontIndex);

                        // 카드를 냈으면 카드의 col값을 1로 변경
                        ((TextBlock)selectCanvas.Children[3]).Text = "1";
                        ((TextBlock)selectCanvas.Children[4]).Text = frontIndex.ToString();

                        //카드 판 정보 갱신..
                        playerBackCanvasList[index] = null;
                        playerFrontCanvasList[frontIndex] = selectCanvas;
                        playerActionPoint--;
                    }
                    // 더이상 카드 못 내게 Lock
                    //cardLock = true;
                    //행동 수치 감소

                }
                else if (checkCount(playerFrontCanvasList) == 4)
                {
                    MyMessageBox myMessage = new MyMessageBox();
                    myMessage.SetText = "더이상 카드를 낼 수 없어요";
                    myMessage.ShowDialog();
                }
                // 덱에 남은 카드가 없다면..
                else if (playerCardDeckQueue == null)
                {
                    MyMessageBox myMessage = new MyMessageBox();
                    myMessage.SetText = "카드가 없어요.";
                    myMessage.ShowDialog();
                }
            }
            // 이미 카드 낸거 예외 처리
            else if (col.Equals("0") && playerActionPoint == 0 && myTurn == true)
            {
                MyMessageBox myMessage = new MyMessageBox();
                myMessage.SetText = "행동 포인트가 부족해요.";
                myMessage.ShowDialog();
            }
            else if (col.Equals("1") && playerActionPoint != 0 && action.Equals("1") && myTurn == true)
            {
                MessageBox.Show("이 카드는 이미 썼잖아");
            }

            // 사용자 턴이 살아있고, 카드를 내민상태이고, 내민 카드를 클릭한거라면 공격!!!
            else if (col.Equals("1") && playerActionPoint != 0 && action.Equals("0") && myTurn == true)
            {
                MyMessageBox myMessage = new MyMessageBox();
                myMessage.SetText = "공격할 대상을 고르세요";
                myMessage.ShowDialog();

                // 사용자가 적을 공격                
                while (!targetCardClick)
                {
                    // 이거 한 줄 구현하는데 시발 4시간걸림 --;;;
                    // 이게 뭐냐면 while문에서 이벤트 함수 호출 가능하게 해주는거..
                    // 즉 while문 돌때는 딴짓 못하는데... 이놈으로 인해서 마우스 클릭 이벤트를 줄 수 있다~ 이말이셈 ㅋ
                    // 그럼 왜 무한 while문 써서 이지랄 하는건지? -> 공격할 타겟 카드 선택할때까지 딴짓 못하게 할라고!!
                    App.DoEvents();
                }
                enemyAttack(targetCanvas, selectCanvas);

                ((TextBlock)selectCanvas.Children[8]).Text = "1"; // 선택 카드의 행동력 Lock
            }
            else if (playerActionPoint == 0 && myTurn == false)
            {
                MyMessageBox myMessage = new MyMessageBox();
                myMessage.SetText = "행동 포인트가 0 이에요.";
                myMessage.ShowDialog();
            }
        }

        private void enemyCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
        // 적카드 내기
        public void enemyCardStickOut()
        {
            int frontIndex = 0;

            // 내는 카드판에 카드 그림
            for (int i = 0; i < 4; i++)
            {
                if (enemyFrontCanvasList[i] == null)
                {
                    frontIndex = i;
                    break;
                }
            }
            string targetGridName = "E_Front" + frontIndex;
            Canvas targetCanvas = randomSelCanvas(enemyBackCanvasList);
            int backIndex = Convert.ToInt32(((TextBlock)targetCanvas.Children[4]).Text);

            moveAnimation(targetCanvas, (Grid)Play_Stage.FindName(targetGridName), frontIndex);
            targetCanvas.Background = new ImageBrush(new BitmapImage(new Uri(targetCanvas.Name + ".png", UriKind.Relative))); // 이미지 꺼내주고

            targetCanvas.Children[0].Visibility = Visibility.Visible;
            targetCanvas.Children[1].Visibility = Visibility.Visible;
            targetCanvas.Children[5].Visibility = Visibility.Visible;
            targetCanvas.Children[6].Visibility = Visibility.Visible;
            targetCanvas.Children[7].Visibility = Visibility.Visible;

            ((TextBlock)enemyBackCanvasList[frontIndex].Children[3]).Text = "1";
            ((TextBlock)enemyBackCanvasList[backIndex].Children[4]).Text = frontIndex.ToString();

            enemyFrontCanvasList[frontIndex] = enemyBackCanvasList[backIndex];
            enemyBackCanvasList[backIndex] = null;
            enemyActionPoint--;
        }

        // 카드 낼때 스무스 하게 애니메이션 적용 
        public void moveAnimation(Canvas target, Grid desti, int index)
        {

            absoluteDestiPoint = desti.TransformToAncestor(PlayStage).Transform(new Point(0, 0)); //절대좌표 구하는식
            absoluteTargetPoint = target.TransformToAncestor(PlayStage).Transform(new Point(0, 0));

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
            //    }));
        }

        public void attackAnimation(Canvas target, string targetName)
        {

            PlayStage.Dispatcher.Invoke(new Action(delegate
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
            Play_Stage.Dispatcher.Invoke(new Action(delegate
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
        }

        private void checkVictory() // 게임의 승패를 확인
        {
            int plyaerHp = Convert.ToInt32(((TextBlock)playerCanvas.Children[0]).Text);
            int enemyHp = Convert.ToInt32(((TextBlock)enemyHeroCanvas.Children[0]).Text);
            int enemyFrontCanvasListCount = checkCount(enemyFrontCanvasList);
            int playerFrontCanvasListCount = checkCount(playerFrontCanvasList);
            int enemyBackCanvasListCount = checkCount(enemyBackCanvasList);
            int playerBackCanvasListCount = checkCount(playerBackCanvasList);

            if (enemyCardDeckQueue.Count == 0 && enemyFrontCanvasListCount == 0 && enemyBackCanvasListCount == 0)
            {
                MyMessageBox myMessage = new MyMessageBox();

                myMessage.SetText = "게임에서 승리 하였어요. 축하해요~";
                myMessage.ShowDialog();
                SamGookStageSelect samgookStageSelect = new SamGookStageSelect();
                MainMenu.getInstance().frame.Navigate(samgookStageSelect);
                //Environment.Exit(1);
            }
            else if (EnemyHeroHP <= 0)
            {
                MyMessageBox myMessage = new MyMessageBox();

                myMessage.SetText = "게임에서 승리 하였어요. 축하해요~";
                myMessage.ShowDialog();
                SamGookStageSelect samgookStageSelect = new SamGookStageSelect();
                MainMenu.getInstance().frame.Navigate(samgookStageSelect);
                //Environment.Exit(1);
            }
            else if (playerCardDeckQueue.Count == 0 && playerFrontCanvasListCount == 0 && playerBackCanvasListCount == 0)
            {
                MyMessageBox myMessage = new MyMessageBox();

                myMessage.SetText = "게임에서 졌어요. 다시 도전해 보세요.";
                myMessage.ShowDialog();
                SamGookStageSelect samgookStageSelect = new SamGookStageSelect();
                MainMenu.getInstance().frame.Navigate(samgookStageSelect);
                //Environment.Exit(1);
            }
            else if (plyaerHp <= 0)
            {
                MyMessageBox myMessage = new MyMessageBox();

                myMessage.SetText = "게임에서 졌어요. 다시 도전해 보세요.";
                myMessage.ShowDialog();
                SamGookStageSelect samgookStageSelect = new SamGookStageSelect();
                MainMenu.getInstance().frame.Navigate(samgookStageSelect);
                //Environment.Exit(1);
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

        private int randomIndex()
        {
            Random rand = new Random(); // 랜덤선언
            //const int max_random = 4; // 5개의 랜덤수

            int num = 0;

            num = rand.Next(0, 4); // 0~5까지 중 랜덤수 하나 추출          

            if (randomIndexs[num] == false) // 숫자가 나오지 않았다면 나왔다는 표시 해주고 카드 꺼내기 실행
            {
                randomIndexs[num] = true; // 랜덤값 마킹
            }
            else if (randomIndexs[num] == true) // 숫자가 나온 숫자라면
            {
                while (!randomIndexs[num] == false)// 나오지 않은 숫자를 찾을 때까지 검색
                {
                    num = rand.Next(0, 4);
                }
                randomIndexs[num] = true; // 랜덤값 마킹
            }
            return num;
        }

        private Canvas randomSelCanvas(Canvas[] targetCanvasList)
        {
            while (true)
            {
                Random rand = new Random();
                int randNum = rand.Next(0, 4);
                if (targetCanvasList[randNum] != null)
                {
                    return targetCanvasList[randNum];
                }
                else if (targetCanvasList[randNum] == null)
                {
                    continue;
                }
            }
        }

        private void cpuAI()
        {

            string cols = ((TextBlock)selectCanvas.Children[3]).Text; // 클릭한놈 인덱스
            string rows = ((TextBlock)selectCanvas.Children[4]).Text; // 클릭한놈 이름
            int index = Convert.ToInt32(rows);

            if (firstTurn) // 첫 턴이면 타이머 줘서 순차적으로 카드 3장 내밀고
            {
                timer2 = new DispatcherTimer();
                timer2.Interval = TimeSpan.FromSeconds(1);
                timer2.Tick += new EventHandler(timer_Tick2);
                timer2.Start();
            }
            else if (firstTurn == false) // 첫 턴이 아니면 이제 AI 대로 진행
            {
                enemyBackTime = checkBackBlankCount(enemyBackCanvasList);
                timer3 = new DispatcherTimer();
                timer3.Interval = TimeSpan.FromSeconds(1);
                timer3.Tick += new EventHandler(timer3_Tick);
                timer3.Start();

                Console.WriteLine("시발동기냐 비동기냐? 골때리네");

            }

        }

        void aaa()
        {
            Random rand = new Random();
            int randNum =0 ;

            if(checkCount(enemyFrontCanvasList) ==0 && checkCount(enemyBackCanvasList)==0) //뒤0 앞0
            {
                checkVictory();
            }
            else if (checkCount(enemyBackCanvasList) <= 4 && checkCount(enemyFrontCanvasList)==0) // 뒤3?4?  앞 0
            {
                randNum = 0;
            }
            else if (checkCount(enemyBackCanvasList) == 4 && checkCount(enemyFrontCanvasList) == 0) // 뒤4 앞 0
            {
                randNum = 0;
            }
            else if (checkCount(enemyBackCanvasList) == 0 && checkCount(enemyFrontCanvasList)<4) // 뒤0 앞 3
            {
               randNum = rand.Next(1, 3);
            }
            else if (checkCount(enemyFrontCanvasList) == 0 && checkCount(enemyBackCanvasList) ==4) // 뒤4 앞 0
            {
                randNum = rand.Next(1, 3);
           }
            else if(checkCount(enemyFrontCanvasList) ==4 && checkCount(enemyBackCanvasList)<4) //뒤 3 앞 4
            {
               randNum = rand.Next(1,3); 
           }
            else if (checkCount(enemyFrontCanvasList) == 4 && checkCount(enemyBackCanvasList) == 4) //뒤4 앞 4
            {
                randNum = rand.Next(1, 3); 
            }
            else if (checkCount(enemyBackCanvasList) == 4 && checkCount(enemyFrontCanvasList) < 4) //뒤4 앞3
            {
                randNum = rand.Next(0, 3);
            }


           if (randNum == 0) // 랜덤 수 0이면 카드 한장 더 내기
                {
                    int indexDesti = checkIndex(enemyFrontCanvasList);
                    string targetGridName = "E_Front" + indexDesti; // 타겟 그리드 이름 설정
                    Canvas targetCanvas1 = randomSelCanvas(enemyBackCanvasList);
                    int col = Convert.ToInt32(((TextBlock)targetCanvas1.Children[3]).Text);
                    int row = Convert.ToInt32(((TextBlock)targetCanvas1.Children[4]).Text);

                    moveAnimation(targetCanvas1, (Grid)PlayStage.FindName(targetGridName), indexDesti); // 이동 애니메이션
                    targetCanvas1.Background = new ImageBrush(new BitmapImage(new Uri(targetCanvas1.Name + ".png", UriKind.Relative)));

                    targetCanvas1.Children[0].Visibility = Visibility.Visible;
                    targetCanvas1.Children[1].Visibility = Visibility.Visible;
                    targetCanvas1.Children[5].Visibility = Visibility.Visible;
                    targetCanvas1.Children[6].Visibility = Visibility.Visible;
                    targetCanvas1.Children[7].Visibility = Visibility.Visible;


                    ((TextBlock)targetCanvas1.Children[3]).Text = "1";
                    ((TextBlock)targetCanvas1.Children[4]).Text = indexDesti.ToString();

                    enemyBackCanvasList[row] = null;
                    enemyFrontCanvasList[indexDesti] = targetCanvas1; //  front에 back놈 삽입

                    checkVictory();
                    enemyActionPoint--; // 행동 포인트 감소
                }
                else if (randNum == 1) // 랜덤 수 1 이면 유저 공격
                {
                    userAttack(randomSelCanvas(playerFrontCanvasList), randomSelCanvas(enemyFrontCanvasList));
                    checkVictory();
                    enemyActionPoint--;
                }
                else if (randNum == 2) // 랜덤 수 2 면 스킬 발동
                {
                    Random skillRand = new Random();
                    int skillRandNum = skillRand.Next(1, 4);
                    enemySelectCanvas = randomSelCanvas(enemyFrontCanvasList);
                    skillEffect = 2;

                    if (skillRandNum == 1) // 스킬 1, 공격력 업
                    {
                        int attackPoint = 0;
                        string[] getPoint = ((TextBlock)selectCanvas.Children[5]).Text.Split('P');
                        string[] getSkillName = ((TextBlock)enemySelectCanvas.Children[5]).Text.Split(':');

                        SkillTextBlock.Text = getSkillName[0];
                        SkillTextBlock.Foreground = Brushes.Blue;
                        SkillTextBlock.Visibility = Visibility.Visible;
                        skillStory.Begin();


                        int selectCanvasAttackPoint = Convert.ToInt32(getPoint[1]);
                        int index = Convert.ToInt32(((TextBlock)enemySelectCanvas.Children[4]).Text);

                        foreach (var item in enemyFrontCanvasList)
                        {
                            if (item != null)
                            {
                                attackPoint = Convert.ToInt32(((TextBlock)item.Children[1]).Text);
                                attackPoint += selectCanvasAttackPoint;
                                ((TextBlock)item.Children[1]).Text = attackPoint.ToString();
                            }
                        }

                        enemyFrontCanvasList[index] = null; // 희생하는 캔버스 리스트 정보 비워주고
                        PlayStage.Children.Remove(enemySelectCanvas); // 희생하는 캔버스 지우고

                        checkVictory();
                        enemyActionPoint--;
                    }
                    else if (skillRandNum == 2) // 스킬2, 체력 업
                    {

                        int healthPoint = 0;

                        string[] getPoint = ((TextBlock)selectCanvas.Children[6]).Text.Split('P');
                        string[] getSkillName = ((TextBlock)enemySelectCanvas.Children[6]).Text.Split(':');

                        SkillTextBlock.Text = getSkillName[0];
                        SkillTextBlock.Foreground = Brushes.Blue;
                        SkillTextBlock.Visibility = Visibility.Visible;
                        skillStory.Begin();


                        int selectCanvasHealthPoint = Convert.ToInt32(getPoint[1]);
                        int index = Convert.ToInt32(((TextBlock)enemySelectCanvas.Children[4]).Text);

                        foreach (var item in enemyFrontCanvasList)
                        {
                            if (item != null)
                            {
                                healthPoint = Convert.ToInt32(((TextBlock)item.Children[0]).Text);
                                healthPoint += selectCanvasHealthPoint;
                                ((TextBlock)item.Children[1]).Text = healthPoint.ToString();
                            }
                        }
                        enemyFrontCanvasList[index] = null;
                        PlayStage.Children.Remove(enemySelectCanvas);

                        checkVictory();
                        enemyActionPoint--;
                    }
                    else if (skillRandNum == 3) // 스킬3, 턴 업!
                    {
                        string[] getPoint = ((TextBlock)selectCanvas.Children[7]).Text.Split('P');
                        string[] getSkillName = ((TextBlock)enemySelectCanvas.Children[7]).Text.Split(':');

                        SkillTextBlock.Text = getSkillName[0];
                        SkillTextBlock.Foreground = Brushes.Blue;
                        SkillTextBlock.Visibility = Visibility.Visible;
                        skillStory.Begin();


                        int selectCanvasTnPoint = Convert.ToInt32(getPoint[1]);

                        int index = Convert.ToInt32(((TextBlock)enemySelectCanvas.Children[4]).Text);

                        enemyActionPoint += selectCanvasTnPoint;
                        enemyFrontCanvasList[index] = null;
                        PlayStage.Children.Remove(enemySelectCanvas);

                        checkVictory();
                        enemyActionPoint--;
                    }
                }

               /*
            else if (firstTurn == false && checkCount(enemyFrontCanvasList) == 4)  // 4장이 이미 나와있다면....
            {
                userAttack(randomSelCanvas(playerFrontCanvasList), randomSelCanvas(enemyFrontCanvasList));
                checkVictory();
                enemyActionPoint--;
            }*/

            // 적이 행동 포인트 다썼다면
            if (enemyActionPoint == 0)
            {
                MessageBox.Show("니 차례다!");  // 유저 차례인거 알려주고

                // 유저 front 카드의 Lock 해제 + 스킬버튼 숨기고
                foreach (var item in playerFrontCanvasList)
                {
                    if (item != null)
                    {
                        ((TextBlock)item.Children[8]).Text = "0";
                    }
                }
                SkillButton.Visibility = Visibility.Hidden;
                AttkUpButton.Visibility = Visibility.Hidden;
                HpUpButton.Visibility = Visibility.Hidden;
                TnUpButton.Visibility = Visibility.Hidden;
            }
        }


        // 적이 사용자를 공격
        void userAttack(Canvas targetCanvas1, Canvas attackerCanvas)
        {


            int enemyTotalDam = 0; // 필드에 깐 적 카드 공격력 합
            int playerTotalHp = 0; // 필드에 깐 유저 카드 체력 합
            int playerHp = Convert.ToInt32(((TextBlock)playerCanvas.Children[0]).Text);
            int targetCanvasHP = Convert.ToInt32(((TextBlock)targetCanvas1.Children[0]).Text);
            int attackerCanvasHP = Convert.ToInt32(((TextBlock)attackerCanvas.Children[0]).Text);
            int index = Convert.ToInt32(((TextBlock)targetCanvas1.Children[4]).Text);

            // 필드에 깔려 있는 적 카드 공격력 합 구하기
            for (int i = 0; i < 4; i++)
            {
                if (enemyFrontCanvasList[i] == null)
                    continue;
                else
                    enemyTotalDam = enemyTotalDam + Convert.ToInt32(((TextBlock)enemyFrontCanvasList[i].Children[1]).Text);
            }

            // 필드에 깔려 있는 유저 카드 체력 합 구하기
            for (int i = 0; i < 4; i++)
            {
                if (playerFrontCanvasList[i] == null)
                    continue;
                else
                    playerTotalHp = playerTotalHp + Convert.ToInt32(((TextBlock)playerFrontCanvasList[i].Children[0]).Text);
            }

            if (playerHp >= enemyTotalDam) // 플레이어 hp가 적 공격 카드의 공격력 합보다 크면
            {

                attackAnimation(attackerCanvas, "user"); // 어택 애니메이션 적용
                loadGif(playerCanvas, slashGif);
                damagedAnimation(playerCanvas);
                playerHp = playerHp - Convert.ToInt32(((TextBlock)attackerCanvas.Children[1]).Text);

                if (playerHp <= 0)
                {
                    loadGif2(targetCanvas1, dieGif);
                    checkVictory();
                }
                else
                    ((TextBlock)playerCanvas.Children[0]).Text = playerHp.ToString();

                checkVictory();
            }
            else if (playerHp < enemyTotalDam) // 플레이어의 hp가 적 공격 카드의 공격력 합보다 작다면
            {
                if (checkCount(playerFrontCanvasList) != 0) // 유저가 깔아 놓은 카드가 있다면
                {
                    attackAnimation(attackerCanvas, "user"); // 어택 애니메이션 적용
                    loadGif(targetCanvas1, slashGif);
                    damagedAnimation(targetCanvas1);

                    targetCanvasHP = targetCanvasHP - Convert.ToInt32(((TextBlock)attackerCanvas.Children[1]).Text); ;

                    if (targetCanvasHP <= 0)
                    {
                        loadGif2(targetCanvas1, dieGif);
                        playerFrontCanvasList[index] = null;
                        PlayStage.Children.Remove(targetCanvas1);
                    }
                    else
                        ((TextBlock)targetCanvas1.Children[0]).Text = targetCanvasHP.ToString();
                    checkVictory();
                }
                else if (checkCount(playerFrontCanvasList) == 0) // 유저가 깔아 놓은 카드가 없다면 유저 직접 공격
                {
                    attackAnimation(attackerCanvas, "user"); // 어택 애니메이션 적용
                    loadGif(playerCanvas, slashGif);
                    damagedAnimation(playerCanvas);

                    playerHp = playerHp - Convert.ToInt32(((TextBlock)attackerCanvas.Children[1]).Text);

                    if (playerHp <= 0)
                    {
                        loadGif2(targetCanvas1, dieGif);
                        checkVictory();
                    }
                    else
                        ((TextBlock)playerCanvas.Children[0]).Text = playerHp.ToString();
                }
                checkVictory();
            }
        }

        // 사용자가 적을 공격
        void enemyAttack(Canvas targetCanvas1, Canvas attackerCanvas)
        {
            attackSound.Play();
            attackAnimation(attackerCanvas, "enemy");
            damagedAnimation(targetCanvas1);
            loadGif(targetCanvas1, slashGif);

            int targetCanvasHp = Convert.ToInt32(((TextBlock)targetCanvas1.Children[0]).Text);
            int attackerCanvasDam = Convert.ToInt32(((TextBlock)attackerCanvas.Children[1]).Text);

            if (targetCanvas.Name.Equals(enemyCard.CardName)) // 적 영웅 공격하는 거면
            {
                targetCanvasHp = targetCanvasHp - attackerCanvasDam; // 공격력 만큼 피 빼주고
                EnemyHeroHP = targetCanvasHp;
                if (targetCanvasHp <= 0)
                {
                    loadGif2(targetCanvas1, dieGif);
                    checkVictory();
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
                    enemyFrontCanvasList[index] = null;
                    PlayStage.Children.Remove(targetCanvas1);
                }
                else
                    ((TextBlock)targetCanvas1.Children[0]).Text = targetCanvasHp.ToString();

                MessageBox.Show("공격성공!");
            }

            // 공격 할 때마다 행동포인트 감소
            playerActionPoint--;
            targetCardClick = false;
            cardLock = false;

            checkVictory(); //승리체크
        }

        void finishTurn(object sender, RoutedEventArgs e)
        {
            if (myTurn == true)
            {
                enemyActionPoint = 3;
                if (playerActionPoint != 0)
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
                cpuAI();
                playerActionPoint = 3;

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

                Canvas.SetLeft(skillLine1, 15);
                Canvas.SetBottom(skillLine1, 35);

                Canvas.SetLeft(skillLine2, 15);
                Canvas.SetBottom(skillLine2, 22);

                Canvas.SetLeft(skillLine3, 15);
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

                hp.Visibility = Visibility.Hidden;
                dam.Visibility = Visibility.Hidden;
                skillLine1.Visibility = Visibility.Hidden;
                skillLine2.Visibility = Visibility.Hidden;
                skillLine3.Visibility = Visibility.Hidden;

                enemySubCanvas.Width = 150;
                enemySubCanvas.Height = 200;
                Canvas.SetTop(enemySubCanvas, 0);
                Canvas.SetLeft(enemySubCanvas, 0);

                PlayStage.Children.Add(enemySubCanvas);

                string targetGridName = "Enemy" + i;
                moveAnimation(enemySubCanvas, (Grid)PlayStage.FindName(targetGridName), i);

                enemySubCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(enemyCard_MouseLeftButtonDown);
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

                Canvas.SetLeft(skillLine1, 15);
                Canvas.SetBottom(skillLine1, 35);

                Canvas.SetLeft(skillLine2, 15);
                Canvas.SetBottom(skillLine2, 22);

                Canvas.SetLeft(skillLine3, 15);
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

                playerSubCanvas.Width = 150;
                playerSubCanvas.Height = 200;

                Canvas.SetTop(playerSubCanvas, 1000 - 200);
                Canvas.SetLeft(playerSubCanvas, 1400 - 150);


                PlayStage.Children.Add(playerSubCanvas);
                string targetGridName = "Me" + i;

                moveAnimation(playerSubCanvas, (Grid)PlayStage.FindName(targetGridName), i);
                playerSubCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(userCard_MouseLeftButtonDown);
            }
        }

        // 첫턴이 아닌 cpu AI  카드 꺼내는 타임틱
        void timer3_Tick(object sender, EventArgs e)
        {
            time3++;
            Console.WriteLine("틱" + time3);

            //적의 back 빈공간 만큼만 호출
            if (time3 <= enemyBackTime)
                checkEnemyBack(); // 턴 시작마다 뒤에 카드 채워 넣기 (첫턴은 다 채워져 있으니 무반응)

            Console.WriteLine("틱뒤" + time3);

            // 적의 빈공간 + 1 때 호출 종료  또는 카드덱 큐가 비어 있으면 호출 중료
            if (time3 == enemyBackTime + 1 || enemyCardDeckQueue.Count == 0)
            {
                timer3.Stop();
                time3 = 0;
                timer4 = new DispatcherTimer(); // 공격 및 스킬 및 카드 내기
                timer4.Interval = TimeSpan.FromSeconds(1);
                timer4.Tick += new EventHandler(timer4_Tick);
                timer4.Start();

                Console.WriteLine("타이머상태" + timer3.IsEnabled);
            }
            //  }));
            // _worker.RunWorkerAsync();
        }

        // 공격 및 스킬 및 카드 내기
        void timer4_Tick(object sender, EventArgs e)
        {
            time3++;
            aaa();

            if (enemyActionPoint == 0)
            {
                timer4.Stop();
                time3 = 0;
                // 플레이어 back 갱신

                playerBackTime = checkBackBlankCount(playerBackCanvasList); // 빈공간 몇개인지 파악해서 타임 돌아갈 횟수 겟..
                timer5 = new DispatcherTimer();
                timer5.Interval = TimeSpan.FromSeconds(1);
                timer5.Tick += new EventHandler(timer5_Tick);
                timer5.Start();

            }
        }

        // 첫 턴이면 타이머 줘서 순차적으로 카드 3장 내밀고  (적)
        void timer_Tick2(object sender, EventArgs e)
        {
            time++;
            int targetIndex = 0;
            int num = 0; // 랜덤 index 숫자

            num = randomIndex();
            targetIndex = checkIndex(enemyFrontCanvasList);
            string targetGridName = "E_Front" + targetIndex; // 타겟 그리드 이름 설정

            moveAnimation(enemyBackCanvasList[num], (Grid)PlayStage.FindName(targetGridName), num); // 이동 애니메이션
            enemyBackCanvasList[num].Background = new ImageBrush(new BitmapImage(new Uri(enemyBackCanvasList[num].Name + ".png", UriKind.Relative)));

            enemyBackCanvasList[num].Children[0].Visibility = Visibility.Visible;
            enemyBackCanvasList[num].Children[1].Visibility = Visibility.Visible;
            enemyBackCanvasList[num].Children[5].Visibility = Visibility.Visible;
            enemyBackCanvasList[num].Children[6].Visibility = Visibility.Visible;
            enemyBackCanvasList[num].Children[7].Visibility = Visibility.Visible;

            ((TextBlock)enemyBackCanvasList[num].Children[3]).Text = "1"; // 카드를 냈으면 col값 1로 변경
            ((TextBlock)enemyBackCanvasList[num].Children[4]).Text = targetIndex.ToString();


            // 적 카드판 정보 갱신...
            enemyFrontCanvasList[targetIndex] = enemyBackCanvasList[num];
            enemyBackCanvasList[num] = null;
            enemyActionPoint--;

            if (time == 3)
            {
                time = 0;
                firstTurn = false;
                MyMessageBox myMessage = new MyMessageBox();
                myMessage.SetText = "첫턴 종료!!";
                myMessage.ShowDialog();
                timer2.Stop();

                playerBackTime = checkBackBlankCount(playerBackCanvasList); // 빈공간 몇개인지 파악해서 타임 돌아갈 횟수 겟..
                timer5 = new DispatcherTimer();
                timer5.Interval = TimeSpan.FromSeconds(1);
                timer5.Tick += new EventHandler(timer5_Tick);
                timer5.Start();
            }
        }


        // 플레이어 back 갱신
        void timer5_Tick(object sender, EventArgs e)
        {
            time5++;

            // 여기 enemy랑 똑같이 if 안줘도 상관없겠지!?!?!!?!??!!??!?!?!?!?!?!?!?!?!?!?!??
            checkPlayerBack();

            if (time5 == playerBackTime + 1 || playerCardDeckQueue.Count == 0)
            {
                timer5.Stop();
                time5 = 0;

                foreach (var item in playerFrontCanvasList)
                {
                    if (item != null)
                    {
                        ((TextBlock)item.Children[8]).Text = "0";
                    }
                }
                MyMessageBox myMessage = new MyMessageBox();
                myMessage.SetText = "턴 시작";
                myMessage.ShowDialog();

                myTurn = true;
            }
        }

        private void skillButtonClick(object sender, RoutedEventArgs e)
        {
            MyMessageBox myMessage = new MyMessageBox();
            if (((TextBlock)selectCanvas.Children[8]).Text.Equals("1"))
            {
                myMessage.SetText = "이미 행동 한 카드!!!";
                myMessage.ShowDialog();
            }
            else if (playerActionPoint == 0)
            {
                myMessage.SetText = "행동 포인트 0!! ";
                myMessage.ShowDialog();
            }
            else
            {
                AttkUpButton.Visibility = Visibility.Visible;
                HpUpButton.Visibility = Visibility.Visible;
                TnUpButton.Visibility = Visibility.Visible;
            }
        }

        private void attkUpButtonClick(object sender, RoutedEventArgs e)
        {
            int attackPoint = 0;
            skillEffect = 1;
            string[] getPoint = ((TextBlock)selectCanvas.Children[5]).Text.Split('P');
            string[] getSkillName = ((TextBlock)selectCanvas.Children[5]).Text.Split(':');

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

            playerFrontCanvasList[index] = null;
            PlayStage.Children.Remove(selectCanvas);
            playerActionPoint--;
            SkillButton.Visibility = Visibility.Hidden;
            AttkUpButton.Visibility = Visibility.Hidden;
            HpUpButton.Visibility = Visibility.Hidden;
            TnUpButton.Visibility = Visibility.Hidden;
        }

        void skillStory_Completed(object sender, EventArgs e) // 스킬 사용 후 
        {
            if(skillEffect ==1)
                loadAttackGif(attackSkillGif0, attackSkillGif1, attackSkillGif2, attackSkillGif3);
            else if(skillEffect ==2)
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
            playerFrontCanvasList[index] = null;
            PlayStage.Children.Remove(selectCanvas);
            playerActionPoint--;
            SkillButton.Visibility = Visibility.Hidden;
            AttkUpButton.Visibility = Visibility.Hidden;
            HpUpButton.Visibility = Visibility.Hidden;
            TnUpButton.Visibility = Visibility.Hidden;
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
            playerFrontCanvasList[index] = null;
            PlayStage.Children.Remove(selectCanvas);
            playerActionPoint--;
            SkillButton.Visibility = Visibility.Hidden;
            AttkUpButton.Visibility = Visibility.Hidden;
            HpUpButton.Visibility = Visibility.Hidden;
            TnUpButton.Visibility = Visibility.Hidden;
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
                        Canvas.SetLeft(AttackUpSkill0, Canvas.GetLeft(playerFrontCanvasList[i])-20);
                        Canvas.SetTop(AttackUpSkill0, Canvas.GetTop(playerFrontCanvasList[i])-20);
                        AttackUpSkill0.Visibility = Visibility.Visible;
                        gif.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                        
                    }
                    else if (i == 1)
                    {
                        Canvas.SetLeft(AttackUpSkill1, Canvas.GetLeft(playerFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill1, Canvas.GetTop(playerFrontCanvasList[i]) - 20);
                        AttackUpSkill1.Visibility = Visibility.Visible;
                        gif2.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                    else if (i == 2)
                    {
                        Canvas.SetLeft(AttackUpSkill2, Canvas.GetLeft(playerFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill2, Canvas.GetTop(playerFrontCanvasList[i]) - 20);
                        AttackUpSkill2.Visibility = Visibility.Visible;
                        gif3.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                    else if (i == 3)
                    {
                        Canvas.SetLeft(AttackUpSkill3, Canvas.GetLeft(playerFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill3, Canvas.GetTop(playerFrontCanvasList[i]) - 20);
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
                        AttackUpSkill0.Visibility = Visibility.Visible;
                        gif.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                    else if (i == 1)
                    {
                        Canvas.SetLeft(AttackUpSkill1, Canvas.GetLeft(enemyFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill1, Canvas.GetTop(enemyFrontCanvasList[i]) - 20);
                        AttackUpSkill1.Visibility = Visibility.Visible;
                        gif2.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                    else if (i == 2)
                    {
                        Canvas.SetLeft(AttackUpSkill2, Canvas.GetLeft(enemyFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill2, Canvas.GetTop(enemyFrontCanvasList[i]) - 20);
                        AttackUpSkill2.Visibility = Visibility.Visible;
                        gif3.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                    else if (i == 3)
                    {
                        Canvas.SetLeft(AttackUpSkill3, Canvas.GetLeft(enemyFrontCanvasList[i]) - 20);
                        Canvas.SetTop(AttackUpSkill3, Canvas.GetTop(enemyFrontCanvasList[i]) - 20);
                        AttackUpSkill3.Visibility = Visibility.Visible;
                        gif4.GifSourceChanged("pack://application:,,,/attackSkill.gif");
                    }
                }

            }
        }

        void loadHealthGif(GifImage gif)
        {

            gif.Visibility = Visibility.Visible;
            AttackUpSkill0.Visibility = Visibility.Visible;
            AttackUpSkill1.Visibility = Visibility.Visible;
            AttackUpSkill2.Visibility = Visibility.Visible;
            AttackUpSkill3.Visibility = Visibility.Visible;

            //Canvas.SetLeft(AttackUpSkill0, Canvas.GetLeft(item));
           // Canvas.SetTop(AttackUpSkill0, Canvas.GetTop(item));

            gif.GifSourceChanged("pack://application:,,,/attack.gif");
        }

        void loadTurnGif(GifImage gif)
        {

            gif.Visibility = Visibility.Visible;
            AttackUpSkill0.Visibility = Visibility.Visible;
            AttackUpSkill1.Visibility = Visibility.Visible;
            AttackUpSkill2.Visibility = Visibility.Visible;
            AttackUpSkill3.Visibility = Visibility.Visible;

           // Canvas.SetLeft(AttackUpSkill0, Canvas.GetLeft(item));
           // Canvas.SetTop(AttackUpSkill0, Canvas.GetTop(item));

            gif.GifSourceChanged("pack://application:,,,/attack.gif");
        }

        private void goldChanceClick(object sender, RoutedEventArgs e)
        {
            if (myTurn == true && goldChanceNum < 2) // 내턴일때만 되도록, 찬스가 2번만 쓸수 있도록
            {
                MiniGame miniGame = new MiniGame();
                miniGame.Show();
                goldChanceNum++;
                if (Question == true)
                {
                    int plyaerHp = Convert.ToInt32(((TextBlock)playerCanvas.Children[0]).Text);
                    ((TextBlock)playerCanvas.Children[0]).Text = plyaerHp.ToString();
                    plyaerHp +=100;
                    Question = false;
                }
                else
                {
                    EnemyHeroHP += 100;
                    ((TextBlock)enemyHeroCanvas.Children[0]).Text = EnemyHeroHP.ToString();
                    Question = false;
                }
            }
        }
    }
}

