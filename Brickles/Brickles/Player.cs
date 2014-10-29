using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brickles
{
    public class Player
    {

        public int Health = 100;
        public int Score = 0;
        public float moveSensitivity = 20f;


        public void UpdateController(GamePadState controllerState)
        {
            float moveX = controllerState.ThumbSticks.Left.X * moveSensitivity;
            float moveY = controllerState.ThumbSticks.Left.Y * moveSensitivity;

            float oldX = Game1.game.paddlePos.X;
            float oldY = Game1.game.paddlePos.Y;


            Game1.game.paddlePos = new Vector3(oldX + moveX, oldY + moveY, 2400f);
        }

        public void UpdateKeyboard(KeyboardState keyboardState)
        {
            float moveX = 0f;
            float moveY = 0f;
            float oldX = Game1.game.paddlePos.X;
            float oldY = Game1.game.paddlePos.Y;

            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))      //down
            {
                moveY = -1 * moveSensitivity;
            }

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))      //up
            {
                moveY = 1 * moveSensitivity;
            }

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))      //left
            {
                moveX = -1 * moveSensitivity;
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))      //right
            {
                moveX = 1 * moveSensitivity;
            }
            
            Game1.game.paddlePos = new Vector3 (oldX + moveX, oldY + moveY, 2400f);
        }
    }
}