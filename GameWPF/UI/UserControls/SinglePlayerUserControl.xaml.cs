using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using GameWPF.UserControls.Elements;

namespace GameWPF.UserControls
{
    /// <summary>
    /// Логика взаимодействия для SinglePlayerUserControl.xaml
    /// </summary>
    public partial class SinglePlayerUserControl : UserControl
    {
        public event EventHandler BackToStartGameMenuClicked;
        public event EventHandler StartGameClicked;
        public event EventHandler ReadyToGetMapList;

        public SinglePlayerUserControl()
        {
            InitializeComponent();

            Loaded += SinglePlayerUserControl_Loaded;
        }

        public void MapList_AddMap(MapUserControl element)
        {
            MapList.Children.Add(element);
        }

        public void MapList_Clear()
        {
            MapList.Children.Clear();
        }

        private void SinglePlayerUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ReadyToGetMapList(this, null);
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
