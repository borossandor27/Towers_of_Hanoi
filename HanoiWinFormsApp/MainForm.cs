using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Towers_of_Hanoi
{
    public partial class MainForm : Form
    {
        private Stack<int>[] towers = new Stack<int>[3];
        private List<(int disk, int from, int to)> moves = new List<(int, int, int)>();
        private int moveIndex = 0;
        private Timer animationTimer;
        private const int DiskHeight = 20;
        private int numDisks = 5;
            private Button startButtonField; // Renamed to avoid ambiguity

        public MainForm()
        {
            InitializeComponent();
            InitTowers();
            GenerateMoves(numDisks, 0, 1, 2);
            SetupTimer();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Hanoi tornyai";
            this.Size = new Size(600, 400);

            startButtonField = new Button // Simplified object initialization
            {
                Text = "Start",
                Location = new Point(10, 10)
            };
            startButtonField.Click += startButton_Click;
            this.Controls.Add(startButtonField);

            Label diskLabel = new Label
            {
                Text = "Korongok száma:",
                Location = new Point(100, 15),
                AutoSize = true
            };
            this.Controls.Add(diskLabel);

            NumericUpDown diskInput = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 10,
                Value = numDisks,
                Location = new Point(200, 10),
                Width = 50
            };
            diskInput.ValueChanged += (s, e) =>
            {
                numDisks = (int)diskInput.Value;
                InitTowers();
                moves.Clear();
                GenerateMoves(numDisks, 0, 1, 2);
                this.Invalidate();
            };
            this.Controls.Add(diskInput);
        }

        private void InitTowers()
        {
            for (int i = 0; i < 3; i++)
                towers[i] = new Stack<int>();

            for (int i = numDisks; i >= 1; i--)
                towers[0].Push(i);
        }

        private void GenerateMoves(int n, int from, int to, int aux)
        {
            if (n == 1)
                moves.Add((1, from, to));
            else
            {
                GenerateMoves(n - 1, from, aux, to);
                moves.Add((n, from, to));
                GenerateMoves(n - 1, aux, to, from);
            }
        }

        private void SetupTimer()
        {
            animationTimer = new Timer
            {
                Interval = 1000 // 1 second
            };
            animationTimer.Tick += (s, e) => PerformNextMove();
        }

        private void PerformNextMove()
        {
            if (moveIndex >= moves.Count)
            {
                animationTimer.Stop();
                return;
            }

            var move = moves[moveIndex++];
            if (towers[move.from].Peek() == move.disk)
                towers[move.from].Pop();
            towers[move.to].Push(move.disk);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            int width = this.ClientSize.Width / 3;
            int marginBottom = 70; // Larger margin for labels
            int baseY = this.ClientSize.Height - marginBottom;

            for (int t = 0; t < 3; t++)
            {
                // Tower labels
                string label = t == 0 ? "A" : t == 1 ? "B" : "C";
                g.DrawString(label, this.Font, Brushes.Black, t * width + width / 2 - 5, baseY + 30);

                // Tower line
                int poleX = t * width + width / 2;
                g.DrawLine(Pens.DarkGray, poleX, baseY - numDisks * DiskHeight, poleX, baseY);

                var tower = towers[t].ToArray();
                Array.Reverse(tower);
                for (int i = 0; i < tower.Length; i++)
                {
                    int disk = tower[i];
                    int diskWidth = 20 + disk * 20;
                    int x = t * width + (width - diskWidth) / 2;
                    int y = baseY - (i + 1) * DiskHeight;
                    g.FillRectangle(Brushes.Blue, x, y, diskWidth, DiskHeight);
                    g.DrawString(disk.ToString(), this.Font, Brushes.White, x + diskWidth / 2 - 6, y + 2);
                }
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            moveIndex = 0;
            InitTowers();
            animationTimer.Start();
        }
    }
}
