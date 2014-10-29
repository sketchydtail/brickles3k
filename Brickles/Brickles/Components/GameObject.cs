using System;
using System.Net.PeerToPeer.Collaboration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brickles
{
    public class GameObject
    {
        public Game1 scene;
        public Model Model;
        public Vector3 Position;
        public Matrix Transform;
        public Matrix[] LocalTransforms;
        public VertexPositionTexture[] tex;
        public Matrix Rotation;
        public Vector3 RotateV;
        public Matrix World;
        

        public GameObject(Game1 scene)
        {
            this.scene = scene;
        }

        public virtual void Draw(GameTime gameTime)
        {
            Model.Draw(Transform, Game1.ViewMatrix, Game1.ProjectionMatrix);
        }
    }
}