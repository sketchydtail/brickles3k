using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Collections.Generic;

namespace Brickles3k
{
    public class Brick
    {
        public Vector3 Position;    //the bricks game position, modified from csv file
        public Model Model;         //bricks model file, used if different types of bricks are needed
        public Matrix Transform;
        public Vector3 GridPos;     //the value read from csv
        public Vector3 Colour;

        public Brick(Vector3 gridPos, float brickSize)
        {
            GridPos = gridPos;
            
        }

        public virtual void Draw(GameTime gameTime)
        {
            
        }
    }
}
