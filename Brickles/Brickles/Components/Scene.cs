using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
