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
        public Game1 scene;
        private int width;
        private int height;


        public Player(Scene scene)
        {
            this.scene = (Game1)scene;
            width = scene.game.GraphicsDevice.Viewport.Width;
            height = scene.game.GraphicsDevice.Viewport.Height;
        }

        public void UpdateController(GamePadState controllerState)
        {
            float moveX = controllerState.ThumbSticks.Left.X * moveSensitivity;
            float moveY = controllerState.ThumbSticks.Left.Y * moveSensitivity;

            float oldX = scene.paddlePos.X;
            float oldY = scene.paddlePos.Y;


            scene.paddlePos = CheckBounds(new Vector3(oldX + moveX, oldY + moveY, 2400f));
        }

        public void UpdateKeyboard(KeyboardState keyboardState)
        {
            float moveX = 0f;
            float moveY = 0f;
            float oldX = scene.paddlePos.X;
            float oldY = scene.paddlePos.Y;

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

            if (keyboardState.IsKeyDown(Keys.Space))        //respawn the ball if you press 
            {
                scene.TriggerBallSpawn();
            }
            
            scene.paddlePos = CheckBounds(new Vector3 (oldX + moveX, oldY + moveY, 2400f));
        }

        public Vector3 CheckBounds(Vector3 pos)
        {

            if (pos.X > width / 2)
            {
                pos.X = width / 2;
            }
            else if (pos.X < -width / 2)
            {
                pos.X = -width / 2;
            }

            if (pos.Y > height / 2)
            {
                pos.Y = height / 2;
            }
            else if (pos.Y < -height / 2)
            {
                pos.Y = -height / 2;
            }

            return pos;
        }

        public void paddleCollision()
        {
            scene.laser.Play();
        }
    }
}