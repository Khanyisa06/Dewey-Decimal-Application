using DeweyDecimalSystem;
using System;
using System.Collections.Generic;
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
    //Navigation Service (King King, 2014) https://stackoverflow.com/questions/26968843/how-to-get-parent-frame-from-page-level



    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
            btnGame1.VerticalAlignment = VerticalAlignment.Top;
            btnGame1.HorizontalAlignment = HorizontalAlignment.Right;
            btnGame2.VerticalAlignment = VerticalAlignment.Top;
            btnGame2.HorizontalAlignment = HorizontalAlignment.Right;
            btnGame3.VerticalAlignment = VerticalAlignment.Top;
            btnGame3.HorizontalAlignment = HorizontalAlignment.Right;

        }

        private void btnGame1_Click(object sender, RoutedEventArgs e)
        {
                                                              //Navigation Service (King King, 2014)   
            Game1 game1 = new Game1();
            NavigationService.Navigate(game1);

        }

        private void btnGame2_Click(object sender, RoutedEventArgs e)
        {
                                                               //Navigation Service (King King, 2014)  
            Game2 game2 = new Game2();
            NavigationService.Navigate(game2);
        }

        private void btnGame3_Click(object sender, RoutedEventArgs e)
        {
            Game3Manu game3manu  = new Game3Manu();
            NavigationService.Navigate(game3manu);
        }
    }
}
