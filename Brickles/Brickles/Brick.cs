using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brickles
{
    public class Brick : GameObject
    {
        public Vector3 Colour;
        public Vector3 GridPos; //the value read from csv

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

            Model = Game1.game.Content.Load<Model>("Models/brick_square");
            LocalTransforms = new Matrix[Model.Bones.Count];
        }
    }
}