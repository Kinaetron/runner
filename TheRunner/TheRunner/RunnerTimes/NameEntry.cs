using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using TheRunner.ScreenManagement;
using Microsoft.Xna.Framework.Input;

namespace TheRunner.RunnerTimes
{
    class NameEntry : GameScreen 
    {
        private char[] characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        private SpriteFont font;
        private ContentManager content;
        private SpriteBatch spriteBatch;

        private string playerName = "";
        private int maxNameLength = 10;


        private HighTimeData playerData;
        private string levelName;
        private int currentLevel;
        private int currentZone;


        //underline for letters 
        private Texture2D block;
        private float flastime = 250.0f;


        private int characterScroll = -1;

        public override void LoadContent()
        {
            content = new ContentManager(ScreenManager.Game.Services, "Content");
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            font = content.Load<SpriteFont>("MenuAssets/gamefont");

            block = content.Load<Texture2D>("square");

            base.LoadContent();
        }

         public NameEntry(HighTimeData playerData, string levelName, int currentZone ,int currentLevel)
         {
             this.playerData = playerData; 
             this.levelName = levelName;
             this.currentLevel = currentLevel;
             this.currentZone = currentZone;


             if (HighTime.PlayerData.PlayerName != null) {
                 this.playerName = HighTime.PlayerData.PlayerName;
             }

             //Append characters to the typedText string when the player types stuff on the keyboard.
             KeyGrabber.InboundCharEvent += (inboundCharacter) =>
             {
                 //Only append characters that exist in the spritefont.
                 if (inboundCharacter < 32)
                     return;

                 if (inboundCharacter > 126)
                     return;

                 if (playerName.Length < maxNameLength){
                     playerName += inboundCharacter;
                 }
             };
         }

         public override void HandleInput(InputMenuState input)
         {

             if (input.IsMenuDown(ControllingPlayer) == true)
             {
                 characterScroll--;

                 if (characterScroll < 0)
                 {
                     characterScroll = characters.Length - 1;
                 }

                 if (characterScroll > characters.Length - 1)
                 {
                     characterScroll = 0;
                 }
             }

             if (input.IsMenuUp(ControllingPlayer) == true)
             {
                 characterScroll++;

                 if (characterScroll < 0)
                 {
                     characterScroll = characters.Length - 1;
                 }

                 if (characterScroll > characters.Length - 1)
                 {
                     characterScroll = 0;
                 }
             }

             if (input.IsButtonAPressed(ControllingPlayer) == true)
             {
                 if (playerName.Length < maxNameLength)
                 {
                     if (characterScroll >= 0)
                     {
                         playerName += characters[characterScroll];
                     }
                 }
             }

             if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Tab) == true)
             {
                 if (input.LastKeyboardStates[0].IsKeyUp(Keys.Tab) == true){
                     if (playerName.Length < maxNameLength)
                     {
                         if (characterScroll >= 0)
                         {
                             playerName += characters[characterScroll];
                         }

                     }
                 }
             }


             if (input.IsButtonBPressed(ControllingPlayer) == true)
             {
                 int removeLetter = playerName.Length - 1;

                 if (removeLetter >= 0)
                 {
                     playerName = playerName.Remove(removeLetter);
                 }
             }


             if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Delete) == true)
             {
                 if (input.LastKeyboardStates[0].IsKeyUp(Keys.Delete) == true) {
                     int removeLetter = playerName.Length - 1;

                     if (removeLetter >= 0)
                     {
                         playerName = playerName.Remove(removeLetter);
                     }
                 }
             }

             if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Back) == true)
             {
                 if (input.LastKeyboardStates[0].IsKeyUp(Keys.Back) == true)
                 {
                     int removeLetter = playerName.Length - 1;

                     if (removeLetter >= 0)
                     {
                         playerName = playerName.Remove(removeLetter);
                     }
                 }
             }

             if (input.IsPauseGame(ControllingPlayer) == true || 
                 input.CurrentKeyboardStates[0].IsKeyDown(Keys.Enter) == true)
             {
                 playerData.PlayerName = playerName;
                 HighTime.SaveData(levelName, playerData);

                 ScreenManager.AddScreen(new TimeScreen(levelName, currentZone, currentLevel),
                                         ControllingPlayer);

             }
         }

         public override void Draw(GameTime gameTime)
         {
             spriteBatch.Begin();

             float elapsed = gameTime.ElapsedGameTime.Milliseconds;

             if (flastime >= 0)
             {
                 flastime -= elapsed;
                 spriteBatch.Draw(block, new Rectangle((int)font.MeasureString(playerName).X, 97, 28, 10), Color.White);
             }
             else
             {
                 flastime = 250.0f;
             }

             spriteBatch.DrawString(font, "Please enter name", new Vector2 (450, 10), Color.White);

             spriteBatch.DrawString(font, playerName, new Vector2(0, 50), Color.White);

             if (characterScroll >= 0)
             {
                 spriteBatch.DrawString(font, characters[characterScroll].ToString(),
                                  new Vector2(font.MeasureString(playerName).X, 50), Color.White);
             }

             spriteBatch.End();
         }
    }
}
