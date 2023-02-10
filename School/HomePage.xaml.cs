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

namespace School
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public static string code;
        public HomePage()
        {
            InitializeComponent();
           
        }

        public HomePage(string code)
        {
            InitializeComponent();
            if (code == "0000")
            {
                Note.Visibility = Visibility.Visible;
            }
            else
            {
                Note.Visibility = Visibility.Collapsed;
            }
        }
        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            WindowAdmin windowPerson = new WindowAdmin();  
            windowPerson.ShowDialog();

            if (code == "0000")
            {
                Note.Visibility = Visibility.Visible;
            }
            else
            {
                Note.Visibility = Visibility.Collapsed;
            }
        }

        private void Note_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.newFrame.Navigate(new PageNearNote());
        }

        private void Services_Click(object sender, RoutedEventArgs e)
        {

            ClassFrame.newFrame.Navigate(new PageListOfService());

        }
    }
}
