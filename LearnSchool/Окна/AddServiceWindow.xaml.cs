using LearnSchool.Pages;
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

namespace LearnSchool.Окна
{
    /// <summary>
    /// Логика взаимодействия для AddServiceWindow.xaml
    /// </summary>
    public partial class AddServiceWindow : Window
    {
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
            Functions.Authorization.backClick = true;
            MainWindow mainWindow = new MainWindow();
            Window window = Window.GetWindow(this);
            mainWindow.Show();
            window.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Functions.Authorization.backClick = true;
            MainWindow mainWindow = new MainWindow();
            Window window = Window.GetWindow(this);
            mainWindow.Show();
            window.Close();
        }
    }
}
