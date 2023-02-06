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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string kod;
        public MainWindow()
        {
            InitializeComponent();
            if (kod == "0000")
            {
                Zap.Visibility = Visibility.Visible;
            }
            else
            {
                Zap.Visibility = Visibility.Collapsed;
            }
            DBase.DB = new Entities();
            ClassFrame.newFrame = frmMain;
            ClassFrame.newFrame.Navigate(new PageListOfService());

        }

        private void Services_Click(object sender, RoutedEventArgs e)
        {

            ClassFrame.newFrame.Navigate(new PageListOfService());

        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            WindowAdmin windowPerson = new WindowAdmin();  // создание объекта окна
            windowPerson.ShowDialog();

            ClassFrame.newFrame.Navigate(new PageListOfService());
        }

        private void Zapisi_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.newFrame.Navigate(new PageNearNote());
        }
    }
}

