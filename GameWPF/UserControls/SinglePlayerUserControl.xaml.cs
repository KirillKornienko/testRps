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
    /// Логика взаимодействия для SinglePlayerUserControl.xaml
    /// </summary>
    public partial class SinglePlayerUserControl : UserControl
    {
        public event EventHandler BackToStartGameMenuClicked;
        public event EventHandler StartGameClicked;

        public SinglePlayerUserControl()
        {
            InitializeComponent();

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {


            StartGameClicked(this, null);
        }

        private void BackMenu_Click(object sender, RoutedEventArgs e)
        {
            BackToStartGameMenuClicked(this, null);
        }
    }
}
