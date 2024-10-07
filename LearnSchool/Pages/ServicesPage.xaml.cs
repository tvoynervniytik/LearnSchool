using System;
using System.Collections.Generic;
using System.Globalization;
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
using LearnSchool.DB;
using LearnSchool.Окна;

namespace LearnSchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        private static List<Service> services { get; set;}
        private static List<Service> services1 = new List<Service>(DBConnection.learnSchool.Service);
        private static bool isAdmin = false;
        private static int countAll;
        
        public ServicesPage()
        {
            InitializeComponent();
            Functions.Authorization.backClick = false;
            services = new List<Service>(DBConnection.learnSchool.Service);
            countAll = new List<Service>(DBConnection.learnSchool.Service).Count;
            countInDBTb.Text = countAll.ToString();
            countInRealTb.Text = services.Count.ToString();
            Refresh();
            servicesLv.ItemsSource = services;
            this.DataContext = this;
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

        private void Refresh()
        {
            services = new List<Service>(DBConnection.learnSchool.Service);
            countAll = services.Count;
            countInDBTb.Text = countAll.ToString();
            if (Functions.Authorization.typeUser == 0)
            {
                adminBtn.Visibility = Visibility.Visible;
                editStP.Visibility = Visibility.Hidden;
                exitBtn.Visibility = Visibility.Hidden;
                isAdmin = false;
            }
            else if (Functions.Authorization.typeUser == 1)
            {
                editStP.Visibility= Visibility.Visible;
                exitBtn.Visibility= Visibility.Visible;
                adminBtn.Visibility = Visibility.Hidden;
                isAdmin = true;
            }

            foreach (Service service in services)
            {
                //    service.DurationInSeconds = service.DurationInSeconds / 60;
                service.Cost = Math.Round(service.Cost, 0);
                service.CostDiscount = Math.Round((decimal)service.CostDiscount, 0);
                //    service.Discount = service.Discount * 100;

                //    if (service.Discount > 0)
                //    {
                //        service.Visibility = Visibility.Visible.ToString();
                //        service.CostDiscount = Math.Round(service.Cost * (100 - (decimal)service.Discount), 0);
                //        service.BackgroundColor = "#E7FABF";
                //    }
                //    else
                //    {
                //        service.Visibility = Visibility.Hidden.ToString();
                //        service.CostDiscount = service.Cost;
                //        service.Width = 0;
                //        service.BackgroundColor = "AliceBlue";
                //    }
                //    if (isAdmin)
                //    {
                //        service.VisibilityAdmin = Visibility.Visible.ToString();
                //    }
                //    else
                //        service.VisibilityAdmin = Visibility.Hidden.ToString();

            }
            if (costCb.SelectedIndex == 0)
            {
                services.Sort((service1, service2) => ((int)service1.CostDiscount).CompareTo((int)service2.CostDiscount));
            }
            else if (costCb.SelectedIndex == 1)
            {
                services.Sort((service1, service2) => ((int)service2.Cost).CompareTo((int)service1.CostDiscount));
            }
            else
                services = new List<Service>(DBConnection.learnSchool.Service);

            //discount ComboBox
            if (saleCb.SelectedIndex == 0)
            {
                services = services.Where(i => i.Discount >= 0 & i.Discount < 5 ).ToList();
            }
            else if (saleCb.SelectedIndex == 1)
            {
                services = services.Where(i => i.Discount >= 5 &  i.Discount < 15 ).ToList();
            }
            else if (saleCb.SelectedIndex == 2)
            {
                services = services.Where(i => i.Discount >= 15 &  i.Discount < 30 ).ToList();
            }
            else if (saleCb.SelectedIndex == 3)
            {
                services = services.Where(i => i.Discount >= 30 &  i.Discount < 70 ).ToList();
            }
            else if (saleCb.SelectedIndex == 4)
            {
                services = services.Where(i => i.Discount >= 70 &  i.Discount < 100 ).ToList();
            }

            services = services.Where(i => i.Title.ToLower().StartsWith(nameTb.Text.ToLower())).ToList();
            countInRealTb.Text = services.Count.ToString();

            servicesLv.ItemsSource = services;
        }

        private void saleCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void delSaleBtn_Click(object sender, RoutedEventArgs e)
        {
            services = new List<Service>(DBConnection.learnSchool.Service);
            saleCb.SelectedItem = null;
            Refresh();
        }

        private void nameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void costCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите выйти из режима администратора?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Functions.Authorization.typeUser = 0;
                Refresh();
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            AddServiceWindow addServiceWindow = new AddServiceWindow();
            Window window = Window.GetWindow(this);
            addServiceWindow.Show();
            window.Close();

        }
        private void HLDelete_Click(object sender, RoutedEventArgs e)
        {
            var serviceDel = (sender as Hyperlink).DataContext as Service;
            try
            {
                var result = MessageBox.Show("Вы действительно хотите удалить данную услугу?", "Подтверждение удаления услуги", 
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    DBConnection.learnSchool.Service.Remove(serviceDel);
                    DBConnection.learnSchool.SaveChanges();
                }
            }
            catch
            {
                MessageBox.Show("This service cannot be removed");
            }

            Refresh();
        }
        private void HLEdit_Click(object sender, RoutedEventArgs e)
        {
            var serviceDEdit= (sender as Hyperlink).DataContext as Service;
            EditServiceWindow editServiceWindow = new EditServiceWindow(serviceDEdit);
            Window window = Window.GetWindow(this);
            editServiceWindow.Show();
            window.Close();

            Refresh();
        }
    }
}
