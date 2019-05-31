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
    /// Логика взаимодействия для MainMenuUC4x3.xaml
    /// </summary>
    public partial class MainMenuUC4x3 : UserControl
    {
        public event Action Button1Clicked;
        public event Action Button2Clicked;
        public event Action Button3Clicked;
        public event Action Button4Clicked;
        public event Action Button5Clicked;

        public MenuType type { get; private set; }

        private Dictionary<string, BitmapImage> get_BtmpImg;

        private Dictionary<Image, string> ImageKeysDict;


        public MainMenuUC4x3()
        {
            InitializeComponent();

            type = MenuType.MainMenu;

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

            Button1.MouseLeftButtonUp += (obj, e) => Button1Clicked();
            Button2.MouseLeftButtonUp += (obj, e) => Button2Clicked();
            Button3.MouseLeftButtonUp += (obj, e) => Button3Clicked();
            Button4.MouseLeftButtonUp += (obj, e) => Button4Clicked();
            Button5.MouseLeftButtonUp += (obj, e) => Button5Clicked();
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

            string sa = $"fdsf {symbol}"

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

            foreach(var button in grid.Children.OfType<Image>())
            {
                ImageKeysDict.Add(button, null);
            }
        }

        /// <summary>
        /// Изменяет тип данного User Control, вызывает методы обновления изображений и событий
        /// </summary>
        /// <param name="type">Тип меню</param>
        public void UpdateType(MenuType type)
        {
            if (this.type != type)
            {
                this.type = type;

                UpdateImageKeys();

                LoadStdSprites();
            }
        }

        private void UpdateImageKeys()
        {
            switch (type)
            {
                case MenuType.MainMenu:
                    ImageKeysDict[Button1] = "MMENUNG";
                    ImageKeysDict[Button2] = "MMENULG";
                    ImageKeysDict[Button3] = "MMENUHS";
                    ImageKeysDict[Button4] = "MMENUCR";
                    ImageKeysDict[Button5] = "MMENUQT";
                    break;

                case MenuType.StartGame | MenuType.LoadGame:
                    ImageKeysDict[Button1] = "GTSINGL";
                    ImageKeysDict[Button2] = "GTMULTI";
                    ImageKeysDict[Button3] = "GTCAMPN";
                    ImageKeysDict[Button4] = "GTTUTOR";
                    ImageKeysDict[Button5] = "GTBACK";
                    break;

            }
        }
    }
}
