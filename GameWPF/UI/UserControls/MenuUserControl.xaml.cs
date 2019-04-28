using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
        private Dictionary<string, BitmapImage> get_BtmpImg;

        private Dictionary<Image, string> ImageKeysDict;


        public MenuUserControl()
        {
            InitializeComponent();

            Loaded += CurrentUserControl_Loaded;
        }

        private void CurrentUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitImageKeyDict();

            LoadStdSprites();

            EventsSubscription();
        }

        private void EventsSubscription()
        {
            foreach (var button_img in ImageKeysDict.Keys)
            {
                button_img.MouseEnter += (obj, e) => SetImage((Image)obj, MouseActions.Enter);
                button_img.MouseLeave += (obj, e) => SetImage((Image)obj, MouseActions.Leave);
                button_img.MouseLeftButtonDown += (obj, e) => SetImage((Image)obj, MouseActions.Down);
            }

            StartGame.MouseLeftButtonUp += StartGame_MouseUp;
            LoadGame.MouseLeftButtonUp += LoadGame_MouseUp;
            HallOfFame.MouseLeftButtonUp += HallOfFame_MouseUp;
            About.MouseLeftButtonUp += About_MouseUp;
            Quit.MouseLeftButtonUp += Quit_MouseUp;
        }


        private BitmapImage GetImage(Image button, MouseActions action)
        {
            string sprite_name = GetSpriteName(button, action);

            try
            {
                return get_BtmpImg[sprite_name];
            }
            catch (KeyNotFoundException)
            {
                return AddImage(sprite_name);
            }
        }

        private string GetSpriteName(Image button, MouseActions action)
        {
            char symbol;

            if (action == MouseActions.Enter)
                symbol = 'H';
            else if (action == MouseActions.Leave)
                symbol = 'N';
            else
                symbol = 'S';

            return ImageKeysDict[button] + symbol;
        }

        private BitmapImage AddImage(string filename)
        {
            BitmapImage src = new BitmapImage();

            src.BeginInit();
            src.UriSource = new Uri("pack://siteoforigin:,,,/data/LOC/sprite/" + filename + ".png");
            src.EndInit();

            get_BtmpImg.Add(filename, src);

            return src;
        }


        private void SetImage(Image sender, MouseActions action)
        {
            sender.Source = GetImage(sender, action);
        }


        private void StartGame_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SetImage((Image)sender, MouseActions.Leave);

            StartGameClicked();
        }

        private void LoadGame_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SetImage((Image)sender, MouseActions.Leave);

            LoadGameClicked();
        }

        private void HallOfFame_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SetImage((Image)sender, MouseActions.Leave);

            HallOfFameClicked();
        }

        private void About_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SetImage((Image)sender, MouseActions.Leave);


            throw new NotImplementedException();
        }

        private void Quit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SetImage((Image)sender, MouseActions.Leave);

            Application.Current.Shutdown();
        }



        private void LoadStdSprites()
        {
            foreach(var image in ImageKeysDict.Keys)
                SetImage(image, MouseActions.Leave);
        }

        private void InitImageKeyDict()
        {
            get_BtmpImg = new Dictionary<string, BitmapImage>();

            ImageKeysDict = new Dictionary<Image, string>();

            ImageKeysDict.Add(StartGame, "MMENUNG");
            ImageKeysDict.Add(LoadGame, "MMENULG");
            ImageKeysDict.Add(HallOfFame, "MMENUHS");
            ImageKeysDict.Add(About, "MMENUCR");
            ImageKeysDict.Add(Quit, "MMENUQT");
        }



        public event Action StartGameClicked;
        public event Action LoadGameClicked;
        public event Action HallOfFameClicked;
    }
}
