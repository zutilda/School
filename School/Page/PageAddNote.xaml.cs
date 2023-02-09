using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для PageAddNote.xaml
    /// </summary>
    public partial class PageAddNote : Page
    {
        Service service;
        public PageAddNote(Service service)
        {
            InitializeComponent();
            this.service = service;
            Title.Text = "Название услуги: " + service.Title + " | " + "Длительность услуги: " + service.time + " минут";
            
            FullName.ItemsSource = DBase.DB.Client.ToList();
            FullName.SelectedValuePath = "ID";
            FullName.DisplayMemberPath = "FullName";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (CheckData())
            {
                DateTime dt = (DateTime)Date.SelectedDate;
                dt = dt.AddHours(Convert.ToInt32(TimeStart.Text.Substring(0, 2)));
                dt = dt.AddMinutes(Convert.ToInt32(TimeStart.Text.Substring(3, 2)));
                string text;
                if (Comment.Text == "")
                {
                    text = null;
                }
                else
                {
                    text = Comment.Text;
                }

                ClientService client = new ClientService()
                {
                    ClientID = (int)FullName.SelectedValue,
                    ServiceID = service.ID,
                    StartTime = dt,
                    Comment = text
                };

                DBase.DB.ClientService.Add(client);
                DBase.DB.SaveChanges();
                MessageBox.Show("Клиент успешно записан", "Запись на услугу", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool CheckData()
        {
            if (FullName.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите клиента", "Запись на услугу", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            else if (Date.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату", "Запись на услугу", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            else if (!Regex.IsMatch(TimeStart.Text, @"^[0-9]{2}:[0-9]{2}$"))
            {
                MessageBox.Show("Введите время корректно", "Запись на услугу", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            return true;
        }

        private void TimeStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(TimeStart.Text, @"^[0-9]{2}:[0-9]{2}$"))
            {
                int hours = Convert.ToInt32(TimeStart.Text.Substring(0, 2));
                int minutes = Convert.ToInt32(TimeStart.Text.Substring(3, 2));

                if (hours < 24 && minutes < 60)
                {
                    int timeEnd = hours * 60 + minutes + service.time;
                    TimeEnd.Text = timeEnd / 60 + ":" + timeEnd % 60;
                }
                else
                {
                    MessageBox.Show("Введите время корректно", "Запись на услугу", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                TimeEnd.Text = "";
            }
        }      
    }
}
