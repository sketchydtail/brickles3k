using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brickles3k
{
    class Player
    {
        public Model Model;
        public Matrix[] Transforms;

        //Position of the model in world space
        public Vector3 Position = Vector3.Zero;

        //Velocity of the model, applied each frame to the model's position
        public Vector3 Velocity = Vector3.Zero;

        private const float VelocityScale = 30.0f;
        public Matrix RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2);
        private float rotation;
        public float Rotation
        {
            get { return rotation; }
            set
            {
                float newVal = value;
                while (newVal >= MathHelper.TwoPi)
                {
                    newVal -= MathHelper.TwoPi;
                }
                while (newVal < 0)
                {
                    newVal += MathHelper.TwoPi;
                }

                if (rotation != value)
                {
                    rotation = value;
                    RotationMatrix =
                        Matrix.CreateRotationX(MathHelper.PiOver2) *
                        Matrix.CreateRotationZ(rotation);
                }
            }
        }

        public void Update(GamePadState controllerState)
        {
            Velocity += RotationMatrix.Right * controllerState.ThumbSticks.Left.X * VelocityScale;
            Velocity += RotationMatrix.Forward * controllerState.ThumbSticks.Left.Y * VelocityScale;


        }
    }
}
