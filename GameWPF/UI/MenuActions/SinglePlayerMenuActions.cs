using System;

using GameWPF.UserControls;
using static GameWPF.MapParams.MapList;

namespace GameWPF.MenuActions
{
    sealed class SinglePlayerMenuActions : Actions
    {
        public override event EventAddElementHandler NewElement;
        public override event EventHandler DeleteElements;

        private SinglePlayerUserControl menu;
        private Actions back_action;

        public SinglePlayerMenuActions(Actions back_action)
        {
            this.back_action = back_action;
        }

        public override void Initialize()
        {
            menu = new SinglePlayerUserControl();

            EventsSubscription();

            NewElement(this, menu);
        }

        protected override void EventsSubscription()
        {
            menu.StartGameClicked += Menu_StartGameClicked;
            menu.BackToStartGameMenuClicked += Menu_BackToStartGameMenuClicked;

            menu.ReadyToGetMapList += Menu_ReadyToGetMapList;
        }

        private void Menu_ReadyToGetMapList(object sender, EventArgs e)
        {
            foreach(var map in GetMapList())
            {
                menu.MapList_AddMap(map);
            }

            
        }

        private void Menu_BackToStartGameMenuClicked(object sender, EventArgs e)
        {
            DeleteElements(this, null);

            back_action.Returned();
        }

        private void Menu_StartGameClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void NewElementSubscription(IActions actions)
        {
            actions.NewElement += (sender, new_element) => NewElement(sender, new_element);
            actions.DeleteElements += (sender, event_args) => DeleteElements(sender, event_args);
        }

        public override void Returned()
        {
            NewElement(this, menu);
        }
    }
}
