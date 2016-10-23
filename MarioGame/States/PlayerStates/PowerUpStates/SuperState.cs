﻿using MarioGame.Entities;

namespace MarioGame.States
{
    class SuperState : MarioPowerUpState
    {
        public SuperState(Mario entity, MarioPowerUpStateMachine stateMachine) : base(entity, stateMachine)
        {
            powerUpState = MarioPowerUpStateEnum.Super;
            _mario.isCollidable = true;
        }
        public override void ChangeToStar()
        {
            base.ChangeToStar();
            _mario.ChangePowerUpState(_stateMachine.SuperStarState);
        }
        public override void onHitByEnemy()
        {
            ChangeToStandard();
            _mario.setInvincible(1);
        }
    }
}
