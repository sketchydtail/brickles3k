using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brickles
{
    public class Court : GameObject
    {
        public Court()
        {
            Position = new Vector3(0, 400, -500); //save modified position
            Transform = Matrix.CreateScale(Game1.scaleRatio*6, Game1.scaleRatio*6, Game1.scaleRatio*12)*
                        Matrix.CreateRotationX((float) Math.PI)*Matrix.CreateTranslation(Position);
            Model = Content.Load<Model>("Models/court_seperated_v01");
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        /*
            foreach (ModelMesh m in courtModel.Meshes)
            {
                //Console.WriteLine("Brick mesh: " + m);
                foreach (BasicEffect e in m.Effects)
                {
                    //e.TextureEnabled = true;
                    e.EnableDefaultLighting();
                    e.World = courtLocalTransforms[m.ParentBone.Index] * courtWorldMatrix;
                    e.View = ViewMatrix;
                    e.Projection = ProjectionMatrix;
                    e.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
                }
                m.Draw();
            }*/
    }
}