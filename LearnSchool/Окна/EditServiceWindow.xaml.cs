using LearnSchool.DB;
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
using System.Windows.Shapes;

namespace LearnSchool.Окна
{
    /// <summary>
    /// Логика взаимодействия для EditServiceWindow.xaml
    /// </summary>
    public partial class EditServiceWindow : Window
    {
        public string ImagePath { get; set; }
        private static Service service; 
        private static List<Service> services;
        private static List<ServicePhoto> servicePhotos { get; set; }
        public EditServiceWindow(Service serviceSended)
        {
            InitializeComponent();
            service = serviceSended;
            costTb.Text = service.Cost.ToString();
            nameTb.Text = service.Title.ToString();
            descriptionTb.Text = service.Description.ToString();
            discountTb.Text = service.Discount.ToString();
            durationTb.Text = service.Duration.ToString();
            idTb.Text = service.ID.ToString();
            if (service.MainImagePath != "")
                ImagePath = service.MainImagePath;
            else
                photoDelBtn.Visibility = Visibility.Hidden;
            servicePhotos = new List<ServicePhoto>(DBConnection.learnSchool.ServicePhoto);
            photosLv.ItemsSource = servicePhotos;
            services = new List<Service>(DBConnection.learnSchool.Service);
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void photoBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPhoto();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (costTb.Text != "" && nameTb.Text != "" && durationTb.Text != "" && discountTb.Text != "")
                {

                    if (services.Where(i => i.Title == nameTb.Text) != null 
                        && services.FirstOrDefault(i => i.Title == nameTb.Text) != service)
                        MessageBox.Show("Услуга с таким названием уже существует", "Ошибка названия", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        if (int.Parse(durationTb.Text) <= 0 || int.Parse(durationTb.Text) > 240)
                            MessageBox.Show("Длительность курса не может быть отрицательной или более 4 часов (240 минут)", "Ошибка ввода длительноости", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            service.Duration = int.Parse(durationTb.Text);
                            service.Cost = decimal.Parse(costTb.Text);
                            service.Title = nameTb.Text;
                            service.Discount = Math.Round(double.Parse(discountTb.Text), 0);
                            service.MainImagePath = ImagePath;
                            if (Math.Round(double.Parse(discountTb.Text), 0) == 0)
                            {
                                service.Width = 0;
                                service.BackgroundColor = "AliceBlue";
                                service.CostDiscount = service.Cost;
                                service.Visibility = "Hidden";
                            }
                            else
                            {
                                service.BackgroundColor = "#E7FABF";
                                service.Width = null;
                                service.CostDiscount = service.Cost - service.Cost * ((decimal)service.Discount / 100);
                                service.Visibility = "Visible";
                            }
                            if (descriptionTb.Text == "")
                            {
                                var result = MessageBox.Show("Вы не заполнинили описание услуги. Оставить незаполненным?", "Описание услуги NULL", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (result == MessageBoxResult.Yes)
                                {
                                    DBConnection.learnSchool.SaveChanges();
                                    CloseWindow();
                                }
                            }
                            else
                            {
                                service.Description = descriptionTb.Text;
                                DBConnection.learnSchool.SaveChanges();
                                CloseWindow();
                            }
                        }
                    }
                }
                else
                    MessageBox.Show("Заполните все данные!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка ввода данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CloseWindow()
        {
            Functions.Authorization.backClick = true;
            Window window = Window.GetWindow(this);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            window.Close();

        }

        private void AddPhoto()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg"
            };

            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                string selectedImagePath = openFileDialog.FileName;

                // Создайте новое имя файла (уникальное)
                string fileName = System.IO.Path.GetFileName(selectedImagePath);

                // Получите путь к папке проекта
                string projectDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string targetDirectory = System.IO.Path.Combine(projectDirectory, "Images");

                // Создайте папку, если она не существует
                System.IO.Directory.CreateDirectory(targetDirectory);

                // Объедините полный путь к файлу в папке проекта
                string newFilePath = System.IO.Path.Combine(targetDirectory, fileName);

                // Скопируйте файл в папку проекта
                System.IO.File.Copy(selectedImagePath, newFilePath, true);


                //df
                ImagePath = newFilePath;

                img.Source = new BitmapImage(new Uri(ImagePath));
                //this.Close();
            }
            if (ImagePath != null)
            {
                photoDelBtn.Visibility = Visibility.Visible;
            }
        }
        private void DelPhotoCommand()
        {
            ImagePath = "";
            img.Source = null;
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите выйти из режима администратора?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Functions.Authorization.typeUser = 0;
                Window window = Window.GetWindow(this);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                window.Close();
            }
        }

        private void photoDelBtn_Click(object sender, RoutedEventArgs e)
        {
            DelPhotoCommand();
            photoDelBtn.Visibility = Visibility.Hidden;
        }

        private void photoDopDelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (photosLv.SelectedItems != null)
            {
                var photo = photosLv.SelectedItem as ServicePhoto;
                DBConnection.learnSchool.ServicePhoto.Remove(photo);
                DBConnection.learnSchool.SaveChanges();
                photosLv.ItemsSource = new List<ServicePhoto>(DBConnection.learnSchool.ServicePhoto);
            }
        }
        private void AddPhotoDop()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg"
                };

                if (openFileDialog.ShowDialog().GetValueOrDefault())
                {
                    string selectedImagePath = openFileDialog.FileName;

                    string fileName = System.IO.Path.GetFileName(selectedImagePath);
                    string projectDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string targetDirectory = System.IO.Path.Combine(projectDirectory, "Images");
                    System.IO.Directory.CreateDirectory(targetDirectory);
                    string newFilePath = System.IO.Path.Combine(targetDirectory, fileName);
                    System.IO.File.Copy(selectedImagePath, newFilePath, true);


                    ServicePhoto servicePhoto = new ServicePhoto();
                    servicePhoto.ServiceID = service.ID;
                    servicePhoto.PhotoPath = newFilePath;
                    DBConnection.learnSchool.ServicePhoto.Add(servicePhoto);
                    DBConnection.learnSchool.SaveChanges();
                    photosLv.ItemsSource = new List<ServicePhoto>(DBConnection.learnSchool.ServicePhoto);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void photoDopBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPhotoDop();
        }
    }
}
