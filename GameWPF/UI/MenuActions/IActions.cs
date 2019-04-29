using System;
using System.Windows.Controls;

namespace GameWPF.MenuActions
{
    public delegate void EventAddElementHandler(UserControl new_element);


    public interface IActions
    {
        event EventAddElementHandler NewElement;

        event Action DeleteElements;

        void Initialize();
    }

#if StandartInheritanceModel
    abstract class Actions : IActions
    {
        public abstract event EventAddElementHandler NewElement;
        public abstract event Action DeleteElements;

        public abstract void Initialize();

        //protected UserControl menu;

        protected abstract void EventsSubscription();

        protected abstract void NewElementSubscription(IActions actions);

        public abstract void Returned();
    }

#else
    // Попытка сделать более правильную модель наследования, реализующая шаблонные функции

    abstract class Actions<T> : IActions where T : UserControl, new()
    {
        public abstract event EventAddElementHandler NewElement;
        public abstract event EventHandler DeleteElements;

        protected T menu;

        public Actions<T> back_action;

        protected Actions(Actions<T> back_action2) 
        {
            this.back_action = back_action2;
        }

        public void Initialize()
        {
            menu = new T();

            EventsSubscription();

            NewElement(this, menu);
        }


        protected abstract void EventsSubscription();

        protected void NewElementSubscription(IActions actions)
        {
            actions.NewElement += (sender, new_element) => NewElement(sender, new_element);
            actions.DeleteElements += (sender, event_args) => DeleteElements(sender, event_args);
        }

        protected void Returned()
        {
            NewElement(this, menu);
            
        }

        protected void BackClicked(object sender, EventArgs e)
        {
            if (back_action != null)
            {
                DeleteElements(this, null);

                back_action.Returned();

            }
        }

    }

#endif
}
