﻿using System;
using MarioGame.Entities.Items;
using MarioGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MarioGame.Entities
{
    public class Star : PowerUp
    {
        public Star(Vector2 position, ContentManager content, Action<Entity> addToScriptEntities) : base(position, content,addToScriptEntities)
        {
            Velocity = MovingVelocity;
        }

        public override void Update(Viewport viewport, int elapsedMilliseconds)
        {
            if (((StarSprite)Sprite).isVisible == false)
            {
                BoundingBox.X = (int)PositionX;
                BoundingBox.Y = (int)PositionY;

            }
            base.Update(viewport, elapsedMilliseconds);
        }
    }
}
