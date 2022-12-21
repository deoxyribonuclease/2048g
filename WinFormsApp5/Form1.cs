using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Timer = System.Windows.Forms.Timer;

namespace WinFormsApp5
{
    public partial class Form1 : Form
    {
        // для обрахунку кількості ходів без руху
        private int temp = 0;
        // для перевірки наявності на полі плитки 2048
        private bool end = false;
        // для перевірки активності паузи
        private bool pause = false;
        // для перевірки чи рухалась плитка
        private bool picMove = false;
        // індекс відповідності до однієї с чотирьох сторін
        private int index = 1;
        // для запису вибраної алгоритмом сторони
        private string side = " ";
        // для запису рахунку
        private int score = 0;
        // поле гри яке задається булевою матрицею (за замовчуванням 6х6)
        private int[,] field = new int[6, 6];


        Form form2 = new Form();
        Label score1 = new Label();
        Label turn = new Label();
        Timer timer = new Timer();


        // створення плиток (pics - плитка, labels - текст на плитці)
        private Label[,] labels = new Label[6,  6];
        private PictureBox[,] pics = new PictureBox[6, 6];

        public Form1()
        {       
            this.MaximizeBox = false;   
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            form2.MaximizeBox = false;
            form2.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
            form2.KeyDown += new KeyEventHandler(Press2);
        }

