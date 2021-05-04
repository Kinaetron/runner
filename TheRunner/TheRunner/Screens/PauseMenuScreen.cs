#region File Description
//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace TheRunner.ScreenManagement
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        #region Initialization

        private int zoneNo;
        private int levelNo;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen(int zoneNo,int levelNo)
            : base("Paused")
        {
            this.zoneNo = zoneNo;
            this.levelNo = levelNo;

            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume");
            MenuEntry restartGameMenuEntry = new MenuEntry("Restart");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit");
            
            // Hook up menu event handlers.

            resumeGameMenuEntry.Selected += OnCancel;
            restartGameMenuEntry.Selected += RedoLevelSelected;
            quitGameMenuEntry.Selected += QuitGame;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(restartGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void QuitGame(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }

        void RedoLevelSelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                              new GameplayScreen(zoneNo,levelNo));
        }


        #endregion
    }
}
