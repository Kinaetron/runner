using System;
using System.Linq;
using System.Collections.Generic;

using PolyOne.Input;

using TheRunner.RunnerTimes;
using TheRunner.ScreenManagement;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace TheRunner
{
    class TimeScreen : GameScreen
    {

        private string levelName;
        private int levelNo;
        private int zoneNo;

        private SpriteFont font;
        private ContentManager content;
        private SpriteBatch spriteBatch;

        private List<HighTimeData> records = new List<HighTimeData>();
        private int currentPage = 1;
        private int pageSize = 10;
        private int maxRecords;
        private int maxPages;

        public override void LoadContent()
        {
            content = new ContentManager(ScreenManager.Game.Services, "Content");
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            font = content.Load<SpriteFont>("MenuAssets/gamefont");

            base.LoadContent();
        }

        public TimeScreen(string levelName, int zoneNo ,int levelNo)
        {
            this.levelName = levelName;
            this.levelNo = levelNo;
            this.zoneNo = zoneNo;

            HighTime.LoadData(levelName);

            records = HighTime.TimeList;

            maxRecords = records.Count;
            maxPages = (int)Math.Ceiling((double)maxRecords / pageSize);
        }

        public override void HandleInput(InputMenuState input)
        {
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ControllingPlayer, out playerIndex) == true)
            {
                ScreenManager.AddScreen(new ScoreTransitionScreen(zoneNo, levelNo),
                                      ControllingPlayer);
            }

            if (input.RightTrigger(playerIndex) > 0.3f || 
                input.CurrentGamePadStates[0].IsButtonDown(Buttons.RightShoulder) == true || 
                input.CurrentKeyboardStates[0].IsKeyDown(Keys.D) == true ||
                input.CurrentKeyboardStates[0].IsKeyDown(Keys.Right) == true)
            {
                ScreenManager.AddScreen(new TimeScreenOnline(levelName, zoneNo, levelNo),
                                       ControllingPlayer);

            }

            TimeScrolling(input);
        }

        private void TimeScrolling(InputMenuState input)
        {
            if (input.IsMenuDown(ControllingPlayer) == true)
            {
                if (currentPage == maxPages)
                    return;
                else
                    currentPage += 1;
            }

            if (input.IsMenuUp(ControllingPlayer) == true)
            {
                if (currentPage == 1)
                    return;
                else
                    currentPage -= 1;
            }
        }

         public override void Draw(GameTime gameTime)
         {
             spriteBatch.Begin();

             spriteBatch.DrawString(font, "Local", new Vector2(640, 0), Color.White);

             List<HighTimeData> showingList = records.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

             for (int i = 0; i < showingList.Count; i++)
             {
                 if (showingList[i].PlayerPosition == null) 
                 {
                     int pos = i + 1;
                     string position = pos.ToString();
                     spriteBatch.DrawString(font, position, new Vector2(10, (72 * i + 20)), Color.White);
                 }
                 else {
                     spriteBatch.DrawString(font, showingList[i].PlayerPosition, new Vector2(10, (72 * i + 20)), Color.White);
                 }
               

                 spriteBatch.DrawString(font, showingList[i].PlayerName, new Vector2(70, 72 * i + 20), Color.White);
                 spriteBatch.DrawString(font, showingList[i].PlayerTime, new Vector2(300, 72 * i + 20), Color.White);
             }

             spriteBatch.End();
         }
    }
}