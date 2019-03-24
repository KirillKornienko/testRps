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

namespace GameWPF.UserControls
{
    /// <summary>
    /// Логика взаимодействия для StartGameUserControl.xaml
    /// </summary>
    public partial class StartGameUserControl : UserControl
    {
        public StartGameUserControl()
        {
            InitializeComponent();
        }

        private void SinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Кнопка старта нажата");
        }

        private void BackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
