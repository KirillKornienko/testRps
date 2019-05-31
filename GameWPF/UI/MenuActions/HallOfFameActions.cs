using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameWPF.UserControls;


namespace GameWPF.MenuActions
{
    class HallOfFameActions : Actions<HallOfFameUserControl>
    {
        public override event EventAddElementHandler NewElement;
        public override event Action DeleteElements;

        public HallOfFameActions(IActions back_action)
        {
            this.back_action = back_action;
        }

        public override void Initialize()
        {
            menu = new HallOfFameUserControl();

            EventsSubscription();

            NewElement(menu);
        }

        public override void Returned()
        {
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
    }
}
