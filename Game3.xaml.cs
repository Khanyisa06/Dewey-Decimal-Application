using DeweyDecimalSystem;
using System;
using System.Collections.Generic;
using System.IO;
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


    //  REFERENCES

    // VARIABLE DECLARATION  Troelsen, A. and Japikse, P. 2017. Pro c#7 with .NET and .NET Core. 8th ed. New York: Apress.

    //Random class (Microsoft, 2022) Available at: https://learn.microsoft.com/en-us/dotnet/api/system.random?view=net-6.0
    //Tree data structure (Kini, 2020) Available at: https://www.c-sharpcorner.com/article/tree-data-structure/
    //List class (Chand, 2023) Available at: https://www.c-sharpcorner.com/article/c-sharp-list/
    //Full File Path (Chand, 2018) Available a:  https://www.c-sharpcorner.com/UploadFile/mahesh/how-to-get-full-path-of-a-file-in-C-Sharp/
    //What is WPF? (Chand,2019) Available at: https://www.c-sharpcorner.com/blogs/what-wpf-is1

    //
    public partial class Game3 : Page
    {
         
        //DECLARE VARIABLES                                                          // Troelsen, and Japikse ( 2017)
        static int score = 0;
        public static Random random = new Random();                                     // (Microsoft, 2022) 
        static TreeNode<Dewey> root = new TreeNode<Dewey>(new Dewey("root", "root"));    // (Kini, 2020)

        public static int GameMode = 0;

        static List<Dewey> QA = new List<Dewey>();                                       //(Chand, 2023)
        static List<int> QAindexes = new List<int>();

        public static int selectedItem;

        public Game3()
        {
            InitializeComponent();
            btnEnd.IsEnabled = false;
            rbOne.Visibility = Visibility.Hidden;
            rbTwo.Visibility = Visibility.Hidden;
            rbThr.Visibility = Visibility.Hidden;
            rbFou.Visibility = Visibility.Hidden;

            //initialises tree from file data
            InitTree();

        }

        //starts new round/game
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnEnd.IsEnabled = false;

            // calculates and displays possible answers
            CalcQA();
            PopAnswers();

            btnEnd.IsEnabled = true;


        }

        //finishes round on button click
        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            if (rbOne.IsChecked == true || rbTwo.IsChecked == true || rbThr.IsChecked == true || rbFou.IsChecked == true)
            {
                CheckAnswer();
            } else
            {
                MessageBox.Show("Please Select an Answer");
            }

            
        }

        // Checks user answer
        private void CheckAnswer()
        {
            if (GameMode == 0)
            {
                if (selectedItem == QAindexes[0])
                {
                    score += 10;
                    MessageBox.Show("CORRECT ITEM SELECTED.  Moving on to the 2ND LEVEL ITEM");
                    GameMode = 1;
                    lblScore.Content = "Concurrent score:  " + score;
                    PopAnswers();
                } else
                {
                    MessageBox.Show(" INCORRECT ITEM SELECTED  Press 'Ok' to start new game");
                    

                    GameMode = 0;
                    score = 0;
                    lblScore.Content = "Final score:  " + score;
                    NewRound();
                }

                
            } else if (GameMode == 1)
            {
                if (selectedItem == QAindexes[0])
                {
                    score += 10;
                    MessageBox.Show("CORRECT ITEM SELECTED Moving on to the THIRD LEVEL ITEM");
                    GameMode = 2;
                    lblScore.Content = "Final score:  " + score;
                    PopAnswers();
                }
                else
                {
                    GameMode = 0;
                    score = 0;
                    MessageBox.Show("CORRECT ITEM SELECTED Press 'Ok' to start new game");
                    lblScore.Content = "Final score:  " + score;
                    NewRound();
                }
    
            } else if (GameMode == 2)
            {
                if (selectedItem == QAindexes[0])
                {
                    score += 10;
                    MessageBox.Show(" CORRECT ITEM SELECTED. Game completed");
                    MessageBox.Show(" Thank you for completing the Game"+ "\n" + "Start a new Game");
                    GameMode = 0;
                    lblScore.Content = "Final score:  " + score;
                    NewRound();
                }
                else
                {
                    GameMode = 0;
                    score = 0;
                    MessageBox.Show("INCORRECT ITEM SELECTED Press 'Ok' to start new game");
                    lblScore.Content = "Concurrent score:  " + score;
                    NewRound();
                }
            }

        }

        //determines the correct answer and question base for the game
        public void CalcQA()
        {
            QA.Clear();
            QAindexes.Clear();
            int lvl1i = GetRandomNumber(0, root.GetCount());
            TreeNode<Dewey> lvl1 = root[lvl1i];
            QA.Add(lvl1.Value);
            QAindexes.Add(lvl1i);

            int lvl2i = GetRandomNumber(0, lvl1.GetCount());
            TreeNode<Dewey> lvl2 = lvl1[lvl2i];
            QA.Add(lvl2.Value);

            int lvl3i = GetRandomNumber(0, lvl2.GetCount());
            TreeNode<Dewey> lvl3 = lvl2[lvl3i];
            QA.Add(lvl3.Value);

            lblQ.Content = QA[2].callDesc;

        }

        // calculates possible answers based on the state of the game, and displays them in their respective radio buttons
        public void PopAnswers()
        {
            List<int> indexes = new List<int>();
            List<Dewey> ans = new List<Dewey>();
            List<TreeNode<Dewey>> sourceL1;

            if (GameMode == 0)
            {
                ans.Add(QA[0]);
                sourceL1 = root.Children;
                for (int i = 0; i < 3; i++)
                {
                    int index = GetRandomNumber(0, sourceL1.Count());
                    if (index == QAindexes[0])
                    {
                        i--;
                        continue;
                    }

                    if (!indexes.Contains(index)) { 
                        indexes.Add(index);
                        ans.Add(sourceL1[index].Value);
                    } 
                    else i--;
                }

                ans = ans.OrderBy(x => x.callNum).ToList();
                QAindexes[0] = ans.IndexOf(QA[0]);

                rbOne.Content = ans[0].ToString();
                rbTwo.Content = ans[1].ToString();
                rbThr.Content = ans[2].ToString();
                rbFou.Content = ans[3].ToString();

                rbOne.Visibility = Visibility.Visible;
                rbTwo.Visibility = Visibility.Visible;
                rbThr.Visibility = Visibility.Visible;
                rbFou.Visibility = Visibility.Visible;

            } else if (GameMode == 1)
            {
                List<string> temp = new List<string>();
                List<TreeNode<Dewey>> sourceL2 = new List<TreeNode<Dewey>>();
                List<Dewey> sources = new List<Dewey>();
                ans.Add(QA[1]);
                temp.Add(QA[1].callNum);
                sourceL1 = root.Children;
                foreach (var child in sourceL1)
                {
                    sourceL2.AddRange(child.Children.ToList());

                }
                foreach(var item in sourceL2)
                {
                    sources.Add(item.Value);
                }

                for (int i = 0; i < 3; i++)
                {
                    int index = GetRandomNumber(0, sources.Count());
                    Dewey dewey = sources[index];

                    if (temp.Contains(dewey.callNum))
                    {
                        i--;
                    } else if (!ans.Contains(dewey))
                    {
                        ans.Add(dewey);
                        temp.Add(dewey.callNum);
                    }

                }

                ans = ans.OrderBy(x => x.callNum).ToList();

                QAindexes[0] = ans.IndexOf(QA[1]);

                rbOne.Content = ans[0].ToString();
                rbTwo.Content = ans[1].ToString();
                rbThr.Content = ans[2].ToString();
                rbFou.Content = ans[3].ToString();

            }
            else if (GameMode == 2)
            {
                List<string> temp = new List<string>();
                ans.Add(QA[2]);
                temp.Add(QA[2].callNum);
                List<TreeNode<Dewey>> lvl1 = root.Children;
                List<TreeNode<Dewey>> lvl2 = new List<TreeNode<Dewey>>();
                List<TreeNode<Dewey>> lvl3 = new List<TreeNode<Dewey>>();
                
                foreach(var l1 in lvl1)
                {
                    lvl2.AddRange(l1.Children);
                }
                foreach(var l2 in lvl2)
                {
                    lvl3.AddRange(l2.Children);
                }
                for (int i = 0; i < 3;  i++)
                {
                    int index = GetRandomNumber(0, lvl3.Count());
                    Dewey dewey = lvl3[index].Value;
                    
                    if (temp.Contains(dewey.callNum))
                    {
                        i--;
                    } else if (!ans.Contains(dewey))
                    {
                        ans.Add(dewey);
                        temp.Add(dewey.callNum);
                    }
  
                    
                }
                // ASWERS ARE ORDERED BY CALL NUMBERS
                ans = ans.OrderBy(x => x.callNum).ToList();

                QAindexes[0] = ans.IndexOf(QA[2]);

                rbOne.Content = ans[0].callNum.ToString();
                rbTwo.Content = ans[1].callNum.ToString();
                rbThr.Content = ans[2].callNum.ToString();
                rbFou.Content = ans[3].callNum.ToString();
            }
        }

        // STARTING A NEW GAME
        public void NewRound()
        {
            CalcQA();
            PopAnswers();
        }


        //generic method to get randomised number within specified parameters
        public static int GetRandomNumber(int min, int max)
        {
            // (Microsoft, 2022)
            int d = random.Next(min, max);
            return d;
        }

        //Reads data from txt file to populate tree
        public void InitTree()
        {
            string dir = Environment.CurrentDirectory;
            string filePath = Directory.GetParent(dir).Parent.Parent.FullName + "\\deweysystem.txt";   // (Chand, 2018)
            string[] sysLines = File.ReadAllLines(filePath);

            List<String> tempL1 = new List<String>();
            List<String> tempL2 = new List<String>();

            foreach (string line in sysLines)
            {
                string[] objs = line.Split('/');

                Dewey call1 = splitString(objs[0]);
                TreeNode<Dewey> nodeLvl1;
                Console.WriteLine(call1.callNum + " " + call1.callDesc);

                if (root.Children.Count() == 0)
                {
                    Console.WriteLine("Node added becuase no children exist");
                    nodeLvl1 = root.AddChild(call1);
                    tempL1.Add(call1.callNum);

                }
                else
                {
                    if (tempL1.Contains(call1.callNum))
                    {
                        nodeLvl1 = FindNode(root, call1);
                        Console.WriteLine("Node found");
                    }
                    else
                    {
                        tempL1.Add(call1.callNum);
                        nodeLvl1 = root.AddChild(call1);
                        Console.WriteLine("Node added becuase does not exist");
                    }

                }

                Dewey call2 = splitString(objs[1]);
                TreeNode<Dewey> nodeLvl2;

                if (nodeLvl1 == null)
                {
                    nodeLvl1 = FindNode(root, call1);
                }

                if (!tempL2.Contains(call2.callNum))
                {
                    nodeLvl2 = nodeLvl1.AddChild(call2);
                    tempL2.Add(call2.callNum);
                }
                else
                {
                    nodeLvl2 = FindNode(nodeLvl1, call2);
                }

                Dewey call3 = splitString(objs[2]);
                TreeNode<Dewey> nodeLvl3;

                nodeLvl3 = nodeLvl2.AddChild(call3);
            }
        }
        //method to split string into call number and call description, returns new Dewey object
        public static Dewey splitString(string line)
        {
            string[] div = line.Split("-");

            return new Dewey(div[0], div[1]);
        }

        //method to find specific dewey object within a node
        public static TreeNode<Dewey> FindNode(TreeNode<Dewey> parent, Dewey search)
        {
            foreach (var node in parent.Children)
            {
                if (node.Value.callNum.Equals(search.callNum))
                    return node;
            }

            return null;
        }


        //basic radiobutton select activity                                                // (Chand,2019)
        private void rbOne_Checked(object sender, RoutedEventArgs e)
        {
            selectedItem = 0;
        }

        private void rbTwo_Checked(object sender, RoutedEventArgs e)
        {
            selectedItem = 1;
        }

        private void rbThr_Checked(object sender, RoutedEventArgs e)
        {
            selectedItem = 2;
        }

        private void rbFou_Checked(object sender, RoutedEventArgs e)
        {
            selectedItem = 3;
        }

        private void btnEnd_Copy_Click(object sender, RoutedEventArgs e)
        {
            Game3Manu game3manu = new Game3Manu();
            NavigationService.Navigate(game3manu);
        }
    }
}
