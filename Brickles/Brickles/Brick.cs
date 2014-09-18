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
    class Brick
    {
        public Vector3 position;    //the bricks game position, modified from csv file
        public Model model;         //bricks model file, used if different types of bricks are needed
        public Matrix[] localTransforms;
        public Vector3 brickRotation;
        public Matrix WorldMatrix;
        public Vector3 gridPos;     //the value read from csv
        public Vector3 colour;
    }
}
