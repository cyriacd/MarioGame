﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarioGame.Sprites;
using Microsoft.Xna.Framework;

namespace MarioGame.Entities.Blocks
{
    public class QuestionBlock : Block
    {
        public QuestionBlock(Vector2 position, Sprite sprite) : base(position, sprite)
        {
            boundingBox = new Rectangle((int)_position.X, (int)_position.Y, 18, 18);
            boxColor = Color.Blue;
        }
    }
}