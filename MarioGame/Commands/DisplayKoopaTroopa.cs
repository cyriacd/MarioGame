﻿using MarioGame.Theming.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioGame.Commands
{
    internal class DisplayKoopaTroopa : ICommand
    {
        public DisplayKoopaTroopa(Scene scene)
        {
            Scene = scene;
        }

        public Scene Scene { get; }

        public void Execute()
        {
        }
    }
}
