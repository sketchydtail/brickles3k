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
        public AssetManager assetManager;

        private Game1 game;
        public GraphicsDeviceManager graphics;
        private Intro intro;
        private Menu menu;
        public SpriteBatch spriteBatch;


        public GameManager()
        {
            assetManager = new AssetManager(this);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferMultiSampling = true; //enable multisampling / anti aliasing
        }


        protected override void Initialize()
        {
            //GraphicsDevice does not exist in the constructor, so the game must be resized here.
            //graphics.IsFullScreen = true; // make it fullscreen
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width; //set game width to screen width
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height; //set game height to screen height

            //SamplerState sstate = new SamplerState();
            //sstate.AddressU = TextureAddressMode.Wrap;
            //sstate.AddressV = TextureAddressMode.Wrap;
            //sstate = SamplerState.PointWrap;
            //GraphicsDevice.SamplerStates[0] = sstate;

            //GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            //GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                //turn off texture blurring for nice sharp retro look

            graphics.ApplyChanges();

            intro = new Intro(this);
            intro.Enabled = true;
            intro.Visible = true;

            menu = new Menu(this);
            menu.Enabled = false;
            menu.Visible = false;

            game = new Game1(this);
            game.Enabled = false;
            game.Visible = false;


            Components.Add(intro);
            Components.Add(menu);
            Components.Add(game);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // TODO: Eat, Sleep, Rave, Repeat

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


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
                    menu.Enabled = false;
                    menu.Visible = false;
                    break;
                case GameScene.Game:
                    intro.Visible = false;
                    intro.Enabled = false;
                    game.Enabled = true;
                    game.Visible = true;
                    menu.Enabled = false;
                    menu.Visible = false;
                    break;
                case GameScene.Menu:
                    intro.Visible = false;
                    intro.Enabled = false;
                    game.Enabled = false;
                    game.Visible = false;
                    menu.Enabled = true;
                    menu.Visible = true;
                    break;
            }
        }
    }
}