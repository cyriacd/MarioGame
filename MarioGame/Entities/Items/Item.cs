﻿using MarioGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using MarioGame.Core;

namespace MarioGame.Entities
{
    public abstract class Item : ContainableHidableEntity
    {
        public Vector2 MovingVelocity { get; set; } = new Vector2(.5f, 0);

        internal Item(Vector2 position, ContentManager content, Action<Entity> addToScriptEntities) : base(position, content,addToScriptEntities)
        {
            BoxPercentSizeOfEntity = 1.2f;
            _isVisible = true;
        }

        public override void LeaveContainer()
        {
            PositionY -= _sprite.FrameHeight;
            Show();
        }
        public override void OnCollide(IEntity otherObject, Sides side, Sides otherSide)
        {
            if (!IsVisible) return;
            base.OnCollide(otherObject, side, otherSide);
            if (otherObject is Mario)
            {
                Delete();
            }
            if (otherObject is Block)
            {
                if (((Block)otherObject).IsVisible)
                {
                    if (side == Sides.Left || side == Sides.Right)
                    {
                        this.FlipHorizontalVelocity();
                    }
                }
            }
        }
        public override void Delete()
        {
            Hide();
            base.Delete();
        }
    }
}
