using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Brickles
{
    class Menu : Scene
    {
        private enum SelectedItem
        {
            Play,
            Options,
            Highscores,
            Collectables,
            Quit
        };

        private GameManager game;

        private int screenWidth;
        private int screenHeight;
        private bool played = false;

        private Texture2D background;

        private Model charModel ;
        private Model collectablesModel;
        private Model highScoresModel;
        private Model menuSceneModel;
        private Model optionsModel;
        private Model playModel;
        private Model quitModel;
        private Model spaceBallModel;


        private KeyboardState keystate;
        private KeyboardState lastState;

        private readonly Vector3 cameraPosition = new Vector3(400, 300f, 1000f);
        private readonly Vector3 cameraTarget = new Vector3(0f, 300f, 0f);

        private Matrix ViewMatrix;
        private Matrix ProjectionMatrix;
        private Matrix MenuTransform;

        private Vector3 MenuPos = new Vector3(0,0,0);

        private SelectedItem selectedItem = SelectedItem.Play;

        public Menu(GameManager game)
            : base(game)
        {
            this.game = game;
            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;
        }

        public override void Initialize()
        {
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(0.9f, game.GraphicsDevice.Viewport.AspectRatio, 0.1f,
                5000f);

            MenuTransform = Matrix.CreateTranslation(MenuPos);

            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            charModel = game.Content.Load<Model>("MenuContent/Character");
            collectablesModel = game.Content.Load<Model>("MenuContent/Collectables");
            highScoresModel = game.Content.Load<Model>("MenuContent/HighScores");
            menuSceneModel = game.Content.Load<Model>("MenuContent/Menu_Scene");
            optionsModel = game.Content.Load<Model>("MenuContent/Options");
            playModel = game.Content.Load<Model>("MenuContent/Play");
            quitModel = game.Content.Load<Model>("MenuContent/Quit");
            spaceBallModel = game.Content.Load<Model>("MenuContent/SpaceBall");

            background = game.Content.Load<Texture2D>("MenuContent/PreRenderedBground");
        }

        public override void Update(GameTime gameTime)
        {
            
            keystate = Keyboard.GetState();

            if (keystate.IsKeyDown(Keys.Down) && !lastState.IsKeyDown(Keys.Up))
            {
                selectedItem++;
            }
            else if (keystate.IsKeyDown(Keys.Up) && !lastState.IsKeyDown(Keys.Down))
            {
                selectedItem--;
            }

            if (selectedItem < SelectedItem.Play)
            {
                selectedItem = SelectedItem.Quit;
            }
            else if (selectedItem > SelectedItem.Quit)
            {
                selectedItem = SelectedItem.Play;
            }


            if (gameTime.TotalGameTime.Seconds > 1)
            {
                if ((keystate.IsKeyDown(Keys.Enter) && !lastState.IsKeyDown(Keys.Enter)) ||
                    (keystate.IsKeyDown(Keys.Space) && !lastState.IsKeyDown(Keys.Space)))
                {
                    switch (selectedItem)
                    {
                        case SelectedItem.Play:
                            NextScene();
                            break;
                        case SelectedItem.Collectables:
                            break;
                        case SelectedItem.Highscores:
                            break;
                        case SelectedItem.Options:
                            break;
                        case SelectedItem.Quit:
                            game.Exit();
                            break;
                    }
                }
            }

            lastState = keystate;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //spaceBallModel.Draw(MenuTransform, ViewMatrix, ProjectionMatrix);
            //menuSceneModel.Draw(MenuTransform, ViewMatrix, ProjectionMatrix);
            //charModel.Draw(MenuTransform, ViewMatrix, ProjectionMatrix);
            Rectangle bgrect = new Rectangle(0,0,game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            game.spriteBatch.Begin();
            //game.spriteBatch.Draw(background,new Vector2(0,0),Color.White);
            game.spriteBatch.Draw(background, bgrect, Color.White);
           // game.spriteBatch.Draw(background, new Vector2(0,0), null,
      //  Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            game.spriteBatch.End();

            bool playSelected = false;
            bool optionsSelected = false;
            bool highSelected = false;
            bool collectSelected = false;
            bool quitSelected = false;

            switch (selectedItem)
            {
                case SelectedItem.Play:
                    playSelected = true;
                    break;
                case SelectedItem.Options:
                    optionsSelected = true;
                    break;
                case SelectedItem.Highscores:
                    highSelected = true;
                    break;
                case SelectedItem.Collectables:
                    collectSelected = true;
                    break;
                case SelectedItem.Quit:
                    quitSelected = true;
                    break;
            }

            DrawOption(collectablesModel, collectSelected);
            DrawOption(highScoresModel, highSelected);
            DrawOption(optionsModel, optionsSelected);
            DrawOption(quitModel, quitSelected);
            DrawOption(playModel, playSelected);
            //collectablesModel.Draw(MenuTransform, ViewMatrix, ProjectionMatrix);
            //highScoresModel.Draw(MenuTransform, ViewMatrix, ProjectionMatrix);
            //optionsModel.Draw(MenuTransform, ViewMatrix, ProjectionMatrix);
            //playModel.Draw(MenuTransform, ViewMatrix, ProjectionMatrix);
            //quitModel.Draw(MenuTransform, ViewMatrix, ProjectionMatrix);
            

            base.Draw(gameTime);
        }

        private void NextScene()
        {
            game.setScene(GameScene.Game);
        }

        private void DrawOption(Model m, Boolean selected)
        {
            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = ViewMatrix;
                    effect.Projection = ProjectionMatrix;
                    effect.World = Matrix.CreateTranslation(MenuPos);
                    effect.EnableDefaultLighting();
                    if (selected)
                    {
                        effect.DiffuseColor = new Vector3(0.9f, 0.1f, 0.1f);
                    }
                    else
                    {
                        effect.DiffuseColor = new Vector3(0.9f,0.9f,0.9f);
                    }
                }
                mesh.Draw();
            }
        }

    }
}