        // перевірка чи можуть плитки рухатися, якщо ні - гра закінчується (поразка)
        private void LostCheck()
        {
            int a = 0, b = 0, c = 0, d = 0;
            side = "Left"; Press();
            if (picMove == false)
                        a = 1;
            side = "Up"; Press();
            if (picMove == false)
                        b = 1;
            side = "Right"; Press();
            if (picMove == false)
                        c = 1;
            side = "Down"; Press();
            if (picMove == false)
                        d = 1;
            int sum = a + b + c + d;
            if (a + b + c + d == 4)
            {
                timer.Stop();
                DialogResult vibor1 = MessageBox.Show("Score: " + score, "Bot lost. ", MessageBoxButtons.OK);
                if (vibor1 == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
                
        }
        // вікно закічення гри (перемога)
        private void EndMessageBox()
        {
             DialogResult vibor2 = MessageBox.Show("Score: " + score, "Bot Win! " ,  MessageBoxButtons.OK);
            if (vibor2 == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        // управляння анімацією ("Space" - пауза, "Escape" - вихід )
        private void Press2(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Space":
                    if (pause == false)
                    {
                        timer.Stop();
                        pause = true;
                    }
                    else
                    {
                        timer.Start();
                        pause = false;
                    }
                    break;
                case "Escape":
                    Application.Exit(); break;
            }
        }
        // CircleBot
        private void Bot1()
        {
            timer.Interval = (int)numericUpDown3.Value;
            timer.Tick += new EventHandler((_s, _e) =>
            {
                if (index == 5)
                    index = 1;

                if (temp == 4)
                {
                    LostCheck();
                }
                if (end)
                {
                    timer.Stop();
                    EndMessageBox();
                }
            switch (index)
            {
                case 1: side = "Left"; break;
                case 2: side = "Up"; break;
                case 3: side = "Right"; break;
                case 4: side = "Down"; break;
            }
                Press();
            index++;
            });
            timer.Start();
        }
        //RandomBot
        private void Bot2()
        {
            Random rand = new Random();
            timer.Interval = (int)numericUpDown3.Value;
            timer.Tick += new EventHandler((_s, _e) =>
            {
                if (index == 5)
                    index = 1;
                if (temp == 8)
                {
                    LostCheck();
                }
                if (end)
                {
                    timer.Stop();
                    EndMessageBox();
                }
                switch (rand.Next(1,5))
                {
                    case 1: side = "Left"; break;
                    case 2: side = "Up"; break;
                    case 3: side = "Right"; break;
                    case 4: side = "Down"; break;
                }
                Press();
                index++;
            });
            timer.Start();
        }
        //LostBot
        private void Bot3()
        {
            timer.Interval = (int)numericUpDown3.Value;
            timer.Tick += new EventHandler((_s, _e) =>
            {
                if (temp == 2 && (index == 1 || index == 2))
                    index = 3;
                else if(temp == 2 && (index == 3 || index == 4))
                    index = 1;
               
                if(temp == 4)
                {
                    LostCheck();
                }

                switch (index)
                {
                    case 1:
                        side = "Left";
                        index++; break;
                    case 2:
                        side = "Right";
                        index--; break;
                    case 3:
                        side = "Up";
                        index++; break;
                    case 4:
                        side = "Down";
                        index--; break;
                }
                Press();
            });
            timer.Start();
        }
        // кнопка створення форми з полем гри
        private void button1_Click(object sender, EventArgs e)
        {

            field = new int[(int)numericUpDown2.Value, (int)numericUpDown1.Value];
            labels = new Label[(int)numericUpDown2.Value, (int)numericUpDown1.Value];
            pics = new PictureBox[(int)numericUpDown2.Value, (int)numericUpDown1.Value];
            // new form
            form2.Text = "2048";
            form2.Size = new Size(56 * (int)numericUpDown1.Value + 35, 56 * (int)numericUpDown2.Value + 120);
            form2.StartPosition = FormStartPosition.CenterScreen;     
            form2.BackColor = Color.FromArgb(188, 175, 161);
            // turn label
            turn.Location = new Point(form2.Width-170, 10);
            turn.ForeColor = Color.FromArgb(140, 110, 101);
            turn.Text = "Turn: None";
            turn.Font = new Font("Arial", 16, FontStyle.Bold);
            turn.Size = new Size(160, 30);
            turn.Anchor = (AnchorStyles.Top);
            // score label
            score1.Location = new Point(10 , 10);
            score1.ForeColor = Color.FromArgb(140, 110, 101);
            score1.Text = "Score: 0";
            score1.Size = new Size(150, 30);
            score1.Font = new Font("Arial", 16, FontStyle.Bold);    
            score1.Anchor = (AnchorStyles.Top);
           

            form2.Controls.Add(turn);
            form2.Controls.Add(score1);

            CreateField();
            GenerateNewPic();
            GenerateNewPic();

            this.Hide();
            form2.Show();
            // вибір бота          
            switch (comboBox1.SelectedItem)
            {
                case "CircleBot": Bot1(); break;
                case "RandomBot": Bot2(); break;
                case "LostBot": Bot3(); break;
            }
        }

        // візуальне строення ігрового поля
        private void CreateField()
        {
            for (int i = 0; i < (int)numericUpDown2.Value; i++)
            {
                for (int j = 0; j < (int)numericUpDown1.Value; j++)
                {
                    PictureBox pic = new PictureBox();
                    pic.Location = new Point(12 + 56 * j, 73 + 56 * i);
                    pic.Size = new Size(50, 50);
                    pic.BackColor = Color.FromArgb(206, 192, 177);
                    pic.Anchor = (AnchorStyles.Top);
                    form2.Controls.Add(pic);
                }
            }
        }

        // генерація плитки 2(80%) або 4(20%) на рандомномній не_зайнятій ячейці 
        private void GenerateNewPic()
        {
            Random rnd = new Random();
            int a = rnd.Next(0, (int)numericUpDown2.Value);
            int b = rnd.Next(0, (int)numericUpDown1.Value);
            while (pics[a, b] != null)
            {
                a = rnd.Next(0, (int)numericUpDown2.Value);
                b = rnd.Next(0, (int)numericUpDown1.Value);
            }
            field[a, b] = 1;
            pics[a, b] = new PictureBox();
            labels[a, b] = new Label();
            labels[a, b].Size = new Size(50, 50);
            labels[a, b].TextAlign = ContentAlignment.MiddleCenter;
            labels[a, b].Font = new Font(new FontFamily("Times New Roman"), 12);
            pics[a, b].Controls.Add(labels[a, b]);
            pics[a, b].Location = new Point(12 + b * 56, 73 + 56 * a);
            pics[a, b].Size = new Size(50, 50);
            pics[a, b].Anchor = (AnchorStyles.Top);
            // ймовірність появи плитки 2 або 4
            if (rnd.Next(1, 11) <= 8)
            {
            labels[a, b].Text = "2";
            pics[a, b].BackColor = Color.FromArgb(239, 229, 219);
            }
            else
            {
                labels[a, b].Text = "4";
                pics[a, b].BackColor = Color.FromArgb(238, 225, 201);
            }
            form2.Controls.Add(pics[a, b]);
             pics[a, b].BringToFront();
        }  
        //метод зміни кольору при зміні значення + визначення кінця гри
        private void ChangeColor(int sum, int k, int j)
        {
            switch (sum)
            {
                case 4:  pics[k, j].BackColor = Color.FromArgb(238, 225, 201); break;
                case 8 : pics[k, j].BackColor = Color.FromArgb(242, 179, 122); break;
                case 16: pics[k, j].BackColor = Color.FromArgb(245, 150, 99); break;
                case 32: pics[k, j].BackColor = Color.FromArgb(246, 125, 95); break;
                case 64: pics[k, j].BackColor = Color.FromArgb(246, 94, 56); break;
                case 128: pics[k, j].BackColor = Color.FromArgb(238, 208, 115); break;
                case 256: pics[k, j].BackColor = Color.FromArgb(238, 205, 97); break;
                case 512: pics[k, j].BackColor = Color.FromArgb(238, 201, 79); break;
                case 1024: pics[k, j].BackColor = Color.FromArgb(238, 198, 62); break;
                case 2048: pics[k, j].BackColor = Color.Yellow; end = true; break;
            }    
        }
        // метод руху по полю
        private void Press()
        {
            picMove = false;
            switch (side)
            {
                case "Right":
                    turn.Text = "Turn: Right →";
                    // цикл відповідає за вибір плиток по вертикалі
                    for (int k = 0; k < (int)numericUpDown2.Value; k++)
                    {
                        // цикл відповідає за рух з кінця (зправа наліво)
                        for (int l = (int)numericUpDown1.Value-2; l >= 0; l--)
                        {
                            // перевірка чи клітинка зайнята
                            if (field[k, l] == 1)
                            {
                                // цикл відповідає за рух до кінця поля, або зайнятої клітинки(зліва направо)
                                for (int j = l + 1; j < (int)numericUpDown1.Value; j++)
                                {
                                    // перевірка чи плитка зправа зайнята чи ні
                                    if (field[k, j] == 0)
                                    {
                                  
                                        picMove = true;
                                        field[k, j - 1] = 0;
                                        field[k, j] = 1;
                                        pics[k, j] = pics[k, j - 1];
                                        pics[k, j - 1] = null;
                                        labels[k, j] = labels[k, j - 1];
                                        labels[k, j - 1] = null;
                                        pics[k, j].Location = new Point(pics[k, j].Location.X + 56, pics[k, j].Location.Y);
                                     
                                    }
                                    else
                                    {
                                        int a = int.Parse(labels[k, j].Text);
                                        int b = int.Parse(labels[k, j - 1].Text);
                                        // якщо значення лівої та правої плитки рівні, плитка зліва видаляється,
                                        // а плитка зправа набуває суми значень двох плиток.
                                        if (a == b)
                                        {
                                            picMove = true;
                                            labels[k, j].Text = (a + b).ToString();
                                            score += (a + b);
                                            ChangeColor(a + b, k, j);
                                            score1.Text = "Score: " + score;
                                            field[k, j - 1] = 0;
                                            form2.Controls.Remove(pics[k, j - 1]);
                                            form2.Controls.Remove(labels[k, j - 1]);
                                            pics[k, j - 1] = null;
                                            labels[k, j - 1] = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "Left":
                    turn.Text = "Turn: Left ←";
                    for (int k = 0; k < (int)numericUpDown2.Value; k++)
                    {
                        for (int l = 1; l < (int)numericUpDown1.Value; l++)
                        {
                            if (field[k, l] == 1)
                            {
                                for (int j = l - 1; j >= 0; j--)
                                {
                                    if (field[k, j] == 0)
                                    {
                                        picMove = true;
                                        field[k, j + 1] = 0;
                                        field[k, j] = 1;
                                        pics[k, j] = pics[k, j + 1];
                                        pics[k, j + 1] = null;
                                        labels[k, j] = labels[k, j + 1];
                                        labels[k, j + 1] = null;
                                        pics[k, j].Location = new Point(pics[k, j].Location.X - 56, pics[k, j].Location.Y);
                                    }
                                    else
                                    {
                                        int a = int.Parse(labels[k, j].Text);
                                        int b = int.Parse(labels[k, j + 1].Text);
                                        if (a == b)
                                        {
                                            picMove = true;
                                            labels[k, j].Text = (a + b).ToString();
                                            score += (a + b);
                                            ChangeColor(a + b, k, j);
                                            score1.Text = "Score: " + score;
                                            field[k, j + 1] = 0;
                                            form2.Controls.Remove(pics[k, j + 1]);
                                            form2.Controls.Remove(labels[k, j + 1]);
                                            pics[k, j + 1] = null;
                                            labels[k, j + 1] = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "Down":
                    turn.Text =  "Turn: Down ↓"; 
                    for (int k = (int)numericUpDown2.Value-2; k >= 0; k--)
                    {
                        for (int l = 0; l < (int)numericUpDown1.Value; l++)
                        {
                            if (field[k, l] == 1)
                            {
                                for (int j = k + 1; j < (int)numericUpDown2.Value; j++)
                                {
                                    if (field[j, l] == 0)
                                    {
                                        picMove = true;
                                        field[j - 1, l] = 0;
                                        field[j, l] = 1;
                                        pics[j, l] = pics[j - 1, l];
                                        pics[j - 1, l] = null;
                                        labels[j, l] = labels[j - 1, l];
                                        labels[j - 1, l] = null;
                                        pics[j, l].Location = new Point(pics[j, l].Location.X, pics[j, l].Location.Y + 56);
                                    }
                                    else
                                    {
                                        int a = int.Parse(labels[j, l].Text);
                                        int b = int.Parse(labels[j - 1, l].Text);
                                        if (a == b)
                                        {
                                            picMove = true;
                                            labels[j, l].Text = (a + b).ToString();
                                            score += (a + b);
                                            ChangeColor(a + b, j, l);
                                            score1.Text = "Score: " + score;
                                            field[j - 1, l] = 0;
                                            form2.Controls.Remove(pics[j - 1, l]);
                                            form2.Controls.Remove(labels[j - 1, l]);
                                            pics[j - 1, l] = null;
                                            labels[j - 1, l] = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "Up":
                    turn.Text = "Turn: Up ↑";
                    for (int k = 1; k < (int)numericUpDown2.Value; k++)
                    {
                        for (int l = 0; l < (int)numericUpDown1.Value; l++)
                        {
                            if (field[k, l] == 1)
                            {
                                for (int j = k - 1; j >= 0; j--)
                                {
                                    if (field[j, l] == 0)
                                    {
                                        picMove = true;
                                        field[j + 1, l] = 0;
                                        field[j, l] = 1;
                                        pics[j, l] = pics[j + 1, l];
                                        pics[j + 1, l] = null;
                                        labels[j, l] = labels[j + 1, l];
                                        labels[j + 1, l] = null;
                                        pics[j, l].Location = new Point(pics[j, l].Location.X, pics[j, l].Location.Y - 56);
                                    }
                                    else
                                    {
                                        int a = int.Parse(labels[j, l].Text);
                                        int b = int.Parse(labels[j + 1, l].Text);
                                        if (a == b)
                                        {
                                            picMove = true;
                                            labels[j, l].Text = (a + b).ToString();
                                            score += (a + b);
                                            ChangeColor(a + b, j, l);
                                            score1.Text = "Score: " + score;
                                            field[j + 1, l] = 0;
                                            form2.Controls.Remove(pics[j + 1, l]);
                                            form2.Controls.Remove(labels[j + 1, l]);
                                            pics[j + 1, l] = null;
                                            labels[j + 1, l] = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;

            }
            if (picMove)
            {
                temp = 0;
                GenerateNewPic();
            }
            else
                temp++;
        }    
        
    }
}
