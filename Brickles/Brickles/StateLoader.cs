using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

namespace Brickles
{
    public enum state { Intro, MainMenu, OptionMenu, Level}
    public static class StateLoader
    {
        private static Game game;
        private static state currentState;
        public static void Begin()
        {
            currentState = state.Intro;
            game = new Intro();
            game.Run();
        }

        public static void changeState(state s)
        {
            currentState = s;
            game.Exit();
            //game = null;            //abandon old game
            switch (currentState)
            {
                case state.Level:
                    Thread.Sleep(1000);
                    game = new Game1();
                    game.Run();
                    break;
            }
        }


    }
}
