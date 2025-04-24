using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HanoiWPF
{
    public partial class MainWindow : Window
    {
        private Stack<int>[] towers = new Stack<int>[3];
        private List<(int disk, int from, int to)> moves = new List<(int, int, int)>();
        private int moveIndex = 0;
        private Timer timer;

        public MainWindow()
        {
            InitializeComponent();
            InitTowers();
            GenerateMoves((int)DiskSlider.Value, 0, 1, 2);
            DrawState();

            timer = new Timer(1000);
            timer.Elapsed += (s, e) => Dispatcher.Invoke(PerformNextMove);
        }

        private void InitTowers()
        {
            for (int i = 0; i < 3; i++)
                towers[i] = new Stack<int>();

            for (int i = (int)DiskSlider.Value; i >= 1; i--)
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

        private void PerformNextMove()
        {
            if (moveIndex >= moves.Count)
            {
                timer.Stop();
                return;
            }

            var move = moves[moveIndex++];
            if (towers[move.from].Peek() == move.disk)
                towers[move.from].Pop();
            towers[move.to].Push(move.disk);
            DrawState();
        }

        private void DrawState()
        {
            DrawArea.Children.Clear();
            double width = DrawArea.ActualWidth / 3;
            double baseY = DrawArea.ActualHeight - 50;
            int diskHeight = 20;

            for (int t = 0; t < 3; t++)
            {
                double centerX = t * width + width / 2;
                Line pole = new Line();
                pole.X1 = centerX;
                pole.X2 = centerX;
                pole.Y1 = baseY - ((int)DiskSlider.Value) * diskHeight;
                pole.Y2 = baseY;
                pole.Stroke = Brushes.Gray;
                pole.StrokeThickness = 4;
                DrawArea.Children.Add(pole);

                TextBlock label = new TextBlock();
                label.Text = t == 0 ? "A" : t == 1 ? "B" : "C";
                label.FontSize = 16;
                Canvas.SetLeft(label, centerX - 8);
                Canvas.SetTop(label, baseY + 10);
                DrawArea.Children.Add(label);

                var stack = towers[t].ToArray();
                Array.Reverse(stack);
                for (int i = 0; i < stack.Length; i++)
                {
                    int disk = stack[i];
                    double diskWidth = 20 + disk * 20;

                    Rectangle rect = new Rectangle();
                    rect.Width = diskWidth;
                    rect.Height = diskHeight;
                    rect.Fill = Brushes.Blue;
                    Canvas.SetLeft(rect, centerX - diskWidth / 2);
                    Canvas.SetTop(rect, baseY - (i + 1) * diskHeight);
                    DrawArea.Children.Add(rect);

                    TextBlock diskLabel = new TextBlock();
                    diskLabel.Text = disk.ToString();
                    diskLabel.Foreground = Brushes.White;
                    diskLabel.Width = diskWidth;
                    diskLabel.TextAlignment = TextAlignment.Center;
                    Canvas.SetLeft(diskLabel, centerX - diskWidth / 2);
                    Canvas.SetTop(diskLabel, baseY - (i + 1) * diskHeight + 2);
                    DrawArea.Children.Add(diskLabel);
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            moveIndex = 0;
            InitTowers();
            moves.Clear();
            GenerateMoves((int)DiskSlider.Value, 0, 1, 2);
            DrawState();
            timer.Start();
        }
    }
}
