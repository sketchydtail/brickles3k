using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Brickles
{
    class Intro : Scene
    {
        private Texture2D splash;
        private GameManager game;
        private Video introvid;
        private VideoPlayer player;

        private Song introMusic;

        private int screenWidth;
        private int screenHeight;
        private bool played = false;

        private KeyboardState keystate;

        public Intro(GameManager game) : base (game)
        {
            
            this.game = game;
            screenWidth = game.GraphicsDevice.Viewport.Width;
            screenHeight = game.GraphicsDevice.Viewport.Height;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            introvid = game.Content.Load<Video>("Video/intro_render");
            introMusic = game.Content.Load<Song>("Music/intro");
            player = new VideoPlayer();
            
        }

        public override void Update(GameTime gameTime)
        {
            keystate = Keyboard.GetState();
            if ((player.State == MediaState.Stopped && played) || (MediaPlayer.State == MediaState.Stopped && played) || keystate.IsKeyDown(Keys.Escape) || keystate.IsKeyDown(Keys.Space))
            {
                MediaPlayer.Stop();
                introMusic = null;
                NextScene();
            }

            else if (player.State == MediaState.Stopped)
            {
                player.IsLooped = false;
                player.Play(introvid);
                played = true;
                MediaPlayer.Play(introMusic);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            Texture2D videoTexture = null;

            if (player.State != MediaState.Stopped)
                videoTexture = player.GetTexture();

            if (videoTexture != null)
            {
                game.spriteBatch.Begin();
                game.spriteBatch.Draw(videoTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                game.spriteBatch.End();
            }
            
            base.Draw(gameTime);
        }

        private void NextScene()
        {
            game.setScene(GameScene.Menu);
        }

    }
}
