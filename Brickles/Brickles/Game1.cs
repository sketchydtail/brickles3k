using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brickles
{
    /// <summary>
    ///     Brickles-3000
    ///     All code  created by the wonderfully talented Sketchy.D.Tail (AKA. Julian Hunt) - Copyright September 2014
    ///     Any resemblance to other code is purely concidental.
    /// </summary>
    public class Game1 : Game
    {
        public const float scaleRatio = 30f; //scale everything by this much
        public static Game1 game;

        private readonly Vector3 cameraPosition = new Vector3(0, 100, 2000f);
        private readonly Vector3 cameraTarget = new Vector3(0f, 0f, 0f);
        public LinkedList<Brick> Bricks = new LinkedList<Brick>();
        public Court Court;
        private LoadLevel Level;
        public Player Player;
        private Matrix ProjectionMatrix;
        private Matrix ViewMatrix;
        private KinectManager _kinectMan;
        private float aspectRatio;


        //private Model PaddleModel;


        public Model brickModel;
        private int camAngle = 0;
        private Vector3 cameraUpVector;
        public Model courtModel;

        public Boolean debugging = true;
        public GraphicsDeviceManager graphics;


        //private int drawbricknum = 0;
        public Texture2D hand;
        public Vector2 handPosition;
        private Texture2D jointTexture;


        private Vector3 nextVector = Vector3.Forward;

        //private Effect postComplementShader;

        public SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferMultiSampling = true; //enable multisampling / anti aliasing  
            game = this;
        }

        protected override void Initialize()
        {
            //GraphicsDevice does not exist in the constructor, so the game must be resized here.
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width; //set game width to screen width
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height; //set game height to screen height
            //graphics.IsFullScreen = true; // make it fullscreen
            graphics.ApplyChanges();

            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(0.8f, GraphicsDevice.Viewport.AspectRatio, 10f,
                100000f);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }


        protected override void LoadContent()
        {
            brickModel = Content.Load<Model>("Models/brick_square"); //need to load model here too to get measurements
            _kinectMan = new KinectManager();
            Court = new Court();
            Player = new Player();
            Level = new LoadLevel("brixel_sphere");

            //postComplementShader = Content.Load<Effect>("Content/post-complement-shader");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var timeDelta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);
            base.Update(gameTime);
        }

        protected void UpdateInput()
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            Player.Update(currentState);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            Court.Draw(gameTime);

            foreach (Brick brick in Bricks)
            {
                brick.Draw(gameTime);
            }

            _kinectMan.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}