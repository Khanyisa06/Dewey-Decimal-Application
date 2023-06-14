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

namespace LibraryTrainer
{
    /// <summary>
      //  REFERENCES

    //General ListBox Tutorial (Chand M, 2021) Available at: https://www.c-sharpcorner.com/UploadFile/mahesh/listbox-in-wpf/
    //Quicksort Sorting Algorithm (CodeMaze, 2022) Available at: https://code-maze.com/csharp-quicksort-algorithm/
    //Random class (Microsoft, 2022) Available at: https://learn.microsoft.com/en-us/dotnet/api/system.random?view=net-6.0
    //ListBox drag and drop functionality  (Wiesław Šoltés, 2015) Available at: https://stackoverflow.com/questions/3350187/wpf-c-rearrange-items-in-listbox-via-drag-and-drop
    // RANDOM ATRING GENARATOR (dtb, 2009) Available at: https://stackoverflow.com/a/1344242



    /// </summary>
    public partial class Game2 : Page
    {
        private int GameMode = 0;

        public static int ConcScoreG2 = 0;

        public static Random random = new Random();
        Dictionary<int, string> _LibraryDic = new Dictionary<int, string>();
        Dictionary<int, string> _UserDic = new Dictionary<int, string>();

        //IList to display generated call numbers as strings, also main interaction point from drag and drop.
        private IList<String> _AList = new ObservableCollection<String>();

        public Game2()
        {
            InitializeComponent();

            //populates library dictionary on startup
            PopulateLibDic();

            //assigns Column listboxes to item sources, by default left column will display all top level call numbers and their descriptions
            lbxQList.ItemsSource = _UserDic.Keys;
            lbxQList.ItemsSource = _LibraryDic;
            lbxAList.ItemsSource = _AList;
            lbxAList.PreviewMouseMove += ListBox_PreviewMouseMove;

            //ListBox events setup (Wiesław S, 2015)
            var style = new Style(typeof(ListBoxItem));
            style.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));
            style.Setters.Add(
                new EventSetter(
                    ListBoxItem.PreviewMouseLeftButtonDownEvent,
                    new MouseButtonEventHandler(ListBoxItem_PreviewMouseLeftButtonDown)));
            style.Setters.Add(
                    new EventSetter(
                        ListBoxItem.DropEvent,
                        new DragEventHandler(ListBoxItem_Drop)));
            lbxAList.ItemContainerStyle = style;

