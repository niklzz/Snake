using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            new Settings(); //вызываем конструктор с настройками по дефолту

            gameTimer.Interval = 1000/Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            StartGame();
        }


        private void StartGame()
        {
            labelGameOver.Visible = false;
            new Settings();


            Snake.Clear();
            Circle head  = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);


            labelScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width/Settings.Width;
            int maxYPos = pbCanvas.Size.Height/Settings.Height;

            Random randomValue = new Random();
            food = new Circle();
            food.X = randomValue.Next(0, maxXPos);
            food.Y = randomValue.Next(0, maxYPos);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver == true)
            {
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if(Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }
            pbCanvas.Invalidate();
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canva = e.Graphics;

            if (!Settings.GameOver )
            {

                Brush snakeColor;

                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                        snakeColor = Brushes.Black;
                    else
                        snakeColor = Brushes.Green;

                    canva.FillEllipse(snakeColor,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width,Settings.Height));


                    canva.FillEllipse(Brushes.Red,
                        new Rectangle(food.X*Settings.Width,
                        food.Y*Settings.Height,Settings.Width,Settings.Height));

                }
            }
            else
            {
                string gameOver = "Игра окончена \n Набранные Вами очки : " + Settings.Score + "\n Нажмите Enter что бы попробовать снова";
                labelGameOver.Visible = true;
                labelGameOver.Text = gameOver;
            }
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

                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    //проверяем выход за границы поля
                    if(Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }

                    //проверяем столкновение со своим телом
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X &&
                            Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    //поедание
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
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

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void Eat()
        {
            Circle food = new Circle();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;

            Snake.Add(food);

            Settings.Score += Settings.Points;
            Settings.Speed += 1;
            labelScore.Text = Settings.Score.ToString();
            gameTimer.Interval = 1000 / Settings.Speed;

            GenerateFood();

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode,true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
    }
}
