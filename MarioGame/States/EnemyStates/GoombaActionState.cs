﻿using MarioGame.Entities;
using Microsoft.Xna.Framework;

namespace MarioGame.States
{
    public class GoombaActionState : EnemyActionState
    {
        public EnemyActionStateEnum EnemyState
        { get; protected set; }

        private Goomba _goomba;
        protected Goomba Goomba => _goomba;
        private GoombaStateMachine _stateMachine;
        protected GoombaStateMachine StateMachine => _stateMachine;
        public GoombaActionState(Goomba entity, GoombaStateMachine stateMachine) : base(entity)
        {
            _stateMachine = stateMachine;
            _goomba = entity;
        }

        public override void Begin(IState prevState)
        {
            base.Begin(prevState);
        }
        public override void ChangeToDead()
        {
            StateMachine.DeadState.Begin(this);
        }
        public virtual void MoveLeft()
        {
            if (Goomba.FacingRight)
            {
                Goomba.TurnLeft();
            }
            else
            {
                Goomba.SetXVelocity(Vector2.One * -1.75f);
            }
        }
        public virtual void MoveRight()
        {
            if (Goomba.FacingLeft)
            {
                Goomba.TurnRight();
            }
            else
            {
                Goomba.SetXVelocity(Vector2.One * 1.75f);
            }
        }
        public virtual void Fall()
        {
        }
    }
}
