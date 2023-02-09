﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
using static System.Net.Mime.MediaTypeNames;

namespace School
{
    /// <summary>
    /// Логика взаимодействия для AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        string path;
        bool flagUpdate;
        Service service;
        ServicePhoto servicephoto;
        public AddPage()
        {
            InitializeComponent();
            AddPhotos.Visibility = Visibility.Collapsed;
        }
        public AddPage(Service service)
        {
            InitializeComponent();
            flagUpdate = true;

            IdService.Visibility = Visibility.Visible;
            IdService.Text = service.ID.ToString();
            NameServices.Text = service.Title;
            Description.Text = service.Description;
            PriceServices.Text = service.cost.ToString();
            TimeServices.Text = service.time.ToString();

            // вывод картинки
            if (service.MainImagePath != null)
            {
                BitmapImage img = new BitmapImage(new Uri(service.MainImagePath, UriKind.RelativeOrAbsolute));
                ImageService.Source = img;
            }

            UpdatePhoto.Visibility = Visibility.Visible;
            IdService.Visibility = Visibility.Visible;

        }
        public bool NameService(string title)
        {
            List<Service> servers = DBase.DB.Service.Where(x => x.Title == title).ToList();
            if (servers.Count > 0)
            {
                MessageBox.Show("Данная услуга уже существует", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            else
            {
                return true;
            }
        }



        public bool SaleService(string discount)
        {
            try
            {
                int price = Convert.ToInt32(discount);
                return true;

            }
            catch
            {
                MessageBox.Show("Укажите значение скидки цифрами", "Ошибка", MessageBoxButton.OK);
                return false;
            }
        }

        public bool PriceService(string cost)
        {
            try
            {
                int price = Convert.ToInt32(cost);
                return true;

            }
            catch
            {
                MessageBox.Show("Укажите значение цены цифрами", "Ошибка", MessageBoxButton.OK);
                return false;
            }
        }
        public bool TimeService(string timeLesson)
        {
            try
            {
                int time = Convert.ToInt32(timeLesson);
                if ((time > 0) && (time <= 240))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Время не должно быть отрицательным или превышать 4 часа", "Ошибка", MessageBoxButton.OK);
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Укажите значение времени в цифрах", "Ошибка", MessageBoxButton.OK);
                return false;
            }
        }
        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 9) + "image\\";
                ofd.ShowDialog();
                string[] array = ofd.FileName.Split('\\');

                if (array.Length != 1)
                {
                    path = "\\" + array[array.Length - 2] + "\\" + array[array.Length - 1];
                    ImageService.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так", "Ошибка", MessageBoxButton.OK);
            }
        }
        int n = 0;
        private void UpdatePhoto_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> servicePhoto = DBase.DB.ServicePhoto.Where(x => x.ServiceID == service.ID).ToList();
            if (servicePhoto.Count >= 1)
            {

                BitmapImage img = new BitmapImage(new Uri(servicePhoto[n].PhotoPath, UriKind.RelativeOrAbsolute));
                ImageService.Source = img;

                AddPhoto.Visibility = Visibility.Collapsed;
                UpdatePhoto.Visibility = Visibility.Collapsed;
                AddPhotos.Visibility = Visibility.Collapsed;
                SavePhoto.Visibility = Visibility.Visible;
                DeletPhoto.Visibility = Visibility.Visible;

            }
            else
            {
                MessageBox.Show("Дополнительные фотографии еще не добавлены", "Ошибка", MessageBoxButton.OK);
            }

        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> servicePhoto = DBase.DB.ServicePhoto.Where(x => x.ServiceID == service.ID).ToList();

            n++;
            if (Back.IsEnabled == false)
            {
                Back.IsEnabled = true;
            }
            if (servicePhoto != null)  // если объект не пустой, начинает переводить байтовый массив в изображение
            {

                BitmapImage img = new BitmapImage(new Uri(servicePhoto[n].PhotoPath, UriKind.RelativeOrAbsolute));
                ImageService.Source = img;
            }
            if (n == servicePhoto.Count - 1)
            {
                Next.IsEnabled = false;
            }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> u = DBase.DB.ServicePhoto.Where(x => x.ServiceID == service.ID).ToList();

