using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using LearnSchool.Functions;
using LearnSchool.Pages;

namespace LearnSchool
{
    /// <summary>
    /// Логика взаимодействия для AdminAuthentification.xaml
    /// </summary>
    public partial class AdminAuthentification : Window
    {
        public AdminAuthentification()
        {
            InitializeComponent();
            Functions.Authorization.backClick = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (passwordTb.Text == Functions.Authorization.password)
            {
                Functions.Authorization.typeUser = 1;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                MessageBox.Show("Здравствуйте, Администратор", "Успешный вход", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("пароль неверный", "Безуспешный вход", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
