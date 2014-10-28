using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brickles
{
    public class Brick : GameObject
    {
        public Vector3 Colour;
        public Vector3 GridPos; //the value read from csv
        public BoundingBox bounding;
        


        public Brick(Vector3 gridPos, float brickSize)
        {
            GridPos = gridPos;

            Position = new Vector3(GridPos.X*brickSize*Game1.scaleRatio, GridPos.Y*brickSize*Game1.scaleRatio,
                GridPos.Z*brickSize*Game1.scaleRatio);

            Transform = Matrix.CreateScale(Game1.scaleRatio, Game1.scaleRatio, Game1.scaleRatio)*
                        Matrix.CreateTranslation(Position);

            var rand = new Random();
            Colour = new Vector3((float) rand.NextDouble(), (float) rand.NextDouble(),
                (float) rand.NextDouble());

            Model = AssetManager.getRandomBrickModel(); //Game1.game.Content.Load<Model>("Models/A1_Brick");
            LocalTransforms = new Matrix[Model.Bones.Count];

            Vector3[] brickPoints = new Vector3[2];
            brickPoints[0] = new Vector3(Position.X, Position.Y, Position.Z);
            brickPoints[1] = new Vector3(Position.X + brickSize, Position.Y + brickSize, Position.Z + brickSize);
            bounding = BoundingBox.CreateFromPoints(brickPoints);

        }
    }
}