using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

    public enum Controller
    {
        Keyboard,
        Gamepad,
        Kinect
    };

    public class Game1 : Scene
    {
        public const float scaleRatio = 1f; //scale everything by this much

        private readonly Vector3 cameraPosition = new Vector3(0, 0, 3000f);
        private readonly Vector3 cameraTarget = new Vector3(0f, 0f, 0f);

        public Text text;

        public LinkedList<Brick> Bricks = new LinkedList<Brick>();
        public LinkedList<Ball> Balls = new LinkedList<Ball>();         //list of balls for multiball compatibility
        public Court Court;
        private LoadLevel Level;
        public Player Player;
        public static Matrix ProjectionMatrix;
        public static Matrix ViewMatrix;
        public KinectManager _kinectMan;
        public SpriteFont ScoreFont;

        public Model brickModel;
        private int camAngle = 0;
        private Vector3 cameraUpVector = Vector3.Up;
        public Model courtModel;

        public Boolean debugging = true;
        public Vector2 handPosition;

        public Model paddleModel;
        public Matrix paddleTransform;
        public Vector3 paddlePos;


        public SoundEffect brickBounce;
        public SoundEffect laser;
        public SoundEffect levelComplete;
        public SoundEffect MenuNotify;
        public SoundEffect Notify;
        public SoundEffect Recycle;
        public SoundEffect WallBounce;

        

        public Difficulty difficulty = Difficulty.Medium;
        public Controller controller = Controller.Keyboard;


        private Vector3 nextVector = Vector3.Forward;

        //private Effect postComplementShader;

        //public SpriteBatch spriteBatch;


        public Game1(GameManager game) : base(game)
        {
            this.game = game;
            
        }

        public override void Initialize()
        {
            text = new Text(this);

            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(0.9f, game.GraphicsDevice.Viewport.AspectRatio, 0.1f,
                5000f);

            

            paddlePos = new Vector3(0, 0, 2400f);        //set paddle in middle of screen

            base.Initialize();
        }


        protected override void LoadContent()
        {
            game.assetManager.LoadModels();
            brickModel = game.Content.Load<Model>("Models/A1_Brick"); //need to load model here too to get measurements
            _kinectMan = new KinectManager(this);
            Court = new Court(this);
            Player = new Player(this);
            Level = new LoadLevel("brixel_sphere", this);
            Balls.AddFirst(new Ball(this));
            paddleModel = game.Content.Load<Model>("Models/Paddle");
            ScoreFont = game.Content.Load<SpriteFont>("Fonts/Scorefont");

            brickBounce = game.Content.Load<SoundEffect>("Sounds/brick_bounce");
            laser = game.Content.Load<SoundEffect>("Sounds/laser");
            levelComplete = game.Content.Load<SoundEffect>("Sounds/level_complete");
            MenuNotify = game.Content.Load<SoundEffect>("Sounds/menu_Selection");
            Notify = game.Content.Load<SoundEffect>("Sounds/notify");
            Recycle = game.Content.Load<SoundEffect>("Sounds/recycle");
            WallBounce = game.Content.Load<SoundEffect>("Sounds/wall_bounce");


        }

        protected override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            var timeDelta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.Exit();


            foreach (Ball b in Balls)
            {
                b.Update(gameTime);
                b.World = Matrix.CreateTranslation(b.Position);

                UpdateInput();

                UpdatePaddlePosition();

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
            if (controller == Controller.Gamepad)
            {

                GamePadState currentState = GamePad.GetState(PlayerIndex.One);
                Player.UpdateController(currentState);
            }
            else if (controller == Controller.Keyboard)
            {
                KeyboardState keystate = Keyboard.GetState();
                Player.UpdateKeyboard(keystate);
            }

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

        private void UpdatePaddlePosition()
        {
            //paddlePos = new Vector3(handPosition.X, handPosition.Y, 1000f);
            paddleTransform = Matrix.CreateTranslation(paddlePos);
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            game.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;         //turn off texture blurring for nice sharp retro look

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


            //Console.WriteLine("Paddlepos: " + paddlePos);
            paddleModel.Draw(paddleTransform, ViewMatrix, ProjectionMatrix);

            text.DrawText(ScoreFont, "Health: " + Player.Health, TextTypes.Health);
            text.DrawText(ScoreFont, "Score: " + Player.Score, TextTypes.Score);

            base.Draw(gameTime);
        }
    }
}