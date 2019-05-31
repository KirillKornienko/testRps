using System;

using GameWPF.UserControls;

namespace GameWPF.MenuActions
{
    sealed class MainMenuActions : Actions<MenuUserControl>
    {
        public override event EventAddElementHandler NewElement;

        public override event Action DeleteElements;


        public override void Initialize()
        {
            menu = new MenuUserControl();

            EventsSubscription();

            NewElement(menu);
        }

        protected override void EventsSubscription()
        {
            menu.StartGameClicked += Menu_StartGameClicked;

            menu.LoadGameClicked += Menu_LoadGameClicked;
        }


        private void Menu_StartGameClicked()
        {
            DeleteElements();

            var start_game_menu = new StartGameMenuActions(this);
            NewElementSubscription(start_game_menu);
            start_game_menu.Initialize();
        }

        protected override void NewElementSubscription(IActions actions)
        {
            actions.NewElement += (new_element) => NewElement(new_element);
            actions.DeleteElements += () => DeleteElements();
        }

        private void Menu_LoadGameClicked()
        {
            DeleteElements();

            var load_game = new LoadGameMenuActions(this);
            NewElementSubscription(load_game);
            load_game.Initialize();
        }

        public override void Returned()
        {
            NewElement(menu);
        }
    }
}
