using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brickles
{
    public enum balls
    {
        Normal,
        Steel,
        Super
    };

    public class AssetManager
    {
        private readonly Random rand = new Random();
        public Model[] brickModels = new Model[11];
        public Game game;
        public Model treasureBrickModel;
        public Model unbreakableBrickModel;

        public AssetManager(Game game)
        {
            this.game = game;
        }

        public void LoadModels()
        {
            brickModels[0] = game.Content.Load<Model>("Models/A1_Brick");
            brickModels[1] = game.Content.Load<Model>("Models/A2_Brick");
            brickModels[2] = game.Content.Load<Model>("Models/A3_Brick");
            brickModels[3] = game.Content.Load<Model>("Models/B1_Brick");
            brickModels[4] = game.Content.Load<Model>("Models/B2_Brick");
            brickModels[5] = game.Content.Load<Model>("Models/B3_Brick");
            brickModels[6] = game.Content.Load<Model>("Models/C1_Brick");
            brickModels[7] = game.Content.Load<Model>("Models/C2_Brick");
            brickModels[8] = game.Content.Load<Model>("Models/C3_Brick");
            brickModels[9] = game.Content.Load<Model>("Models/D1_Brick");
            brickModels[10] = game.Content.Load<Model>("Models/D2_Brick");

            treasureBrickModel = game.Content.Load<Model>("Models/Chest_Brick");
            unbreakableBrickModel = game.Content.Load<Model>("Models/Unbreakable_Brick");
        }

        private Model getRandomBrickModel()
        {
            int randValue = rand.Next(0, 10);

            return brickModels[randValue];
        }

        public Model getBrickModel(BrickType type)
        {
            if (type == BrickType.Treasure)
            {
                return treasureBrickModel;
            }

            if (type == BrickType.Unbreakable)
            {
                return unbreakableBrickModel;
            }

            return getRandomBrickModel();
        }

        public Model getBallModel(balls b)
        {
            switch (b)
            {
                case balls.Normal:
                    return game.Content.Load<Model>("Models/Ball");
                    break;
            }
            return null;
        }
    }
}