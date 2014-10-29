using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brickles
{
    internal class LoadLevel
    {
        private readonly List<Vector3> sortedBricks = new List<Vector3>(); //intermediate lists
        private List<Vector3> BrickList = new List<Vector3>();

        public LoadLevel(String filename)
        {
            ParseCSV(ReadFile(filename));
            SortBricks(); //sort bricks before generating brick positions;
            GenerateBrickPos();
        }


        private void SortBricks()
        {
            float lowestZ = 0; //this is the back position, these bricks will be drawn first


            //presort
            foreach (Vector3 brick in BrickList)
                //find furthest back brick, bricks will be sorted with this at the beginning
            {
                //if (Vector3.DistanceSquared(brick, cameraPosition) > lowestZ)
                if (brick.Z < lowestZ)
                {
                    lowestZ = brick.Z;
                }
            }

            //sort
            int numBricksRemaining = BrickList.Count;
            while (numBricksRemaining > 0)
            {
                foreach (Vector3 brick in BrickList)
                {
                    if (brick.Z == lowestZ)
                    {
                        sortedBricks.Add(brick);
                        numBricksRemaining--;
                    }
                }
                lowestZ++; //move onto next closest level when finished;
            }
            BrickList = sortedBricks;
        }

        private void GenerateBrickPos()
        {
            Console.WriteLine("Generating bricks");
            float brickSize = 0f;


            //measure the brick model
            foreach (ModelMesh mesh in Game1.game.brickModel.Meshes)
            {
                BoundingSphere bs = mesh.BoundingSphere;
                brickSize = bs.Radius;
            }
            //int brickNumber = 0;


            foreach (Vector3 brick in BrickList)
            {
                if (brick != null)
                {
                    //apply scaling and transform then add it to the list - 'temp' is just used for debugging purposes
                   // Console.WriteLine("X:" + brick.X + " Y:" + brick.Y + " Z:" + brick.Z);
                    Game1.game.Bricks.AddLast(new Brick(brick, brickSize)); //create a new brick in dictionary
                }
            }
            Console.WriteLine("Brick processing complete!");
            Console.WriteLine(" Created " + Game1.game.Bricks.Count + " brix");
            GenerateSpecialBricks(Game1.game.difficulty);
        }

        private void GenerateSpecialBricks(Difficulty dif)
        {
            float specialPercent = 0;         //overal percentage of special bricks
            float treasurePercent = 0;
            int totalBricks = Game1.game.Bricks.Count;
            switch (dif)
            {
                case Difficulty.Tutorial:
                    specialPercent = 0.15f;
                    treasurePercent = 1f;
                    break;                      //tutorial gets 15% special, all of which are treasure
                case Difficulty.Easy:
                    specialPercent = 0.1f;      //easy gets 10% special, 80% of which are treasure
                    treasurePercent = 0.8f;
                    break;
                case Difficulty.Medium:         //medium gets 5% specials, 50% of which are treasure
                    specialPercent = 0.05f;
                    treasurePercent = 0.5f;
                    break;
                case Difficulty.Hard:
                    specialPercent = 0.1f;      //hard gets 10% specials, 20% of which are treasure
                    treasurePercent = 0.2f;
                    break;
                case Difficulty.Impossible:     //impossible gets 15% special, all of which are unbreakable
                    specialPercent = 0.15f;
                    treasurePercent = 0f;
                    break;
            }

            int specialCount = (int)Math.Round(totalBricks*specialPercent);
            int treasureCount = (int) Math.Round(specialCount*treasurePercent);
            int unbreakableCount = specialCount - treasureCount;


            Random rand = new Random();
            for (int i = unbreakableCount; i > 0; i--)      //foreach unbreakable brick
            {
                
                int b = rand.Next(totalBricks - 1);         //choose a random brick
                Game1.game.Bricks.ElementAt(b).setBrickType(BrickType.Unbreakable);     //set to unbreakable
            }
            for (int t = treasureCount; t > 0; t--)         //bug: treasure bricks can overwrite unbreakable bricks... not a huge issue
            {
                int b = rand.Next(totalBricks - 1);         //choose a random brick
                Game1.game.Bricks.ElementAt(b).setBrickType(BrickType.Treasure);     //set to treasure
            }
        }


        private void ParseCSV(string csv)
        {
            string[] brickRefs;
            BrickList = new List<Vector3>();
            brickRefs = csv.Split('{');
            foreach (string brickRef in brickRefs)
            {
                //Console.WriteLine("Brickref >" + brickRef + "<");
                if (brickRef.Length > 4)
                {
                    string trimmedBrick = brickRef.TrimEnd('\n');
                    //trimmedBrick = brickRef.TrimEnd('\r');
                    //trimmedBrick = brickRef.TrimEnd('\f');
                    trimmedBrick = trimmedBrick.Replace('\n', '0');
                    trimmedBrick = trimmedBrick.Replace('\r', '0');
                    trimmedBrick = trimmedBrick.Replace('\f', '0');
                    trimmedBrick = trimmedBrick.Replace('}', '0');
                    //Console.WriteLine("Brick at: >" + trimmedBrick + "<");
                    BrickList.Add(Vector3FromString(trimmedBrick));
                }
            }
        }

        private string ReadFile(string mapName)
        {
            string mapContents = "";
            using (Stream stream = TitleContainer.OpenStream("Content/Maps/" + mapName + ".csv"))
            {
                using (var reader = new StreamReader(stream))
                {
                    mapContents = reader.ReadToEnd();
                }
            }
            return mapContents;
        }

        //a huge hack of some dodgy code found on unity forums
        private Vector3 Vector3FromString(string Vector3string)
        {
            //get first number (x)
            int startChar = 0;
            int endChar = Vector3string.IndexOf(",");
            var returnx = (float) Convert.ToDecimal(Vector3string.Substring(startChar, endChar));
            returnx = Convert.ToInt32(returnx);
            //get second number (y)
            Vector3string = Vector3string.Substring(endChar + 1, Vector3string.Length - endChar - 1);
            endChar = Vector3string.IndexOf(",");
            var returny = (float) Convert.ToDecimal(Vector3string.Substring(startChar, endChar));
            returny = Convert.ToInt32(returny);
            //get third number (z)
            Vector3string = Vector3string.Substring(endChar + 1, Vector3string.Length - endChar - 2);
            endChar = Vector3string.Length;
            var returnz = (float) Convert.ToDecimal(Vector3string.Substring(startChar, endChar -1));
            returnz = Convert.ToInt32(returnz);

            //remove these to return to single spacing
          //  returnx = returnx/2;
          //  returny = returny/2;
           // returnz = returnz/2;
            //Console.WriteLine("Z:" + returnz);
            return new Vector3(returnx, returny, returnz);
        }
    }
}