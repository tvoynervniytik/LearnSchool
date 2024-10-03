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
        public AddServiceWindow()
        {
            InitializeComponent();
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
            CloseWindow();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            Functions.Authorization.backClick = true;
            MainWindow mainWindow = new MainWindow();
            Window window = Window.GetWindow(this);
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
