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

                    if (int.Parse(durationTb.Text) <= 0 || int.Parse(durationTb.Text) > 240)
                        MessageBox.Show("Длительность курса не может быть отрицательной или более 4 часов (240 минут)", "Ошибка ввода длительноости", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        service.Duration = int.Parse(durationTb.Text);
                        service.Cost = decimal.Parse(costTb.Text);
                        service.Title = nameTb.Text;
                        service.Discount = Math.Round(double.Parse(discountTb.Text), 0);
                        service.MainImagePath = ImagePath;
                        if (service.Discount == 0)
                        {
                            service.Width = 0;
                            service.BackgroundColor = "AliceBlue";
                            service.CostDiscount = service.Cost;
                            service.Visibility = "Hidden";
                        }
                        else
                        {
                            service.BackgroundColor = "#E7FABF";
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
                ImagePath = openFileDialog.FileName;
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
            Functions.Authorization.typeUser = 0;
            Window window = Window.GetWindow(this);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            window.Close();
        }

        private void photoDelBtn_Click(object sender, RoutedEventArgs e)
        {
            DelPhotoCommand();
            photoDelBtn.Visibility = Visibility.Hidden;
        }
    }
}
