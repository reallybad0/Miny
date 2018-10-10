using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
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
        
        Grid DynamicGrid = new Grid();
        int fieldsize = 15;
        string[,] gf = new string[50,50];

        public MainWindow()
        {
            InitializeComponent();

            //První kliknutí nikdy není bomba

            CreateGridTable(fieldsize);
            FillGrid(fieldsize);
            FillCB();
            //GenerateGameField();

        }
        //Fill Combobox
        public void FillCB()
        {
            int starter = 5;
            for(int i = 0; i < 8; i++)
            {
                fieldsizecb.Items.Add(starter);
                starter = starter + 5;
            }
            fieldsizecb.SelectedIndex = 0;
        }
        
        //Create grid table
        public void CreateGridTable(int fieldsize)
        {
                        
            DynamicGrid = new Grid();

            int columncount = fieldsize;
            int rowcount = fieldsize;

            DynamicGrid.Width = 400;
            DynamicGrid.HorizontalAlignment = HorizontalAlignment.Center;
            DynamicGrid.VerticalAlignment = VerticalAlignment.Top;
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
        public void FillGrid(int fs)
        {
            int bombcount = fs;
            int fieldsize = fs;
            // ! ! ! !  !
            for (int c = 0; c < fieldsize; c++)
            {
                for (int v = 0; v < fieldsize; v++)
                {
                    gf[c, v] = "0";
                }
            }


            Random r = new Random();
            //Place bombs
            for (int i = 0; i < bombcount; i++)
            {
                int randX = r.Next(0, fieldsize-1);
                int randY = r.Next(0, fieldsize-1);

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


            for (int c = 0; c < fieldsize; c++)
            {
                for (int v = 0; v < fieldsize; v++)
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

            //Add all to label
            contenthere.Content = DynamicGrid;

        }
        //Button clicked
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
                FillGrid(fieldsize);
            }
            if (clickedInArray == "0")
            {
                for (int c = 0; c < fieldsize; c++)
                {
                    for (int v = 0; v < fieldsize; v++)
                    {
                        if(gf[c,v] == "0")
                        {
                            //get button from grid other than clicked
                            var rr = (Button)DynamicGrid.Children.Cast<UIElement>().First(m => Grid.GetRow(m) == c && Grid.GetColumn(m) == v);
                            rr.Content = "0";
                        }
                    }
                }
            }

            clickcounter++;
            moves.Content = clickcounter;


            //add CHECK button, checks if button is clicked
        }
        
        //New game, same field size
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clickcounter = 0;
            moves.Content = clickcounter;
            FillGrid(fieldsize);
        }
        //New game, different field size
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            clickcounter = 0;
            fieldsize = (Int32)fieldsizecb.SelectedItem;
            CreateGridTable(fieldsize);
            FillGrid(fieldsize);

        }
        //checks if bomb buttons were clicked
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            bool somethingclicked=false;
            for (int c = 0; c < fieldsize; c++)
            {
                for (int v = 0; v < fieldsize; v++)
                {
                    if (gf[c, v] == "B")
                    {
                        //get button from grid other than clicked
                        var rr = (Button)DynamicGrid.Children.Cast<UIElement>().First(m => Grid.GetRow(m) == c && Grid.GetColumn(m) == v);
                        //CHECK IF CLICKED
                        //nový pole s tím jeslti se na něj kliklo?
                        
                    }
                }
            }

            if (somethingclicked == false)
            {
                MessageBox.Show("YOU WIN");
            }
        }
    }
}
