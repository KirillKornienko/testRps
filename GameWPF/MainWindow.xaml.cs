using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Drawing;
using Settings = GameWPF.Properties.Settings;
using GameWPF.MenuActions;

using GameWPF.GameLogic;

namespace GameWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow() {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }



        private void MainWindow_Loaded(object sender, EventArgs e)
        {
            IActions menu_actions = new MainMenuActions();

            EventSubscription(menu_actions);

            menu_actions.Initialize();


            VisibleInfo.Max_width = (int)grid.ActualWidth / 64;        // !!Сетка не изменяется при изменении разрешения!!
            VisibleInfo.Max_height = (int)grid.ActualHeight / 64;

            VisibleInfo.Player_position_y = VisibleInfo.Max_height / 2;
            VisibleInfo.Player_position_x = VisibleInfo.Max_width / 2;        //предположим, что x всегда нечётный (1)

            //bitmapobj = new Bitmap((int)grid.ActualWidth, (int)grid.ActualHeight);
            //graphics = Graphics.FromImage(bitmapobj);

        }

        void EventSubscription(IActions menu_actions)
        {
            menu_actions.NewElement += NewElement;

            menu_actions.DeleteElements += DeleteElements;
        }

        private void NewElement(object sender, System.Windows.Controls.UserControl new_element)
        {
            grid.Children.Add(new_element);
        }

        private void DeleteElements(object sender, EventArgs e)
        {
            grid.Children.Clear();
        }

    }
}
