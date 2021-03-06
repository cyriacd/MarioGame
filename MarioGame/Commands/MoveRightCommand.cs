﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarioGame.Theming;
using MarioGame.Core;

namespace MarioGame.Commands
{
    internal class MoveRightCommand : ScriptCommand
    {
        public MoveRightCommand(Script script) : base(script)
        {
        }

        public override void Execute()
        {
            Script.MakeMarioMoveRight();
        }
    }
}
