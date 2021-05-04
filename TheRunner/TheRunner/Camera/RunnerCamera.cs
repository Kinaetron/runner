using System;

using PolyOne.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TheRunner.RunCamera
{
    public class RunnerCamera : Camera
    {
        //private float cameraLerpFactorSpeed = 0.1f;
        private float cameraLerpFactor = 0.15f;
        private const float cameraLerpFactorUp = 0.05f;
        private float multiplyBy = 0;
        float newX;
        private Random random = new Random();

        public Rectangle CameraTrap
        {
            get { return cameraTrap; }
            set { cameraTrap = value; }
        }
        private Rectangle cameraTrap;

        public WaypointList Waypoints
        {
            get { return waypoints; }
        }
        private WaypointList waypoints;

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }
        Vector2 origin;

        public float ShakeTimer
        {
            get { return shaketimer; }
        }

        private float shaketimer;

        private const float atDestinationLimit = 20f;

        /// <summary>
        /// Linear distance to the Tank's current destination
        /// </summary>
        public float DistanceToDestination
        {
            get { return Vector2.Distance(Position, waypoints.Peek()); }
        }

        /// <summary>
        /// True when the tank is "close enough" to it's destination
        /// </summary>
        public bool AtDestination
        {
            get { return DistanceToDestination < atDestinationLimit; }
        }


        public void LoadContent(ContentManager content)
        {
            waypoints = new WaypointList();
            Waypoints.LoadContent(content);
        }

        public void LinearNodeMovement(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (waypoints.Count > 0)
            {
                if (AtDestination)
                {
                    waypoints.Dequeue();
                }
                else
                {
                    Vector2 direction = -(Position - Waypoints.Peek());
                    direction.Normalize();

                    Position += (direction * 4.0f);
                }
            }
        }


        public void ScreenShake(GameTime gameTime, float shakeTime, float positionAmount)
        {
            if (shaketimer <= 0) {
                shaketimer = shakeTime;
            }

            if (shaketimer > 0) {
                shaketimer -= gameTime.ElapsedGameTime.Milliseconds;

                Position += new Vector2((float)((random.NextDouble() * 2) - 1) * positionAmount,
                      (float)((random.NextDouble() * 2) - 1) * positionAmount);
            }
        }

        public void LockToTarget(Vector2 velocity, Rectangle bounds, int screenWidth, int screenHeight)
        {
            if (bounds.Right > cameraTrap.Right) {
                multiplyBy = 0.3f;
                cameraTrap.X = bounds.Right - CameraTrap.Width;
            }

            if (bounds.Left < CameraTrap.Left) {
                multiplyBy = 0.6f;
                cameraTrap.X = bounds.Left;
            }

            if (bounds.Bottom > CameraTrap.Bottom) {
                cameraTrap.Y = bounds.Bottom - CameraTrap.Height;
            }

            if (bounds.Top < CameraTrap.Top) {
                cameraTrap.Y = bounds.Top;
            }

            newX = cameraTrap.X + (cameraTrap.Width * multiplyBy) - (screenWidth * multiplyBy);
            Position.X = (int)Math.Round(MathHelper.Lerp(Position.X, newX, cameraLerpFactor));
            Position.Y = (int)Math.Round((double)cameraTrap.Y + (cameraTrap.Height / 2) - (screenHeight / 2));
        }

        public void MoveTrapUp(float target)
        {
            float moveCamera = target - cameraTrap.Height;
            cameraTrap.Y = (int)MathHelper.Lerp(CameraTrap.Y, moveCamera, cameraLerpFactorUp);
        }
    }
}
