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

namespace LearnSchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        public ServicesPage()
        {
            InitializeComponent();
            if (Functions.Authorization.typeUser == 0)
            {
                editStP.Visibility = Visibility.Hidden;
            }
            else if (Functions.Authorization.typeUser == 1)
            {
                adminBtn.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Functions.Authorization.typeUser == 0)
                NavigationService.Navigate(new StartingPage());
            else
                NavigationService.Navigate(new AdminPage());
        }

        private void adminBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminAuthentification adminAuthentification = new AdminAuthentification();
            adminAuthentification.Show();

            Window window = Window.GetWindow(this);
            window.Close();
        }
    }
}
