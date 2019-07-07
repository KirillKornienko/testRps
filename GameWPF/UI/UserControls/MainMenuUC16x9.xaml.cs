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
    /// Логика взаимодействия для MainMenuUC16x9.xaml
    /// </summary>
    public partial class MainMenuUC16x9 : UserControl
    {
        public MainMenuUC16x9()
        {
            //grid_center.Children.Add(new MainMenuUC4x3());

            InitializeComponent();

            //Loaded += MainMenuUC16x9_Loaded;
        }

        private void MainMenuUC16x9_Loaded(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
