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
using System.Windows.Threading;

namespace MINY
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int clickcounter;
        private int increment = 0;
        Grid DynamicGrid = new Grid();
        int fieldsize;
        string[,] gf = new string[50, 50];
        int flagcounter;

        
        public MainWindow()
        {
            InitializeComponent();
            fieldsize = 15;
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
            for (int i = 0; i < 8; i++)
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
            // Clear field, in case of new game
            for (int c = 0; c < fieldsize; c++)
            {
                for (int v = 0; v < fieldsize; v++)
                {
                    gf[c, v] = "0";
                }
            }
            
            Random r = new Random();
            //Place bombs
            for (int i = 0; i < bombcount + 1; i++)
            {
                int randX = r.Next(0, fieldsize - 1);
                int randY = r.Next(0, fieldsize - 1);

                gf[randX, randY] = "💣";
            }
            //Check if there are bombs around => add up 
            for (int c = 0; c < fieldsize; c++)
            {
                for (int v = 0; v < fieldsize; v++)
                {
                    int bombsaround = 0;
                    if (gf[c, v] == "💣")
                    {
                        //break 
                    }
                    else
                    {
                        if (c - 1 < fieldsize - 1 && v - 1 < fieldsize - 1 && c - 1 >= 0 && v - 1 >= 0) { if (gf[c - 1, v - 1] == "💣") { bombsaround++; } }
                        if (c - 1 < fieldsize - 1 && v < fieldsize - 1 && c - 1 >= 0 && v >= 0) { if (gf[c - 1, v] == "💣") { bombsaround++; } }
                        if (c - 1 < fieldsize - 1 && v + 1 < fieldsize - 1 && c - 1 >= 0 && v + 1 >= 0) { if (gf[c - 1, v + 1] == "💣") { bombsaround++; } }
                        if (c < fieldsize - 1 && v - 1 < fieldsize - 1 && c >= 0 && v - 1 >= 0) { if (gf[c, v - 1] == "💣") { bombsaround++; } }
                        if (c < fieldsize - 1 && v + 1 < fieldsize - 1 && c >= 0 && v + 1 >= 0) { if (gf[c, v + 1] == "💣") { bombsaround++; } }
                        if (c + 1 < fieldsize - 1 && v - 1 < fieldsize - 1 && c + 1 >= 0 && v - 1 >= 0) { if (gf[c + 1, v - 1] == "💣") { bombsaround++; } }
                        if (c + 1 < fieldsize - 1 && v < fieldsize - 1 && c + 1 >= 0 && v >= 0) { if (gf[c + 1, v] == "💣") { bombsaround++; } }
                        if (c + 1 < fieldsize - 1 && v + 1 < fieldsize - 1 && c + 1 >= 0 && v + 1 >= 0) { if (gf[c + 1, v + 1] == "💣") { bombsaround++; } }
                        gf[c, v] = bombsaround.ToString();
                    }
                }
            }
            //generate buttons          
            for (int c = 0; c < fieldsize; c++)
            {
                for (int v = 0; v < fieldsize; v++)
                {
                    Button btn = new Button();
                    btn.FontSize = 10;
                    //btn.Click += new RoutedEventHandler(ClickHandler);
                    btn.PreviewMouseDown += ClickHandlerTwo;
                    btn.FontWeight = FontWeights.Bold;
                   //btn.Foreground = Brushes.Red;
                    Grid.SetRow(btn, c);
                    Grid.SetColumn(btn, v);
                    DynamicGrid.Children.Add(btn);
                }

            }
            //Add all to label
            contenthere.Content = DynamicGrid;

        }
        //New game, same field size
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            increment = 0;
            flagcounter = 0;
            clickcounter = 0;
            moves.Content = clickcounter;
            FillGrid(fieldsize);
        }
        //New game, different field size
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            increment = 0;
            clickcounter = 0;
            flagcounter = 0;
            fieldsize = (Int32)fieldsizecb.SelectedItem;
            CreateGridTable(fieldsize);
            FillGrid(fieldsize);

        }
        //CHECK if bomb buttons were clicked
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            bool taggedall = true;
            for (int c = 0; c < fieldsize; c++)
            {
                for (int v = 0; v < fieldsize; v++)
                {
                    if (gf[c, v] == "💣")
                    {
                        //get button from grid other than clicked
                       var rr = (Button)DynamicGrid.Children.Cast<UIElement>().First(m => Grid.GetRow(m) == c && Grid.GetColumn(m) == v);
                       if(rr.Content == "🏴")
                        {

                        }
                        else
                        {
                            taggedall = false;
                        }


                    }
                }
            }

            if (taggedall == false)
            {
                MessageBox.Show("YOU LOSE");
            }else if(taggedall == true)
            {
                MessageBox.Show("YOU WIN ☺ YOUR TIME WAS " + increment);
                int finaltime = increment;
            }
        }
        //Button clicked
        private void ClickHandlerTwo(object sender, MouseButtonEventArgs t)
        {
            Button srcbtn = sender as Button;
            int X = Grid.GetRow((Button)sender);
            int Y = Grid.GetColumn((Button)sender);


            //If user tagged bomb ::
            if (t.RightButton == MouseButtonState.Pressed)
            {
                
                //if bomb is already flagged, put the flag away
                if (srcbtn.Content == "🏴")
                {
                    srcbtn.Content = "";
                    flagcounter = flagcounter - 1;
                }
                else
                {
                    if (flagcounter > fieldsize)
                    {
                        MessageBox.Show("No more flags to put");
                    }
                    else
                    {
                        Debug.WriteLine(gf[X, Y]);
                        srcbtn.Content = "🏴";
                        flagcounter++;

                    }
                }
            }
            //If user casually clicked
            else if (t.LeftButton == MouseButtonState.Pressed)
            {

                string clickedInArray = gf[X, Y];
                srcbtn.Content = clickedInArray;
                
                srcbtn.IsEnabled = false;
                
                //If user clicked on bomb
                if (clickedInArray == "💣")
                {
                    //game over
                    //show whole field 
                    for (int c = 0; c < fieldsize; c++)
                    {
                        for (int v = 0; v < fieldsize; v++)
                        {
                            if (gf[c, v] == "💣")
                            {
                                //get button from grid other than clicked
                                var rr = (Button)DynamicGrid.Children.Cast<UIElement>().First(m => Grid.GetRow(m) == c && Grid.GetColumn(m) == v);
                                rr.Content = "💣";
                                rr.Foreground = Brushes.Red;
                            }
                        }
                    }

                    MessageBox.Show("Prohráli jste! °n° ");

                    increment = 0;
                    flagcounter = 0;
                    clickcounter = 0;
                    moves.Content = clickcounter;
                    clickcounter = -1;
                    DynamicGrid.Children.Clear();
                    FillGrid(fieldsize);
                }
                //If nothing was there
                if (clickedInArray == "0")
                {
                    for (int c = 0; c < fieldsize; c++)
                    {
                        for (int v = 0; v < fieldsize; v++)
                        {
                            if (gf[c, v] == "0")
                            {
                                //get button from grid other than clicked
                                var rr = (Button)DynamicGrid.Children.Cast<UIElement>().First(m => Grid.GetRow(m) == c && Grid.GetColumn(m) == v);
                                rr.Content = "0";
                                rr.IsEnabled = false;
                            }
                        }
                    }
                }
                //click counter add
                clickcounter++;
                moves.Content = clickcounter;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick+= dtTicker;
            dt.Start();
            

        }
        private void dtTicker(object sender, EventArgs e)
        {
            //if game won, increment stop
            increment++;
            timer.Content = increment.ToString();
        }
        //get values of element in the field
        //var rr = (Button)DynamicGrid.Children.Cast<UIElement>().First(m => Grid.GetRow(m) == c && Grid.GetColumn(m) == v);

    }
}

