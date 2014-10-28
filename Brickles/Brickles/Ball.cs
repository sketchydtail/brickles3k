using System;
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

        public Ball()
        {
            Model = AssetManager.getBallModel(balls.Normal);

            Position = new Vector3(0, 0, 2900f); //save modified position
            LocalTransforms = new Matrix[Model.Bones.Count];
            RotateV = new Vector3(0, MathHelper.ToRadians(0), 0);
        }

        public void changeBall(balls b)
        {
            Model = AssetManager.getBallModel(b);

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
         

    }
}
