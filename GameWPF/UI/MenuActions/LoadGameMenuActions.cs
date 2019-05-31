using System;

using GameWPF.UserControls;

namespace GameWPF.MenuActions
{
    sealed class LoadGameMenuActions : Actions<LoadGameUserControl>
    {
        public override event EventAddElementHandler NewElement;
        public override event Action DeleteElements;


        public LoadGameMenuActions(IActions back_action)
        {
            this.back_action = back_action;
        }

        public override void Initialize()
        {
            menu = new LoadGameUserControl();

            EventsSubscription();

            NewElement(menu);
        }

        protected override void EventsSubscription()
        {
            throw new NotImplementedException();
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
