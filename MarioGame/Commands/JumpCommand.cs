﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarioGame.Theming;
using MarioGame.Core;

namespace MarioGame.Commands
{
    internal class JumpCommand : ScriptCommand
    {
        public JumpCommand(Script script) : base(script)
        {
        }

        public override void Execute()
        {
            Script.MakeMarioJump();
        }
    }
}
