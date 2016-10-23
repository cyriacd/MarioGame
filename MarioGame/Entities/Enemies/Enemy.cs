﻿using MarioGame.Core;
using MarioGame.Sprites;
using MarioGame.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MarioGame.Entities
{
    public class Enemy : Entity
    {
        public AnimatedSprite eSprite;
        protected bool _isDead;
        public bool Dead { get { return _isDead; } }
        protected bool _hurts;
        public bool Hurts { get { return _hurts; } }
        public Enemy(Vector2 position, ContentManager content) : base(position, content)
        {
            _isDead = false;
            _hurts = true;
        }
        public virtual void JumpedOn() { }

        public override void onCollide(IEntity otherObject, Sides side)
        {
            base.onCollide(otherObject, side);
            if (otherObject is Mario )
            {
                if (side == Sides.Top)
                {
                    _isDead = true;
                }
            } 
        }
    }
}
