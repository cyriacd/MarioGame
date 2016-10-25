﻿using MarioGame.Entities;

namespace MarioGame.States
{
    class DeadState : MarioPowerUpState
    {
        public DeadState(Mario entity, MarioPowerUpStateMachine stateMachine) : base(entity, stateMachine)
        {
            powerUpState = MarioPowerUpStateEnum.Dead;
            _mario.IsCollidable = false;
        }

        public override void Begin(MarioPowerUpState prevState)
        {
            _mario.ChangePowerUpState(_stateMachine.DeadState);
            _mario.SetVelocityToIdle();
        }
    }
}
