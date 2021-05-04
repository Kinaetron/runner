using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheRunner.ScreenManagement
{
    class ZoneSelectionScreen : MenuScreen
    {
         /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public ZoneSelectionScreen()
            : base("Select a Zone")
        {
            int totalZones = 2;

            MenuEntry[] zoneEntries = new MenuEntry[totalZones];

            // Create our menu entries.
            for (int i = 0; i < totalZones; i++)
            {
                // Add entries to the menu
                zoneEntries[i] = new MenuEntry("Zone " + (i + 1));
                MenuEntries.Add(zoneEntries[i]);

                // Hook up menu event handlers
                zoneEntries[i].Selected += delegate(object sender, PlayerIndexEventArgs e)
                {
                    MenuEntrySelected(sender, e, selectedEntry);
                };
            }
        }
        /// <summary>                      
        /// Event handler for when a menu entry is selected.
        /// </summary>
        void MenuEntrySelected(object sender, PlayerIndexEventArgs e, int currentZone)
        {
            int zoneLevelTotal = 0;
            int totalLevelsZoneOne = 10;
            int totalLevelsZoneTwo = 10;

            if (selectedEntry == 0) {
                zoneLevelTotal = totalLevelsZoneOne;
            }
            else if (selectedEntry == 1) {
                zoneLevelTotal = totalLevelsZoneTwo;
            }

           ScreenManager.AddScreen(new LevelSelectScreen(currentZone + 1, zoneLevelTotal), e.PlayerIndex);
        }
    }
}
