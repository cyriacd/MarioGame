﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MarioGame.Entities
{
    public class FireFlower : Item
    {
        public FireFlower(Vector2 position, ContentManager content) : base(position, content)
        {
            int _height = 40;
            int _width = 20;
            boundingBox = new Rectangle((int)_position.X, (int)_position.Y, _width, _height/2);
            boxColor = Color.Green;
        }
    }
}