            n--;
            if (Next.IsEnabled == false)
            {
                Next.IsEnabled = true;
            }
            if (u != null)  // если объект не пустой, начинает переводить байтовый массив в изображение
            {

                BitmapImage img = new BitmapImage(new Uri(u[n].PhotoPath, UriKind.RelativeOrAbsolute));
                ImageService.Source = img;
            }
            if (n == 0)
            {
                Back.IsEnabled = false;
            }
        }

        private void SavePhoto_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> u = DBase.DB.ServicePhoto.Where(x => x.ServiceID == service.ID).ToList();
            service.MainImagePath = u[n].PhotoPath;
            DBase.DB.SaveChanges();
            MessageBox.Show("Фотография изменена");
            SavePhoto.Visibility = Visibility.Collapsed;
            AddPhoto.Visibility = Visibility.Visible;
            UpdatePhoto.Visibility = Visibility.Visible;
            AddPhotos.Visibility = Visibility.Visible;
            DeletPhoto.Visibility = Visibility.Collapsed;
            ClassFrame.newFrame.Navigate(new PageListOfService());
        }


        private void AddPhotos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;

                if ((bool)ofd.ShowDialog())
                {
                    foreach (string path in ofd.FileNames)
                    {
                        string[] array = path.Split('\\');

                        try
                        {
                            File.Copy(path, Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 9) + "image\\" + array[array.Length - 1]);
                        }
                        catch
                        {

                        }

                        ServicePhoto photo = new ServicePhoto()
                        {
                            ServiceID = service.ID,
                            PhotoPath = "\\image\\" + array[array.Length - 1]
                        };

                        DBase.DB.ServicePhoto.Add(photo);
                    }

                    DBase.DB.SaveChanges();
                    MessageBox.Show("Фото успешно добавлены", "Услуги", MessageBoxButton.OK, MessageBoxImage.Information);

                }

            }
            catch
            {
                MessageBox.Show("Что-то пошло не так. Часть файлов не удалось загрузить", "Услуги", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeletPhoto_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> photos = DBase.DB.ServicePhoto.Where(x => x.ServiceID == service.ID).ToList();
            if (photos[n].PhotoPath != service.MainImagePath)
            {
                ServicePhoto photo = photos.FirstOrDefault(x => x.PhotoPath == photos[n].PhotoPath);
                DBase.DB.ServicePhoto.Remove(photo);
                DBase.DB.SaveChanges();
                ClassFrame.newFrame.Navigate(new AddPage(service));
            }
            else
            {
                MessageBox.Show("Выбранное фото нельзя удалить", "Ошибка", MessageBoxButton.OK);
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (NameServices.Text == "" || PriceServices.Text == "" || TimeServices.Text == "" || Sale.Text == "" || path == null)
            {
                MessageBox.Show("Обязательные поля не заполнены", "Ошибка", MessageBoxButton.OK);
            }
            else
            {
                if (NameService(NameServices.Text))
                {
                    if (TimeService(TimeServices.Text))
                    {
                        if (PriceService(PriceServices.Text))
                        {
                            if (SaleService(Sale.Text))
                            {
                                double? discount;
                                if (Sale.Text == "" || Convert.ToInt32(Sale.Text) == 0)
                                {
                                    discount = null;
                                }
                                else
                                {
                                    discount = Convert.ToInt32(Sale.Text) / 100.0;
                                }

                                service.Title = NameServices.Text;
                                service.Cost = Convert.ToDecimal(PriceServices.Text);
                                service.DurationInSeconds = Convert.ToInt32(TimeServices.Text) * 60;
                                service.Description = Description.Text;
                                service.Discount = discount;
                                service.MainImagePath = path;

                                if (flagUpdate == false)
                                {

                                    DBase.DB.Service.Add(service);
                                    DBase.DB.SaveChanges();
                                    MessageBox.Show("Услуга успешно добавлена", "Услуги", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ClassFrame.newFrame.Navigate(new HomePage());
                                }
                                else
                                {
                                    DBase.DB.SaveChanges();
                                    MessageBox.Show("Услуга успешно изменена", "Услуги", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ClassFrame.newFrame.Navigate(new HomePage());
                                }
                            }
                        }
                    }

                }
            }
        }

    }
}

