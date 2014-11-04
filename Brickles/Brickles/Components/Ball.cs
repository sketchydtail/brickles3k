﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private float bounce = 0.9f;    //ball bounces back multiplied by this much
        private float speed = 15f;
        public BoundingSphere bounding;

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

            Vector3 translation = Vector3.Transform(Vector3.Forward * speed, Rotation);
            Position += translation;
            Transform =  Matrix.CreateTranslation(Position);

        }

        public void Bounce()        //this method is called when the ball collides with a brick, its spozed to make the ball bounce off, please fix
        {

            //fix this
            float angle = MathHelper.ToDegrees(RotateV.Y);
            double resultAngle = Math.Sin(angle);
            Console.WriteLine("Bounce angle: " + angle  + " result: " + -angle);
            RotateV = new Vector3(-RotateV.X, RotateV.Y, -RotateV.Z);

           // RotateV.X = oldPos.X + speed * (float)Math.Cos(direction);
           // position.Y = oldPos.Y + speed * (float)Math.Sin(direction);

            Random random = new Random();
            RotateV.X = Position.X * bounce * random.Next();
            RotateV.Y = Position.Y * bounce * random.Next();
            RotateV.Z = Position.Z * bounce * random.Next();


            scene.brickBounce.Play();

        }
         

    }
}
