﻿using MarioGame.Entities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace MarioGame.Sprites
{
    public class MushroomSuperSprite : AnimatedSprite //TODO: refactor this class to use either ANimated Sprite or Sprite
    {
        public MushroomSuperSprite(IEntity entity, ContentManager content, Viewport viewport) : base(entity, content, viewport)
        {
            _assetName = "mushroomSuper.png";
        }
    }
}

