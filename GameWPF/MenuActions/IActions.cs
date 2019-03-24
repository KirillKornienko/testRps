using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWPF.MenuActions
{
    public delegate void EventAddElementHandler(object sender, UserControl new_element);


    interface IActions
    {
        event EventAddElementHandler NewElement;

        event EventHandler DeleteElements;

        void Initialize();
    }

    abstract class Actions : IActions
    {
        public abstract event EventAddElementHandler NewElement;
        public abstract event EventHandler DeleteElements;

        public abstract void Initialize();

        //protected UserControl menu;

        protected abstract void EventsSubscription();

        protected abstract void NewElementSubscription(IActions actions);
    }
}
