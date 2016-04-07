using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
// CPU가 매턴 마다 필드에 나온 카드 수를 체크해서 3장 이상의 카드를 유지, 덱이 비었거나 
// 사용자가 CPU를 공격할 수 있도록 해 놓을것        *
// 카드 소멸될때 Label 도 같이 없애줄것
namespace HistoryKing_AI_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Stack<Card> My_Deck = new Stack<Card>(); // 사용자의 카드덱, 스텍으로 구현
        Stack<Card> CPU_Deck = new Stack<Card>(); // CPU의 카드덱, 스텍으로 구현
        
        List<Card>My_FrontDeck = new List<Card>(); // 사용자가 선택할 수 있는 카드
        List<Card> CPU_FrontDeck = new List<Card>(); // CPU가 선택할 수 있는 카드

        List<Card> My_TopCard = new List<Card>(); // 사용자가 선택한 카드를 판에 낸 카드
        List<Card> CPU_TopCard = new List<Card>(); // CPU가 선택한 카드를 판에 낸 카드

        Boolean[] User_CardAlive = new Boolean[4]; // 사용자 카드 소멸 되었는는 확인하는 bool값
        Boolean[] CPU_CardAlive = new Boolean[4]; // CPU의 카드 소멸 되었는지 확인하는 bool값

        Label[] label;
        Label[] cpu_label;
        String name = "button"; // button의 이름, button1, button2...

        int SelectCardIndex = 0; // My_TopCard에서 사용자가 선택한 값을 가리킨다

        Boolean Skill_Execute = false;          //스킬 시전 true, false

        int x = 0;
        int y = 462;

        int turn = 1;

        int User_Life = 24;
        int CPU_Life = 24;

        int MySkillPoint = 3;
        int CPUSkillPoint = 3;

        int skill_num = 0;

        int My_SelectCard_Index;
        string My_SelectCard_Name;
        string CPU_TargetCard_Name;

        Card nullCard = new Card("", 0, 0,"","","");

        private void Form1_Load(object sender, EventArgs e) // 폼이 생성될때
        {
            Card card1 = new Card("시발카드",4, 2, "시발체업","시발공업","시발턴업"); // 카드이름, 카드HP, 카드AP
            Card card2 = new Card("개카드", 4, 3, "개체업", "개공업", "개턴업"); // 카드이름, 카드HP, 카드AP
            Card card5 = new Card("십원카드", 1, 1, "십원체업", "십원공업", "십원턴업"); // 카드이름, 카드HP, 카드AP
            Card card7 = new Card("열살카드", 8, 6, "열살체업", "열살공업", "열살턴업"); // 카드이름, 카드HP, 카드AP
            Card card9 = new Card("XXX카드", 3, 2, "XXX체업", "XXX공업", "XXX턴업");

            Card card3 = new Card("병신카드", 1, 3, "병신체업", "병신공업", "병신턴업"); // 카드이름, 카드HP, 카드AP
            Card card4 = new Card("병진카드", 2, 2, "병진체업", "병진공업", "병진턴업"); // 카드이름, 카드HP, 카드AP
            Card card6 = new Card("병맛카드", 1, 4, "병맛체업", "병맛공업", "병맛턴업"); // 카드이름, 카드HP, 카드AP
            Card card8 = new Card("병장카드", 10, 5, "병장체업", "병장공업", "병장턴업"); // 카드이름, 카드HP, 카드AP
            Card card10 = new Card("XYZ", 3, 4, "XYZ체업", "XYZ공업", "XYZ턴업"); 

            My_Deck.Push(card1); // 스택으로 구현한 카드댁에 card1을 push
            My_Deck.Push(card2); // 스택으로 구현한 카드댁에 card2을 push
            My_Deck.Push(card5); // 스택으로 구현한 카드댁에 card5을 push
            My_Deck.Push(card7); // 스택으로 구현한 카드댁에 card7을 push
            My_Deck.Push(card9);

            CPU_Deck.Push(card3); // 스택으로 구현한 카드댁에 card3을 push
            CPU_Deck.Push(card4); // 스택으로 구현한 카드댁에 card4을 push
            CPU_Deck.Push(card6); // 스택으로 구현한 카드댁에 card6을 push
            CPU_Deck.Push(card8); // 스택으로 구현한 카드댁에 card8을 push
            CPU_Deck.Push(card10);

            this.label = new Label[4];
            this.cpu_label = new Label[4];

            for (int i = 0; i < 4; i++)  // 게임 시작시 판에 나온 카드가 없기 때문에 초기값은 false
            {
                User_CardAlive[i] = false;
                CPU_CardAlive[i] = false;
                My_FrontDeck.Add(null);
                My_TopCard.Add(null);
                CPU_FrontDeck.Add(null);
                CPU_TopCard.Add(null);

            }
            lbl_MySkillPoint.Text = MySkillPoint.ToString();
            lbl_CPUSkillPoint.Text = CPUSkillPoint.ToString();
            lbl_User_Life.Text = User_Life.ToString();
            lbl_CPU_LIFE.Text = CPU_Life.ToString();
        }
        
        private void button1_Click(object sender, EventArgs e) // 댁에서 카드를 꺼내어 배치하는 이벤트
        {
            textBox1.Text = textBox1.Text + Environment.NewLine + turn + "턴 입니다";
            MySkillPoint = 3;
            lbl_MySkillPoint.Text = MySkillPoint.ToString();
            if (My_Deck.Count == 0) // 카드덱에서 카드가 비었다면
            {
                MessageBox.Show("덱이 비어있습니다."); // 메세지 출력
            }
            else if (My_Deck.Count != 0) // 카드덱에서 카드가 비어있지 않다면
            {
                for (int i = 0; i < 4; i++) // 사용자가 선택할 수 있는 카드 수 만큼 반복하여 카드를 댁에서 꺼내어 배치한다
                {
                    try
                    {
                        if (My_FrontDeck[i] == null)
                        {
                            My_FrontDeck[i] = My_Deck.Pop(); // 사용자가 선택할 수 있는 카드를 List로 구현, 스택으로 구현한  댁에서 카드를 꺼내어 List에 삽입
                            this.Controls[name + (i + 1).ToString()].Text = My_FrontDeck[i].Return_CardName(); //버튼에 카드를 배치, 버튼의 text를 카드의 이름으로 바꿈
                        }
                    }
                    catch(Exception)
                    {
                        MessageBox.Show(e.ToString());
                    }
                 }              
            }
        }

        private void button2_Click(object sender, EventArgs e) 
        {
            //cpu에게 직접 공격
            CPU_Life -= My_TopCard[My_SelectCard_Index].CardAttack();       
            
            if (CPU_Life > 0)   
            {
                
                textBox1.Text = textBox1.Text + Environment.NewLine + "CPU 라이프 " + CPU_Life + "남았습니다.";
            }
            else if (CPU_Life <= 0) {
                CPU_Life = 0;
                textBox1.Text = textBox1.Text + Environment.NewLine + "CPU 라이프 " + CPU_Life + "남았습니다.";
                Check_CardDeck();   //승리했는지 확인
            }
            lbl_CPU_LIFE.Text = CPU_Life.ToString();
            


        }

        private void button1_Click_1(object sender, EventArgs e) // 1번카드
        {
            topCard(button1.Text, sender);
        }

        private void button2_Click_1(object sender, EventArgs e) // 2번카드
        {
            topCard(button2.Text, sender);
        }

        private void button3_Click(object sender, EventArgs e) // 3번카드
        {
            topCard(button3.Text, sender);
        }

        private void button4_Click(object sender, EventArgs e) // 4번카드
        {
            topCard(button4.Text, sender);
        }

        private void btn_endTurn_Click(object sender, EventArgs e) // 턴종료
        {
            textBox1.Text = textBox1.Text + Environment.NewLine + "************사용자의 턴을 종료 합니다.************";
            turn++;
            MySkillPoint = 3;
            lbl_MySkillPoint.Text = MySkillPoint.ToString();
            CPU_AI();
        }

        //**************************************************************************************
        private void Check_CardDeck() // 게임의 승패를 확인하는 
        {

            if (CPU_Deck.Count == 0 && CPU_TopCard.Count==0&& CPU_FrontDeck.Count==0)
            {
                MessageBox.Show("게임에서 승리 하였습니다.");
                Environment.Exit(1);
            }
            else if (CPU_Life <= 0) {
                MessageBox.Show("게임에서 승리 하였습니다.");
                Environment.Exit(1);
            }
            else if (My_Deck.Count == 0 && My_FrontDeck.Count == 0 && My_TopCard.Count == 0)
            {
                MessageBox.Show("게임에서 패배 하였습니다.");
                Environment.Exit(1);
            }
            else if (User_Life <= 0) {
                MessageBox.Show("게임에서 패배 하였습니다.");
                Environment.Exit(1);
            }

        }

        private void CPU_AI() // CPU의 턴, 사용자가 공격을 하고 턴이 끝났을때 바로 CPU_AI 메소드를 실행
        {
            int My_hpSum=0;
            int CPU_AcSum=0;
            int cpu_top_number = 0;

            if (turn == 2)
            {
                textBox1.Text = textBox1.Text + Environment.NewLine + turn+"턴 입니다.";
                for (int i = 0; i < 4; i++) // 사용자가 선택할 수 있는 카드 수 만큼 반복하여 카드를 댁에서 꺼내어 배치한다
                {
                    CPU_FrontDeck[i] = CPU_Deck.Pop(); // 사용자가 선택할 수 있는 카드를 List로 구현, 스택으로 구현한 댁에서 카드를 꺼내어 List에 삽입
                }

                for (int j = 0; CPUSkillPoint > 0; j++)
                {
                    CPU_TopCard[j] = CPU_FrontDeck[j];
                    CPU_FrontDeck[j] = null;
                    textBox1.Text = textBox1.Text + Environment.NewLine + CPU_TopCard[j].Return_CardName() + " 공격포인트: " + CPU_TopCard[j].CardAttack() + " 체력포인트: \n" + CPU_TopCard[j].CardHealth();
                    
                    this.cpu_label[j] = new Label();
                    this.cpu_label[j].BackColor = Color.Pink;
                    this.cpu_label[j].BringToFront();
                    x += 80; y = 70;
                    this.cpu_label[j].Size = new Size(60, 16);
                    this.cpu_label[j].Location = new Point(x, y);
                    this.cpu_label[j].TextAlign = ContentAlignment.MiddleCenter; // 텍스트 가운데 정렬
                    this.cpu_label[j].Text = CPU_TopCard[j].Return_CardName();
                    this.cpu_label[j].MouseClick += new MouseEventHandler(label_Click);
                    this.Controls.Add(this.cpu_label[j]);
                    
                    CPU_CardAlive[j] = true;
                    CPUSkillPoint--;
                }
                textBox1.Text = textBox1.Text + Environment.NewLine + "************cpu의 턴을 종료 합니다.*****************";
                
                MySkillPoint = 3;
                lbl_MySkillPoint.Text = MySkillPoint.ToString();
            }
            else {  //2턴이 아닌경우
                textBox1.Text = textBox1.Text + Environment.NewLine + turn + "턴 입니다." + CPU_TopCard.Count;
                CPUSkillPoint=3;
                int CPU_Number=0;
                
                for (int i = CPUSkillPoint; i > 0; i--) { // 스킬포인트 만큼 행동
                    Check_CardDeck();
                    // 필드에 나와있는 상대방의 카드의 체력 총합을 계산
                    for (int j = 0; j < My_TopCard.Count; j++)
                    {
                        try
                        {
                            My_hpSum += My_TopCard[j].CardHealth();
                        }
                        catch
                        {
                            continue;
                        }
                     }
                     // 필드에 나온 CPU의 카드의 공격력의 총합
                     for (int k = 0; k < CPU_TopCard.Count; k++)
                            {
                                try
                                {
                                    CPU_AcSum += CPU_TopCard[k].CardAttack(); //CPU 필드 위에 올려진 카드들의 공격력의 총 합
                                    CPU_Number = k; //CPU 필드 위의 카드의 배열 index
                                }
                                catch
                                {
                                    continue;
                                }
                             }
                        
                        
                     try
                     {
                         //1. CPU가 매턴 마다 필드에 나온 카드 수를 체크
                         //2. 3장 이상의 카드를 유지

                         for (int a = 0; a < CPU_TopCard.Count; a++) 
                         { 
                            //카드 수 체크

                             if (CPU_TopCard[a] == null || CPU_TopCard.Count() < 3) // 필드에 카드가 없다면
                             {
                                 if (CPU_Deck.Count != 0) // 덱에 잔여 카드가 있다면
                                 {
                                     for (int x = 0; x < CPU_FrontDeck.Count; x++) // 대기카드열 검색
                                     {
                                         if (CPU_FrontDeck[x] == null) // 대기카드열에 빈자리가 있다면
                                         {
                                             CPU_FrontDeck[x] = CPU_Deck.Pop(); // 대기카드열에 덱에서 꺼낸 카드 넣기
                                         }
                                      }

                                     try
                                     {
                                         cpu_top_number = a;
                                         CPU_TopCard[a] = CPU_FrontDeck[0];
                                         CPU_FrontDeck[0] = null;
                                         textBox1.Text = textBox1.Text + Environment.NewLine + "새로운 카드 " + CPU_TopCard[a].Return_CardName() + "를 올려 놓습니다.";
                                         CPUSkillPoint--;
                                         // 라벨추가
                                         break;
                                     }
                                     catch
                                     {
                                         if (CPU_FrontDeck[1] != null)
                                         {
                                             CPU_TopCard[a] = CPU_FrontDeck[1];
                                             CPU_FrontDeck[1] = null;
                                         }
                                         else if (CPU_FrontDeck[2] != null)
                                         {
                                             CPU_TopCard[a] = CPU_FrontDeck[2];
                                             CPU_FrontDeck[2] = null;
                                         }
                                         else if (CPU_FrontDeck[3] != null)
                                         {
                                             CPU_TopCard[a] = CPU_FrontDeck[3];
                                             CPU_FrontDeck[3] = null;
                                         }
                                             textBox1.Text = textBox1.Text + Environment.NewLine + "새로운 카드 " + CPU_TopCard[a].Return_CardName() + "를 올려 놓습니다.";
                                         CPUSkillPoint--;
                                         // 라벨추가
                                         break;
                                     }
                                  }
                                 else if (CPU_Deck.Count == 0) // 덱에 잔여 카드가 없다면
                                 {

                                         try
                                         {
                                             if (CPU_TopCard[a] == null) // 대기카드열에 빈자리가 있다면
                                             {
                                                 CPU_TopCard[a] = CPU_FrontDeck[0];
                                                 CPU_FrontDeck[0] = null;
                                                 CPUSkillPoint--;
                                                 textBox1.Text = textBox1.Text + Environment.NewLine + "새로운 카드 " + CPU_TopCard[a].Return_CardName() + "를 올려 놓습니다.";
                                                 break;
                                                 
                                             }
                                         }
                                         catch
                                         {
                                             if (CPU_FrontDeck[1] != null)
                                             {
                                                 CPU_TopCard[a] = CPU_FrontDeck[1];
                                                 CPU_FrontDeck[1] = null;
                                             }
                                             else if (CPU_FrontDeck[2] != null)
                                             {
                                                 CPU_TopCard[a] = CPU_FrontDeck[2];
                                                 CPU_FrontDeck[2] = null;
                                             }
                                             else if (CPU_FrontDeck[3] != null)
                                             {
                                                 CPU_TopCard[a] = CPU_FrontDeck[3];
                                                 CPU_FrontDeck[3] = null;
                                             }
                                             CPUSkillPoint--;
                                             textBox1.Text = textBox1.Text + Environment.NewLine + "새로운 카드 " + CPU_TopCard[a].Return_CardName() + "를 올려 놓습니다.";
                                             break;
                                          }
                                      
                                 }
                                 else if (CPU_Deck.Count == 0 && CPU_FrontDeck.Count == 0)
                                 {
                                     textBox1.Text = textBox1.Text + Environment.NewLine + "CPU 카드덱에 카드가 없습니다.";
                                     break;
                                 }
                             }
                         }

                         // 사용자 카드의 체력의 합이 CPU 카드의 공격력의 합 보다 크다면
                         if (My_hpSum >= CPU_AcSum)
                         {
                             User_Life -= CPU_TopCard[CPU_Number].CardAttack();
                             textBox1.Text = textBox1.Text + Environment.NewLine + "CPU가 사용자를 직접 공격 하였습니다. CPU의 공격력 : " + CPU_TopCard[CPU_Number].CardAttack() + " 만큼 사용자의 체력이 감소 하였습니다.";
                             lbl_User_Life.Text = User_Life.ToString();
                             Check_CardDeck();
                             continue;
                             //User_Life = User_Life- CPU_TopCard[x].Attack;
                         }

                         // 사용자 카드의 체력의 합이 CPU 카드 공격력의 합 보다 작다면
                         else if (My_hpSum < CPU_AcSum)
                         {

                             // 유저 필드 위에 카드가 있다면
                             if (My_TopCard.Count != 0)
                             {
                                 // 스킬을 시전 하지 않았다면
                                 if (Skill_Execute == false)
                                 {
                                     textBox1.Text = textBox1.Text + Environment.NewLine + CPU_TopCard[CPU_Number].Return_SkillName1() + "기술을 씁니다.";
                                     Skill_Execute = true;
                                     CPU_TopCard[CPU_Number] = null;             // 스킬쓰면 카드 삭제. CPU_Number가 아님.
                                     CPU_TopCard[1].Return_SkillName1_Effect(); // 1번 스킬 시전, 아무 스킬이나 랜덤으로 쓰도록 만들어야함. [1]번째가 아닌 CPU 필드에 나온 카드를 랜덤으로 선택되게 만듬
                                     textBox1.Text = textBox1.Text + Environment.NewLine + "스킬효과 발동 필드에 아무 카드 카드 HP+2 " + CPU_TopCard[1].CardHealth();
                                     // 스킬시전 결과
                                     continue;

                                 }
                                 // 스킬을 이전에 시전 하였다면
                                 else if (Skill_Execute == true)
                                 {
                                     // 카드를 직접 공격, 역시 CPU_Number가 아님
                                     textBox1.Text = textBox1.Text + Environment.NewLine + "CPU의 카드 " + CPU_TopCard[CPU_Number].Return_CardName() + "가 사용자의 카드 " + My_TopCard[CPU_Number].Return_CardName() + " 를 공격합니다."; // 어떤 카드를 공격할지 바꾸기 CPU_Number 아님
                                     My_TopCard[CPU_Number].set_HP -= CPU_TopCard[CPU_Number].CardAttack();
                                 }
                             }
                             // 유저 필드에 카드가 없다면 사용자를 직접 공격함
                             else
                             {
                                 User_Life -= CPU_TopCard[CPU_Number].CardAttack(); //CPU_Number가 아님
                                 continue;
                             }
                         }
                     }
                     catch {
                            // MessageBox.Show("NullPoint 332 Line ");
                            continue; 
                            }
                    }
            }
            turn++;
        }
        
        private void topCard(string buttonName, object sender) // 선택한 카드를 판에 올려놓는 이벤트
        {
            if (MySkillPoint > 0)
            {
                for (int i = 0; i < My_FrontDeck.Count(); i++) // List만큼 반복
                {
                    try // 예외처리
                    {
                        if (My_FrontDeck[i].Return_CardName().CompareTo(buttonName) == 0) // My_FrontDeck의 이름을 가진 List에서 buttonName과 이름이 같은 객체를 찾는다
                        {
                            DialogResult result = MessageBox.Show(buttonName + "을 내겠습니까?", "확인", MessageBoxButtons.OKCancel); // Ok, Cancel 버튼을 가진 Dialog창을 생성
                            if (result == DialogResult.OK) // OK 버튼을 선택했을때 이벤트
                            {
                                My_TopCard[i] = My_FrontDeck[i]; // 사용자가 선택한 카드를 판에 올려 놓는다
                                //My_FrontDeck.Insert(i, null); // List에서 사용자가 선택한 카드를 null로 넣는다
                                My_FrontDeck[i] = null;
                                textBox1.Text = textBox1.Text + Environment.NewLine + My_TopCard[i].Return_CardName() + " 공격포인트: " + My_TopCard[i].CardAttack() + " 체력포인트: \n" + My_TopCard[i].CardHealth();
                                User_CardAlive[i] = true; // 판에 올려 놓았기 때문에 bool값을 true
                                this.Controls[name + (i + 1).ToString()].Text = "비어있습니다."; // 선택된 카드가 표시된 Button의 Text값을 "비어있습니다."로 표시
                                MySkillPoint += -1;
                                lbl_MySkillPoint.Text = MySkillPoint.ToString(); // 스킬포인트 감소
                                // Label을 생성하여 앞쪽으로 카드를 내민다
                                this.label[i] = new Label();
                                this.label[i].BackColor = Color.Green;
                                this.label[i].BringToFront();
                                x += 80;
                                this.label[i].Size = new Size(60, 16);
                                this.label[i].Location = new Point(x, y);
                                this.label[i].TextAlign = ContentAlignment.MiddleCenter; // 텍스트 가운데 정렬
                                this.label[i].Text = My_TopCard[i].Return_CardName();
                                this.label[i].MouseClick += new MouseEventHandler(label_Click);
                                this.Controls.Add(this.label[i]);
                                // i값을 저장
                                SelectCardIndex = i;
                            }
                            else if (result == DialogResult.Cancel)
                            {
                                break; // 취소 버튼을 선택했기 때문에 아무런 이벤트를 하지 않는다.
                            }
                        }
                    }
                    catch // 예외처리: My_FrontDeck에서 카드를 꺼내고 그 자리를 null로 채우는데 CompareTo 명령을 수행할때 null값을 가리킬때 널포인트 예외를 던지기 때문에 다음줄로 수행을 못한다.
                    {
                        continue; // 예외를 무시하고 다음 반복을 수행한다. List에서 객체 탐색을 수행하는 코드이기 때문에 예외를 무시하고 다음 반복을 수행해야 한다.
                    }
                }
            }
            else
                MessageBox.Show("스킬포인트가 부족 합니다.");
        }
        
        public void label_Click(object sender, MouseEventArgs e) 
        {
            string[] name = sender.ToString().Split(':');
            string[] name2 = name[1].Split(' ');

            for (int i = 0; i < My_TopCard.Count; i++)
            {
                try
                {
                    if (My_TopCard[i].Return_CardName().CompareTo(name2[1]) == 0)
                    {
                        btn_Skill1.Text = My_TopCard[i].Return_SkillName1();
                        btn_Skill2.Text = My_TopCard[i].Return_SkillName2();
                        btn_Skill3.Text = My_TopCard[i].Return_SkillName3();
                        My_SelectCard_Name = name2[1]; // 공격할 대상
                        My_SelectCard_Index = i;
                        //textBox1.Text = textBox1.Text + Environment.NewLine + "***********" + i;
                    }
                    
                    if (My_TopCard[i].Return_CardName().CompareTo(name2[1]) == 0 && Skill_Execute == true)
                    {
                        My_SelectCard_Index = i;
                       // textBox1.Text = textBox1.Text + Environment.NewLine + "" + i;
                    }

                    // 체크
                    if (MySkillPoint > 0)
                    {
                        try
                        {
                            if (CPU_TopCard[i].Return_CardName().CompareTo(name2[1]) == 0)
                            {
                                CPU_TargetCard_Name = name2[1];
                                DialogResult result = MessageBox.Show("사용자가 선택한 타겟 : " + name2[1] + "공격 할 카드 :" + My_SelectCard_Name, "확인", MessageBoxButtons.OKCancel); // Ok, Cancel 버튼을 가진 Dialog창을 생성
                                if (result == DialogResult.OK) // OK 버튼을 선택했을때 이벤트
                                {
                                    textBox1.Text = textBox1.Text + Environment.NewLine + "사용자 카드 " + My_SelectCard_Name + " 가 CPU카드 " + name2[1] + "를 공격합니다.";
                                    CPU_TopCard[i].set_HP -= My_TopCard[My_SelectCard_Index].CardAttack();
                                    MySkillPoint -= 1;
                                    lbl_MySkillPoint.Text = MySkillPoint.ToString();

                                    if (CPU_TopCard[i].CardHealth() <= 0)
                                    {
                                        textBox1.Text = textBox1.Text + Environment.NewLine + "CPU 카드 " + CPU_TopCard[i].Return_CardName() + " 의 체력: 0 소멸";
                                       
                                        for (int j = 0; j < cpu_label.Count(); j++)
                                        {
                                            if (cpu_label[j].Text.CompareTo(CPU_TopCard[i].Return_CardName()) == 0)
                                            {
                                                cpu_label[j].Visible = false;
                                                this.Container.Remove(cpu_label[j]);
                                                CPU_TopCard[i] = null;
                                                break;
                                            }
                                        }
                                        // 라벨 삭제
                                    }
                                    else
                                    {
                                        textBox1.Text = textBox1.Text + Environment.NewLine + "CPU 카드 " + CPU_TopCard[i].Return_CardName() + " 의 체력: " + CPU_TopCard[i].CardHealth().ToString();
                                    }
                                    Check_CardDeck();
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }

                        if (My_TopCard[i].Return_CardName().CompareTo(name2[1]) == 0 && Skill_Execute == true && skill_num == 1) // HP 업
                        {
                            //MessageBox.Show(My_TopCard[i].Return_CardName());
                            My_TopCard[My_SelectCard_Index].set_HP = My_TopCard[My_SelectCard_Index].Return_SkillName1_Effect();
                            textBox1.Text = textBox1.Text + Environment.NewLine + My_TopCard[My_SelectCard_Index].Return_CardName() + "의 체력은 " + My_TopCard[My_SelectCard_Index].CardHealth();
                            Skill_Execute = false;
                            skill_num = 0;
                            MySkillPoint -= 1;
                            lbl_MySkillPoint.Text = MySkillPoint.ToString();
                        }

                        if (My_TopCard[i].Return_CardName().CompareTo(name2[1]) == 0 && Skill_Execute == true && skill_num == 2)
                        {
                            My_TopCard[My_SelectCard_Index].Set_AP = My_TopCard[My_SelectCard_Index].Return_SkillName2_Effect();
                            textBox1.Text = textBox1.Text + Environment.NewLine + My_TopCard[My_SelectCard_Index].Return_CardName() + "의 공격력은 " + My_TopCard[My_SelectCard_Index].CardAttack();
                            Skill_Execute = false;
                            skill_num = 0;
                            MySkillPoint--;
                            lbl_MySkillPoint.Text = MySkillPoint.ToString();
                        }
                    }
                    else 
                    {
                        MessageBox.Show("스킬 포인트 부족");
                        break;
                    }                       
                }
                catch
                {
                    continue;
                }
            }
        }

        private void btn_Skill1_Click(object sender, EventArgs e)
        {
            //왼쪽
            Skill_Execute = true;
            skill_num = 1;
        }

        private void btn_Skill2_Click(object sender, EventArgs e)
        {
            //오른쪽
            Skill_Execute = true;
            skill_num = 2;
        }
    }
}