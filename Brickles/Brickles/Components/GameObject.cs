using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brickles
{
    public class GameObject
    {
        public Matrix[] LocalTransforms;
        public Model Model;
        public Vector3 Position;
        public Vector3 RotateV;
        public Matrix Rotation;
        public Matrix Transform;
        public Matrix World;
        public Game1 scene;
        public VertexPositionTexture[] tex;


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