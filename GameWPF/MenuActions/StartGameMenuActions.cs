using System;

using GameWPF.UserControls;

namespace GameWPF.MenuActions
{
    sealed class StartGameMenuActions : Actions
    {
        public override event EventAddElementHandler NewElement;
        public override event EventHandler DeleteElements;

        private StartGameUserControl menu;
        private Actions back_action;

        public StartGameMenuActions(Actions back_action)
        {
            this.back_action = back_action;
        }

        protected override void EventsSubscription()
        {
            menu.SinglePlayerClicked += Menu_SinglePlayerClicked; ;

            menu.BackToMainMenuClicked += Menu_BackToMainMenuClicked;
        }

        private void Menu_BackToMainMenuClicked(object sender, EventArgs e)
        {
            DeleteElements(this, null);

            back_action.Initialize();
        }

        private void Menu_SinglePlayerClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            menu = new StartGameUserControl();

            EventsSubscription();

            NewElement(this, menu);
        }

        protected override void NewElementSubscription(IActions actions)
        {
            actions.NewElement += (sender, new_element) => NewElement(sender, new_element);
            actions.DeleteElements += (sender, event_args) => DeleteElements(sender, event_args);
        }
    }
}
