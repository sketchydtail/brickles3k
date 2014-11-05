using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Brickles
{
   public class Health
    {
        public Texture2D heartBeatTexture;
        public Vector2 Position;
        public SpriteEffects spriteEffects = SpriteEffects.None;

        int frameCount = 24;
        int colCount = 4;
        int frameWidth = 992 / 4;
        int frameHeight = 960 / 6;
        int heartRateSpeed = 10;

        public void Initialize(Texture2D playerTexture, Vector2 Position)
        {
            this.heartBeatTexture = playerTexture;
            this.Position = Position;
        }

        public void setHeartRate(int heartRateSpeed) {
            this.heartRateSpeed = heartRateSpeed;
        }

        public void Draw(SpriteBatch spriteBatch, float time)
        {
            int frameID = 0;
            frameID += (int)(time * heartRateSpeed) % frameCount;
            int row = frameID / colCount;
            int col = frameID % colCount;

            Console.WriteLine("Frame: " + frameID + " row: " + row + " col: " + col);
            spriteBatch.Draw(heartBeatTexture, Position, new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight), Color.White, 0.0f, new Vector2(36, 49), 1.0f, spriteEffects, 0.5f);
        }
    }
}
