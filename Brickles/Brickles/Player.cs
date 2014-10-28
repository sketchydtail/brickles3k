using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brickles
{
    public class Player
    {
        private const float VelocityScale = 30.0f;
        public Model Model;

        public int Health = 100;
        public int Score = 0;

        //Position of the model in world space
        public Vector3 Position = Vector3.Zero;

        //Velocity of the model, applied each frame to the model's position
        public Matrix RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2);
        public Matrix[] Transforms;
        public Vector3 Velocity = Vector3.Zero;
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
                        Matrix.CreateRotationX(MathHelper.PiOver2)*
                        Matrix.CreateRotationZ(rotation);
                }
            }
        }

        public void Update(GamePadState controllerState)
        {
            Velocity += RotationMatrix.Right*controllerState.ThumbSticks.Left.X*VelocityScale;
            Velocity += RotationMatrix.Forward*controllerState.ThumbSticks.Left.Y*VelocityScale;
        }
    }
}