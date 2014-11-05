using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Brickles
{
    /// <summary>
    ///     Brickles-3000
    ///     All code  created by the wonderfully talented Sketchy.D.Tail (AKA. Julian Hunt) - Copyright September 2014
    ///     Any resemblance to other code is purely concidental.
    /// </summary>
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
        public static Matrix ProjectionMatrix;
        public static Matrix ViewMatrix;

        private readonly Vector3 cameraPosition = new Vector3(0, 0, 3000f);
        private readonly Vector3 cameraTarget = new Vector3(0f, 0f, 0f);

        public LinkedList<Ball> Balls = new LinkedList<Ball>(); //list of balls for multiball compatibility
        public LinkedList<Brick> Bricks = new LinkedList<Brick>();
        public Court Court;
        private LoadLevel Level;
        public SoundEffect MenuNotify;
        public SoundEffect Notify;
        public Player Player;
        public SoundEffect Recycle;
        public SpriteFont ScoreFont;
        public SoundEffect WallBounce;
        public KinectManager _kinectMan;
        public bool addBall = false;
        public SoundEffect brickBounce;

        public Model brickModel;
        private int camAngle = 0;
        private Vector3 cameraUpVector = Vector3.Up;
        public Controller controller = Controller.Keyboard;
        public Model courtModel;

        public Boolean debugging = true;
        public Difficulty difficulty = Difficulty.Medium;
        public Vector2 handPosition;

        public SoundEffect laser;
        public SoundEffect levelComplete;


        private Vector3 nextVector = Vector3.Forward;
        public Model paddleModel;
        public Vector3 paddlePos;
        public Matrix paddleTransform;
        public Song song1;
        public Text text;


        Vector2 heartPosition;
        Health heartBeat;

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

            paddlePos = new Vector3(0, 0, 2400f); //set paddle in middle of screen

            //heartbeat code
            heartBeat = new Health();

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

            song1 = game.Content.Load<Song>("Music/menu");


            //heartbeat code
            heartPosition = new Vector2((game.GraphicsDevice.Viewport.Width - (992 / 4)), game.GraphicsDevice.Viewport.Height - (960 / 6));
            heartBeat.Initialize(game.Content.Load<Texture2D>("Sprites/heart"), heartPosition);
        }

        protected override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(song1);
            }

            // var timeDelta = (float) gameTime.ElapsedGameTime.TotalSeconds;
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

            SpawnBall(); //spawn a ball if requested

            base.Update(gameTime);
        }

        public void TriggerBallSpawn()
        {
            addBall = true;
        }

        private void SpawnBall()
        {
            if (addBall)
            {
                Balls.RemoveFirst();
                Balls.AddFirst(new Ball(this));
                addBall = false;
            }
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

            //text.DrawText(ScoreFont, "Health: " + Player.Health, TextTypes.Health);
            text.DrawText(ScoreFont, "Score: " + Player.Score, TextTypes.Score);


            //heartbeat code
            float time = (float)gameTime.TotalGameTime.TotalSeconds;
            game.spriteBatch.Begin();
            heartBeat.Draw(game.spriteBatch, time);
            game.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}