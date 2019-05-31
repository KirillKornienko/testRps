using System;

using GameWPF.UserControls;

namespace GameWPF.MenuActions
{
    sealed class StartGameMenuActions : Actions<StartGameUserControl>
    {
        public override event EventAddElementHandler NewElement;
        public override event Action DeleteElements;


        public StartGameMenuActions(IActions back_action)
        {
            this.back_action = back_action;
        }

        protected override void EventsSubscription()
        {
            menu.SinglePlayerClicked += SinglePlayerClicked;

            menu.BackToMainMenuClicked += BackToMainMenuClicked;
        }

        private void BackToMainMenuClicked()
        {
            DeleteElements();

            back_action.Returned();
        }

        private void SinglePlayerClicked()
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
