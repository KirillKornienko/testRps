﻿using System;
using System.IO;

using GameWPF.UserControls;
using GameWPF.UserControls.Elements;
using GameWPF.MenuActions.Elements;
using GameWPF.MapParams;
using Settings = GameWPF.Properties.Settings;


namespace GameWPF.MenuActions
{
    sealed class SinglePlayerMenuActions : Actions
    {
        public override event EventAddElementHandler NewElement;
        public override event EventHandler DeleteElements;

        private SinglePlayerUserControl menu;
        private Actions back_action;

        private MapElementActions selected_map;

        public SinglePlayerMenuActions(Actions back_action)
        {
            this.back_action = back_action;
        }

        public override void Initialize()
        {
            menu = new SinglePlayerUserControl();

            EventsSubscription();

            NewElement(this, menu);
        }

        protected override void EventsSubscription()
        {
            menu.StartGameClicked += Menu_StartGameClicked;
            menu.BackToStartGameMenuClicked += Menu_BackToStartGameMenuClicked;

            menu.ReadyToGetMapList += Menu_ReadyToGetMapList;
        }

        private void Menu_ReadyToGetMapList(object sender, EventArgs e)
        {
            FillMapList();
        }

        private void Menu_BackToStartGameMenuClicked(object sender, EventArgs e)
        {
            DeleteElements(this, null);

            back_action.Returned();
        }

        private void Menu_StartGameClicked(object sender, EventArgs e)
        {
            if(selected_map != null)
                StartGame();
        }

        private void StartGame()
        {

        }

        protected override void NewElementSubscription(IActions actions)
        {
            actions.NewElement += (sender, new_element) => NewElement(sender, new_element);
            actions.DeleteElements += (sender, event_args) => DeleteElements(sender, event_args);
        }

        public override void Returned()
        {
            NewElement(this, menu);
        }

        public void FillMapList(string directory = null)
        {
            if (directory != null && directory != Settings.Default.MAPS_DIRECTORY_NAME)
                new MapElementActions(this, GetReturnedElement(directory));

            foreach (var map in MapList.GetMapList(directory))
            {
                new MapElementActions(this, map);
            }
        }

        public void AddMapElement(MapUserControl element)
        {
            menu.MapList_AddMap(element);
        }

        public void ClearMapElements()
        {
            menu.MapList_Clear();
        }

        public void MapSelected(MapElementActions map)
        {
            selected_map = map;


        }

        private MapParameters GetReturnedElement(string directory)
        {
            return MapParameters.GetDirectoryParams(Path.GetDirectoryName(directory));
        }
    }
}