            //disables end button on startup
            btnEnd.IsEnabled = false;
        
            
        }

        //Populates Library System
        private void PopulateLibDic()
        {
            _LibraryDic.Clear();
            _LibraryDic.Add(000, "GENERAL KNOWLEDGE");
            _LibraryDic.Add(100, "PHILOSOPHY & PSYCHOLOGY");
            _LibraryDic.Add(200, "RELIGION");
            _LibraryDic.Add(300, "SOCIAL SCIENCES");
            _LibraryDic.Add(400, "LANGUAGES");
            _LibraryDic.Add(500, "SCIENCE");
            _LibraryDic.Add(600, "TECHNOLOGY");
            _LibraryDic.Add(700, "ARTS & RECREATION");
            _LibraryDic.Add(800, "LITERATURE");
            _LibraryDic.Add(900, "HISTORY & GEOGRAPHY");
            Debug.WriteLine("dic added");
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            lbxQList.Visibility = Visibility.Visible;
            btnEnd.IsEnabled = true;
            Start();
        }

        //Basic method to restart the game
        private void Start()
        {
            PopulateQList();
            
        }

        //Checks the current game mode, calls methods to generate questions accordingly
        private void PopulateQList()
        {
            if (GameMode == 0)
            {
                CreateQAListsZero();
                GameMode = 1;
            } else
            {
                CreateQAListsOne();
                GameMode = 0;
            }
        }

        //Populates a list of questions and answers where descriptions are the questions in Column A and answers are call numbers in Column B
        private void CreateQAListsOne()
        {
            lbxAList.ItemsSource = null;
            lbxQList.ItemsSource = null;
            _AList.Clear();
            _UserDic.Clear();

            int i = 0;
            int j = 0;
            List<int> temp = new List<int>();
            temp.Clear();

            while (i < 4)
            {
                int res = GetRandomNumber(0, _LibraryDic.Count() - 1);

                if (!temp.Contains(res))
                {
                    temp.Add(res);
                    _UserDic.Add(_LibraryDic.ElementAt(res).Key, _LibraryDic.ElementAt(res).Value);
                    i++;
                }
            }


            while (j < 3)
            {
                int res = GetRandomNumber(0, _LibraryDic.Count() - 1);

                if (!temp.Contains(res))
                {
                    temp.Add(res);
                    j++;
                }

            }

            int t = 0;
            while (t < 7)
            {
                if (temp.Count() > 0)
                {
                    int randomIndex = GetRandomNumber(0, temp.Count() - 1);
                    int val = temp[randomIndex];


                    _AList.Add(_LibraryDic.ElementAt(val).Key.ToString());
                    temp.RemoveAt(randomIndex);
                }
                t++;

            }

            lbxAList.ItemsSource = _AList;
            lbxQList.ItemsSource = _UserDic.Values;
        }

        //Populates a list of questions and answers where call numbers are the questions in Column A and answers are descriptions in Column B
        private void CreateQAListsZero()
        {
            lbxAList.ItemsSource = null;
            lbxQList.ItemsSource = null;
            _AList.Clear();
            _UserDic.Clear();
            
            int i = 0;
            int j = 0;
            List<int> temp = new List<int>();
            temp.Clear();

            while (i < 4)
            {
                int res = GetRandomNumber(0, _LibraryDic.Count() - 1);

                if (!temp.Contains(res))
                {
                    temp.Add(res);
                    _UserDic.Add(_LibraryDic.ElementAt(res).Key, _LibraryDic.ElementAt(res).Value);
                    i++;
                }
            }
          

            while (j < 3)
            {
                int res = GetRandomNumber(0, _LibraryDic.Count() - 1);

                if (!temp.Contains(res))
                {
                    temp.Add(res);
                    j++;
                }
                
            }

            int t = 0;
            while ( t < 7)
            {
                if (temp.Count() > 0)
                {
                    int randomIndex = GetRandomNumber(0, temp.Count() - 1);
                    int val = temp[randomIndex];
                    

                    _AList.Add(_LibraryDic.ElementAt(val).Value);
                    temp.RemoveAt(randomIndex);
                }
                t++;

            }

            lbxAList.ItemsSource = _AList;
            lbxQList.ItemsSource = _UserDic.Keys;
        }

        //calls methods to calculate score based on game mode, displays scores to the user, then restarts the game
        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            int score = 0;
            int score2 = 0;
            if (GameMode == 0)
            {
                score = CalcScoreZero();
            } else
            {
                score = CalcScoreOne();
            }

            

            score2 = score * 25;
            ConcScoreG2 += score2;
            lblScore.Content = "Score:  " + score2.ToString();
            lblConcScore.Content = "Concurrent Score:  " + ConcScoreG2.ToString();

            MessageBox.Show($"You got {score} items correct!\nYou have gained {score2} points!");
            Start();
        }
        //calculates score by checking if the user input matches the question's answers - if the answers are call numbers
        private int CalcScoreZero()
        {
           
            int ScoreCount = 0;

            for(int i = 0; i < _UserDic.Count; i++)
            {
                int key = _UserDic.ElementAt(i).Key;
                int ans = Int32.Parse(_AList.ElementAt(i));

                if (key == ans)
                {
                    ScoreCount++;
                }
            }
            return ScoreCount;
        }
        //calculates score by checking if the user input matches the question's answers - if the answers are descriptions
        private int CalcScoreOne()
        {
            int ScoreCount = 0;

            for (int i = 0; i < _UserDic.Count; i++)
            {
                string key = _UserDic.ElementAt(i).Value;

                if (key.Equals(_AList.ElementAt(i))) 
                {
                    ScoreCount++;
                }
            }
            return ScoreCount;
        }

        //generic method to get randomised number within specified parameters
        public static int GetRandomNumber(int min, int max)
        {
            // (Microsoft, 2022)
            int d = random.Next(min , max);
            return d;
        }


        
        
        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(null);
            Vector diff = _dragStartPoint - point;
            if (e.LeftButton == MouseButtonState.Pressed) //&& (Math.Abs(diff.X) > SystemParameters.MinimumVerticalDragDistance)
            {
                var lb = sender as ListBox;
                var lbi = FindVisualParent<ListBoxItem>(((DependencyObject)e.OriginalSource));
                if (lbi != null)
                {
                    DragDrop.DoDragDrop(lb, lbi.DataContext, DragDropEffects.Move);
                };
            }
        }
        //ListBox events setup (Wiesław S, 2015)
        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPoint = e.GetPosition(null);
        }
        //ListBox events setup (Wiesław S, 2015)
        private void ListBoxItem_Drop(object sender, DragEventArgs e)
        {
            if (sender is ListBoxItem)
            {
                var source = e.Data.GetData(DataFormats.Text).ToString();
                var target = ((ListBoxItem)(sender)).DataContext.ToString();

                int sourceIndex = lbxAList.Items.IndexOf(source);
                int targetIndex = lbxAList.Items.IndexOf(target);

                Move(source, sourceIndex, targetIndex);

            }
        }
        //ListBox events setup (Wiesław S, 2015)
        private void Move(string source, int srcI, int tgtI)
        {
            if (srcI < tgtI)
            {
                _AList.Insert(tgtI + 1, source);
                _AList.RemoveAt(srcI);
            }
            else
            {
                int removeI = srcI + 1;
                if (_AList.Count + 1 > removeI)
                {
                    _AList.Insert(tgtI, source);
                    _AList.RemoveAt(removeI);
                }
            }
        }

      
        private Point _dragStartPoint;
        private T FindVisualParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            var parentObj = VisualTreeHelper.GetParent(child);
            if (parentObj == null)
                return null;
            T parent = parentObj as T;
            if (parent != null)
                return parent;
            return FindVisualParent<T>(parentObj);
        }

        private void btnEnd_Copy_Click(object sender, RoutedEventArgs e)
        {
            MainMenu Back = new MainMenu();

            NavigationService.Navigate(Back);
        }
    }

}
