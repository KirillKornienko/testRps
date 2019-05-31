using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameWPF.UserControls;

namespace GameWPF.MenuActions
{
    class NewMenuActions : Actions2<MainMenuUC4x3>
    {
        public NewMenuActions(AspectRatio ratio) : base (null)
        {
        }

        protected override void EventsSubscription()
        {
            menu.Button1Clicked += Menu_Button1Clicked;
            menu.Button2Clicked += Menu_Button2Clicked;
            menu.Button3Clicked += Menu_Button3Clicked;
            menu.Button4Clicked += Menu_Button4Clicked;
            menu.Button5Clicked += Menu_Button5Clicked;
        }

        private void Menu_Button5Clicked()
        {
           switch (menu.type)
            {
                case MenuType.MainMenu:
                    System.Windows.Application.Current.Shutdown();
                    break;

                case MenuType.StartGame | MenuType.LoadGame:
                    menu.UpdateType(MenuType.MainMenu);
                    break;
            }
        }

        private void Menu_Button4Clicked()
        {
            throw new NotImplementedException();
        }

        private void Menu_Button3Clicked()
        {
            
        }

        private void Menu_Button2Clicked()
        {
            switch (menu.type)
            {
                case MenuType.MainMenu:
                    menu.UpdateType(MenuType.LoadGame);
                    break;

                case MenuType.StartGame:
                    break;

                case MenuType.LoadGame:
                    break;
            }
        }

        private void Menu_Button1Clicked()
        {
            switch (menu.type)
            {
                case MenuType.MainMenu:
                    menu.UpdateType(MenuType.StartGame);
                    break;

                case MenuType.StartGame:
                    InitializeChildElement(new SinglePlayerMenuActions(this));
                    break;

                case MenuType.LoadGame:
                    //InitializeChildElement(new LoadGameUserAction(this));
                    break;
            }
        }

    }
}
