using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading;
using System.Windows;
using System.Drawing;
//using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using ImageDraw = System.Drawing;
using SETTINGS = Properties.Settings.Default;

namespace GameWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int width, height;
        bool displaicement;
        MapCellInfo[,] GlobalCellInfo;
        MapCellInfo[,] VisibleMap;
        Bitmap bitmapobj;
        Graphics graphics;
        Dictionary<string, ImageDraw.Image> Textures;
        Dictionary<string, CreatureInfo> ArmyInfo;
        Dictionary<string, HeroesInfo> HeroInfo;
        Dictionary<int, string> EnumMap;
        Font arial18 = new Font(new FontFamily("Arial"), 18);
        string map_filepath;

        //Игроки:
        Player MyPlayer;
        MapArmyInfo MyPlayerArmy;
        Player[] Allies = null;
        Player[] Enemies = null;


        int min_y_border, max_y_border, min_x_border, max_x_border;
        bool mouse_move;

        /*public static void ChangeText_element(Label obj, string text)
        {
            obj.Content = text;
        }
        */

        public MainWindow() {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, EventArgs e) {

            /*Button button = new Button();
            button.Width = 100;
            button.Height = 100;
            button.Content = "KGF";
            button.VerticalAlignment = VerticalAlignment.Top;
            button.Visibility = Visibility.Visible;
            button.HorizontalAlignment = HorizontalAlignment.Left;
            UniformGr.Children.Add(button);
            */
            Label1.Content = "Ширина: " + FirstColumn.ActualWidth;
            Label2.Content = "Высота: " + grid.ActualHeight;
            Label3.Content = "Количество по ширине " + FirstColumn.ActualWidth / 64;
            Label4.Content = "Количество по высоте " + grid.ActualHeight / 64;
            if ((int)FirstColumn.ActualWidth % 2 == 1) width = (int)FirstColumn.ActualWidth / 128 + 1;
            else width = ((int)FirstColumn.ActualWidth / 128);
            Label5.Content = "Клетка по ширине: " + width;
            if ((int)grid.ActualHeight % 2 == 1) height = (int)grid.ActualHeight / 128;
            else height = ((int)grid.ActualHeight / 128);
            Label6.Content = "Клетка по высоте: " + height;
            

            VisibleInfo.Max_width = (int)FirstColumn.ActualWidth / 64;        // !!Сетка не изменяется при изменении разрешения!!
            VisibleInfo.Max_height = (int)grid.ActualHeight / 64;

            VisibleInfo.Player_position_y = VisibleInfo.Max_height / 2;
            VisibleInfo.Player_position_x = VisibleInfo.Max_width / 2;        //предположим, что x всегда нечётный (1)

            bitmapobj = new Bitmap((int)FirstColumn.ActualWidth, (int)grid.ActualHeight);
            graphics = Graphics.FromImage(bitmapobj);

            MenuDemo();
        }

        private bool LoadTextures()
        {
            try
            {
                Textures = new Dictionary<string, ImageDraw.Image>();
                Textures.Add(Properties.Settings.Default.TEXTURES_BOG, ImageDraw.Image.FromFile("sprites/Bog.jpg"));
                Textures.Add("Player", ImageDraw.Image.FromFile("sprites/RedPlayer.png"));
                Textures.Add("Grs", ImageDraw.Image.FromFile("sprites/Grass.jpg"));
                Textures.Add("Grd", ImageDraw.Image.FromFile("sprites/Ground.jpg"));
                Textures.Add("Snd", ImageDraw.Image.FromFile("sprites/Sand.jpg"));
                Textures.Add("Snw", ImageDraw.Image.FromFile("sprites/Snow.jpg"));
                Textures.Add("Wtr", ImageDraw.Image.FromFile("sprites/Water.jpg"));
                Textures.Add("NULL", ImageDraw.Image.FromFile("sprites/Emptiness.jpg"));
                Textures.Add("Lav", ImageDraw.Image.FromFile("sprites/Lava.jpg"));

                Textures.Add("Cs0", ImageDraw.Image.FromFile("sprites/Towns/Cas0.png"));
                return true;
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("File " + e.Message + " not found.");
                return false;
            }
        }

        private void SetEvents()
        {
            BitmapImg.MouseMove += BitmapImg_MouseMove;
            BitmapImg.MouseLeftButtonDown += BitmapImg_MouseLeftButtonDown;
            PreviewTextInput += Window_PreviewTextInput;
        }

        public void MenuDemo()
        {
            EnumMap = new Dictionary<int, string>();

            int y = 0;
            graphics.DrawString("Maps:", arial18, Brushes.White, new PointF(0, 0));
            graphics.DrawString("Name:", arial18, Brushes.White, new PointF(250, 0));
            graphics.DrawString("Size:", arial18, Brushes.White, new PointF(500, 0));
            graphics.DrawString("Players:", arial18, Brushes.White, new PointF(750, 0));


            foreach (var s in GetMapList()) {
                var file = File.OpenText(s);
                graphics.DrawString(s.Split(new char[] { '\\' }).Last(), new Font(new FontFamily("Arial"), 16), Brushes.White, new PointF(0, y += 20));
                graphics.DrawString(file.ReadLine(), new Font(new FontFamily("Arial"), 16), Brushes.White, new PointF(250, y));
                graphics.DrawString(file.ReadLine(), new Font(new FontFamily("Arial"), 16), Brushes.White, new PointF(500, y));
                graphics.DrawString(file.ReadLine(), new Font(new FontFamily("Arial"), 16), Brushes.White, new PointF(750, y));
                EnumMap.Add(y / 20, s);     //"y" начинается с 1
                file.Close();
            }
            graphics.Flush();

            BitmapImg.MouseLeftButtonDown += MouseClickMenu;

            BitmapImg.Source = BitmapToImageSource(bitmapobj);
        }

        public List<string> GetMapList()
        {
            try
            {
                string directory = Properties.Settings.Default.MAPS_DIRECTORY_NAME;
                List<string> maps = new List<string>();

                foreach (var file in Directory.GetFiles(directory))
                {
                    if (Path.GetExtension(file) == Properties.Settings.Default.MAPS_EXTENSION)
                        maps.Add(file);
                }
                maps.Reverse();
                return maps;
            }
            catch(Exception e)
            {
                MessageBox.Show("Error loading map list. \n" + e.Message);
                throw new Exception();
            }
        }

        private void MouseClickMenu(object sender, MouseButtonEventArgs e)
        {
            var x = (int) e.GetPosition(BitmapImg).X;
            var y = (int) e.GetPosition(BitmapImg).Y;

            ClickX.Content = x;
            ClickY.Content = y;

            int posY = y / 20;
            if (posY <= EnumMap.Count && posY != 0)
            {
                BitmapImg.MouseLeftButtonDown -= MouseClickMenu;

                MapMenu(EnumMap[posY]);
            }

        }

        public void MapMenu(string filepath)
        {
            map_filepath = filepath;
            BitmapImg.MouseLeftButtonDown += MouseClickMapMenu;

            var file = File.OpenText(filepath);
            string[] MapSettings;
            MapInfo.MapName = file.ReadLine();
            MapSettings = file.ReadLine().Split('x');
            MapInfo.Max_width = Convert.ToInt32(MapSettings[0]);
            MapInfo.Max_height = Convert.ToInt32(MapSettings[1]);
            MapSettings = file.ReadLine().Split('v');
            MapInfo.Num_of_allies = Convert.ToInt32(MapSettings[0]) - 1;
            MapInfo.Num_of_enemies = Convert.ToInt32(MapSettings[1]);
            MapSettings = file.ReadLine().Split('x');
            MapInfo.Player_position_x = Convert.ToInt32(MapSettings[0]);
            MapInfo.Player_position_y = Convert.ToInt32(MapSettings[1]);
            var xex = file.ReadLine();
            MapInfo.Info = xex.Remove(0, 1).Remove(xex.Length - 2, 1);
            MapInfoLabel.Content = MapInfo.Info;

            Allies = new Player[MapInfo.Num_of_allies];
            Enemies = new Player[MapInfo.Num_of_enemies];

            MyPlayer = new Player("ИМЯ", BaseTown.Castle, Color.Green, 10000, 20, 20, 10, 10, 10, 10);
            ArmyInfo = new Dictionary<string, CreatureInfo>();
            ArmyInitialized();
            MyPlayerArmy = new MapArmyInfo(MyPlayer, new HeroesInfo[] { AdelaInitialized() });
            
            for (int i = 0; i < Allies.Length; i++)
            {
                Allies[i] = new Player("Allies " + i, BaseTown.Castle, Color.Blue, 10000, 20, 20, 10, 10, 10, 10);
            }
            for( int i = 0; i < Enemies.Length; i++)
            {
                Enemies[i] = new Player("Enemies " + i, BaseTown.Castle, Color.Red, 10000, 20, 20, 10, 10, 10, 10);
            }

            RenderMapMenu();

        }

        private void MouseClickMapMenu(object sender, MouseButtonEventArgs e)
        {

            BitmapImg.MouseLeftButtonDown -= MouseClickMapMenu;

            LoadMap(File.OpenText(map_filepath).ReadToEnd().Split('{').Last().Remove(0, 2).Split('|').ToArray());

        }

        private void RenderMapMenu()
        {
            graphics.Clear(Color.Black);
            int y = 0;
            graphics.DrawString("Name:", arial18, Brushes.White, new PointF(0, 0));
            graphics.DrawString("Starting town:", arial18, Brushes.White, new PointF(250, 0));
            graphics.DrawString("Starting hero:", arial18, Brushes.White, new PointF(500, 0));
            graphics.DrawString("Colour:", arial18, Brushes.White, new PointF(750, 0));
            graphics.DrawString("Resources:", arial18, Brushes.White, new PointF(1000, 0));

            y += 20;
            //player
            graphics.DrawString("Player", arial18, Brushes.White, new PointF(0, y));
            graphics.DrawString("Castle", arial18, Brushes.White, new PointF(250, y));
            graphics.DrawString("Adela", arial18, Brushes.White, new PointF(500, y));
            graphics.DrawString("Blue", arial18, Brushes.White, new PointF(750, y));
            graphics.DrawString("Medium", arial18, Brushes.White, new PointF(1000, y));


            for (int i = 0; i< MapInfo.Num_of_allies; i++)
            {
                if (i == 0) graphics.DrawString("Allies:", arial18, Brushes.White, new PointF(0, y+=20));
                y += 20;
                graphics.DrawString("Allies" + i, arial18, Brushes.White, new PointF(0, y));
                graphics.DrawString("NoCastle", arial18, Brushes.White, new PointF(250, y));
                graphics.DrawString("Adela", arial18, Brushes.White, new PointF(500, y));
                graphics.DrawString("Blue", arial18, Brushes.White, new PointF(750, y));
            }

            for (int i = 0; i < MapInfo.Num_of_enemies; i++)
            {
                if (i == 0) graphics.DrawString("Enemies:", arial18, Brushes.White, new PointF(0, y += 20));
                y += 20;
                graphics.DrawString("Enemies" + i, arial18, Brushes.White, new PointF(0, y));
                graphics.DrawString("NoCastle", arial18, Brushes.White, new PointF(250, y));
                graphics.DrawString("Adela", arial18, Brushes.White, new PointF(500, y));
                graphics.DrawString("Blue", arial18, Brushes.White, new PointF(750, y));
            }

            graphics.Flush();

            BitmapImg.Source = BitmapToImageSource(bitmapobj);

        }

        public void LoadMap(string[] GlobalMap)
        {
            //GlobalMap.

            GlobalCellInfo = new MapCellInfo[MapInfo.Max_width, MapInfo.Max_height];
            for (int y = 0; y < MapInfo.Max_height; y++)
            {
                for (int x = 0; x < MapInfo.Max_width; x++)
                {
                    if (GlobalMap[y * MapInfo.Max_width + x].Split(',').Length == 1)
                        GlobalCellInfo[x, y] = new MapCellInfo((SurfaceTypes)Enum.Parse(typeof(SurfaceTypes), GlobalMap[y * MapInfo.Max_width + x]));
                    else
                    {
                        var tmp = GlobalMap[y * MapInfo.Max_width + x].Split(',');
                        Buildings building;
                        Mobs mob;
                        Items item;
                        if (Enum.TryParse(tmp[1], out building))
                            GlobalCellInfo[x, y] = new MapCellInfo((SurfaceTypes)Enum.Parse(typeof(SurfaceTypes), tmp[0]), building);
                        else if (Enum.TryParse(tmp[1], out mob))
                            GlobalCellInfo[x, y] = new MapCellInfo((SurfaceTypes)Enum.Parse(typeof(SurfaceTypes), tmp[0]), mob);
                        else if (Enum.TryParse(tmp[1], out item))
                            GlobalCellInfo[x, y] = new MapCellInfo((SurfaceTypes)Enum.Parse(typeof(SurfaceTypes), tmp[0]), item);
                    }
                }
            }

            min_y_border = (int)grid.ActualHeight / 11;
            max_y_border = (int)grid.ActualHeight * 10 / 11;
            min_x_border = (int)FirstColumn.ActualWidth / 11;
            max_x_border = (int)FirstColumn.ActualWidth * 10 / 11;
            mouse_move = false;

            if (!LoadTextures())
            {
                BitmapImg.MouseLeftButtonDown += MouseClickMenu;
                return;
            }
            SetEvents();
            Game();
            Rendering();

            //Player:
            PlayerInfo.Standart_move = PlayerInfo.Max_move = 100;
            PlayerInfo.Move = PlayerInfo.Max_move;

        }

        public unsafe void LoadMapUnsafe(string[] GlobalMap)
        {
            //GlobalMap.

            GlobalCellInfo = new MapCellInfo[MapInfo.Max_width, MapInfo.Max_height];
            for (int y = 0; y < MapInfo.Max_height; y++)
            {
                for (int x = 0; x < MapInfo.Max_width; x++)
                {
                    if (GlobalMap[y * MapInfo.Max_width + x].Split(',').Length == 1)
                        GlobalCellInfo[x, y] = new MapCellInfo((SurfaceTypes)Enum.Parse(typeof(SurfaceTypes), GlobalMap[y * MapInfo.Max_width + x]));
                    else
                    {
                        var tmp = GlobalMap[y * MapInfo.Max_width + x].Split(',');
                        Buildings building;
                        Mobs mob;
                        Items item;
                        if (Enum.TryParse(tmp[1], out building))
                            GlobalCellInfo[x, y] = new MapCellInfo((SurfaceTypes)Enum.Parse(typeof(SurfaceTypes), tmp[0]), building);
                        else if (Enum.TryParse(tmp[1], out mob))
                            GlobalCellInfo[x, y] = new MapCellInfo((SurfaceTypes)Enum.Parse(typeof(SurfaceTypes), tmp[0]), mob);
                        else if (Enum.TryParse(tmp[1], out item))
                            GlobalCellInfo[x, y] = new MapCellInfo((SurfaceTypes)Enum.Parse(typeof(SurfaceTypes), tmp[0]), item);
                    }
                }
            }

            min_y_border = (int)grid.ActualHeight / 11;
            max_y_border = (int)grid.ActualHeight * 10 / 11;
            min_x_border = (int)FirstColumn.ActualWidth / 11;
            max_x_border = (int)FirstColumn.ActualWidth * 10 / 11;
            mouse_move = false;

            if (!LoadTextures())
            {
                BitmapImg.MouseLeftButtonDown += MouseClickMenu;
                return;
            }
            SetEvents();
            Game();
            Rendering();

            //Player:
            PlayerInfo.Standart_move = PlayerInfo.Max_move = 100;
            PlayerInfo.Move = PlayerInfo.Max_move;

        }

        public void Game()
        {
            VisibleMap = new MapCellInfo[VisibleInfo.Max_width, VisibleInfo.Max_height];
            VisibleInfo.Camera_position_x = MapInfo.Player_position_x;
            VisibleInfo.Camera_position_y = MapInfo.Player_position_y;

            for (int y = 0; y < VisibleInfo.Max_height; y++)
                for (int x = 0; x < VisibleInfo.Max_width; x++)
                    EnumMas(VisibleMap, x, y);
        }

        private void Rendering()
        {
            for (int y = 0; y < VisibleInfo.Max_height * 64; y += 64)
            {
                for (int x = 0; x < VisibleInfo.Max_width * 64; x += 64)
                {
                    graphics.DrawImage(Textures[VisibleMap[x / 64, y / 64].SurfaceType.ToString()], new PointF(x, y));

                    if (VisibleMap[x / 64, y / 64].Mob != Mobs.NULL)
                        graphics.DrawImage(Textures[VisibleMap[x / 64, y / 64].Mob.ToString()], new PointF(x, y));

                    if (VisibleMap[x / 64, y / 64].Building != Buildings.NULL)
                        graphics.DrawImage(Textures[VisibleMap[x / 64, y / 64].Building.ToString()], new PointF(x, y));
                }
            }
            graphics.Flush();

            BitmapImg.Source = BitmapToImageSource(bitmapobj);
            Coordinate.Content = "x: " + MapInfo.Player_position_x + " y: " + MapInfo.Player_position_y;
            GC.Collect();
        }

        char keychar = ' ';
        int zi = 0;
        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var tmp = e.Text[0];
            if (tmp != keychar)
            {
                keychar = tmp;
                zi = 0;
                LabelKeyInfo.Content = e.Text;
                LabelKeyCount.Content = zi;
            }
            else
                LabelKeyCount.Content = ++zi;

            if ((tmp == 'w' || tmp =='W') && MapInfo.Player_position_y > 1) KeyPress('w');
            else if ((tmp == 's' || tmp =='S') && MapInfo.Player_position_y < MapInfo.Max_height) KeyPress('s');
            else if ((tmp == 'a' || tmp =='A') && MapInfo.Player_position_x > 1) KeyPress('a');
            else if ((tmp == 'd' || tmp =='D') && MapInfo.Player_position_x < MapInfo.Max_width) KeyPress('d');
            //диагональ
            else if ((tmp == 'q' || tmp == 'Q') && MapInfo.Player_position_y > 1 && MapInfo.Player_position_x > 1) KeyPress('q');
            else if ((tmp == 'e' || tmp == 'E') && MapInfo.Player_position_y > 1 && MapInfo.Player_position_x < MapInfo.Max_width) KeyPress('e');
            else if ((tmp == 'c' || tmp == 'C') && MapInfo.Player_position_x < MapInfo.Max_width && MapInfo.Player_position_y < MapInfo.Max_height) KeyPress('c');
            else if ((tmp == 'z' || tmp == 'Z') && MapInfo.Player_position_y < MapInfo.Max_height && MapInfo.Player_position_x > 1) KeyPress('z');
        }

        public void KeyPress(char symbol)
        {
            double diagonal_coff = 1;
            if (displaicement) {
                Game();
                displaicement = false;
            }

            if (symbol == 'w' && PlayerInfo.Move >= VisibleMap[VisibleInfo.Player_position_x, VisibleInfo.Player_position_y - 1].Patency)
            {
                GlobalCellInfo[MapInfo.Player_position_x - 1, MapInfo.Player_position_y - 1].Mob = Mobs.NULL;
                MapInfo.Player_position_y--;
            }
            else if (symbol == 's' && PlayerInfo.Move >= VisibleMap[VisibleInfo.Player_position_x, VisibleInfo.Player_position_y + 1].Patency)
            {
                GlobalCellInfo[MapInfo.Player_position_x - 1, MapInfo.Player_position_y - 1].Mob = Mobs.NULL;
                MapInfo.Player_position_y++;
            }
            else if (symbol == 'a' && PlayerInfo.Move >= VisibleMap[VisibleInfo.Player_position_x - 1, VisibleInfo.Player_position_y].Patency)
            {
                GlobalCellInfo[MapInfo.Player_position_x - 1, MapInfo.Player_position_y - 1].Mob = Mobs.NULL;
                MapInfo.Player_position_x--;
            }
            else if (symbol == 'd' && PlayerInfo.Move >= VisibleMap[VisibleInfo.Player_position_x + 1, VisibleInfo.Player_position_y].Patency)
            {
                GlobalCellInfo[MapInfo.Player_position_x - 1, MapInfo.Player_position_y - 1].Mob = Mobs.NULL;
                MapInfo.Player_position_x++;
            }
            //Угловые:
            else if (symbol == 'q' && PlayerInfo.Move >= VisibleMap[VisibleInfo.Player_position_x - 1, VisibleInfo.Player_position_y - 1].Patency * 1.4)
            {
                GlobalCellInfo[MapInfo.Player_position_x - 1, MapInfo.Player_position_y - 1].Mob = Mobs.NULL;
                MapInfo.Player_position_y--;
                MapInfo.Player_position_x--;
                diagonal_coff = 1.4;
            }
            else if (symbol == 'e' && PlayerInfo.Move >= VisibleMap[VisibleInfo.Player_position_x + 1, VisibleInfo.Player_position_y - 1].Patency * 1.4)
            {
                GlobalCellInfo[MapInfo.Player_position_x - 1, MapInfo.Player_position_y - 1].Mob = Mobs.NULL;
                MapInfo.Player_position_y--;
                MapInfo.Player_position_x++;
                diagonal_coff = 1.4;
            }
            else if (symbol == 'c' && PlayerInfo.Move >= VisibleMap[VisibleInfo.Player_position_x + 1, VisibleInfo.Player_position_y + 1].Patency * 1.4)
            {
                GlobalCellInfo[MapInfo.Player_position_x - 1, MapInfo.Player_position_y - 1].Mob = Mobs.NULL;
                MapInfo.Player_position_y++;
                MapInfo.Player_position_x++;
                diagonal_coff = 1.4;
            }
            else if (symbol == 'z' && PlayerInfo.Move >= VisibleMap[VisibleInfo.Player_position_x - 1, VisibleInfo.Player_position_y + 1].Patency * 1.4)
            {
                GlobalCellInfo[MapInfo.Player_position_x - 1, MapInfo.Player_position_y - 1].Mob = Mobs.NULL;
                MapInfo.Player_position_y++;
                MapInfo.Player_position_x--;
                diagonal_coff = 1.4;
            }
            else return;

            MapCellInfo[,] tmp = new MapCellInfo[VisibleInfo.Max_width, VisibleInfo.Max_height];
            GlobalCellInfo[MapInfo.Player_position_x - 1, MapInfo.Player_position_y - 1].Mob = Mobs.Player;
            VisibleMap[VisibleInfo.Player_position_x, VisibleInfo.Player_position_x].Mob = Mobs.NULL;
            VisibleInfo.Camera_position_x = MapInfo.Player_position_x;
            VisibleInfo.Camera_position_y = MapInfo.Player_position_y;

            for (int y = 0; y < VisibleInfo.Max_height; y++)
            {
                for (int x = 0; x < VisibleInfo.Max_width; x++)
                {
                    if (symbol == 'w' && y != 0)
                        tmp[x, y] = VisibleMap[x, y - 1];
                    else if (symbol == 's' && y < VisibleInfo.Max_height - 1)
                        tmp[x, y] = VisibleMap[x, y + 1];
                    else if (symbol == 'a' && x != 0)
                        tmp[x, y] = VisibleMap[x - 1, y];
                    else if (symbol == 'd' && x < VisibleInfo.Max_width - 1)
                        tmp[x, y] = VisibleMap[x + 1, y];
                    else
                        EnumMas(tmp, x, y);
                }
            }
            tmp[VisibleInfo.Player_position_x, VisibleInfo.Player_position_y].Mob = Mobs.Player;
            VisibleMap = tmp;
            PlayerInfo.Move -= VisibleMap[VisibleInfo.Player_position_x, VisibleInfo.Player_position_y].Patency * diagonal_coff;
            Rendering();
            Label_turn.Content = 100d / PlayerInfo.Max_move * PlayerInfo.Move;
            ProgressBar_Turn.Value = 100d / PlayerInfo.Max_move * PlayerInfo.Move;
        }

        int? tmp_x = null;
        int? tmp_y = null;
        WaySelection way;
        private void BitmapImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var x = (int)e.GetPosition(BitmapImg).X / 64 - VisibleInfo.Player_position_x;
            var y = (int)e.GetPosition(BitmapImg).Y / 64 - VisibleInfo.Player_position_y;
            int X_way = x + VisibleInfo.Camera_position_x - MapInfo.Player_position_x;
            int Y_way = y + VisibleInfo.Camera_position_y - MapInfo.Player_position_y;

            if (MapInfo.Player_position_x + X_way > 0 && MapInfo.Player_position_y + Y_way > 0 &&
                MapInfo.Player_position_x + X_way < MapInfo.Max_width + 1 && MapInfo.Player_position_y + Y_way < MapInfo.Max_height + 1) {
                if ((tmp_x == null || tmp_x != x) || (tmp_y == null || tmp_y != y))
                {
                    way = new WaySelection(x, y);
                    way.GetWay();
                    tmp_x = x;
                    tmp_y = y;
                }
                else if (tmp_x == x && tmp_y == y)
                {
                    while (way.X_way != 0 || way.Y_way != 0)
                    {
                        if (way.X_way > 0 && way.Y_way > 0)
                        {
                            KeyPress('c');
                            way.X_way--;
                            way.Y_way--;
                        }
                        else if (way.X_way > 0 && way.Y_way < 0)
                        {
                            KeyPress('e');
                            way.X_way--;
                            way.Y_way++;
                        }
                        else if (way.X_way < 0 && way.Y_way > 0)
                        {
                            KeyPress('z');
                            way.X_way++;
                            way.Y_way--;
                        }
                        else if (way.X_way < 0 && way.Y_way < 0)
                        {
                            KeyPress('q');
                            way.Y_way++;
                            way.X_way++;
                        }
                        //Without diagonal
                        else if (way.X_way > 0)
                        {
                            KeyPress('d');
                            way.X_way--;

                        }
                        else if (way.X_way < 0)
                        {
                            KeyPress('a');
                            way.X_way++;

                        }
                        else if (way.Y_way > 0)
                        {
                            KeyPress('s');
                            way.Y_way--;

                        }
                        else if (way.Y_way < 0)
                        {
                            KeyPress('w');
                            way.Y_way++;

                        }
                    }
                    tmp_x = null;
                    tmp_y = null;
                }
                ClickX.Content = way.X_way;
                ClickY.Content = way.Y_way;
            }
        }

        private void NextTurn_Click(object sender, RoutedEventArgs e)
        {
            PlayerInfo.Move = PlayerInfo.Max_move;
            ProgressBar_Turn.Value = 100;
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            BitmapImg.MouseMove -= BitmapImg_MouseMove;

            BitmapImg.MouseLeftButtonDown -= BitmapImg_MouseLeftButtonDown;

            PreviewTextInput -= Window_PreviewTextInput;

            graphics.Clear(Color.Black);
            MenuDemo();
            
        }

        private void SaveMap_Click(object sender, RoutedEventArgs e)
        {
            var file = new StreamWriter(File.Create("saves/save_map.sav"));
            file.WriteLine(MapInfo.MapName);
            file.WriteLine(MapInfo.Max_width + "x" + MapInfo.Max_height);
            file.WriteLine((MapInfo.Num_of_allies + 1) + "v" + MapInfo.Num_of_enemies);
            file.WriteLine(MapInfo.Player_position_x + "x" + MapInfo.Player_position_y);
            file.WriteLine("<" + MapInfo.Info + ">");
            file.WriteLine("x.y{");
            for(int y = 0; y<MapInfo.Max_height; y++)
            {
                for (int x = 0; x < MapInfo.Max_width; x++)
                {
                    string mob = GlobalCellInfo[x, y].Mob.ToString() != "NULL" ? "," + GlobalCellInfo[x, y].Mob : "";
                    string construct = GlobalCellInfo[x, y].Building.ToString() != "NULL" ? "," + GlobalCellInfo[x, y].Building : "";
                    string item = GlobalCellInfo[x, y].Item.ToString() != "NULL" ? "," + GlobalCellInfo[x, y].Item : "";
                    file.Write(GlobalCellInfo[x, y].SurfaceType.ToString() + mob + construct + item + '|');
                }
            }
            file.Close();
        }

        private void BitmapImg_MouseMove(object sender, MouseEventArgs e)
        {
            var positionY = e.GetPosition(BitmapImg).Y;
            var positionX = e.GetPosition(BitmapImg).X;
            Label7.Content = positionX;
            Label8.Content = positionY;

            if (positionY < min_y_border || positionY > max_y_border ||
                positionX < min_x_border || positionX > max_x_border)
            {
                //Ограничения
                if (positionX < min_x_border && VisibleInfo.Camera_position_x == 0) return;
                else if (positionX > max_x_border && VisibleInfo.Camera_position_x == MapInfo.Max_width) return;
                else if (positionY < min_y_border && VisibleInfo.Camera_position_y == 0) return;
                else if (positionY > max_y_border && VisibleInfo.Camera_position_y == MapInfo.Max_height) return;

                mouse_move = !mouse_move;
                if (mouse_move)
                {
                    MapCellInfo[,] tmp = new MapCellInfo[VisibleInfo.Max_width, VisibleInfo.Max_height];
                    displaicement = true;

                    if (positionY < min_y_border)
                        VisibleInfo.Camera_position_y--;
                    if (positionY > max_y_border)
                        VisibleInfo.Camera_position_y++;
                    if (positionX < min_x_border)
                        VisibleInfo.Camera_position_x--;
                    if (positionX > max_x_border)
                        VisibleInfo.Camera_position_x++;

                    for (int y = 0; y < VisibleInfo.Max_height; y++)
                    {
                        for (int x = 0; x < VisibleInfo.Max_width; x++)
                        {
                            if (positionY < min_y_border && positionX < min_x_border && x != 0 && y != 0)
                                tmp[x, y] = VisibleMap[x - 1, y - 1];
                            else if (positionY < min_y_border && positionX > max_x_border && x < VisibleInfo.Max_width - 1 && y != 0)
                                tmp[x, y] = VisibleMap[x + 1, y - 1];
                            else if (positionY > max_y_border && positionX < min_x_border && x != 0 && y < VisibleInfo.Max_height - 1)
                                tmp[x, y] = VisibleMap[x - 1, y + 1];
                            else if (positionY > max_y_border && positionX > max_x_border && x < VisibleInfo.Max_width - 1 && y < VisibleInfo.Max_height - 1)
                                tmp[x, y] = VisibleMap[x + 1, y + 1];

                            else if (positionY < min_y_border && y != 0 && positionX > min_x_border && positionX < max_x_border)
                                tmp[x, y] = VisibleMap[x, y - 1];
                            else if (positionY > max_y_border && y < VisibleInfo.Max_height - 1 && positionX > min_x_border && positionX < max_x_border)
                                tmp[x, y] = VisibleMap[x, y + 1];
                            else if (positionX < min_x_border && x != 0 && positionY > min_y_border && positionY < max_y_border)
                                tmp[x, y] = VisibleMap[x - 1, y];
                            else if (positionX > max_x_border && x < VisibleInfo.Max_width - 1 && positionY > min_y_border && positionY < max_y_border)
                                tmp[x, y] = VisibleMap[x + 1, y];
                            else
                                EnumMas(tmp, x, y);
                        }
                    }
                    VisibleMap = tmp;
                    Rendering();
                    CamX.Content = VisibleInfo.Camera_position_x;
                    CamY.Content = VisibleInfo.Camera_position_y;
                }
            }
            else
                mouse_move = false;
        }

        private void EnumMas(MapCellInfo[,] tmp, int x, int y)
        {
            if (VisibleInfo.Camera_position_y - 1 >= VisibleInfo.Max_height / 2 - y)
            {
                if (VisibleInfo.Camera_position_x - 1 >= VisibleInfo.Max_width / 2 - x)
                {
                    if (MapInfo.Max_width > VisibleInfo.Camera_position_x - VisibleInfo.Max_width / 2 + x - 1 &&
                        MapInfo.Max_height > VisibleInfo.Camera_position_y - VisibleInfo.Max_height / 2 + y - 1)
                    {
                        tmp[x, y] = GlobalCellInfo[VisibleInfo.Camera_position_x - VisibleInfo.Max_width / 2 + x - 1,
                            VisibleInfo.Camera_position_y - VisibleInfo.Max_height / 2 + y - 1];
                    }
                    else
                        tmp[x, y] = new MapCellInfo();
                }
                else
                    tmp[x, y] = new MapCellInfo();
            }
            else
                tmp[x, y] = new MapCellInfo();
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap) {
            using (MemoryStream memory = new MemoryStream()) {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }

        void Load(string filepath)
        {
            map_filepath = filepath;
            //BitmapImg.MouseLeftButtonDown += MouseClickMapMenu;

            var file = File.OpenText(filepath);
            string[] MapSettings;
            MapInfo.MapName = file.ReadLine();
            MapSettings = file.ReadLine().Split('x');
            MapInfo.Max_width = Convert.ToInt32(MapSettings[0]);
            MapInfo.Max_height = Convert.ToInt32(MapSettings[1]);
            MapSettings = file.ReadLine().Split('v');
            MapInfo.Num_of_allies = Convert.ToInt32(MapSettings[0]) - 1;
            MapInfo.Num_of_enemies = Convert.ToInt32(MapSettings[1]);
             


        }
    }
    struct MapInfo
    {
        public static string MapName;
        public static string Info;
        public static int Max_width;
        public static int Max_height;
        public static int Player_position_x;
        public static int Player_position_y;
        public static int Num_of_allies;
        public static int Num_of_enemies;


    }
    struct VisibleInfo
    {
        public static int Max_width;
        public static int Max_height;
        public static int Player_position_x;
        public static int Player_position_y;
        public static int Camera_position_x;
        public static int Camera_position_y;
    }
    struct PlayerInfo
    {
        public static Player player;
        public static double Max_move;     //Максимальная дальность ходьбы на 1 ход
        public static double Standart_move;        //Максимальная дальность хода для героя (стандартная)
        public static double Move;
    }
    class MapCellInfo
    {
        public MapCellInfo() { }
        public MapCellInfo(SurfaceTypes SurfaceType)
        {
            this.SurfaceType = SurfaceType;
            Patency_Method();
        }
        public MapCellInfo(SurfaceTypes SurfaceType, Buildings Building)
        {
            this.SurfaceType = SurfaceType;
            this.Building = Building;
            Patency_Method();
        }
        public MapCellInfo(SurfaceTypes SurfaceType, Mobs Mob)
        {
            this.SurfaceType = SurfaceType;
            this.Mob = Mob;
            Patency_Method();
        }
        public MapCellInfo(SurfaceTypes SurfaceType, Items Item)
        {
            this.SurfaceType = SurfaceType;
            this.Item = Item;
            Patency_Method();
        }
        public SurfaceTypes SurfaceType { get; private set; }
        public Buildings Building { get; private set; }
        public Mobs Mob { get; set; }
        public Items Item { get; set; }
        public byte Patency;
        public bool? pl0;

        private void Patency_Method()
        {
            if (SurfaceType == SurfaceTypes.NULL) Patency = 255;
            else if (SurfaceType == SurfaceTypes.Snd) Patency = 2;
            else if (SurfaceType == SurfaceTypes.Wtr) Patency = 10;
            else if (SurfaceType == SurfaceTypes.Snw) Patency = 5;
            else if (SurfaceType == SurfaceTypes.Bog) Patency = 15;
            else Patency = 1;                                            // Ground & Grass

            if (Building != Buildings.NULL) Patency = 255;

        }
    }
    class WaySelection
    {
        public WaySelection(int X, int Y)
        {
            X_position = X;
            Y_position = Y;
        }
        public int X_way;
        public int Y_way;
        public int X_position { get; set; }
        public int Y_position { get; set; }

        public void GetWay()
        {
            X_way = X_position + VisibleInfo.Camera_position_x - MapInfo.Player_position_x;
            Y_way = Y_position + VisibleInfo.Camera_position_y - MapInfo.Player_position_y;
            
        }

    }
    class Player
    {
        public Player(string name, BaseTown basetown, Color color, int gold, int wood, int ore, int mercury, int sulfur, int crystal, int gems)
        {
            this.name = name;
            this.basetown = basetown;
            this.color = color;
            this.gold = gold;
            this.wood = wood;
            this.ore = ore;
            this.mercury = mercury;
            this.sulfur = sulfur;
            this.crystal = crystal;
            this.gems = gems;

        }

        public string name { get; set; }
        public Color color { get; set; }
        public BaseTown basetown { get; set; }
        public int gold { get; set; }
        public int wood { get; set; }
        public int ore { get; set; }
        public int mercury { get; set; }
        public int sulfur { get; set; }
        public int crystal { get; set; }
        public int gems { get; set; }

    }
    // Информация о всех героях у игрока
    class MapArmyInfo
    {
        public MapArmyInfo(Player player, HeroesInfo[] heroes)
        {
            this.player = player;
            this.heroes = heroes;
        }

        public Player player { get; set; }
        public HeroesInfo[] heroes { get; set; }
    }
    class TownInfo
    {
        public TownInfo(string name, Player player, BaseTown town, bool port)
        {
            this.name = name;
            this.player = player;
            this.town = town;

            load_std_params(town, port);
        }

        public TownInfo(string name, Player player, BaseTown town, 
            byte hall, byte hallmax,
            byte kingdom, byte kingdommax,
            byte tavern, byte tavernmax,
            byte forge, byte forgemax,
            byte market, byte marketmax,
            byte magic, byte magicmax,
            byte port, byte portmax,
            byte spec1, byte spec1max,
            byte spec2, byte spec2max,
            byte spec3, byte spec3max,
            byte lvl1, byte lvl1max,
            byte lvl2, byte lvl2max,
            byte lvl3, byte lvl3max,
            byte lvl4, byte lvl4max,
            byte lvl5, byte lvl5max,
            byte lvl6, byte lvl6max,
            byte lvl7, byte lvl7max, HeroesInfo armytown)
        {
            this.name = name;
            this.player = player;
            this.town = town;
            this.hall = hall;
            this.hallmax = hallmax;
            this.kingdom = kingdom;
            this.kingdommax = kingdommax;
            this.tavern = tavern;
            this.tavernmax = tavernmax;
            this.forge = forge;
            this.forgemax = forgemax;
            this.market = market;
            this.marketmax = marketmax;
            this.magic = magic;
            this.magicmax = magicmax;
            this.port = port; //or spec
            this.portmax = portmax;
            this.spec1 = spec1;
            this.spec1max = spec1max;
            this.spec2 = spec2;
            this.spec2max = spec2max;
            this.spec3 = spec3;
            this.spec3max = spec3max;
            this.lvl1 = lvl1;
            this.lvl1max = lvl1max;
            this.lvl2 = lvl2;
            this.lvl2max = lvl2max;
            this.lvl3 = lvl3;
            this.lvl3max = lvl3max;
            this.lvl4 = lvl4;
            this.lvl4max = lvl4max;
            this.lvl5 = lvl5;
            this.lvl5max = lvl5max;
            this.lvl6 = lvl6;
            this.lvl6max = lvl6max;
            this.lvl7 = lvl7;
            this.lvl7max = lvl7max;
            this.armytown = armytown;

        }

        public string name { get; set; }
        public Player player { get; set; }
        public BaseTown town { get; set; }
        public byte hall { get; set; }
        public byte hallmax { get; set; }
        public byte kingdom { get; set; }
        public byte kingdommax { get; set; }
        public byte tavern { get; set; }
        public byte tavernmax { get; set; }
        public byte forge { get; set; }
        public byte forgemax { get; set; }
        public byte market { get; set; }
        public byte marketmax { get; set; }
        public byte magic { get; set; }
        public byte magicmax { get; set; }
        public byte port { get; set; }
        public byte portmax { get; set; }
        public byte spec1 { get; set; }
        public byte spec1max { get; set; }
        public byte spec2 { get; set; }
        public byte spec2max { get; set; }
        public byte spec3 { get; set; }
        public byte spec3max { get; set; }
        public byte lvl1 { get; set; }
        public byte lvl1max { get; set; }
        public byte lvl2 { get; set; }
        public byte lvl2max { get; set; }
        public byte lvl3 { get; set; }
        public byte lvl3max { get; set; }
        public byte lvl4 { get; set; }
        public byte lvl4max { get; set; }
        public byte lvl5 { get; set; }
        public byte lvl5max { get; set; }
        public byte lvl6 { get; set; }
        public byte lvl6max { get; set; }
        public byte lvl7 { get; set; }
        public byte lvl7max { get; set; }
        public HeroesInfo armytown { get; set; }

        public void building()
        {

        }

        void load_std_params(BaseTown town, bool port)
        {
            var tmptownparam = File.OpenText("data/stdtownsparams.dat");
            string tmptown;
            for(; ; )
            {
                tmptown = tmptownparam.ReadLine();
                if(tmptown.Split('[')[0] == town.ToString())
                {
                    tmptown = tmptown.Split('[')[1];
                    break;
                }
            }
            tmptownparam.Close();

            byte[] param = tmptown.Split(',').Select(n => Convert.ToByte(n)).ToArray();
            hallmax = param[0];
            kingdommax = param[1];
            tavernmax = param[2];
            forgemax = param[3];
            marketmax = param[4];
            magicmax = param[5];
            if (port) portmax = 1;
            else portmax = 0;
            spec1max = param[6];
            spec2max = param[7];
            spec3max = param[8];
            lvl1max = param[9];
            lvl2max = param[10];
            lvl3max = param[11];
            lvl4max = param[12];
            lvl5max = param[13];
            lvl6max = param[14];
            lvl7max = param[15];
        }
    }
    class HeroesInfo
    {
        public HeroesInfo(string Name, int Level, int exp, int power, int defense, int spell_power, int knowledge, int mana_points, int Max_move,
            ArmySlotInfo Slot1,  ArmySlotInfo Slot2, ArmySlotInfo Slot3, ArmySlotInfo Slot4,  ArmySlotInfo Slot5, ArmySlotInfo Slot6, ArmySlotInfo Slot7)
        {
            this.Name = Name;
            this.Level = Level;
            this.exp = exp;
            this.power = power;
            this.defense = defense;
            this.spell_power = spell_power;
            this.knowledge = knowledge;
            this.mana_points = mana_points;
            Current_mana_points = mana_points;
            this.Max_move = Max_move;
            Move = Max_move;
            this.Slot1 = Slot1;
            this.Slot2 = Slot2;
            this.Slot3 = Slot3;
            this.Slot4 = Slot4;
            this.Slot5 = Slot5;
            this.Slot6 = Slot6;
            this.Slot7 = Slot7;
        }

        public string Name;
        public int Level;
        public int exp;
        public int power;
        public int defense;
        public int spell_power;
        public int knowledge;
        public int Current_mana_points;
        public int mana_points;
        public int Max_move;     //Максимальная дальность ходьбы на 1 ход
        public int Move;
        public ArmySlotInfo Slot1;
        public ArmySlotInfo Slot2;
        public ArmySlotInfo Slot3;
        public ArmySlotInfo Slot4;
        public ArmySlotInfo Slot5;
        public ArmySlotInfo Slot6;
        public ArmySlotInfo Slot7;
    }
    class ArmySlotInfo
    {
        public ArmySlotInfo(CreatureInfo Creature, int amount)
        {
            this.Creature = Creature;
            this.amount = amount;
        }

        public CreatureInfo Creature;
        public int amount;
    }
    class CreatureInfo
    {
        public CreatureInfo(string Name, int Hit_points, int min_attack, int max_attack, int power, int protect, int speed, bool flying, byte level, bool Modernized)
        {
            this.Name = Name;
            this.Hit_points = Hit_points;
            Current_Hit_points = Hit_points;
            this.min_attack = min_attack;
            this.max_attack = max_attack;
            this.power = power;
            this.protect = protect;
            this.speed = speed;
            this.flying = flying;
            this.level = level;
            this.Modernized = Modernized;

        }

        public string Name { get; private set; }
        public int Hit_points { get; private set; }
        public int Current_Hit_points { get; set; }
        public int min_attack { get; private set; }
        public int max_attack { get; private set; }
        public int power { get; private set; }
        public int protect { get; private set; }
        public int speed { get; private set; }
        public bool flying { get; private set; }
        public byte level { get; private set; }
        public bool Modernized { get; private set; }
    }
    class MenuUI : UIElement
    {
        public MenuUI()
        {
            //UIElement jdf = new UIElement();
            //jdf.AllowDrop = false;
            //jdf.Clip = System.Windows.Media.Geometry;
        }

        
    }
    enum SurfaceTypes
    {
        NULL,
        Grd,    // ground 
        Grs,    // grass
        Wtr,    // water
        Snd,    // sand
        Snw,    // snow
        Bog,    // bog
        Lav,    // lava
    }
    enum Buildings
    {
        NULL, Home, Wall, Mountain1, Tree1, 
        Cs0, Cs1, Cs2,   //Castle
        Tw0, Tw1, Tw2,   //Tower
        Rm0, Rm1, Rm2,   //Rampart
        If0, If1, If2,   //Inferno
        Nc0, Nc1, Nc2,   //Necropolis
        Dn0, Dn1, Dn2,   //Dungeon
        Sr0, Sr1, Sr2,   //Stronghold
        Fr0, Fr1, Fr2,   //Fortress
        Cn0, Cn1, Cn2,   //Conflux

        Cs0_1, Cs0_2, Cs0_3,
    }
    enum Mobs
    {
        NULL, Player, Dog, Enemy, 
    }
    enum Items
    {
        NULL, Gold
    }
    enum Adds
    {
        NULL,


    }
    enum Difficult
    {
        Low, Medium, Hard,
    }
    enum BaseTown
    {
        Castle, Tower, Rampart, Inferno, Necropolis, Dungeon, Stronghold, Fortress, Conflux
    }
}
