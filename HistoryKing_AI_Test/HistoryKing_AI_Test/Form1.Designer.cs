namespace HistoryKing_AI_Test
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Open = new System.Windows.Forms.Button();
            this.btn_Attack = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btn_endTurn = new System.Windows.Forms.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape4 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectangleShape3 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectangleShape2 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.lbl_MySkillPoint = new System.Windows.Forms.Label();
            this.lbl_CPUSkillPoint = new System.Windows.Forms.Label();
            this.btn_Skill1 = new System.Windows.Forms.Button();
            this.btn_Skill2 = new System.Windows.Forms.Button();
            this.btn_Skill3 = new System.Windows.Forms.Button();
            this.lbl_CPU_LIFE = new System.Windows.Forms.Label();
            this.lbl_User_Life = new System.Windows.Forms.Label();
            this.lbl_CPU = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Open
            // 
            this.btn_Open.Location = new System.Drawing.Point(39, 600);
            this.btn_Open.Name = "btn_Open";
            this.btn_Open.Size = new System.Drawing.Size(77, 98);
            this.btn_Open.TabIndex = 0;
            this.btn_Open.Text = "카드깔기";
            this.btn_Open.UseVisualStyleBackColor = true;
            this.btn_Open.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_Attack
            // 
            this.btn_Attack.Location = new System.Drawing.Point(589, 609);
            this.btn_Attack.Name = "btn_Attack";
            this.btn_Attack.Size = new System.Drawing.Size(86, 98);
            this.btn_Attack.TabIndex = 1;
            this.btn_Attack.Text = "공격";
            this.btn_Attack.UseVisualStyleBackColor = true;
            this.btn_Attack.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(22, 138);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(884, 297);
            this.textBox1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(135, 550);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 82);
            this.button1.TabIndex = 3;
            this.button1.Text = "카드";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(217, 550);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(76, 82);
            this.button2.TabIndex = 4;
            this.button2.Text = "카드";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(299, 550);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(76, 82);
            this.button3.TabIndex = 5;
            this.button3.Text = "카드";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(381, 550);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(76, 82);
            this.button4.TabIndex = 6;
            this.button4.Text = "카드";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btn_endTurn
            // 
            this.btn_endTurn.Location = new System.Drawing.Point(494, 609);
            this.btn_endTurn.Name = "btn_endTurn";
            this.btn_endTurn.Size = new System.Drawing.Size(89, 98);
            this.btn_endTurn.TabIndex = 7;
            this.btn_endTurn.Text = "턴 종료";
            this.btn_endTurn.UseVisualStyleBackColor = true;
            this.btn_endTurn.Click += new System.EventHandler(this.btn_endTurn_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape4,
            this.rectangleShape3,
            this.rectangleShape2,
            this.rectangleShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(928, 720);
            this.shapeContainer1.TabIndex = 8;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape4
            // 
            this.rectangleShape4.Location = new System.Drawing.Point(699, 449);
            this.rectangleShape4.Name = "rectangleShape4";
            this.rectangleShape4.Size = new System.Drawing.Size(203, 125);
            // 
            // rectangleShape3
            // 
            this.rectangleShape3.Location = new System.Drawing.Point(237, 36);
            this.rectangleShape3.Name = "rectangleShape3";
            this.rectangleShape3.Size = new System.Drawing.Size(665, 84);
            // 
            // rectangleShape2
            // 
            this.rectangleShape2.Location = new System.Drawing.Point(22, 451);
            this.rectangleShape2.Name = "rectangleShape2";
            this.rectangleShape2.Size = new System.Drawing.Size(665, 84);
            // 
            // rectangleShape1
            // 
            this.rectangleShape1.Location = new System.Drawing.Point(22, 541);
            this.rectangleShape1.Name = "rectangleShape1";
            this.rectangleShape1.Size = new System.Drawing.Size(665, 171);

            // 
            // lbl_MySkillPoint
            // 
            this.lbl_MySkillPoint.AutoSize = true;
            this.lbl_MySkillPoint.Location = new System.Drawing.Point(483, 550);
            this.lbl_MySkillPoint.Name = "lbl_MySkillPoint";
            this.lbl_MySkillPoint.Size = new System.Drawing.Size(65, 12);
            this.lbl_MySkillPoint.TabIndex = 9;
            this.lbl_MySkillPoint.Text = "스킬포인트";
            // 
            // lbl_CPUSkillPoint
            // 
            this.lbl_CPUSkillPoint.AutoSize = true;
            this.lbl_CPUSkillPoint.Location = new System.Drawing.Point(163, 109);
            this.lbl_CPUSkillPoint.Name = "lbl_CPUSkillPoint";
            this.lbl_CPUSkillPoint.Size = new System.Drawing.Size(65, 12);
            this.lbl_CPUSkillPoint.TabIndex = 10;
            this.lbl_CPUSkillPoint.Text = "스킬포인트";
            // 
            // btn_Skill1
            // 
            this.btn_Skill1.Location = new System.Drawing.Point(716, 467);
            this.btn_Skill1.Name = "btn_Skill1";
            this.btn_Skill1.Size = new System.Drawing.Size(75, 40);
            this.btn_Skill1.TabIndex = 11;
            this.btn_Skill1.Text = "button5";
            this.btn_Skill1.UseVisualStyleBackColor = true;
            this.btn_Skill1.Click += new System.EventHandler(this.btn_Skill1_Click);
            // 
            // btn_Skill2
            // 
            this.btn_Skill2.Location = new System.Drawing.Point(810, 467);
            this.btn_Skill2.Name = "btn_Skill2";
            this.btn_Skill2.Size = new System.Drawing.Size(75, 40);
            this.btn_Skill2.TabIndex = 12;
            this.btn_Skill2.Text = "button6";
            this.btn_Skill2.UseVisualStyleBackColor = true;
            this.btn_Skill2.Click += new System.EventHandler(this.btn_Skill2_Click);
            // 
            // btn_Skill3
            // 
            this.btn_Skill3.Location = new System.Drawing.Point(766, 522);
            this.btn_Skill3.Name = "btn_Skill3";
            this.btn_Skill3.Size = new System.Drawing.Size(75, 40);
            this.btn_Skill3.TabIndex = 13;
            this.btn_Skill3.Text = "button7";
            this.btn_Skill3.UseVisualStyleBackColor = true;
            // 
            // lbl_CPU_LIFE
            // 
            this.lbl_CPU_LIFE.AutoSize = true;
            this.lbl_CPU_LIFE.Location = new System.Drawing.Point(48, 60);
            this.lbl_CPU_LIFE.Name = "lbl_CPU_LIFE";
            this.lbl_CPU_LIFE.Size = new System.Drawing.Size(38, 12);
            this.lbl_CPU_LIFE.TabIndex = 14;
            this.lbl_CPU_LIFE.Text = "label1";
            // 
            // lbl_User_Life
            // 
            this.lbl_User_Life.AutoSize = true;
            this.lbl_User_Life.Location = new System.Drawing.Point(766, 619);
            this.lbl_User_Life.Name = "lbl_User_Life";
            this.lbl_User_Life.Size = new System.Drawing.Size(38, 12);
            this.lbl_User_Life.TabIndex = 15;
            this.lbl_User_Life.Text = "label1";
            // 
            // lbl_CPU
            // 
            this.lbl_CPU.AutoSize = true;
            this.lbl_CPU.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_CPU.Location = new System.Drawing.Point(156, 60);
            this.lbl_CPU.Name = "lbl_CPU";
            this.lbl_CPU.Size = new System.Drawing.Size(55, 24);
            this.lbl_CPU.TabIndex = 16;
            this.lbl_CPU.Text = "CPU";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 720);
            this.Controls.Add(this.lbl_CPU);
            this.Controls.Add(this.lbl_User_Life);
            this.Controls.Add(this.lbl_CPU_LIFE);
            this.Controls.Add(this.btn_Skill3);
            this.Controls.Add(this.btn_Skill2);
            this.Controls.Add(this.btn_Skill1);
            this.Controls.Add(this.lbl_CPUSkillPoint);
            this.Controls.Add(this.lbl_MySkillPoint);
            this.Controls.Add(this.btn_endTurn);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_Attack);
            this.Controls.Add(this.btn_Open);
            this.Controls.Add(this.shapeContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Open;
        private System.Windows.Forms.Button btn_Attack;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btn_endTurn;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape3;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape2;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape1;
        private System.Windows.Forms.Label lbl_MySkillPoint;
        private System.Windows.Forms.Label lbl_CPUSkillPoint;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape4;
        private System.Windows.Forms.Button btn_Skill1;
        private System.Windows.Forms.Button btn_Skill2;
        private System.Windows.Forms.Button btn_Skill3;
        private System.Windows.Forms.Label lbl_CPU_LIFE;
        private System.Windows.Forms.Label lbl_User_Life;
        private System.Windows.Forms.Label lbl_CPU;
    }
}

