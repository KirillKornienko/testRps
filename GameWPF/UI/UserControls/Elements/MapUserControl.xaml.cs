using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;

using Core.Maps.MapParams;
using static Core.Maps.Properties.Properties;


namespace GameWPF.UserControls.Elements
{
    /// <summary>
    /// Логика взаимодействия для MapUserControl.xaml
    /// </summary>
    public partial class MapUserControl : UserControl
    {
        public event EventHandler Clicked;

        public event EventHandler DoubleClicked;

        public MapUserControl()
        {
            InitializeComponent();
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            Clicked(this, null);
        }

        public void SetContent(MapParameters parameters)
        {
            if (!parameters.IsFolder)
            {
                ScenarioName.Content = parameters.Basic.scenario_name;
                PlayersValue.Content = parameters.Basic.players_value;
                MapSize.Content = parameters.Basic.width_size.ToString() + 'x' + parameters.Basic.height_size.ToString();
                VictoryConditions.Content = parameters.Basic.victory_conditions;
                DefeatConditions.Content = parameters.Basic.defeat_conditions;
            }
            else
            {
                string name = Path.GetFileName(parameters.Basic.scenario_name);
                if (name == MAPS_DIR_NAME)
                {
                    ScenarioName.Content = "...";
                }
                else
                    ScenarioName.Content = name;

            }
        }

        private void MapButton_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DoubleClicked(this, null);
        }
    }
}
