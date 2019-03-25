using System;

using GameWPF.UserControls;

namespace GameWPF.MenuActions
{
    sealed class MainMenuActions : Actions
    {
        public override event EventAddElementHandler NewElement;

        public override event EventHandler DeleteElements;

        private MenuUserControl menu;

        public override void Initialize()
        {
            menu = new MenuUserControl();

            EventsSubscription();

            NewElement(this, menu);
        }

        protected override void EventsSubscription()
        {
            menu.StartGameClicked += Menu_StartGameClicked;

            menu.LoadGameClicked += Menu_LoadGameClicked;
        }


        private void Menu_StartGameClicked(object sender, EventArgs e)
        {
            DeleteElements(this, null);

            var start_game_menu = new StartGameMenuActions(this);
            NewElementSubscription(start_game_menu);
            start_game_menu.Initialize();
        }

        protected override void NewElementSubscription(IActions actions)
        {
            actions.NewElement += (sender, new_element) => NewElement(sender, new_element);
            actions.DeleteElements += (sender, event_args) => DeleteElements(sender, event_args);
        }

        private void Menu_LoadGameClicked(object sender, EventArgs e)
        {
            DeleteElements(this, null);

            var load_game = new LoadGameMenuActions(this);
            NewElementSubscription(load_game);
            load_game.Initialize();
        }

        public override void Returned()
        {
            NewElement(this, menu);
        }
    }
}
