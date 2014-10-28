using System;
using System.Net.PeerToPeer.Collaboration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brickles
{
    public class GameObject : Game
    {
        public Model Model;
        public Vector3 Position;
        public Matrix Transform;
        public Matrix[] LocalTransforms;
        public VertexPositionTexture[] tex;
        public Matrix Rotation;
        public Vector3 RotateV;


        public virtual void Draw(GameTime gameTime)
        {
            Model.Draw(Transform, Game1.ViewMatrix, Game1.ProjectionMatrix);
        }
    }
}