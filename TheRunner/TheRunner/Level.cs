using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using PolyOne.LevelProcessor;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using PolyOne.Utility;
using PolyOne.Collision;
using TheRunner.RunCamera;
using TheRunner.RunnerTimes;

namespace TheRunner
{
    class Level : IDisposable
    {
        private SpriteFont menuFont;
        private float elapsed;
        private int totalFrames;
        int fps;

        Texture2D block;

        private int globalX;
        private int globalY;

        public Player ActualPlayer
        {
            get { return player; }
        }
        Player player;

        public Tile[,] tiles;

        TileMap tileMap;

        public RunnerCamera Camera
        {
            get { return camera; }
        }
        RunnerCamera camera;

        public bool ReachedExit
        {
            get { return reachedExit; }
        }
        bool reachedExit;

        // Level content.        
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;


        Vector2 start = new Vector2();
        private List<Rectangle> exits = new List<Rectangle>();

        private void LoadTiles(CollisionLayer colLayer)
        {
            globalX = colLayer.Width;
            globalY = colLayer.Height;

            tiles = new Tile[colLayer.Width, colLayer.Height];

            for (int x = 0; x < colLayer.Width; ++x)
                for (int y = 0; y < colLayer.Height; ++y)
                    tiles[x, y] = LoadTile(colLayer.GetCellIndex(x, y), x, y);

            //if (exits.Count == 0)
            //    throw new NotSupportedException("A level must have an exit.");
        }

        public Level(ContentManager serviceProvider, TileMap tileMap, RunnerCamera camera)
        {
            this.camera = camera;
            camera.LoadContent(serviceProvider);

            this.tileMap = tileMap;

            content = serviceProvider;

            menuFont = content.Load<SpriteFont>("MenuAssets/gamefont");
            block = content.Load<Texture2D>("square");

            foreach (CollisionLayer layer in tileMap.colLayers)
            {
                LoadTiles(layer);

                foreach (var cam in layer.camReferences)
                {
                    camera.Waypoints.Enqueue(TileInformation.GetPosition(cam.X, cam.Y));
                }
            }

            CollisionRetrieve.tileMap = tileMap;
            CollisionRetrieve.tile = tiles;

            player = new Player(serviceProvider, start, camera, tileMap.LevelName);

            HighTime.LoadData(tileMap.LevelName);
        }

        private Tile LoadTile(string tileType, int x, int y)
        {
            int tileNumberType;
            bool isNum = int.TryParse(tileType, out tileNumberType);
            string[] numbers = null;

            int addtionalNumber = 0;

            if (!isNum)
            {
                numbers = tileType.Split('#');
                tileNumberType = int.Parse(numbers[0]);
                addtionalNumber = int.Parse(numbers[1]);
            }

            switch (tileNumberType)
            {
                case 0:
                    return new Tile(null, TileCollision.Passable);

                case 1:
                    return new Tile(null, TileCollision.Impassable);

                // Exit
                case 2:
                    return new Tile(null, TileCollision.NormalTile);

                case 6:
                    return new Tile(null, TileCollision.RightSlope);

                case 7:
                    return new Tile(null, TileCollision.LeftSlope);

                case 8:
                    return LoadStartTile(x, y);

                case 9:
                    return LoadExitTile(x, y);

                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }

        /// <summary>
        /// Unloads the level content.
        /// </summary>
        public void Dispose()
        {
            //Content.Unload();
        }

        /// <summary>
        /// Instantiates a player, puts him in the level, and remembers where to put him when he is resurrected.
        /// </summary>
        private Tile LoadStartTile(int x, int y)
        {
            //if (Player != null)
            //    throw new NotSupportedException("A level may only have one starting point.");
            start = RectangleExtensions.GetBottomCenter(CollisionRetrieve.GetBounds(x, y));
            return new Tile(null, TileCollision.Passable);
        }

        /// <summary>
        /// Remembers the location of the level's exit.
        /// </summary>
        private Tile LoadExitTile(int x, int y)
        {
            Vector2 exitPoint = new Vector2(x * 32, y *32);


            exits.Add(new Rectangle((int)exitPoint.X,(int)exitPoint.Y, 32, 32));
            return new Tile(null, TileCollision.Passable);
        }

        public void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed > 1000.0f)
            {
                fps = totalFrames;
                totalFrames = 0;
                elapsed = 0;
            }

            if (player != null)
            {
                player.Update(gameTime);
                player.FallDeath(tileMap.Layers[0].WidthInPixels, tileMap.Layers[0].HeightInPixels);
                camera.ClampToArea((int)tileMap.Layers[0].WidthInPixels - 1280, (int)tileMap.Layers[0].HeightInPixels - 720);
            }

            foreach (Rectangle exit in exits)
            {
                if(exit.Intersects(player.PlayerRectangle) && player.IsOnGround == true)
                {
                    OnExitReached();
                }
            }
        }

        /// <summary>
        /// Called when the player reaches the level's exit.
        /// </summary>
        private void OnExitReached()
        {
            reachedExit = true;
            player.StopTimer = true;
            player.OnReachedExit();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, RunnerCamera camera)
        {
            camera.Waypoints.Draw(spriteBatch, camera);

            if (player != null)
            {
                player.Draw(gameTime, spriteBatch);
            }

            totalFrames++;

            spriteBatch.Begin(SpriteSortMode.Deferred,
                          null,
                         SamplerState.PointWrap, null, null, null,
                         camera.TransformMatrix);


            if (player.DebugInformation == true) {
                spriteBatch.DrawString(menuFont, "Fps : " + fps,
                           new Vector2(camera.Position.X + 1000, camera.Position.Y + 120), Color.Black);
            }

            //foreach (Rectangle exit in exits) {
            //    spriteBatch.Draw(block, exit, Color.White);
            //}

            spriteBatch.End();
        }
    }
}
