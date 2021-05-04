using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TheRunner
{
    class Clouds
    {

        private ContentManager content;
        private Vector2 screenSize;

        private Texture2D CloudOne;
        private Texture2D CloudTwo;

        private Vector2 cloudMovementOne;
        private Vector2 cloudMovementTwo;

        public Clouds(ContentManager content, Vector2 screenSize)
        {
            this.content = content;
            this.screenSize = screenSize;

            LoadContent();

            cloudMovementOne.Y = CloudOne.Height + 50;
            cloudMovementTwo.Y = CloudOne.Height + 100;
        }

        public void LoadContent()
        {
            CloudOne = content.Load<Texture2D>("Cloud1");
            CloudTwo = content.Load<Texture2D>("Cloud2");
        }

        public void Update(GameTime gameTime)
        {
            cloudMovementOne.X += 1;
            cloudMovementTwo.X += 0.5f;
          
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            spritebatch.Draw(CloudOne, cloudMovementOne, Color.White);
            spritebatch.Draw(CloudTwo, cloudMovementTwo, Color.White);
        }
    }
}
