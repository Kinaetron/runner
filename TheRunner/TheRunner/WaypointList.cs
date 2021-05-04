using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using TheRunner.RunCamera;

namespace TheRunner
{
    public class WaypointList : Queue<Vector2>
    {
        // Draw data
        Texture2D waypointTexture;
        Vector2 waypointCentre;

        public void LoadContent(ContentManager content)
        {
            waypointTexture = content.Load<Texture2D>("dot");
            waypointCentre =
                new Vector2(waypointTexture.Width / 2, waypointTexture.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch, RunnerCamera camera)
        {
            if (Count == 1)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate,
                              BlendState.AlphaBlend,
                              null, null, null, null,
                              camera.TransformMatrix);

                spriteBatch.Draw(waypointTexture, Peek(), null, Color.Red,
                    0f, waypointCentre, 1f, SpriteEffects.None, 0f);

                spriteBatch.End();
            }
            else if (Count > 0)
            {
                float numberPoints = this.Count - 1;
                float i = 0;

                spriteBatch.Begin(SpriteSortMode.Immediate,
                              BlendState.AlphaBlend,
                              null, null, null, null,
                              camera.TransformMatrix);
                foreach (Vector2 location in this)
                {
                    spriteBatch.Draw(waypointTexture, location, null,
                        new Color(Vector4.Lerp(Color.Red.ToVector4(),
                        Color.Blue.ToVector4(), i / numberPoints)),
                        0f, waypointCentre, 1f, SpriteEffects.None, 0f);

                    i++;
                }
                spriteBatch.End();
            }
        }
    }
}
