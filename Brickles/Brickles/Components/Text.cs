using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brickles
{
    public enum TextTypes
    {
        Score,
        Health,
        Message,
        Debug
    };

    public class Text
    {
        public Scene scene;
        public Text(Scene scene)
        {
            this.scene = scene;
        }
        public void DrawText(SpriteFont font, String text, TextTypes type)
        {

            // Find the center of the string
            Vector2 FontOrigin = new Vector2(0f,0f);//font.MeasureString(text) / 2;
            // Draw the string
            Vector2 FontPos = new Vector2(5f,5f);
            Color textColour = Color.Red;

            switch (type)
            {
                case TextTypes.Debug:
                    break;
                case TextTypes.Health:
                    FontPos = new Vector2(scene.game.GraphicsDevice.Viewport.Width - font.MeasureString(text).X, scene.game.GraphicsDevice.Viewport.Height - 40);
                    textColour = Color.Red;
                    break;
                case TextTypes.Message:
                    break;
                case TextTypes.Score:
                    textColour = Color.LightGreen;
                   // FontPos = new Vector2(20f,50f);
                    break;
            }
            scene.game.spriteBatch.Begin();
            scene.game.spriteBatch.DrawString(font, text, FontPos, textColour,
                0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            scene.game.spriteBatch.End();
        }
    }
}
