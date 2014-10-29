using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brickles
{
    class Intro : Game
    {
       public GraphicsDeviceManager graphics;
        private Texture2D splash;
        private SpriteBatch sb;

        public Intro()
        {
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferMultiSampling = true; //enable multisampling / anti aliasing  
            
        }

        protected override void Initialize()
        {
            sb = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            splash = Content.Load<Texture2D>("Sprites/Splash");
            Console.WriteLine("Intro loaded");
        }

        protected override void Update(GameTime gameTime)
        {

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.Draw(splash, new Rectangle(0,0,1024,768), Color.White);
            
            sb.End();
            
            base.Draw(gameTime);
            NextScene();
        }

        private void NextScene()
        {
            Thread.Sleep(3000);
            StateLoader.changeState(state.Level);
        }

    }
}
