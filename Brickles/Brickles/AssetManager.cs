using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Brickles
{
    public enum balls { Normal, Steel, Super };
    public static class AssetManager
    {
        public static Model[] brickModels = new Model[11];
        public static Model treasureBrickModel;
        public static Model unbreakableBrickModel;

        public static void LoadModels()
        {
            brickModels[0] = Game1.game.Content.Load<Model>("Models/A1_Brick");
            brickModels[1] = Game1.game.Content.Load<Model>("Models/A2_Brick");
            brickModels[2] = Game1.game.Content.Load<Model>("Models/A3_Brick");
            brickModels[3] = Game1.game.Content.Load<Model>("Models/B1_Brick");
            brickModels[4] = Game1.game.Content.Load<Model>("Models/B2_Brick");
            brickModels[5] = Game1.game.Content.Load<Model>("Models/B3_Brick");
            brickModels[6] = Game1.game.Content.Load<Model>("Models/C1_Brick");
            brickModels[7] = Game1.game.Content.Load<Model>("Models/C2_Brick");
            brickModels[8] = Game1.game.Content.Load<Model>("Models/C3_Brick");
            brickModels[9] = Game1.game.Content.Load<Model>("Models/D1_Brick");
            brickModels[10] = Game1.game.Content.Load<Model>("Models/D2_Brick");

            treasureBrickModel = Game1.game.Content.Load<Model>("Models/Chest_Brick");
            unbreakableBrickModel = Game1.game.Content.Load<Model>("Models/Unbreakable_Brick");

        }

        private static Model getRandomBrickModel()
        {
            Random rand = new Random();
            int randValue = rand.Next(0, 10);

            return brickModels[randValue];
        }

        public static Model getBrickModel(BrickType type)
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

        public static Model getBallModel(balls b)
        {

            switch (b)
            {
                case balls.Normal:
                    return Game1.game.Content.Load<Model>("Models/Ball");
                    break;
            }
            return null;

        }
    }
}
