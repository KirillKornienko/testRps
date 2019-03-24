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

namespace GameWPF
{
    /// <summary>
    /// Логика взаимодействия для MenuUserControl.xaml
    /// </summary>
    public partial class MenuUserControl : UserControl
    {
        public MenuUserControl()
        {
            InitializeComponent();
        }

        private void StartGameClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Кнопка №1 нажата");
        }

        private void LoadGameClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Кнопка №2 нажата");
        }

        private void ExitGameClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
