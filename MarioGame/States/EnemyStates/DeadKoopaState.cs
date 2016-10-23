﻿using MarioGame.Entities;
using Microsoft.Xna.Framework;

namespace MarioGame.States
{
    class DeadKoopaState : KoopaActionState
    {
        public DeadKoopaState(KoopaTroopa entity, KoopaStateMachine stateMachine) : base(entity, stateMachine)
        {
            enemyState = EnemyActionStateEnum.Dead;
        }
        public override void Begin(KoopaActionState prevState)
        {
            koopa.SetVelocityToIdle();
            koopa.ChangeActionState(_stateMachine.DeadState);
        }
        public override void JumpedOn()
        {
            if (Vector2.Equals(koopa.Velocity, Entity.idleVelocity))
            {
                koopa.SetShellVelocityToMoving();
            }
            else
            {
                koopa.SetVelocityToIdle();
            }
        }
    }
}
