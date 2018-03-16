using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_Game
{
    public partial class Form1 : Form
    {        
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
            new Settings(); 
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();
            this.Focus();
            choose.Text = "Choose Level";
         
        }

        private void NoWallMode(int i)
        {
                    if (Snake[i].X > pbCanvas.Width / Settings.Width)

                        Snake[i].X = 0;

                    if (Snake[i].X < 0)

                        Snake[i].X = pbCanvas.Width / Settings.Width;

                    if (Snake[i].Y < 0)
                        Snake[i].Y = pbCanvas.Height / Settings.Height;

                    if (Snake[i].Y > pbCanvas.Height / Settings.Height)
                        Snake[i].Y = 0;

        }

        private void UpdateScreen(object sender, EventArgs e)
        {           
            if (Settings.GameOver)
            {
                if (Input.KeyPressed(Keys.Space))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;
                MovePlayer();
            }

            pbCanvas.Invalidate(); //check!
        }

        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {

                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;

                    }
                    //Detect Snake is Out of field
                    if (Settings.noWall)
                        NoWallMode(i);
                    else
                    {
                        if (Snake[i].X < 0 || Snake[i].X >= pbCanvas.Width / Settings.Width || Snake[i].Y < 0 || Snake[i].Y >= pbCanvas.Height / Settings.Height)
                            Die();
                    }
                    //Detect collision with body
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                            Die();
                    }
                    //Detect Collision with Food
                    if (Snake[i].X == food.X && Snake[i].Y == food.Y)
                    {
                        Eat();

                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }

            }
        }

        private void Eat()
        {
            Circle newCirc = new Circle();
            newCirc.X = Snake[Snake.Count-1].X;
            newCirc.Y = Snake[Snake.Count-1].Y;
            Snake.Add(newCirc);
            Settings.Score += Settings.Points;
            lbl1score.Text = Settings.Score.ToString();
            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
            this.Focus();
        }

        private void StartGame()
        {      
            lblGameOver.Visible = false;
            choose.Visible = false;
            //set settings to default
            new Settings();
            Snake.Clear();
            //Create new player object
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);
            lbl1score.Text = Settings.Score.ToString();
            GenerateFood();           
        }

        private void GenerateFood()
        {
            int maxPosX = pbCanvas.Size.Width/Settings.Width;
            int maxPosY = pbCanvas.Size.Height/Settings.Height;
            Random rd = new Random();
            food = new Circle();
            food.X = rd.Next(0, maxPosX-1);
            food.Y = rd.Next(0, maxPosY-1);
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Brush snakecolor;
            if (Settings.GameOver == false)
            {
                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                        snakecolor = Brushes.Red;
                    else
                        snakecolor = Brushes.Blue;
                    canvas.FillEllipse(snakecolor, new Rectangle(Snake[i].X * Settings.Width, Snake[i].Y * Settings.Height, Settings.Width, Settings.Height));

                    canvas.FillEllipse(Brushes.Black, new Rectangle(food.X * Settings.Width, food.Y * Settings.Height, Settings.Width, Settings.Height));
                }
            }
            else
            {
                this.Focus();
                lblGameOver.Visible = true;
                lblGameOver.Text = "Game Over. Total Score is " + Settings.Score + "\nPress Space to restart game";
            }
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);   
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
        private void speedToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void DisableAndStart()
        {
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            this.Focus();
            StartGame();

        }
        private void slowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slowToolStripMenuItem.Checked = true;
            normalToolStripMenuItem.Checked = false;
            fastToolStripMenuItem.Checked = false;
            insaneToolStripMenuItem.Checked = false;
            gameTimer.Interval = 120;
            DisableAndStart();
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slowToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = true;
            fastToolStripMenuItem.Checked = false;
            insaneToolStripMenuItem.Checked = false;
            gameTimer.Interval = 67;
            DisableAndStart();
        }

        private void fastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slowToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = false;
            fastToolStripMenuItem.Checked = true;
            insaneToolStripMenuItem.Checked = false;
            gameTimer.Interval = 35;
            DisableAndStart();
        }

        private void insaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slowToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = false;
            fastToolStripMenuItem.Checked = false;
            insaneToolStripMenuItem.Checked = true;
            gameTimer.Interval = 20;
            DisableAndStart();
        }
        private void button1_Click(object sender, EventArgs e)
        {     
            gameTimer.Interval = 120;
            DisableAndStart();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            gameTimer.Interval = 67;
            DisableAndStart();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            gameTimer.Interval = 35;
            DisableAndStart();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            gameTimer.Interval = 20;
            DisableAndStart();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableAndStart();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void noWallModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.noWall = true;
            noWallModeToolStripMenuItem.Checked = true;
            normalToolStripMenuItem1.Checked = false;
        }

        private void defaultToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Settings.noWall = false;
            noWallModeToolStripMenuItem.Checked = false;
            normalToolStripMenuItem1.Checked = true;
        }
    }   
}
