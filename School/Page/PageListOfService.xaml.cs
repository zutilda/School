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
    /// Логика взаимодействия для PageListOfService.xaml
    /// </summary>
    public partial class PageListOfService : Page
    {
        public static string admin;
        public PageListOfService()
        {
            InitializeComponent();
            ListService.ItemsSource = DBase.DB.Service.ToList();
            Sorting.SelectedIndex = 0;
            Filtering.SelectedIndex = 0;
            CountService.Text = DBase.DB.Service.ToList().Count + "/" + DBase.DB.Service.ToList().Count;
            if (admin == "0000")
            {
                Service.btn_admin = Visibility.Visible;
                AddService.Visibility = Visibility.Visible;
            }
            else
            {
                Service.btn_admin = Visibility.Collapsed;
                AddService.Visibility = Visibility.Collapsed;
            }
        }
        void Filter()
        {
            List<Service> services = new List<Service>();
            services = DBase.DB.Service.ToList();

            //Поиск по названию

            if (!string.IsNullOrWhiteSpace(SearchName.Text))  // Проверка пустую запись и запись состоящую из пробелов
            {
                services = services.Where(x => x.Title.ToLower().Contains(SearchName.Text.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(SearchDescription.Text)) 
            {

                List<Service> description = services.Where(x => x.Description != null).ToList();
                if (description.Count > 0)
                {

                    services = description.Where(x => x.Description.ToLower().Contains(SearchDescription.Text.ToLower())).ToList();

                }
                else
                {
                    MessageBox.Show("Записей с таким описанием нет");
                    SearchDescription.Text = "";
                }

            }
            //Фильтрация по размеру скидки

            switch (Filtering.SelectedIndex)
            {
                case 0:
                    {
                        services = services.ToList();
                    }
                    break;
                case 1:
                    {
                        services = services.Where(x => ((x.Discount >= 0) && (x.Discount * 100 < 5))).ToList();
                    }
                    break;
                case 2:
                    {
                        services = services.Where(x => ((x.Discount * 100 >= 5) && (x.Discount * 100 < 15))).ToList();
                    }
                    break;
                case 3:
                    {
                        services = services.Where(x => ((x.Discount * 100 >= 15) && (x.Discount * 100 < 30))).ToList();

                    }
                    break;
                case 4:
                    {
                        services = services.Where(x => ((x.Discount * 100 >= 30) && (x.Discount * 100 < 70))).ToList();
                    }
                    break;
                case 5:
                    {
                        services = services.Where(x => ((x.Discount * 100 >= 70) && (x.Discount * 100 < 100))).ToList();
                    }
                    break;
            }

            //сортировка

            switch (Sorting.SelectedIndex)
            {
                case 0:
                    {
                        services.Sort((x, y) => x.Cost.CompareTo(y.Cost));
                    }
                    break;
                case 1:
                    {
                        services.Sort((x, y) => x.Cost.CompareTo(y.Cost));
                        services.Reverse();
                    }
                    break;
            }

            ListService.ItemsSource = services;
            if (services.Count == 0)
            {
                MessageBox.Show("нет записей");
                CountService.Text = DBase.DB.Service.ToList().Count + "/" + DBase.DB.Service.ToList().Count;
                SearchName.Text = "";
                SearchDescription.Text = "";
                Sorting.SelectedIndex = 0;
                Filtering.SelectedIndex = 0;

            }
            CountService.Text = services.Count + "/" + DBase.DB.Service.ToList().Count;

        }
        private void SearchName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void SearchDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void Filtering_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void Sorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int id = Convert.ToInt32(btn.Uid);
            Service serv = DBase.DB.Service.FirstOrDefault(x => x.ID == id);
            List<ClientService> clientservices = DBase.DB.ClientService.Where(x => x.ServiceID == serv.ID).ToList();
            if (clientservices.Count > 0)
            {
                MessageBox.Show("Данную услугу нельзя удалить");
            }
            else
            {
                DBase.DB.Service.Remove(serv);
                DBase.DB.SaveChanges();
                ClassFrame.newFrame.Navigate(new PageListOfService());
            }

        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.newFrame.Navigate(new AddPage()); 
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int id = Convert.ToInt32(btn.Uid);
            Service service = DBase.DB.Service.FirstOrDefault(x => x.ID == id);
            ClassFrame.newFrame.Navigate(new AddPage(service));
        }

        private void SingUp_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int id = Convert.ToInt32(btn.Uid);
            Service service = DBase.DB.Service.FirstOrDefault(x => x.ID == id);
            ClassFrame.newFrame.Navigate(new PageAddNote(service));
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.newFrame.Navigate(new HomePage());
        }
    }
}

