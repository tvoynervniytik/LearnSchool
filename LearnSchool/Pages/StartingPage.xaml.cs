﻿using System;
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
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class StartingPage : Page
    {
        public StartingPage()
        {
            InitializeComponent();
            Functions.Authorization.backClick = false;
            Refresh();
        }
        
        private void adminBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminAuthentification adminAuthentification = new AdminAuthentification();
            adminAuthentification.Show();

            Window window = Window.GetWindow(this);
            window.Close();
        }
        public void Refresh()
        {
            if (Functions.Authorization.typeUser == 0)
            {
                adminBtn.Visibility = Visibility.Visible;
                exitBtn.Visibility = Visibility.Hidden;
            }
            else if (Functions.Authorization.typeUser == 1)
            {
                exitBtn.Visibility = Visibility.Visible;
                adminBtn.Visibility = Visibility.Hidden;
            }
        }

        private void servicesBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServicesPage());
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите выйти из режима администратора?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Refresh();
                Functions.Authorization.typeUser = 0;
            }
        }
    }
}
