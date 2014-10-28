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
    /// 

    public enum Difficulty
    {
        Tutorial,
        Easy,
        Medium,
        Hard,
        Impossible
    };
    public class Game1 : Game
    {
        public const float scaleRatio = 1f; //scale everything by this much
        public static Game1 game;

        private readonly Vector3 cameraPosition = new Vector3(0, 0, 3000f);
        private readonly Vector3 cameraTarget = new Vector3(0f, 0f, 0f);
        public LinkedList<Brick> Bricks = new LinkedList<Brick>();
        public LinkedList<Ball> Balls = new LinkedList<Ball>();         //list of balls for multiball compatibility
        public Court Court;
        private LoadLevel Level;
        public Player Player;
        public static Matrix ProjectionMatrix;
        public static Matrix ViewMatrix;
        private KinectManager _kinectMan;

        //private Model PaddleModel;


        public Model brickModel;
        private int camAngle = 0;
        private Vector3 cameraUpVector = Vector3.Up;
        public Model courtModel;

        public Boolean debugging = true;
        public GraphicsDeviceManager graphics;

        public Texture2D hand;
        public Vector2 handPosition;
        private Texture2D jointTexture;

        public Difficulty difficulty = Difficulty.Medium;


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
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(0.9f, GraphicsDevice.Viewport.AspectRatio, 0.1f,
                5000f);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }


        protected override void LoadContent()
        {
            AssetManager.LoadModels();
            brickModel = Content.Load<Model>("Models/A1_Brick"); //need to load model here too to get measurements
            _kinectMan = new KinectManager();
            Court = new Court();
            Player = new Player();
            Level = new LoadLevel("brixel_sphere");
            Balls.AddFirst(new Ball());


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


            foreach (Ball b in Balls)
            {
                b.Update(gameTime);
                b.World = Matrix.CreateTranslation(b.Position);


                foreach (Brick brick in Bricks)
                {
                    if (brick.Type != BrickType.Dead)
                    {
                        if (IsCollision(b.Model, b.World, brick.Model, brick.World))
                        {
                            Console.WriteLine("Collision: " + b + " and " + brick);
                            b.Bounce();
                            brick.hitBrick();
                        }
                    }
                }

            }

            base.Update(gameTime);
        }

        protected void UpdateInput()
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            Player.Update(currentState);
        }

        private Ball.CollisionType CheckCollision(BoundingSphere sphere)
        {
            foreach (Brick b in Bricks)
            {
                if (b.bounding.Contains(sphere) != ContainmentType.Disjoint)
                    return Ball.CollisionType.Brick;
            }

            foreach (BoundingBox box in Court.bboxes)
            if (box.Contains(sphere) != ContainmentType.Contains)
                return Ball.CollisionType.Wall;

            return Ball.CollisionType.None;
        }

        private bool IsCollision(Model model1, Matrix world1, Model model2, Matrix world2)
        {
            for (int meshIndex1 = 0; meshIndex1 < model1.Meshes.Count; meshIndex1++)
            {
                BoundingSphere sphere1 = model1.Meshes[meshIndex1].BoundingSphere;
                sphere1 = sphere1.Transform(world1);

                for (int meshIndex2 = 0; meshIndex2 < model2.Meshes.Count; meshIndex2++)
                {
                    BoundingSphere sphere2 = model2.Meshes[meshIndex2].BoundingSphere;
                    sphere2 = sphere2.Transform(world2);

                    if (sphere1.Intersects(sphere2))
                        return true;
                }
            }
            return false;
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

                Court.Draw(gameTime);
            
            foreach (Brick brick in Bricks)
            {
                if (brick.Type != BrickType.Dead)
                {
                    brick.Draw(gameTime);
                }
            }
            
            foreach (Ball ball in Balls)
            {
                ball.Draw(gameTime);
            }
            

            _kinectMan.Draw(gameTime);



            base.Draw(gameTime);
        }
    }
}