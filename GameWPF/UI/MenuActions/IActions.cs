//#define StandartInheritanceModel

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

        void Returned();
    }

    abstract class Actions<T> : IActions where T: UserControl
    {
        public abstract event EventAddElementHandler NewElement;
        public abstract event Action DeleteElements;

        public abstract void Initialize();

        public abstract void Returned();

        protected T menu;

        protected IActions back_action;

        protected abstract void EventsSubscription();

        protected abstract void NewElementSubscription(IActions actions);
    }


    // Попытка сделать более правильную модель наследования, реализующая шаблонные функции
    public class Actions2<T> : IActions where T : UserControl, new()
    {
        public event EventAddElementHandler NewElement;
        public event Action DeleteElements;

        protected T menu;

        protected IActions back_action;

        protected Actions2(IActions back_action) 
        {
            this.back_action = back_action;
        }

        public void Initialize()
        {
            menu = new T();

            EventsSubscription();

            NewElement(menu);
        }

        protected void InitializeChildElement(IActions new_element) 
        {
            DeleteElements();

            NewElementSubscription(new_element);
            new_element.Initialize();
        }

        protected virtual void EventsSubscription()
        {
            throw new Exception("Метод не переопределён");
        }

        protected void NewElementSubscription(IActions actions)
        {
            actions.NewElement += (new_element) => NewElement(new_element);
            actions.DeleteElements += () => DeleteElements();
        }

        public void Returned()
        {
            DeleteElements();
            NewElement(menu);
        }

        protected void BackClicked(object sender, EventArgs e)
        {
            if (back_action != null)
            {
                DeleteElements();

                back_action.Returned();
            }
        }

    }

}
