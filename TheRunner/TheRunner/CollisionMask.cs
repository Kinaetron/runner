using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace TheRunner
{
    public class CollisionMask
    {
        Texture2D leftSlope;
        Texture2D rightSlope;

        Color[] slopeLeftTextureData;
        Color[] slopeRightTextureData;

        public bool[,] RightCollisionMask
        {
            get { return rightCollisionMask; }
        }
        bool[,] rightCollisionMask;


        public bool[,] LeftCollisionMask
        {
            get { return leftCollisionMask; }
        }
        bool[,] leftCollisionMask;

        public void Initialize(ContentManager content)
        {
            leftSlope = content.Load<Texture2D>("LeftSlopeBlock");
            rightSlope = content.Load<Texture2D>("RightSlopeBlock");

            slopeRightTextureData = new Color[rightSlope.Width * rightSlope.Height];
            rightSlope.GetData(slopeRightTextureData);
            rightCollisionMask = new bool[rightSlope.Width, rightSlope.Height];

            slopeLeftTextureData = new Color[leftSlope.Width * leftSlope.Height];
            leftSlope.GetData(slopeLeftTextureData);
            leftCollisionMask = new bool[leftSlope.Width, leftSlope.Height];

            CreateHeightMasks();
        }

        private void CreateHeightMasks()
        {
            for (int i = 0; i < rightSlope.Width; i++)
            {
                for (int j = 0; j < rightSlope.Height; j++)
                {
                    Color colourA = slopeRightTextureData[i + j * 32];

                    if (colourA.A == 255) {
                        rightCollisionMask[i, j] = true;
                    }
                    else {
                        rightCollisionMask[i, j] = false;
                    }
                }
            }

            for (int i = 0; i < leftSlope.Width; i++)
            {
                for (int j = 0; j < leftSlope.Height; j++)
                {
                    Color colourA = slopeLeftTextureData[i + j * 32];

                    if (colourA.A > 150) {
                        leftCollisionMask[i, j] = true;
                    }
                    else {
                        leftCollisionMask[i, j] = false;
                    }
                }
            }
        }
    }
}
