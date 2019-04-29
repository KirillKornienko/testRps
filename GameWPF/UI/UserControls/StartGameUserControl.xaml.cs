using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.Generic;

namespace GameWPF.UserControls
{
    /// <summary>
    /// Логика взаимодействия для StartGameUserControl.xaml
    /// </summary>
    public partial class StartGameUserControl : UserControl
    {
        public event Action SinglePlayerClicked;
        public event Action MultiplayerClicked;
        public event Action CampaignClicked;
        public event Action GuideClicked;
        public event Action BackToMainMenuClicked;


        private Dictionary<string, BitmapImage> get_BtmpImg;

        private Dictionary<Image, string> ImageKeysDict;


        public StartGameUserControl()
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
                button_img.MouseLeftButtonUp += (obj, e) => SetImage((Image)obj, MouseActions.Leave);
            }

            SinglePlayer.MouseLeftButtonUp += (obj, e) => SinglePlayerClicked();
            Multiplayer.MouseLeftButtonUp += (obj, e) => MultiplayerClicked();
            Campaign.MouseLeftButtonUp += (obj, e) => CampaignClicked();
            Guide.MouseLeftButtonUp += (obj, e) => GuideClicked();
            BackToMainMenu.MouseLeftButtonUp += (obj, e) => BackToMainMenuClicked();
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



        private void LoadStdSprites()
        {
            foreach (var image in ImageKeysDict.Keys)
                SetImage(image, MouseActions.Leave);
        }

        private void InitImageKeyDict()
        {
            get_BtmpImg = new Dictionary<string, BitmapImage>();

            ImageKeysDict = new Dictionary<Image, string>();

            ImageKeysDict.Add(SinglePlayer, "GTSINGL");
            ImageKeysDict.Add(Multiplayer, "GTMULTI");
            ImageKeysDict.Add(Campaign, "GTCAMPN");
            ImageKeysDict.Add(Guide, "GTTUTOR");
            ImageKeysDict.Add(BackToMainMenu, "GTBACK");
        }
    }
}
