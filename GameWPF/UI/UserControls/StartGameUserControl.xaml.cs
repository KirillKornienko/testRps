using System;
using System.Windows;
using System.Windows.Controls;

namespace GameWPF.UserControls
{
    /// <summary>
    /// Логика взаимодействия для StartGameUserControl.xaml
    /// </summary>
    public partial class StartGameUserControl : UserControl
    {
        public event EventHandler SinglePlayerClicked;

        public event EventHandler BackToMainMenuClicked;


        public StartGameUserControl()
        {
            InitializeComponent();
        }

        private void SinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            SinglePlayerClicked(this, null);
        }

        private void BackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            BackToMainMenuClicked(this, null);
        }

    }
}
