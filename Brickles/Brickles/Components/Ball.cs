using System;
using Microsoft.Xna.Framework;

namespace Brickles
{
    public class Ball : GameObject
    {
        public enum CollisionType
        {
            Brick,
            Ball,
            Wall,
            Player,
            None
        };

        private float bounce = 0.9f; //ball bounces back multiplied by this much
        public BoundingSphere bounding;
        private float speed = 15f;

        public Ball(Game1 scene) : base(scene)
        {
            this.scene = scene;
            Model = scene.game.assetManager.getBallModel(balls.Normal);

            //Position = new Vector3(0, 0, 2900f); //save modified position
            Position = scene.paddlePos;
            LocalTransforms = new Matrix[Model.Bones.Count];
            RotateV = new Vector3(0, MathHelper.ToRadians(3), 0);
        }

        public void changeBall(balls b)
        {
            Model = scene.game.assetManager.getBallModel(b);

            switch (b)
            {
                case balls.Normal:
                    bounce = 0.8f;
                    break;
                case balls.Steel:
                    bounce = 0.3f;
                    break;
                case balls.Super:
                    bounce = 1.3f;
                    break;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            Rotation = Matrix.CreateFromYawPitchRoll(RotateV.Y, RotateV.X, 0);

            Vector3 translation = Vector3.Transform(Vector3.Forward*speed, Rotation);
            Position += translation;
            Transform = Matrix.CreateTranslation(Position);
        }

        public void PerformWallCollision()
        {

            //it SHOULD make the ball hit bounces on the wall
            //but it is not working
            //needs to be fixed
            const float WALL_HEIGHT = 1.2f * 2019.592f; //courtsize radius is 2019.592f
            const float FLOOR_PLANE_SIZE = 1.2f * 2019.592f;
            const float CAMERA_BOUNDS_MIN_X = -FLOOR_PLANE_SIZE;
            const float CAMERA_BOUNDS_MAX_X = FLOOR_PLANE_SIZE;
            const float CAMERA_BOUNDS_MIN_Y = -WALL_HEIGHT;
            const float CAMERA_BOUNDS_MAX_Y = WALL_HEIGHT;
            const float CAMERA_BOUNDS_MIN_Z = -FLOOR_PLANE_SIZE;
            const float CAMERA_BOUNDS_MAX_Z = FLOOR_PLANE_SIZE;

            if (Position.X > CAMERA_BOUNDS_MAX_X)
            {
                Bounce();
                //Console.WriteLine("Here 1");
            }

            if (Position.X < CAMERA_BOUNDS_MIN_X)
            {
                Bounce();
                //Console.WriteLine("Here 2");
            }

            if (Position.Y > CAMERA_BOUNDS_MAX_Y)
            {
                Bounce();
                //Console.WriteLine("Here 3");
            }

            if (Position.Y < CAMERA_BOUNDS_MIN_Y)
            {
                Bounce();
                //Console.WriteLine("Here 4");
            }

            if (Position.Z > CAMERA_BOUNDS_MAX_Z)
            {
                Bounce();
                //Console.WriteLine("Here 5");
            }

            if (Position.Z < CAMERA_BOUNDS_MIN_Z)
            {
                Bounce();
                //Console.WriteLine("Here 6");
            }

        }

        public void Bounce()
            //this method is called when the ball collides with a brick, its spozed to make the ball bounce off, please fix
        {
            //fix this
            float angleX = MathHelper.ToDegrees(RotateV.X);
            float angleY = MathHelper.ToDegrees(RotateV.Y);
            float angleZ = MathHelper.ToDegrees(RotateV.Z);
            Console.WriteLine("Bounce angle: " + angleX + " result: " + -angleX);
            RotateV = new Vector3(-RotateV.X, RotateV.Y, -RotateV.Z);


            //LESS RANDOM BOUNCING

            RotateV.X = Position.X + speed * (float)Math.Cos(angleX);
            RotateV.Y = Position.Y + speed * (float)-Math.Sin(angleY);
            RotateV.Z = Position.Z;

            //RANDOM BOUCING

            //var random = new Random();
            //RotateV.X = Position.X*bounce*random.Next();
            //RotateV.Y = Position.Y*bounce*random.Next();
            //RotateV.Z = Position.Z*bounce*random.Next();

            //Console.WriteLine("RotateV.X " + RotateV.X + " RotateV.Y " + RotateV.Y + " RotateV.Z " + RotateV.Z);

            scene.brickBounce.Play();
        }
    }
}