using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheRunner.ScreenManagement
{
    class LevelSelectScreen : MenuScreen
    {
         /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public LevelSelectScreen(int currentZone,int totalLevels)
            : base("Select a Level")
        {
            MenuEntry[] levelEntries = new MenuEntry[totalLevels];

            // Create our menu entries.
            for (int i = 0; i < totalLevels; i++)
            {
                // Add entries to the menu
                levelEntries[i] = new MenuEntry("Level " + (i + 1));
                MenuEntries.Add(levelEntries[i]);

                // Hook up menu event handlers
                levelEntries[i].Selected += delegate(object sender, PlayerIndexEventArgs e)
                {
                    MenuEntrySelected(sender, e, selectedEntry, currentZone);
                };
            }
        }

        /// Event handler for when a menu entry is selected.
        /// </summary>
        void MenuEntrySelected(object sender, PlayerIndexEventArgs e, int currentLevel, int currentZone)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen(currentZone, currentLevel));
        }
    }
}
