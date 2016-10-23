﻿
using MarioGame.Sprites;
using Microsoft.Xna.Framework.Graphics;
using MarioGame.States;
using Microsoft.Xna.Framework;
using MarioGame.Entities;
using Microsoft.Xna.Framework.Content;

namespace MarioGame.Entities
{
    public class Goomba : Enemy
    {
        //public GoombaSprite eSprite;
        GoombaActionState eState;
        public readonly static Vector2 movingVelocity = new Vector2(-1, 0);
        GoombaStateMachine stateMachine;
        private static int boundingBoxWidth = 10;
        private static int boundingBoxHeight = 10;
        private int _height;
        private int _width;

        public Goomba(Vector2 position, ContentManager content) : base(position, content)
        {
            stateMachine = new GoombaStateMachine(this);
            aState = stateMachine.WalkingGoomba;
            eState = (GoombaActionState)aState;
            eSprite = (GoombaSprite)_sprite;
            boundingBox = new Rectangle((int)(_position.X + 3), (int)(_position.Y + 5), boundingBoxWidth, boundingBoxHeight);
            boxColor = Color.Red;
            _height = boundingBoxHeight;
            _width = boundingBoxWidth;
            _velocity = movingVelocity;
        }
        public void ChangeActionState(GoombaActionState newState)
        {
            eState = newState;
            ((GoombaSprite)eSprite).changeActionState(newState);
        }

        public override void Halt()
        {
            _position -= _velocity;
            eState.Halt();
        }

        public void ChangeToDeadState()
        {
            eState.ChangeToDead();
        }
        public override void JumpedOn()
        {
            eState.JumpedOn();
            _isDead = true;
            
        }
        public override void Update(Viewport viewport)
        {
            if(_sprite.Visible != false)
            {
                _velocity = idleVelocity;
            }
            base.Update();
            Vector2 pos = _position;
            if (_position.X < 0)
            {
                pos.X = 0;
                ChangeVelocityDirection();
            }
            else if (_position.X + _width > viewport.Width)
            {
                pos.X = viewport.Width - _width;
                ChangeVelocityDirection();
            }
            _position = pos;

            boundingBox.X = (int)_position.X + 3;
            boundingBox.Y = (int)_position.Y + 5;
        }
        public override void ChangeVelocityDirection()
        {
            Vector2 newVelocity = _velocity * -1;
            this.setVelocity(newVelocity);
        }
    }
}
