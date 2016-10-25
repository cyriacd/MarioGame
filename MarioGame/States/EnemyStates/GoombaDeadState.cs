﻿using MarioGame.Entities;
using Microsoft.Xna.Framework;

namespace MarioGame.States
{
    internal class GoombaDeadState : GoombaActionState
    {
        private float _deleteTimer = 0.0f;
        public GoombaDeadState(Goomba entity, GoombaStateMachine stateMachine) : base(entity, stateMachine)

        {
            EnemyState = EnemyActionStateEnum.Dead;
        }
        public override void Begin(IState prevState)
        {
            base.Begin(prevState);
            Goomba.SetVelocityToIdle();
            Goomba.ChangeActionState(StateMachine.DeadGoomba);

        }

        public override void UpdateEntity(GameTime gameTime)
        {
            if (_deleteTimer > 1)
            {
                _deleteTimer = 0.0f;
                Goomba.Delete();
            }
            else
            {
                _deleteTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}