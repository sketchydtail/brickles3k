using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brickles
{
    public enum BrickType
    {
        Normal,
        Unbreakable,
        Treasure,
        Dead
    };
    public class Brick : GameObject
    {

        public Vector3 Colour;
        public Vector3 GridPos; //the value read from csv
        public BoundingBox bounding;
        public BrickType Type;
        
        public Brick(Vector3 gridPos, float brickSize)
        {
            GridPos = gridPos;
            Type = BrickType.Normal;            //set brick type
            Model = AssetManager.getBrickModel(Type);       //load normal brick model

            Position = new Vector3(GridPos.X*brickSize*Game1.scaleRatio, GridPos.Y*brickSize*Game1.scaleRatio,
                GridPos.Z*brickSize*Game1.scaleRatio);

            Transform = Matrix.CreateScale(Game1.scaleRatio, Game1.scaleRatio, Game1.scaleRatio)*
                        Matrix.CreateTranslation(Position);

            LocalTransforms = new Matrix[Model.Bones.Count];
            /*
            Vector3[] brickPoints = new Vector3[2];
            brickPoints[0] = new Vector3(Position.X - (brickSize/2), Position.Y - (brickSize/2), Position.Z - (brickSize/2));
            brickPoints[1] = new Vector3(Position.X + (brickSize/2), Position.Y + (brickSize/2), Position.Z + (brickSize/2));
            bounding = BoundingBox.CreateFromPoints(brickPoints);
            */
            World = Matrix.CreateTranslation(Position);
        }

        public void setBrickType(BrickType type)
        {
            Type = type;
            Model = AssetManager.getBrickModel(Type);       //change brick model to appropriate special type
        }

        public void hitBrick()
        {
            if (Type == BrickType.Treasure)
            {
                Type = BrickType.Dead;
            }
            else if (Type == BrickType.Normal)
            {
                Type = BrickType.Dead;
            }
        }

    }
}