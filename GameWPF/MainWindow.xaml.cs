using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Settings = GameWPF.Properties.Settings;
using GameWPF.MenuActions;

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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            string text = "SizeChanged, prev: " + e.PreviousSize.Height + "x" + e.PreviousSize.Width + " cur: " + e.NewSize.Height + "x" + e.NewSize.Width;
            MessageBox.Show(text);
        }

        private void MainWindow_Loaded(object sender, EventArgs e)
        {
            SizeChanged += Window_SizeChanged;


            IActions menu_actions = new MainMenuActions();

            EventSubscription(menu_actions);

            menu_actions.Initialize();


            //VisibleInfo.Max_width = (int)grid.ActualWidth / 64;        // !!Сетка не изменяется при изменении разрешения!!
            //VisibleInfo.Max_height = (int)grid.ActualHeight / 64;

            //VisibleInfo.Player_position_y = VisibleInfo.Max_height / 2;
            //VisibleInfo.Player_position_x = VisibleInfo.Max_width / 2;        //предположим, что x всегда нечётный (1)

            //bitmapobj = new Bitmap((int)grid.ActualWidth, (int)grid.ActualHeight);
            //graphics = Graphics.FromImage(bitmapobj);

        }

        void EventSubscription(IActions menu_actions)
        {
            menu_actions.NewElement += NewElement;

            menu_actions.DeleteElements += DeleteElements;
        }

        private void NewElement(UserControl new_element)
        {
            grid.Children.Add(new_element);
        }

        private void DeleteElements()
        {
            grid.Children.Clear();
        }

    }
}
