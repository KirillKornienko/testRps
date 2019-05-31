using System;

using GameWPF.UserControls;


namespace GameWPF.MenuActions
{
#if !StandartInheritanceModel

    class NewMenuAction : Actions2<MenuUserControl>
    {
        public NewMenuAction() : base(null)
        {
        }

        protected override void EventsSubscription()
        {
            menu.StartGameClicked += StartGameClicked;

            menu.LoadGameClicked += LoadGameClicked;
        }

        private void LoadGameClicked()
        {
            InitializeChildElement(new StartGameUserAction(this));
        }

        private void StartGameClicked()
        {
            InitializeChildElement(new StartGameUserAction(this));
        }
    }

    class StartGameUserAction : Actions2<StartGameUserControl>
    {

        public StartGameUserAction(IActions back_action) : base (back_action)
        {
        }

        protected override void EventsSubscription()
        {
            menu.SinglePlayerClicked += SinglePlayerClicked;

            menu.BackToMainMenuClicked += BackToMainMenuClicked;
        }

        private void BackToMainMenuClicked()
        {
            back_action.Returned();
        }

        private void SinglePlayerClicked()
        {
            InitializeChildElement(new SinglePlayerMenuActions(this));
        }

    }

    class SinglePlayerGameUserAction : Actions2<SinglePlayerUserControl>
    {
        public SinglePlayerGameUserAction(IActions back_action) : base(back_action)
        {
        }


        protected override void EventsSubscription()
        {
            menu.StartGameClicked += StartGameClicked;
            menu.BackToStartGameMenuClicked += BackToStartGameMenuClicked;

            menu.ReadyToGetMapList += ReadyToGetMapList;
        }

        private void ReadyToGetMapList(object sender, EventArgs e)
        {
        }

        private void BackToStartGameMenuClicked(object sender, EventArgs e)
        {
            back_action.Returned();
        }

        private void StartGameClicked(object sender, EventArgs e)
        {

        }


    }

    class LoadGameUserAction : Actions2<LoadGameUserControl>
    {
        public LoadGameUserAction(IActions back) : base (back)
        {
        }

        protected override void EventsSubscription()
        {
            
        }
    }
#endif
}
