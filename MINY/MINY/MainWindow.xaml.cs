using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MINY
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int clickcounter;
        string[,] gf = new string[10, 10];
        Grid DynamicGrid = new Grid();
        int fieldsize = 10;
        public MainWindow()
        {
            InitializeComponent();

            //První kliknutí nikdy není bomba

            CreateGridTable();
            FillGrid();
            //GenerateGameField();


        }

        public void CreateGridTable()
        {
            int columncount = 10;
            int rowcount = 10;


            DynamicGrid.Width = 400;
            DynamicGrid.HorizontalAlignment = HorizontalAlignment.Center;
            DynamicGrid.VerticalAlignment = VerticalAlignment.Top;
            //DynamicGrid.ShowGridLines = true;
            DynamicGrid.Background = new SolidColorBrush(Colors.LightGray);

            //Sloupce
            for (int x = 0; x < columncount; x++)
            {
                ColumnDefinition gridColx = new ColumnDefinition();
                DynamicGrid.ColumnDefinitions.Add(gridColx);
            }
            //Řádky

            for (int y = 0; y < rowcount; y++)
            {
                RowDefinition gridRowx = new RowDefinition();
                gridRowx.Height = new GridLength(20);
                DynamicGrid.RowDefinitions.Add(gridRowx);
            }
        }

        //Create clickable grid
        public void FillGrid()
        {

            int bombcount = 10;
            int fieldsize = 10;

            Random r = new Random();

            for (int i = 0; i < bombcount; i++)
            {
                int randX = r.Next(0, 9);
                int randY = r.Next(0, 9);

                gf[randX, randY] = "B";
            }

            for (int c = 0; c < fieldsize; c++)
            {

                for (int v = 0; v < fieldsize; v++)
                {
                    int bombsaround = 0;
                    if (gf[c, v] == "B")
                    {
                        //break 
                    }
                    else
                    {
                        if (c - 1 < fieldsize - 1 && v - 1 < fieldsize - 1 && c - 1 >= 0 && v - 1 >= 0) { if (gf[c - 1, v - 1] == "B") { bombsaround++; } }
                        if (c - 1 < fieldsize - 1 && v < fieldsize - 1 && c - 1 >= 0 && v >= 0) { if (gf[c - 1, v] == "B") { bombsaround++; } }
                        if (c - 1 < fieldsize - 1 && v + 1 < fieldsize - 1 && c - 1 >= 0 && v + 1 >= 0) { if (gf[c - 1, v + 1] == "B") { bombsaround++; } }
                        if (c < fieldsize - 1 && v - 1 < fieldsize - 1 && c >= 0 && v - 1 >= 0) { if (gf[c, v - 1] == "B") { bombsaround++; } }
                        if (c < fieldsize - 1 && v + 1 < fieldsize - 1 && c >= 0 && v + 1 >= 0) { if (gf[c, v + 1] == "B") { bombsaround++; } }
                        if (c + 1 < fieldsize - 1 && v - 1 < fieldsize - 1 && c + 1 >= 0 && v - 1 >= 0) { if (gf[c + 1, v - 1] == "B") { bombsaround++; } }
                        if (c + 1 < fieldsize - 1 && v < fieldsize - 1 && c + 1 >= 0 && v >= 0) { if (gf[c + 1, v] == "B") { bombsaround++; } }
                        if (c + 1 < fieldsize - 1 && v + 1 < fieldsize - 1 && c + 1 >= 0 && v + 1 >= 0) { if (gf[c + 1, v + 1] == "B") { bombsaround++; } }
                        gf[c, v] = bombsaround.ToString();
                    }

                }
            }


            for (int c = 0; c < 10; c++)
            {
                for (int v = 0; v < 10; v++)
                {
                    Button btn = new Button();
                    btn.FontSize = 10;
                    btn.Click += new RoutedEventHandler(ClickHandler);
                    btn.FontWeight = FontWeights.Bold;
                    if (gf[c, v] == "1")
                    {
                        btn.Foreground = Brushes.Blue;
                    } else if (gf[c, v] == "2")
                    {
                        btn.Foreground = Brushes.Green;
                    } else if (gf[c, v] == "3")
                    {
                        btn.Foreground = Brushes.Red;
                    }
                    else if (gf[c, v] == "4")
                    {
                        btn.Foreground = Brushes.DarkRed;
                    }
                    else if (gf[c, v] == "5")
                    {
                        btn.Foreground = Brushes.DarkBlue;
                    }


                    // btn.Content = gf[c, v];


                    Grid.SetRow(btn, c);
                    Grid.SetColumn(btn, v);

                    DynamicGrid.Children.Add(btn);
                }

            }

            //add all to label
            contenthere.Content = DynamicGrid;

        }
        private void ClickHandler(object sender, EventArgs e)
        {
            Button srcbtn = sender as Button;
            int X = Grid.GetRow((Button)sender);
            int Y = Grid.GetColumn((Button)sender);

            string clickedInArray = gf[X, Y];
            srcbtn.Content = clickedInArray;

            if (clickedInArray == "B")
            {
                //game over
                MessageBox.Show("Prohráli jste! °n° ");
                clickcounter = -1;
                DynamicGrid.Children.Clear();
                FillGrid();
            }
            /*
            if (clickedInArray == "0")
            {

                for (int c = 0; c < fieldsize; c++)
                {
                    for (int v = 0; v < fieldsize; v++)
                    {
                        if(gf[c,v] == "0")
                        {
                            
                        }
                    }
                }
            }*/
            clickcounter++;
            moves.Content = clickcounter;

        }
        //New Game
        private void Button_Click(object sender, RoutedEventArgs e)
        {
        clickcounter = 0;
        moves.Content = clickcounter;
        FillGrid();
        }
    }
}
