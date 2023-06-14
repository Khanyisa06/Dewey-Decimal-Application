using LibraryTrainer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace DeweyDecimalSystem
{
    /// <summary>
     
    /// </summary>
    public partial class Game3Manu : Page
    {
        public Game3Manu()
        {
            InitializeComponent();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Game3 game3 = new Game3();
            NavigationService.Navigate(game3);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            // txt File Path
            string filePath = @"deweysystem.txt";

            // List to store Text                                                        
            List<string> lines = new List<string>();

            // read the lines from the text
            lines = File.ReadAllLines(filePath).ToList();



            foreach (string line in lines)
            {
                // display txt in a listbox
                Listbox.Items.Add(line);

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainMenu Back = new MainMenu();

            NavigationService.Navigate(Back);
        }
    }
    }
    

