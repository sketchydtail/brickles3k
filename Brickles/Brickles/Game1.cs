using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brickles3k
{
    /// <summary>
    ///     Brickles-3000
    ///     All code  created by the wonderfully talented Sketchy.D.Tail (AKA. Julian Hunt) - Copyright September 2014
    ///     Any resemblance to other code is purely concidental.
    /// </summary>
    public class Game1 : Game
    {
        private const float scaleRatio = 30f; //scale everything by this much
        private readonly Dictionary<int, Brick> Bricks = new Dictionary<int, Brick>();
        private readonly Vector3 cameraPosition = new Vector3(0, 100, 2000f);
        private readonly Vector3 cameraTarget = new Vector3(0f, 0f, 0f);
        public GraphicsDeviceManager graphics;
        public static Game1 game;

        private readonly Player player = new Player();
        private readonly List<Vector3> sortedBricks = new List<Vector3>();
        private List<Vector3> BrickList = new List<Vector3>();
        private Model PaddleModel;

        private Matrix ProjectionMatrix;
        private Matrix ViewMatrix;
        private float aspectRatio;
        //private Matrix WorldMatrix;
        private Texture2D background;
        private Model brickModel;
        public Matrix[] brickTransforms;
        private int camAngle = 0;
        private Vector3 cameraUpVector;


        public Matrix[] courtLocalTransforms;
        private Model courtModel;
        public Vector3 courtPosition; //the bricks game position, modified from csv file
        public Vector3 courtRotation;
        public Matrix courtWorldMatrix;
        public Boolean debugging = true;


        private int drawbricknum = 0;

        private Matrix[] easterEggLocalTranforms;
        private Model easterEggModel;
        public Vector3 easterEggPosition;
        private Matrix easterEggWorldMatrix;
        public Texture2D hand;
        public Vector2 handPosition;
        private Texture2D jointTexture;

        private KinectManager _kinectMan = new KinectManager();


        private Vector3 nextVector = Vector3.Forward;

        //private Effect postComplementShader;


        private SpriteBatch spriteBatch;
        private int switchDelay = 2;
        private float timeSinceSwitch = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferMultiSampling = true; //enable multisampling / anti aliasing  
            game = this;
        }

        protected override void Initialize()
        {
            //kinectage
            try
            {
                KinectSensor.KinectSensors.StatusChanged += _kinectMan.KinectSensors_StatusChanged;
                _kinectMan.DiscoverKinectSensor();

                Debug.WriteLineIf(debugging, _kinectMan.kinect.Status);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            //end kinectage

            //GraphicsDevice does not exist in the constructor, so the game must be resized here.
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width; //set game width to screen width
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height; //set game height to screen height
            graphics.PreferMultiSampling = true;
            //graphics.IsFullScreen = true; // make it fullscreen
            graphics.ApplyChanges();
            aspectRatio = graphics.PreferredBackBufferWidth/graphics.PreferredBackBufferHeight;

            cameraUpVector = Vector3.Up; //default to camera up being straight along y
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(0.8f, aspectRatio, 10f, 100000f);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }


        protected override void LoadContent()
        {
            background = Content.Load<Texture2D>("Content/bgtest");
            brickModel = Content.Load<Model>("Content/Models/brick1");
            courtModel = Content.Load<Model>("Content/Models/court_seperated_v01");
            //PaddleModel = Content.Load<Model>("Content/Models/Paddle");
            //easterEggModel = Content.Load<Model>("Content/Models/Cactus_Tall_01");

            jointTexture = Content.Load<Texture2D>("Content/joint");
            hand = Content.Load<Texture2D>("Content/hand");

            //postComplementShader = Content.Load<Effect>("Content/post-complement-shader");

            PlaceCourt();

            ParseCSV(ReadFile("brixel_sphere.csv")); //move / change this to the level in question
            SortBricks(); //sort bricks before generating brick positions;
            GenerateBrickPos();
            LoadBrickModels();
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

            //courtWorldMatrix = Matrix.CreateRotationX(0.001f);

            PlaceEasterEgg();
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);
            base.Update(gameTime);
        }

        protected void UpdateInput()
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            player.Update(currentState);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            /*
            //draw the background sprite
            spriteBatch.Begin(SpriteSortMode.FrontToBack,
                BlendState.Opaque);
              spriteBatch.Draw(background,
                new Rectangle(0, 0, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height), Color.White);
              * 
            spriteBatch.End();
            */

            DrawCourt();
            // graphics.GraphicsDevice.BlendState = BlendState.Opaque;
            DrawBricks();


            if (_kinectMan.kinected)
            {
                spriteBatch.Begin();
                /*spriteBatch.Draw(colourVideo,
                    new Rectangle(0, 0, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height),
                    Color.White); */
                //spriteBatch.Draw(depthVideo, new Rectangle(0,0,screenWidth,240), Color.White);
                spriteBatch.Draw(hand, handPosition, Color.White);
                _kinectMan.DrawSkeleton(spriteBatch,
                    new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), jointTexture);
                spriteBatch.End();
            }

            //graphics.GraphicsDevice.BlendState = BlendState.Additive;


            

            //DrawEasterEgg();
            base.Draw(gameTime);
        }

        private void PlaceCourt()
        {
            courtPosition = new Vector3(0, 400, -500); //save modified position
            courtLocalTransforms = new Matrix[courtModel.Bones.Count];
            //courtWorldMatrix = Matrix.Identity;
            courtWorldMatrix = Matrix.CreateScale(scaleRatio*6, scaleRatio*6, scaleRatio*12)*
                               Matrix.CreateRotationX((float) Math.PI)*Matrix.CreateTranslation(courtPosition);
        }

        private void DrawCourt()
        {
            courtModel.CopyAbsoluteBoneTransformsTo(courtLocalTransforms);
            foreach (ModelMesh m in courtModel.Meshes)
            {
                //Console.WriteLine("Brick mesh: " + m);
                foreach (BasicEffect e in m.Effects)
                {
                    //e.TextureEnabled = true;
                    e.EnableDefaultLighting();
                    e.World = courtLocalTransforms[m.ParentBone.Index]*courtWorldMatrix;
                    e.View = ViewMatrix;
                    e.Projection = ProjectionMatrix;
                    e.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
                }
                m.Draw();
            }
        }

        private void DrawBricks()
        {
            foreach (var brick in Bricks)
            {
                brick.Value.model.CopyAbsoluteBoneTransformsTo(brick.Value.localTransforms);
                foreach (ModelMesh m in brick.Value.model.Meshes)
                {
                    foreach (BasicEffect e in m.Effects)
                    {
                        //e.TextureEnabled = true;
                        e.EnableDefaultLighting();
                        e.World = brick.Value.localTransforms[m.ParentBone.Index]*brick.Value.WorldMatrix;
                        e.View = ViewMatrix;
                        e.Projection = ProjectionMatrix;
                        e.DiffuseColor = brick.Value.colour;
                        //postComplementShader.CurrentTechnique = postComplementShader.Techniques["PostComplement"];
                    }
                    m.Draw();
                }
            }
        }

        private void DrawEasterEgg()
        {
            easterEggModel.CopyAbsoluteBoneTransformsTo(easterEggLocalTranforms);
            foreach (ModelMesh m in easterEggModel.Meshes)
            {
                foreach (BasicEffect e in m.Effects)
                {
                    //e.TextureEnabled = true;
                    e.EnableDefaultLighting();
                    e.World = easterEggLocalTranforms[m.ParentBone.Index]*easterEggWorldMatrix;
                    e.View = ViewMatrix;
                    e.Projection = ProjectionMatrix;
                    e.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
                    //postComplementShader.CurrentTechnique = postComplementShader.Techniques["PostComplement"];
                }
                m.Draw();
            }
        }

        private void PlaceEasterEgg()
        {
            //easterEggPosition = new Vector3(0, 400, -500); //save modified position
            easterEggLocalTranforms = new Matrix[courtModel.Bones.Count];
            //courtWorldMatrix = Matrix.Identity;
            easterEggWorldMatrix = Matrix.CreateScale(scaleRatio, scaleRatio, scaleRatio)*
                                   Matrix.CreateTranslation(easterEggPosition);
        }


        private void SortBricks()
        {
            float lowestZ = 0; //this is the back position, these bricks will be drawn first


            //presort
            foreach (Vector3 brick in BrickList)
                //find furthest back brick, bricks will be sorted with this at the beginning
            {
                //if (Vector3.DistanceSquared(brick, cameraPosition) > lowestZ)
                if (brick.Z < lowestZ)
                {
                    lowestZ = brick.Z;
                }
            }

            //sort
            int numBricksRemaining = BrickList.Count;
            while (numBricksRemaining > 0)
            {
                foreach (Vector3 brick in BrickList)
                {
                    if (brick.Z == lowestZ)
                    {
                        sortedBricks.Add(brick);
                        numBricksRemaining--;
                    }
                }
                lowestZ++; //move onto next closest level when finished;
            }
            BrickList = sortedBricks;
        }


        public void LoadBrickModels()
        {
            foreach (var brick in Bricks)
            {
                brick.Value.model = Content.Load<Model>("Content/Models/brick_square"); //get brick model
            }
        }

        private void GenerateBrickPos()
        {
            Console.WriteLine("Generating bricks");
            float brickSize = 0f;
            //measure the brick model
            foreach (ModelMesh mesh in brickModel.Meshes)
            {
                BoundingSphere bs = mesh.BoundingSphere;
                brickSize = bs.Radius;
            }

            int brickNumber = 0;

            var rand = new Random();
            foreach (Vector3 brick in BrickList)
            {
                if (brick != null)
                {
                    //apply scaling and transform then add it to the list - 'temp' is just used for debugging purposes
                    Console.WriteLine("Z:" + brick.Z);
                    var temp = new Vector3(brick.X*brickSize*scaleRatio, brick.Y*brickSize*scaleRatio,
                        brick.Z*brickSize*scaleRatio);
                    //Console.WriteLine("Brick number: " + brickNumber);

                    //Console.WriteLine("Brick @: " + temp);

                    var thisBrick = new Brick();
                    //bricks go in the brictionary
                    Bricks.Add(brickNumber, thisBrick); //create a new brick in dictionary
                    brickNumber++;
                    thisBrick.gridPos = brick; //save raw csv values here
                    thisBrick.position = temp; //save modified position
                    thisBrick.localTransforms = new Matrix[brickModel.Bones.Count];
                    thisBrick.WorldMatrix = Matrix.Identity;
                    thisBrick.WorldMatrix = Matrix.CreateScale(scaleRatio, scaleRatio, scaleRatio)*
                                            Matrix.CreateTranslation(thisBrick.position);
                    thisBrick.colour = new Vector3((float) rand.NextDouble(), (float) rand.NextDouble(),
                        (float) rand.NextDouble());

                    //create and store a transform matrix that places the brick in the right place & at the right scale
                }
            }
            Console.WriteLine("Brick processing complete!");
            Console.WriteLine(" Created " + brickNumber + " brix");
        }


        private void ParseCSV(string csv)
        {
            string[] brickRefs;
            BrickList = new List<Vector3>();
            brickRefs = csv.Split('{');
            foreach (string brickRef in brickRefs)
            {
                //Console.WriteLine("Brickref >" + brickRef + "<");
                if (brickRef.Length > 4)
                {
                    string trimmedBrick = brickRef.TrimEnd('\n');
                    //trimmedBrick = brickRef.TrimEnd('\r');
                    //trimmedBrick = brickRef.TrimEnd('\f');
                    trimmedBrick = trimmedBrick.Replace('\n', '0');
                    trimmedBrick = trimmedBrick.Replace('\r', '0');
                    trimmedBrick = trimmedBrick.Replace('\f', '0');
                    trimmedBrick = trimmedBrick.Replace('}', '0');
                    //Console.WriteLine("Brick at: >" + trimmedBrick + "<");
                    BrickList.Add(Vector3FromString(trimmedBrick));
                }
            }
        }

        private string ReadFile(string mapName)
        {
            string mapContents = "";
            using (Stream stream = TitleContainer.OpenStream("Content/Content/Maps/" + mapName))
            {
                using (var reader = new StreamReader(stream))
                {
                    mapContents = reader.ReadToEnd();
                }
            }
            return mapContents;
        }

        //a huge hack of some dodgy code found on unity forums
        private Vector3 Vector3FromString(string Vector3string)
        {
            //get first number (x)
            int startChar = 0;
            int endChar = Vector3string.IndexOf(",");
            var returnx = (float) Convert.ToDecimal(Vector3string.Substring(startChar, endChar - 1));
            returnx = Convert.ToInt32(returnx);
            //get second number (y)
            Vector3string = Vector3string.Substring(endChar + 1, Vector3string.Length - endChar - 1);
            endChar = Vector3string.IndexOf(",");
            var returny = (float) Convert.ToDecimal(Vector3string.Substring(startChar, endChar - 1));
            returny = Convert.ToInt32(returny);
            //get third number (z)
            Vector3string = Vector3string.Substring(endChar + 1, Vector3string.Length - endChar - 1);
            endChar = Vector3string.Length;
            var returnz = (float) Convert.ToDecimal(Vector3string.Substring(startChar, endChar - 1));
            returnz = Convert.ToInt32(returnz);

            //remove these to return to single spacing
            returnx = returnx/2;
            returny = returny/2;
            returnz = returnz/2;
            return new Vector3(returnx, returny, returnz);
        }
    }
}