using LearnSchool.DB;
using LearnSchool.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace LearnSchool.Окна
{
    /// <summary>
    /// Логика взаимодействия для AddServiceWindow.xaml
    /// </summary>
    public partial class AddServiceWindow : Window
    {
        public string ImagePath { get; set; }
        private static List<Service> services {  get; set; }
        public AddServiceWindow()
        {
            InitializeComponent();
            services = new List<Service>(DBConnection.learnSchool.Service);
            Functions.Authorization.backClick = false;
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Functions.Authorization.typeUser = 0;
            Window window = Window.GetWindow(this);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            window.Close();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Service service = new Service();
                if (costTb.Text != "" && nameTb.Text != "" && durationTb.Text != "" && discountTb.Text != "")
                {
                    service.Cost = decimal.Parse(costTb.Text);
                    service.Title = nameTb.Text;
                    service.Discount = Math.Round(double.Parse(discountTb.Text) / 100, 0);
                    service.Duration = int.Parse(durationTb.Text);
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
                            services.Add(service);
                            DBConnection.learnSchool.Service.Add(service);
                            DBConnection.learnSchool.SaveChanges();
                            CloseWindow();
                        }
                    }
                    else
                    {
                        service.Description = descriptionTb.Text;
                        DBConnection.learnSchool.Service.Add(service);
                        DBConnection.learnSchool.SaveChanges();
                        CloseWindow();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
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
        }

        private void photoBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPhoto();
        }

    }
}
