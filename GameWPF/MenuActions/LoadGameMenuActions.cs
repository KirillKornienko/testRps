using System;

using GameWPF.UserControls;

namespace GameWPF.MenuActions
{
    class LoadGameMenuActions : Actions
    {
        public override event EventAddElementHandler NewElement;
        public override event EventHandler DeleteElements;

        private LoadGameUserControl menu;
        private Actions back_action;

        public LoadGameMenuActions(Actions back_action)
        {
            this.back_action = back_action;
        }

        public override void Initialize()
        {
            menu = new LoadGameUserControl();

            EventsSubscription();

            NewElement(this, menu);
        }

        protected override void EventsSubscription()
        {
            throw new NotImplementedException();
        }

        protected override void NewElementSubscription(IActions actions)
        {
            actions.NewElement += (sender, new_element) => NewElement(sender, new_element);
            actions.DeleteElements += (sender, event_args) => DeleteElements(sender, event_args);
        }
    }
}
