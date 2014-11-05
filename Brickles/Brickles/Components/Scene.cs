using Microsoft.Xna.Framework;

namespace Brickles
{
    public class Scene : DrawableGameComponent
    {
        public GameManager game;

        public Scene(GameManager game) : base(game)
        {
            this.game = game;
        }
    }
}