using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace LibraryTrainer
{
     
      //  REFERENCES

    //General ListBox Tutorial (Chand M, 2021) Available at: https://www.c-sharpcorner.com/UploadFile/mahesh/listbox-in-wpf/
    //Quicksort Sorting Algorithm (CodeMaze, 2022) Available at: https://code-maze.com/csharp-quicksort-algorithm/
    //Random class (Microsoft, 2022) Available at: https://learn.microsoft.com/en-us/dotnet/api/system.random?view=net-6.0
    //ListBox drag and drop functionality  (Wiesław Šoltés, 2015) Available at: https://stackoverflow.com/questions/3350187/wpf-c-rearrange-items-in-listbox-via-drag-and-drop
    // RANDOM ATRING GENARATOR (dtb, 2009) Available at: https://stackoverflow.com/a/1344242

   
    public partial class Game1_Replace : Window
    {
        public int HighScore = 0;
        // (Microsoft, 2022)
        public static Random random = new Random();
        //IList to store correctly sorted answers
        private IList<Double> _BookList = new ObservableCollection<Double>();
        //IList to display generated call numbers as strings, also main interaction point from drag and drop.
        private IList<String> _UserList = new ObservableCollection<String>();
        //IList that contains player's final answers
        private IList<Double> _FinalList = new ObservableCollection<Double>();

        public Game1_Replace()
        {
            InitializeComponent();

            lbxSystemList.ItemsSource = _UserList;

            lbxSystemList.PreviewMouseMove += ListBox_PreviewMouseMove;

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
            lbxSystemList.ItemContainerStyle = style;

            //Basic UI Inits
            btnEnd.IsEnabled = false;
            lblScore.Visibility = Visibility.Hidden;
            lbxSystemList.Visibility = Visibility.Hidden;
            lblheader.Visibility = Visibility.Visible;

        }

        //Methods initialises ILists by randomly generating call numbers
        public void InitList()
        {
            _BookList.Clear();
            _UserList.Clear();
            for (int i = 0; i < 10; i++)
            {
                double callNum = GetRandomNumber(1000, 0);
                _BookList.Add(callNum);
                _UserList.Add(callNum.ToString());
            }
        }
        //ListBox events setup (Wiesław S, 2015)
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

                int sourceIndex = lbxSystemList.Items.IndexOf(source);
                int targetIndex = lbxSystemList.Items.IndexOf(target);

                Move(source, sourceIndex, targetIndex);

            }
        }
        //ListBox events setup (Wiesław S, 2015)
        private void Move(string source, int srcI, int tgtI)
        {
            if (srcI < tgtI)
            {
                _UserList.Insert(tgtI + 1, source);
                _UserList.RemoveAt(srcI);
            }
            else
            {
                int removeI = srcI + 1; 
                if (_UserList.Count + 1 > removeI)
                {
                    _UserList.Insert(tgtI, source);
                    _UserList.RemoveAt(removeI);
                }
            }
        }

                                                                  // (Wiesław Šoltés, 2015)  
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
            return FindVisualParent <T>(parentObj);
        }

        public static double GetRandomNumber(double min, double max)
        {
            // (Microsoft, 2022)
            double d = Math.Round( random.NextDouble() * (max - min) + min, 3);
            return d;
        }

        //Starts the game by making the list and making game components visible
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            InitList();
            btnEnd.IsEnabled = true;
            lblScore.Content = "Your score: ";
            lblScore.Visibility = Visibility.Visible;
            lbxSystemList.Visibility = Visibility.Visible;
            lblheader.Visibility = Visibility.Visible;

        }

        //Ends game, calculates & displays player's score
        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            //Sorts original list
            QuickSortList(_BookList, 0, _BookList.Count - 1);
            //Converts user's input into Double IList
            _FinalList.Clear();
            foreach (string s in _UserList)
            {
                _FinalList.Add(Double.Parse(s));
            }

            //calls method to compare lists and calculate score
            int score = CalcScore();
            lblScore.Content = "Your score: " + score +"/10";
            
            if (score > HighScore)
                HighScore = score;

            lblHighScore.Content = "High Score: " + HighScore;
            btnEnd.IsEnabled = false;
        }

        //Compares user input to sorted list, calculates score
        public int CalcScore()
        {
            int score = 0;
            for (int i = 0; i < _BookList.Count; i++)
            {
                if (_BookList[i].Equals(_FinalList[i]))
                {
                    score++;
                }
            }

            return score;
        }

        //Quicksort algorithm, modified from source to accomodate IList (CodeMaze, 2022).
        public IList<double> QuickSortList(IList<double> list, int lIndex, int rIndex)
        {
            var l = lIndex;
            var r = rIndex;
            var pivot = list[lIndex];

            while (l <= r)
            {
                while (list[l] < pivot)
                {
                    l++;
                }

                while (list[r] > pivot)
                {
                    r--;
                }

                if (l <= r)
                {
                    double temp = list[l];
                    list[l] = list[r];
                    list[r] = temp;
                    l++;
                    r--;
                }
            }

            if (lIndex < r)
            {
                QuickSortList(list, lIndex, r);
            }

            if (l < rIndex)
            {
                QuickSortList(list, l, rIndex);
            }

            return list;
        }
    }
}
