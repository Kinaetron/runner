using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using PolyOne.LevelProcessor;
using PolyOne.Utility;

using TheRunner.RunCamera;
using TheRunner.RunnerTimes;


namespace TheRunner.ScreenManagement
{
    class GameplayScreen : GameScreen
    {

       private ContentManager content;
       private int currentLevel;
       private int currentZone;
       float pauseAlpha;

       private Point minDrawDistance;
       private Point maxDrawDistance;

       private Texture2D backgroundImage1;
       private Texture2D backgroundImage2;
       private Texture2D backgroundImageCloud;

       private Vector2 previousCameraPosition;

       float cloudMovement;

       private SpriteBatch spriteBatch;
       public RunnerCamera camera = new RunnerCamera();

       public TileMap tileMap = new TileMap();
       private Level level;

       private bool timeSaved;

       private Color backgroundColour = new Color(1f, 1f, 1f, 1f);

        public GameplayScreen(int zone, int level)
        {
            currentLevel = level;
            currentZone = zone;
              
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            minDrawDistance = new Point(0, 0);
            maxDrawDistance = new Point(50, 50);

        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

             tileMap = content.Load<TileMap>(String.Format("Maps/Zone{0}/{1}",currentZone,currentLevel));

            backgroundImage1 = content.Load<Texture2D>("bg_background2");
            backgroundImage2 = content.Load<Texture2D>("cityBackground");
            backgroundImageCloud = content.Load<Texture2D>("backgroundCloud");


            PolyDebug.LoadContent(ScreenManager.GraphicsDevice);
            
            // Load the level.
            level = new Level(content, tileMap, camera);

            base.LoadContent();
        }

          /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            }


            if (IsActive == true )
            {
                level.Update(gameTime);
            }

            if (level.ReachedExit == true && timeSaved == false)
            {
                ScreenManager.AddScreen(new NameEntry(level.ActualPlayer.PlayerData, 
                                                      tileMap.LevelName, currentZone, currentLevel), ControllingPlayer);
                timeSaved = true;
            }

            Rectangle bounds = level.ActualPlayer.PlayerRectangle;
            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width)) - 1;
             int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height)) - 1;

            minDrawDistance = new Point(leftTile - 40, topTile - 40);
            maxDrawDistance = new Point(rightTile + 40, bottomTile + 40);
        }

        public override void HandleInput(InputMenuState input)
        {

            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(currentZone, currentLevel), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                            BlendState.NonPremultiplied,
                            SamplerState.PointWrap, null, null, null,
                            camera.TransformMatrix);

            if (IsActive == true) {
                cloudMovement += 0.1f;
            }


            //for (int i = 0; i < 3000; i++)
            //{
                spriteBatch.Draw(backgroundImage1, new Vector2(camera.Position.X, camera.Position.Y), new Rectangle((int)(camera.Position.X * 0.9f), (int)(camera.Position.Y * 0.009f),
                                  backgroundImage1.Width, backgroundImage1.Height), Color.White);

                spriteBatch.Draw(backgroundImageCloud, new Vector2(camera.Position.X, camera.Position.Y + 240), new Rectangle((int)(cloudMovement), (int)(camera.Position.Y * 0.009f),
                                    backgroundImageCloud.Width, backgroundImageCloud.Height), Color.White);

                spriteBatch.Draw(backgroundImage2, new Vector2(camera.Position.X, camera.Position.Y + 30), new Rectangle((int)(camera.Position.X * 0.9f), (int)(camera.Position.Y * 0.01f),
                                 backgroundImage2.Width, backgroundImage2.Height), Color.White);
            //}


            if (camera.Position.X > previousCameraPosition.X)
            {
                minDrawDistance.X -= 1;
                maxDrawDistance.X += 1;
            }

            previousCameraPosition = camera.Position;

            if (tileMap.Layers.Count == 2)
            {
                tileMap.Layers[1].DrawBackgroundWithoutBatch(spriteBatch, camera, backgroundColour, minDrawDistance, maxDrawDistance);
            }
            else if (tileMap.Layers.Count == 3)
            {
                tileMap.Layers[2].DrawBackgroundWithoutBatch(spriteBatch, camera, backgroundColour, minDrawDistance, maxDrawDistance);
                tileMap.Layers[1].DrawWithoutBatch(spriteBatch, camera, new Color(1f, 1f, 1f, 1f), minDrawDistance, maxDrawDistance);

            }

            tileMap.Layers[0].DrawWithoutBatch(spriteBatch, camera, new Color(1f, 1f, 1f, 1f), minDrawDistance, maxDrawDistance);

            spriteBatch.End();
        
            level.Draw(gameTime, spriteBatch, camera);
           

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }

            base.Draw(gameTime);
        }
    }
}
