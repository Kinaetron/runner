using Microsoft.Xna.Framework;

namespace TheRunner.ScreenManagement
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class ScoreTransitionScreen : MenuScreen
    {
        #region Initialization

        private int levelNo;
        private int zoneNo;

        private int levelAmountZone1 = 9;
        private int levelAmountZone2 = 9;
        private int levelAmount;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScoreTransitionScreen(int zoneNo, int levelNo)
            : base(" ")
        {
            this.levelNo = levelNo;
            this.zoneNo = zoneNo;

            if (zoneNo == 1) {
                levelAmount = levelAmountZone1;
            }

            if (zoneNo == 2) {
                levelAmount = levelAmountZone2;
            }

            // Create our menu entries.
            MenuEntry nextGameMenuEntry = new MenuEntry("Next Level");
            MenuEntry resumeGameMenuEntry = new MenuEntry("Redo Level");
            MenuEntry quitGameMenuEntry = new MenuEntry("Back to Main Menu");

            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += RedoLevelSelected;
            nextGameMenuEntry.Selected += NextLevelSelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(nextGameMenuEntry);
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }


        #endregion

        #region Handle Input

        void RedoLevelSelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                              new GameplayScreen(zoneNo, levelNo));
        }

        void NextLevelSelected(object sender, PlayerIndexEventArgs e)
        {
            if (levelNo + 1 > levelAmount) {
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                               new MainMenuScreen());
            }
            else {
                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                            new GameplayScreen(zoneNo ,levelNo + 1));
            }
        }



        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>www.
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }


        #endregion
    }
}
