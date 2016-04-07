using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace HistoryKing_AI_Test
{
    public class Card
    {
       private int HP;
        
       public int set_HP
       {
            get { return HP; }
            set { HP = value; }
       }

       private int AP;

       public int Set_AP
       {
           get { return AP; }
           set { AP = value; }
       }
       private String CardName;
       private String SkillName1, SkillName2, SkillName3;
       private int[] R;
       
       public Card()
       {
       }

       public Card(String CardName, int HP, int AP, String SkillName1, String SkillName2, String SkillName3)
       {
           this.CardName = CardName;
           this.HP = HP;
           this.AP = AP;
           this.SkillName1 = SkillName1;
           this.SkillName2 = SkillName2;
           this.SkillName3 = SkillName3;

           R = new int[1];
       }

       public String Return_CardName()
       {
           return CardName;
       }

       public int CardAttack()
       {
          return this.AP;       
       }

       public int CardHealth()
       {
           return this.HP;
       }
       
       public String Return_SkillName1()
       {
           return this.SkillName1;
       }

       public int Return_SkillName1_Effect()
       {
           int SkillEffect = 0;
           SkillEffect = this.HP + 3;

           return SkillEffect;
       }

       public String Return_SkillName2()
       {
           return this.SkillName2;
       }

       public int Return_SkillName2_Effect()
       {
           int SkillEffect = 0;
           SkillEffect = this.AP + 2;
           return SkillEffect;
       }

       public String Return_SkillName3()
       {
           return this.SkillName3;
       }

       public int Return_SkillName3_Effect()
       {           
           int SkillEffect = 1;
           return SkillEffect;
       }
    }
    
}
