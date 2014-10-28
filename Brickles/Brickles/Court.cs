using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brickles
{
    public class Court : GameObject
    {
        public LinkedList<BoundingBox> bboxes = new LinkedList<BoundingBox>(); 
        public Court()
        {
            Position = new Vector3(0, 0, 0); //save modified position
            Transform = Matrix.CreateScale(Game1.scaleRatio, Game1.scaleRatio, Game1.scaleRatio)*
                        Matrix.CreateRotationX((float) Math.PI)*Matrix.CreateTranslation(Position);
            Model = Game1.game.Content.Load<Model>("Models/Court");
            LocalTransforms = new Matrix[Model.Bones.Count];

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