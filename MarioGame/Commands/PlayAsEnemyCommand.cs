﻿using MarioGame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioGame.Commands
{
    class PlayAsEnemyCommand : GameCommand
    {
        public PlayAsEnemyCommand(Game1 game) : base(game)
        {

        }

        public override void Execute()
        {
            Game.PlayAsEnemyCommand();
        }
    }
}
