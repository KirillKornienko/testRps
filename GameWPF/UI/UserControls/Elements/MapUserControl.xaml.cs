using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GameWPF.MapParams;

namespace GameWPF.UserControls.Elements
{
    /// <summary>
    /// Логика взаимодействия для MapUserControl.xaml
    /// </summary>
    public partial class MapUserControl : UserControl
    {
        public readonly MapParameters parameters;

        public MapUserControl(MapParameters parameters)
        {
            this.parameters = parameters;

            InitializeComponent();

            PlayersValue.Content = parameters.Basic.players_value;
            MapSize.Content = parameters.Basic.width_size + 'x' + parameters.Basic.height_size;
            ScenarioName.Content = parameters.Basic.scenario_name;
            VictoryConditions.Content = parameters.Basic.victory_conditions;
            DefeatConditions.Content = parameters.Basic.defeat_conditions;
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            parameters.GetAdvancedParams();
            MessageBox.Show("Hello!");
        }
    }
}
