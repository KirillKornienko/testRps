using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameWPF.UserControls.Elements;
using GameWPF.MapParams;


namespace GameWPF.MenuActions.Elements
{
    class MapElementActions
    {
        readonly SinglePlayerMenuActions parent;

        readonly MapParameters parameters;

        MapUserControl UI_element;


        public MapElementActions(SinglePlayerMenuActions parent, MapParameters parameters)
        {
            this.parent = parent;
            this.parameters = parameters;

            Initialize();
        }

        private void Initialize()
        {
            UI_element = new MapUserControl();
            UI_element.SetContent(parameters);

            EventsSubscription();

            parent.AddMapElement(UI_element);

        }

        private void EventsSubscription()
        {
            UI_element.Clicked += UI_element_Clicked;

            UI_element.DoubleClicked += UI_element_DoubleClicked;
        }


        private void UI_element_Clicked(object sender, EventArgs e)
        {
            if(!parameters.IsFolder())
            {
                parameters.GetAdvancedParams();

                parent.MapSelected(this);
            }
        }

        private void UI_element_DoubleClicked(object sender, EventArgs e)
        {
            if (parameters.IsFolder())
            {
                parent.ClearMapElements();

                parent.FillMapList(parameters.Basic.scenario_name);
            }
        }

    }
}
