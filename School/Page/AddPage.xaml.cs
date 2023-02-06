using Microsoft.Win32;
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
    /// Логика взаимодействия для AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        string path;
        bool flagUpdate;
        bool flagUpdatePhoto;
        Service ser;
        ServicePhoto servicephoto;
        public AddPage()
        {
            InitializeComponent();
            addPhotos.Visibility = Visibility.Collapsed;
        }
        public AddPage(Service ser)
        {
            InitializeComponent();
            flagUpdate = true;

            this.ser = ser;
            NameServices.Text = ser.Title;
            double skid = ser.Discount.Value * 100;
            Sale.Text = Convert.ToString(skid);
            double pric = Convert.ToDouble(ser.Cost);
            PriceServices.Text = Convert.ToString(pric);
            int tim = ser.DurationInSeconds / 60;
            TimeServices.Text = Convert.ToString(tim);
            Description.Text = ser.Description;
            IdService.Text = Convert.ToString(ser.ID);

            // вывод картинки
            if (ser.MainImagePath != null)
            {
                BitmapImage img = new BitmapImage(new Uri(ser.MainImagePath, UriKind.RelativeOrAbsolute));
                ImageService.Source = img;
            }
            UpdatePhoto.Visibility = Visibility.Visible;
            IdService.Visibility = Visibility.Visible;

        }
        public bool NameService(string a)
        {
            List<Service> servers = DBase.DB.Service.Where(x => x.Title == a).ToList();
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



        public bool SaleService(string a)
        {
            try
            {
                int price = Convert.ToInt32(a);
                return true;

            }
            catch
            {
                MessageBox.Show("Напишите количество скидки цифрами", "Ошибка", MessageBoxButton.OK);
                return false;
            }
        }

        public bool PriceService(string a)
        {
            try
            {
                int price = Convert.ToInt32(a);
                return true;

            }
            catch
            {
                MessageBox.Show("Напишите количество цены цифрами", "Ошибка", MessageBoxButton.OK);
                return false;
            }
        }
        public bool TimeService(string a)
        {
            try
            {
                int time = Convert.ToInt32(a);
                if ((time > 0) && (time <= 240))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Время не должно быть отрицательное, а так же не должно быть больше 4 часов", "Ошибка", MessageBoxButton.OK);
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Напишите количество времени цифрами", "Ошибка", MessageBoxButton.OK);
                return false;
            }
        }
        private void addPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();  // создаем объект диалогового окна
                OFD.ShowDialog();  // открываем диалоговое окно
                path = OFD.FileName;  // извлекаем полный путь к картинке
                string[] arrayPath = path.Split('\\');  // разделяем путь к картинке в массив
                path = "\\" + arrayPath[arrayPath.Length - 2] + "\\" + arrayPath[arrayPath.Length - 1];  // записываем в бд путь, начиная с имени папки
                List<ServicePhoto> photos = DBase.DB.ServicePhoto.Where(x => x.ServiceID == ser.ID).ToList();
                if (photos.Count == 0)
                {
                    servicephoto = new ServicePhoto();
                    servicephoto.ServiceID = ser.ID;
                    servicephoto.PhotoPath = ser.MainImagePath;
                    DBase.DB.ServicePhoto.Add(servicephoto);
                }

                BitmapImage image = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                ImageService.Source = image;
            }
            catch
            {
                MessageBox.Show("Что то пошло не так", "Ошибка", MessageBoxButton.OK);
            }
        }
        int n = 0;
        private void UpdatePhoto_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> servicePhoto = DBase.DB.ServicePhoto.Where(x => x.ServiceID == ser.ID).ToList();
            if (servicePhoto.Count > 1)
            {

                BitmapImage img = new BitmapImage(new Uri(servicePhoto[n].PhotoPath, UriKind.RelativeOrAbsolute));
                ImageService.Source = img;

                Next.Visibility = Visibility.Visible;
                Bakc.Visibility = Visibility.Visible;
                savePhoto.Visibility = Visibility.Visible;
                addPhoto.Visibility = Visibility.Collapsed;
                UpdatePhoto.Visibility = Visibility.Collapsed;
                Back.Visibility = Visibility.Visible;
                addPhotos.Visibility = Visibility.Collapsed;
                DeletPhoto.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Нет дополнительных фотографий", "Ошибка", MessageBoxButton.OK);
            }

        }

        private void add_Click(object sender, RoutedEventArgs e)
        {

            if (flagUpdate == false)
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
                                    ser = new Service();
                                    ser.Title = NameServices.Text;
                                    ser.Cost = Convert.ToInt32(PriceServices.Text);
                                    double skidk = Convert.ToDouble(Sale.Text) / 100;
                                    ser.Discount = skidk;
                                    int timeSecond = Convert.ToInt32(TimeServices.Text) * 60;
                                    ser.DurationInSeconds = timeSecond;
                                    ser.MainImagePath = path;
                                    if (Description.Text == "")
                                    {
                                        ser.Description = null;
                                    }
                                    else
                                    {
                                        ser.Description = Description.Text;
                                    }

                                    DBase.DB.Service.Add(ser);
                                    servicephoto = new ServicePhoto();
                                    servicephoto.ServiceID = ser.ID;
                                    servicephoto.PhotoPath = path;
                                    DBase.DB.ServicePhoto.Add(servicephoto);

                                    DBase.DB.SaveChanges();
                                    MessageBox.Show("Информация добавлена");

                                    ClassFrame.newFrame.Navigate(new PageListOfService());
                                }
                            }
                        }

                    }
                }
            }
            else
            {
                if (NameServices.Text == "" || PriceServices.Text == "" || TimeServices.Text == "" || Sale.Text == "")
                {
                    MessageBox.Show("Обязательные поля не заполнены", "Ошибка", MessageBoxButton.OK);
                }
                else
                {
                    if (TimeService(TimeServices.Text))
                    {
                        if (PriceService(PriceServices.Text))
                        {
                            if (SaleService(Sale.Text))
                            {
                                ser.Title = NameServices.Text;
                                ser.Cost = Convert.ToInt32(PriceServices.Text);
                                double skidk = Convert.ToDouble(Sale.Text) / 100;
                                ser.Discount = skidk;
                                int timeSecond = Convert.ToInt32(TimeServices.Text) * 60;
                                ser.DurationInSeconds = timeSecond;
                                if (path == null)
                                {
                                    path = ser.MainImagePath;
                                }
                                if ((path != null) && (flagUpdatePhoto == false))
                                {
                                    servicephoto = new ServicePhoto();
                                    servicephoto.ServiceID = ser.ID;
                                    servicephoto.PhotoPath = path;
                                    DBase.DB.ServicePhoto.Add(servicephoto);

                                }
                                ser.MainImagePath = path;
                                if (Description.Text == "")
                                {
                                    ser.Description = null;
                                }
                                else
                                {
                                    ser.Description = Description.Text;
                                }
                                DBase.DB.SaveChanges();
                                MessageBox.Show("Информация добавлена");

                                ClassFrame.newFrame.Navigate(new PageListOfService());
                            }
                        }

                    }
                }
            }

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> servicePhoto = DBase.DB.ServicePhoto.Where(x => x.ServiceID == ser.ID).ToList();

            n++;
            if (Bakc.IsEnabled == false)
            {
                Bakc.IsEnabled = true;
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


        private void Bakc_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> u = DBase.DB.ServicePhoto.Where(x => x.ServiceID == ser.ID).ToList();

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
                Bakc.IsEnabled = false;
            }
        }

        private void savePhoto_Click(object sender, RoutedEventArgs e)
        {
            flagUpdatePhoto = true;
            List<ServicePhoto> u = DBase.DB.ServicePhoto.Where(x => x.ServiceID == ser.ID).ToList();
            ser.MainImagePath = u[n].PhotoPath;
            DBase.DB.SaveChanges();
            MessageBox.Show("Фотография изменена");
            Next.Visibility = Visibility.Collapsed;
            Bakc.Visibility = Visibility.Collapsed;
            savePhoto.Visibility = Visibility.Collapsed;
            addPhoto.Visibility = Visibility.Visible;
            Back.Visibility = Visibility.Collapsed;
            UpdatePhoto.Visibility = Visibility.Visible;
            addPhotos.Visibility = Visibility.Visible;
            DeletPhoto.Visibility = Visibility.Collapsed;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            flagUpdatePhoto = false;
            Next.Visibility = Visibility.Collapsed;
            Bakc.Visibility = Visibility.Collapsed;
            savePhoto.Visibility = Visibility.Collapsed;
            addPhoto.Visibility = Visibility.Visible;
            Back.Visibility = Visibility.Collapsed;
            UpdatePhoto.Visibility = Visibility.Visible;
            addPhotos.Visibility = Visibility.Visible;
            DeletPhoto.Visibility = Visibility.Collapsed;
            if (ser.MainImagePath != null)
            {
                BitmapImage img = new BitmapImage(new Uri(ser.MainImagePath, UriKind.RelativeOrAbsolute));
                ImageService.Source = img;
            }

        }
        private void addPhotos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();  // создаем диалоговое окно
                OFD.Multiselect = true;  // открытие диалогового окна с возможностью выбора нескольких элементов
                if (OFD.ShowDialog() == true)  // пока диалоговое окно открыто, будет в цикле записывать каждое выбранное изображение в БД
                {
                    foreach (string file in OFD.FileNames)  // цикл организован по именам выбранных файлов
                    {
                        ServicePhoto u = new ServicePhoto();
                        u.ServiceID = ser.ID;
                        path = file;  // извлекаем полный путь к картинке
                        string[] arrayPath = path.Split('\\');  // разделяем путь к картинке в массив
                        path = "\\" + arrayPath[arrayPath.Length - 2] + "\\" + arrayPath[arrayPath.Length - 1];  // записываем в бд путь, начиная с имени папки
                        u.PhotoPath = path;
                        DBase.DB.ServicePhoto.Add(u);
                            
                    }
                    DBase.DB.SaveChanges();
                    MessageBox.Show("Фото добавлены");
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так");
            }
        }

        private void DeletPhoto_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> photos = DBase.DB.ServicePhoto.Where(x => x.ServiceID == ser.ID).ToList();
            if (photos[n].PhotoPath != ser.MainImagePath)
            {
                ServicePhoto photo = photos.FirstOrDefault(x => x.PhotoPath == photos[n].PhotoPath);
                DBase.DB.ServicePhoto.Remove(photo);
                DBase.DB.SaveChanges();
                ClassFrame.newFrame.Navigate(new AddPage(ser));
            }
            else
            {
                MessageBox.Show("Выбранное фото нельзя удалить", "Ошибка", MessageBoxButton.OK);
            }
        }
    }
}

