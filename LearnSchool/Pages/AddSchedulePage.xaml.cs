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

namespace LearnSchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddSchedulePage.xaml
    /// </summary>
    public partial class AddSchedulePage : Page
    {
        public static List<Service> services {  get; set; }
        public static List<Client> clients {  get; set; }
        private string timeService;
        private static Service service1;
        public AddSchedulePage(Service service)
        {
            InitializeComponent();

            services = new List<Service>(DBConnection.learnSchool.Service);
            clients = new List<Client>(DBConnection.learnSchool.Client);
            service1 = service;
            titleTb.Text = service.Title;
            durationTb.Text = service.Duration.ToString();

            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServicesPage());
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите выйти из режима администратора?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Functions.Authorization.typeUser = 0;
                NavigationService.Navigate(new ServicesPage());
            }
        }

        private void timeTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = timeTb.Text;
            string filteredText = new string(text.Where(c => char.IsDigit(c) || c == ':').ToArray());
            int count = text.Count(c => c == ':');

            if (filteredText.Length > 0)
            {
                //удаление повторного :
                if (filteredText.Contains("::"))
                {
                    filteredText = filteredText.Substring(0, filteredText.Length - 1);
                }

                if (filteredText.Length >= 3)
                {
                    //запрет на ввод больше 3 чисел подряд
                    for (int i = 0; i <= filteredText.Length - 3; i++)
                    {
                        if (char.IsDigit(filteredText[i]) && char.IsDigit(filteredText[i + 1]) && char.IsDigit(filteredText[i + 2]))
                        {
                            filteredText = filteredText.Remove(i + 2, 1);
                            break;
                        }
                    }
                    //запрет на ввод больше 5 знаков (2 цифры - часов, 2 цифры - минут, 1 - :)
                    if (filteredText.Length > 5)
                    {
                        filteredText.Substring(0, 5);
                    }

                    string[] parts = filteredText.Split(':');
                    // Проверка и исправление первой части (часы)
                    if (parts.Length > 0 && parts[0].Length > 0)
                    {
                        if (parts[0].Length == 1)
                        {
                            parts[0] = "0" + parts[0]; // Добавить 0, если введено одно число
                        }
                        // Проверка на корректное значение часов
                        if (int.TryParse(parts[0], out int hours))
                        {
                            if (hours < 0 || hours > 23)
                            {
                                parts[0] = "00"; // Установить часы в 00, если некорректное значение
                            }
                        }
                        filteredText = parts[0]+":";

                        if (parts.Length > 1 && parts[1].Length > 0)
                        {
                            // Проверка на корректное значение минут
                            if (int.TryParse(parts[1], out int minutes))
                            {
                                if (minutes < 0 || minutes > 59)
                                {
                                    parts[1] = "00"; // Установить минуты в 00, если некорректное значение
                                }
                            }
                            filteredText += parts[1];
                        }
                    } 
                }
            }
            timeTb.Text = filteredText;
            timeService = filteredText;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            var client = clientsCb.SelectedItem as Client;
            ClientService clientService = new ClientService();
            if (timeTb.Text == "" || dateDp.SelectedDate == null || client == null)
            {
                MessageBox.Show("Вы заполнили не все данные!", "Ошибка заполнения данных", MessageBoxButton.OK, MessageBoxImage.Error); ;
            }  
            else
            {
                clientService.ServiceID = service1.ID;
                clientService.ClientID = client.ID;
                TimeSpan time = DateTime.Parse(timeService).TimeOfDay;

                var startTime = dateDp.SelectedDate.ToString().Substring(0, 11) + time.ToString();
                clientService.StartTime = DateTime.Parse(startTime.Trim());

                DBConnection.learnSchool.ClientService.Add(clientService);
                DBConnection.learnSchool.SaveChanges();
                MessageBox.Show($"Добавлена запись на услугу \"{service1.Title.Trim()}\" клиента {client.FirstName.Trim()} {client.LastName.Trim()[0]}.{client.Patronymic.Trim()[0]}. на {startTime}");
                NavigationService.Navigate(new ServicesPage());
            }
        }

    }
}
