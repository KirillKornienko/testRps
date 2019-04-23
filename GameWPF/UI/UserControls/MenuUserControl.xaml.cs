using System;
using System.Windows;
using System.Windows.Controls;

namespace GameWPF.UserControls
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
            StartGameClicked(this, null);
        }

        private void LoadGameClick(object sender, RoutedEventArgs e)
        {
            LoadGameClicked(this, null);
        }

        private void ExitGameClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public event EventHandler StartGameClicked;
        public event EventHandler LoadGameClicked;
    }
}
