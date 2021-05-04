using System;
using System.Linq;
using System.Collections.Generic;

using PolyOne.Input;

using TheRunner.RunnerTimes;
using TheRunner.ScreenManagement;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace TheRunner
{
    class TimeScreen : GameScreen
    {

        private string levelName;
        private int levelNo;

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

        public TimeScreen(string levelName, int levelNo)
        {
            this.levelName = levelName;
            this.levelNo = levelNo;

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
                ScreenManager.AddScreen(new ScoreTransitionScreen(levelNo),
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

             List<HighTimeData> showingList = records.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

             for (int i = 0; i < showingList.Count; i++)
             {
                 spriteBatch.DrawString(font, showingList[i].PlayerPosition + " " +  showingList[i].PlayerName + "   " + showingList[i].PlayerTime,
                   new Vector2(0, (72 * i)), Color.White);
             }

             spriteBatch.End();
         }
    }
}