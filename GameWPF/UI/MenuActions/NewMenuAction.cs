using System;

using GameWPF.UserControls;


namespace GameWPF.MenuActions
{
#if !StandartInheritanceModel

    class NewMenuAction : Actions<MenuUserControl> 
    {
        public override event EventAddElementHandler NewElement;
        public override event EventHandler DeleteElements;

        protected override void EventsSubscription()
        {
            menu.StartGameClicked += Menu_StartGameClicked;

            
        }

        private void Menu_StartGameClicked(object sender, EventArgs e)
        {
            DeleteElements(this, null);

            var start_game_menu = new StartGameUserAction(this);
            NewElementSubscription(start_game_menu);
            start_game_menu.Initialize();

        }
    }

    class StartGameUserAction : Actions<StartGameUserControl>
    {
        public override event EventAddElementHandler NewElement;
        public override event EventHandler DeleteElements;

        public StartGameUserAction(Actions<T> back_action)
        {
            this.back_action = back_action;
        }

        protected override void EventsSubscription()
        {
            throw new NotImplementedException();
        }
    }
#endif
}
