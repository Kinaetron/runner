#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TheRunner.ScreenManagement
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry sfxVolume;
        MenuEntry musicVolume;
        MenuEntry fullScreen;

       public static int sfxNumber = 2;
       public static int musicNumber = 7;
       static bool fullScreenBool = false;


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.

            sfxVolume   = new MenuEntry(string.Empty);
            musicVolume = new MenuEntry(string.Empty);
            fullScreen = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            sfxVolume.SelectedRight += SFXMenuEntrySelectedRight;
            sfxVolume.SelectedLeft += SFXMenuEntrySelectedLeft;

            musicVolume.SelectedRight += MusicMenuEntrySelectedRight;
            musicVolume.SelectedLeft += MusicMenuEntrySelectedLeft;

            fullScreen.SelectedRight += FullScreenMenuEntrySelectedRight;
            fullScreen.SelectedLeft += FullScreenMenuEntrySelectedLeft;

            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(sfxVolume);
            MenuEntries.Add(musicVolume);
            MenuEntries.Add(fullScreen);
            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            sfxVolume.Text = "SFX Volume: " + sfxNumber;
            musicVolume.Text = "Music Volume: " + musicNumber;


            if(fullScreenBool == true)
                fullScreen.Text = "Fullscreen: On";
            else
                fullScreen.Text = "Fullscreen: Off";

        }

        #endregion

        #region Handle Input

        void SFXMenuEntrySelectedRight(object sender, PlayerIndexEventArgs e)
        {
            sfxNumber++;

            if (sfxNumber > 10)
                sfxNumber = 10;

            SetMenuEntryText();
        }

        void SFXMenuEntrySelectedLeft(object sender, PlayerIndexEventArgs e)
        {
            sfxNumber--;

            if (sfxNumber < 0)
                sfxNumber = 0;

            SetMenuEntryText();
        }

        void MusicMenuEntrySelectedRight(object sender, PlayerIndexEventArgs e)
        {
            musicNumber++;

            if (musicNumber > 10)
                musicNumber = 10;

           SetMenuEntryText();
        }

        void MusicMenuEntrySelectedLeft(object sender, PlayerIndexEventArgs e)
        {
            musicNumber--;

            if (musicNumber < 0)
                musicNumber = 0;

            SetMenuEntryText();
        }

        void FullScreenMenuEntrySelectedRight(object sender, PlayerIndexEventArgs e)
        {
            fullScreenBool = true;

            if (ScreenManager.GraphicsManager.IsFullScreen == false)
            {
                ScreenManager.GraphicsManager.IsFullScreen = fullScreenBool;
                ScreenManager.GraphicsManager.ApplyChanges();
                SetMenuEntryText();
            }
        }

        void FullScreenMenuEntrySelectedLeft(object sender, PlayerIndexEventArgs e)
        {
            fullScreenBool = false;

            if (ScreenManager.GraphicsManager.IsFullScreen == true)
            {
                ScreenManager.GraphicsManager.IsFullScreen = fullScreenBool;
                ScreenManager.GraphicsManager.ApplyChanges();

                SetMenuEntryText();
            }
        }
        #endregion
    }
}
