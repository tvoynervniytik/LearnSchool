using LearnSchool.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
using System.Windows.Threading;

namespace LearnSchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        private static List<ClientService> clientService {  get; set; }
        private DispatcherTimer _timer;
        private DispatcherTimer _timerS;
        private int seconds;
        public SchedulePage()
        {
            InitializeComponent();
            Refresh();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(30);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            _timerS = new DispatcherTimer();
            _timerS.Interval = TimeSpan.FromSeconds(1); 
            _timerS.Tick += Timer_Tick_S;
            _timerS.Start();
        }

        private void Refresh()
        {
            seconds = 30;
            secTb.Text = seconds.ToString();
            DateTime today = DateTime.Today;
            DateTime yesterday = DateTime.Today.AddDays(1);
            clientService = new List<ClientService>(DBConnection.learnSchool.ClientService.
                Where(i => i.StartTime > DateTime.Now
                && 
                (
                (DbFunctions.TruncateTime(i.StartTime) == DbFunctions.TruncateTime(today)) 
                ||
                (DbFunctions.TruncateTime(i.StartTime) == DbFunctions.TruncateTime(yesterday))
                )
                ));
            clientService.Sort((cs1, cs2) => cs1.StartTime.CompareTo(cs2.StartTime));
            //__________________
            //MessageBox.Show(clientService[0].StartTime.ToString());
            //MessageBox.Show(DateTime.Today.ToString());
            //------------------
            foreach (ClientService service in clientService)
            {
                TimeSpan Time = service.StartTime - DateTime.Now;
                service.TimeToStart = Time;
                if (Time.TotalHours < 1)
                    service.Color = "Red";
                else
                    service.Color = "Black";
            }
            
            scheduleLv.ItemsSource = clientService;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }
        private void Timer_Tick_S(object sender, EventArgs e)
        {
            seconds -= 1;
            if (seconds < 9)
            secTb.Text = "0" + seconds.ToString();
            else
                secTb.Text = seconds.ToString();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите выйти из режима администратора?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Functions.Authorization.typeUser = 0;
                NavigationService.Navigate(new StartingPage());
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPage());
        }
    }
}
