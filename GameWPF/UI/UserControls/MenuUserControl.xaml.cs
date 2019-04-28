using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

using System.IO;
using System.Collections.Generic;
using System.Windows.Input;

namespace GameWPF.UserControls
{
    /// <summary>
    /// Логика взаимодействия для MenuUserControl.xaml
    /// </summary>
    public partial class MenuUserControl : UserControl
    {
        private Dictionary<string, BitmapImage> get_image;

        private Dictionary<Button, string> ButtonToImageKey;

        private Dictionary<Button, Image> GetImageName;


        public MenuUserControl()
        {
            InitializeComponent();

            Loaded += MenuUserControl_Loaded;
        }

        private void MenuUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            get_image = new Dictionary<string, BitmapImage>();

            InitImageKeyDict();

            LoadSprites();

            EventsSubscription();
        }

        private void EventsSubscription()
        {
            StartGame.MouseEnter += Button_MouseEnter;
            StartGame.MouseLeave += Button_MouseLeave;
            StartGame.MouseDown += Button_MouseDown;
            StartGame.MouseUp += StartGame_MouseUp;

            LoadGame.MouseEnter += Button_MouseEnter;
            LoadGame.MouseLeave += Button_MouseLeave;
            LoadGame.MouseDown += Button_MouseDown;
            LoadGame.MouseUp += LoadGame_MouseUp;

            HallOfFame.MouseEnter += Button_MouseEnter;
            HallOfFame.MouseLeave += Button_MouseLeave;
            HallOfFame.MouseDown += Button_MouseDown;
            HallOfFame.MouseUp += HallOfFame_MouseUp;

            About.MouseEnter += Button_MouseEnter;
            About.MouseLeave += Button_MouseLeave;
            About.MouseDown += Button_MouseDown;
            About.MouseUp += About_MouseUp;

            Quit.MouseEnter += Button_MouseEnter;
            Quit.MouseLeave += Button_MouseLeave;
            Quit.MouseDown += Button_MouseDown;
            Quit.MouseUp += Quit_MouseUp;
        }


        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            GetImageName[(Button)sender].Source = GetImage((Button)sender, MouseActions.Enter);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            GetImageName[(Button)sender].Source = GetImage((Button)sender, MouseActions.Leave);
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            GetImageName[(Button)sender].Source = GetImage((Button)sender, MouseActions.Down);
        }


        private BitmapImage GetImage(Button button, MouseActions action)
        {
            string sprite_name = GetSpriteName(button, action);

            try
            {
                return get_image[sprite_name];
            }
            catch (KeyNotFoundException)
            {
                return AddImage(sprite_name);
            }
        }

        private string GetSpriteName(Button button, MouseActions action)
        {
            char symbol;

            if (action == MouseActions.Enter)
                symbol = 'H';
            else if (action == MouseActions.Leave)
                symbol = 'N';
            else
                symbol = 'S';

            return ButtonToImageKey[button] + symbol;
        }

        private BitmapImage AddImage(string filename)
        {
            BitmapImage src = new BitmapImage();

            src.BeginInit();
            src.UriSource = new Uri("pack://siteoforigin:,,,/data/LOC/sprite/" + filename + ".png");
            src.EndInit();

            get_image.Add(filename, src);

            return src;
        }



        private void StartGame_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((Button)sender).Content = GetImage((Button)sender, MouseActions.Leave);

            StartGameClicked();
        }

        private void LoadGame_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((Button)sender).Content = GetImage((Button)sender, MouseActions.Leave);

            LoadGameClicked();
        }

        private void HallOfFame_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((Button)sender).Content = GetImage((Button)sender, MouseActions.Leave);

            HallOfFameClicked();
        }

        private void About_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((Button)sender).Content = GetImage((Button)sender, MouseActions.Leave);


            throw new NotImplementedException();
        }

        private void Quit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((Button)sender).Content = GetImage((Button)sender, MouseActions.Leave);


            throw new NotImplementedException();
        }



        private void ExitGameClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoadSprites()
        {
            
        }

        private void InitImageKeyDict()
        {
            ButtonToImageKey = new Dictionary<Button, string>();

            ButtonToImageKey.Add(StartGame, "GTSINGL");
            ButtonToImageKey.Add(LoadGame, "MMENULG");
            ButtonToImageKey.Add(HallOfFame, "MMENUHS");
            ButtonToImageKey.Add(About, "MMENUCR");
            ButtonToImageKey.Add(Quit, "MMENUQT");


            GetImageName = new Dictionary<Button, Image>();
            GetImageName.Add(StartGame, StartGameImage);
            GetImageName.Add(LoadGame, LoadGameImage);
            GetImageName.Add(HallOfFame, HallOfFameImage);
            GetImageName.Add(About, AboutImage);
            GetImageName.Add(Quit, QuitImage);
        }



        public event Action StartGameClicked;
        public event Action LoadGameClicked;
        public event Action HallOfFameClicked;
    }
}
