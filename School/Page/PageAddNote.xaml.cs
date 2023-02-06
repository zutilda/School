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
    /// Логика взаимодействия для PageAddNote.xaml
    /// </summary>
    public partial class PageAddNote : Page
    {
        Service ser;
        ClientService client;
        public PageAddNote(Service service)
        {
            InitializeComponent();
            this.ser = service;
            Title.Text = "Название услуги: " + service.Title + " | " + "Длительность услуги: " + service.time + " минут";
            List<Client> clients = DBase.DB.Client.ToList();
            for (int i = 0; i < clients.Count; i++)  
            {
                FullName.Items.Add(clients[i].FullName);
            }

            hh.Text = DateTime.Now.ToString("HH");
            mm.Text = DateTime.Now.ToString("mm");
            int HH = Convert.ToInt32(DateTime.Now.ToString("HH"));
            int MM = Convert.ToInt32(DateTime.Now.ToString("mm"));
            DateTime date = new DateTime(2000, 2, 2, HH, MM, 0);
            DateTime data = date.AddMinutes(Convert.ToInt32(service.time));
            TimeEnd.Text = data.ToShortTimeString();
        }
        void TIMER()
        {
            try
            {
                int h = Convert.ToInt32(hh.Text);
                int m = Convert.ToInt32(mm.Text);
                if ((h < 24) && (m < 60))
                {

                    int HH = Convert.ToInt32(h);
                    int MM = Convert.ToInt32(m);
                    DateTime date = new DateTime(2000, 2, 2, HH, MM, 0);
                    DateTime data = date.AddMinutes(Convert.ToInt32(ser.time));
                    TimeEnd.Text = data.ToShortTimeString();
                }
                else
                {
                    MessageBox.Show("Введите время правильно", "Ошибка", MessageBoxButton.OK);

                }
            }
            catch
            {

                MessageBox.Show("Что-то пошло не так", "Ошибка", MessageBoxButton.OK);
            }
        }
        private void hh_TextChanged(object sender, TextChangedEventArgs e)
        {
            TIMER();
        }

        private void mm_TextChanged(object sender, TextChangedEventArgs e)
        {
            TIMER();
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (FullName.Text == "" || hh.Text == "" || mm.Text == "" || StartDate.Text == "")
            {
                MessageBox.Show("Обязательные поля не заполнены", "Ошибка", MessageBoxButton.OK);
            }
            else
            {
                client = new ClientService();
                client.ServiceID = ser.ID;
                client.ClientID = FullName.SelectedIndex + 1;
                string date = StartDate.Text;
                string[] Dat = date.Split('.');
                int h = Convert.ToInt32(hh.Text);
                int m = Convert.ToInt32(mm.Text);
                DateTime dateStar = new DateTime(Convert.ToInt32(Dat[2]), Convert.ToInt32(Dat[1]), Convert.ToInt32(Dat[0]), h, m, 0);
                client.StartTime = dateStar;
                DBase.DB.ClientService.Add(client);

                DBase.DB.SaveChanges();
                MessageBox.Show("Клиент записан");

                ClassFrame.newFrame.Navigate(new PageListOfService());
            }
        }
    }
}
