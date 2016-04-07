using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HistoryKing_client.Interfaces
{
    class Story_divide
    {
        private String name;

        String[] str;//이름 구분 짓는데 사용하는 문자

        private String[] main_char = new String[10];//주인공
        private int main_count = 0;//주인공 등장수

        private String[] stone = new String[10];//돌데크만
        private int stone_count = 0;//돌데크만 등장수

        private String[] hero = new String[10];//영웅
        private int hero_count = 0;//영웅 등장수

        private TextBox textbox1;//주인공 전용 대화창
        private TextBox textbox2;//돌데크만과 위인들의 대화창

        private int str_count = 1;

        private Image img1;
        private Image img2;

        private String hero_name;

        private FileStream fs;
        private String temp;

        public string[] HeroName;

        public Story_divide(String filename, TextBox text1, TextBox text2, Image image1, Image image2, String heroName)
        {
            this.name = filename;
            this.textbox1 = text1;
            this.textbox2 = text2;
            this.img1 = image1;
            this.img2 = image2;
            this.hero_name = heroName;

            myFace();

            textbox1.TextWrapping = TextWrapping.Wrap;
            textbox2.TextWrapping = TextWrapping.Wrap;
            
            fs = new FileStream(name, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader st = new StreamReader(fs, System.Text.Encoding.Default);
            st.BaseStream.Seek(0, SeekOrigin.Begin);
            
            while (st.Peek() > -1)
            {
                temp = st.ReadLine();  
            }
        }

        public void getDivide(){
            
            char[] delimiterChars = { '/' };

            string[] lines = temp.Split(delimiterChars);

            if (str_count >= lines.Length) {//배열의 끝값을 만난다면 나간다.
                string Hero = HeroName[0];
                SamGookStage1 samgookstage1 = new SamGookStage1(Hero);
                MainMenu.getInstance().WindowState = WindowState.Maximized;
                MainMenu.getInstance().frame.Navigate(samgookstage1);

                return;
            }

            str = lines[str_count].Split(':');

            if (str[0].Equals("주인공"))
            {
                main_char[main_count] = lines[str_count];
                //Console.WriteLine(lines[str_count]);
                textbox1.Text = main_char[main_count];

                textbox2.Text = "";
                main_count++;
                str_count++;
            }

            else if (str[0].Equals("돌데크만"))
            {
                stone[stone_count] = lines[str_count];
                //Console.WriteLine(stone[stone_count]);
                textbox2.Text = stone[stone_count];
                img2.Source = setImageChange("돌데크만.png");
                textbox1.Text = "";
                stone_count++;
                str_count++;
            }

            else//위인들
            {
                hero[hero_count] = lines[str_count];
                //Console.WriteLine(hero[hero_count]);
                textbox2.Text = lines[str_count];
                HeroName = lines[str_count].Split(':');
                img2.Source = setImageChange(hero_name);
                textbox1.Text = "";
                hero_count++;
                str_count++;
            }
        }

        public BitmapImage setImageChange(String imgName)
        {
            // 생성시킨다.
            BitmapImage image = new BitmapImage();
            // 생성한 이미지 조작 시작
            image.BeginInit();
            // filestream을 통해서 해당 경로의 파일을 읽어온다.
            FileStream fs = new FileStream(imgName, FileMode.Open, FileAccess.Read);
            // 해당 이미지를 바이트 단위로 읽어 들인다.
            Byte[] byImage = new Byte[(int)fs.Length];
            // 해당 데이터를 받아온다.. 
            fs.Read(byImage, 0, (int)fs.Length);
            fs.Close();
            // 영상을 복사한다.
            image.StreamSource = new MemoryStream(byImage);
            // 생성한 영상 조작을 끝낸다.
            image.EndInit();
            // 영상을  복사한다.
      
            return image;
        }

        public String[] getHero()
        {

            return hero;
        }

        public String[] getMainChar() 
        {
            return main_char;
        }

        public String[] getStone()
        {
            return stone;
        }

        public int getMain_count()
        {
            return main_count;
        }

        public int getStone_count() 
        {
            return stone_count;
        }

        public int getHero_count()
        {
            return hero_count;
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
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(getimg);
            IntPtr hBitmap = bmp.GetHbitmap();
            System.Windows.Media.ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            img1.Source = wpfBitmap;
           // img1.Width = 150;
           // img1.Height = 200;
            img1.Stretch = System.Windows.Media.Stretch.Fill;
        }
    }
}