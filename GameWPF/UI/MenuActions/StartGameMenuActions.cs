using System;

using GameWPF.UserControls;

namespace GameWPF.MenuActions
{
    sealed class StartGameMenuActions : Actions
    {
        public override event EventAddElementHandler NewElement;
        public override event Action DeleteElements;

        private StartGameUserControl menu;
        private Actions back_action;

        public StartGameMenuActions(Actions back_action)
        {
            this.back_action = back_action;
        }

        protected override void EventsSubscription()
        {
            menu.SinglePlayerClicked += Menu_SinglePlayerClicked;

            menu.BackToMainMenuClicked += Menu_BackToMainMenuClicked;
        }

        private void Menu_BackToMainMenuClicked()
        {
            DeleteElements();

            back_action.Returned();
        }

        private void Menu_SinglePlayerClicked()
        {
            DeleteElements();

            var single_player_menu = new SinglePlayerMenuActions(this);
            NewElementSubscription(single_player_menu);
            single_player_menu.Initialize();
        }

        public override void Initialize()
        {
            menu = new StartGameUserControl();

            EventsSubscription();

            NewElement(menu);
        }

        protected override void NewElementSubscription(IActions actions)
        {
            actions.NewElement += (new_element) => NewElement(new_element);
            actions.DeleteElements += () => DeleteElements();
        }

        public override void Returned()
        {
            NewElement(menu);
        }
    }
}
