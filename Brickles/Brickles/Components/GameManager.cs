using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brickles
{
    public enum GameScene
        {
            Intro,
            Menu,
            Options,
            Game,
            GameOver
        };

    public class GameManager : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public AssetManager assetManager;

        private Game1 game;
        private Intro intro;
        

        public GameManager()
        {
            assetManager = new AssetManager(this);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //game.Content.RootDirectory = "Content";
            graphics.PreferMultiSampling = true; //enable multisampling / anti aliasing
             //intro = new Intro(this);
            // game1 = new Game1(this);

            
        }


        protected override void Initialize()
        {
            //GraphicsDevice does not exist in the constructor, so the game must be resized here.
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width; //set game width to screen width
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height; //set game height to screen height
            //graphics.IsFullScreen = true; // make it fullscreen
            graphics.ApplyChanges();

            intro = new Intro(this);
            intro.Enabled = true;
            intro.Visible = true;

            game = new Game1(this);
            game.Enabled = false;
            game.Visible = false;



            Components.Add(intro);
            Components.Add(game);
            
            //Services.RemoveService(typeof(Intro));
            //((Game1)Services.GetService(typeof (Game1))).Enabled = false;


            
            base.Initialize();
            
            
            //intro.Enabled = true;
            //game1.Enabled = false;
            //intro.Initialize();
            //game1.Initialize();

            
            //Components.Add(game1);

        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            

           
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void setScene(GameScene s)
        {
            
            switch (s)
            {
                case GameScene.Intro:
                    intro.Visible = true;
                    intro.Enabled = true;
                    game.Enabled = false;
                    game.Visible = false;
                    break;
                case GameScene.Game:
                    intro.Visible = false;
                    intro.Enabled = false;
                    game.Enabled = true;
                    game.Visible = true;
                    break;
            }
            
        }


    }
}
