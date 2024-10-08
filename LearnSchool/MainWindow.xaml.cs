﻿using LearnSchool.Pages;
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

namespace LearnSchool
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (Functions.Authorization.backClick)
                navFr.Navigate(new ServicesPage());
            else
            {
                if (Functions.Authorization.typeUser == 1)
                {
                    navFr.Navigate(new AdminPage());

                }
                else
                {
                    navFr.Navigate(new StartingPage());
                }
            }
        }

    }
}
