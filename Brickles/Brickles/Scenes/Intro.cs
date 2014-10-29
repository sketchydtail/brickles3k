using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brickles
{
    class Intro : Scene
    {
        private Texture2D splash;
        private GameManager game;

        public Intro(GameManager game) : base (game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            splash = game.Content.Load<Texture2D>("Sprites/Splash");
            Console.WriteLine("Intro loaded");
        }

        public override void Update(GameTime gameTime)
        {

            Console.WriteLine("Gametime: " + gameTime.TotalGameTime.TotalSeconds);

            if (gameTime.TotalGameTime.TotalSeconds > 5)
            {
                NextScene();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            game.spriteBatch.Begin();
            game.spriteBatch.Draw(splash, new Rectangle(0,0,1024,768), Color.White);
            
            game.spriteBatch.End();
            
            base.Draw(gameTime);
            //
        }

        private void NextScene()
        {
            //Thread.Sleep(3000);
            game.setScene(GameScene.Game);
        }

    }
}
