﻿using System;
using MarioGame.Entities;
using Microsoft.Xna.Framework;

namespace MarioGame.States
{
    public abstract class MarioActionState : ActionState
    {
        public MarioActionStateEnum actionState
        {
            get; protected set; //TODO: make this read from some shared enum with Sprites
        }
        protected Mario Mario => (Mario)Entity;
        protected MarioActionStateMachine StateMachine;

        public MarioActionState(Mario entity, MarioActionStateMachine stateMachine) : base(entity)
        {
            this.StateMachine = stateMachine;
        }

        public virtual void Begin(MarioActionState prevState)
        {
            base.Begin(prevState);
        }
        public virtual void Jump() {
            StateMachine.JumpingMarioState.Begin(this);
        }

        public virtual void MoveLeft()
        {
            if (Mario.FacingRight)
            {
                Mario.TurnLeft();
            }
            else
            {
                Mario.SetXVelocity(Vector2.One * -1.75f);
            }
        }
        public virtual void MoveRight()
        {
            if (Mario.FacingLeft)
            {
                Mario.TurnRight();
            }
            else
            {
                Mario.SetXVelocity(Vector2.One * 1.75f);
            }
        }
        public virtual void Fall()
        {
        }

        public virtual void Crouch() {
            StateMachine.CrouchingMarioState.Begin(this);
        }
        public void Halt()
        {
            StateMachine.IdleMarioState.Begin(this);
        }
        public override void UpdateEntity(int gametime)
        {
            if (Mario.Velocity.Y > .2f)
            {
                if(!Mario.MarioActionState.Equals(StateMachine.FallingMarioState))
                    Mario.ChangeActionState(StateMachine.FallingMarioState);
            }
            else if (Mario.Velocity.Y < 0)
            {
                if (!Mario.MarioActionState.Equals(StateMachine.JumpingMarioState))
                    Mario.ChangeActionState(StateMachine.JumpingMarioState);
            }
            else if (Mario.Velocity.X < .001f && Mario.Velocity.X > -.001f)
            {
                if (!Mario.MarioActionState.Equals(StateMachine.IdleMarioState))
                    Mario.ChangeActionState(StateMachine.IdleMarioState);
            }
            else
            {
                if (!Mario.MarioActionState.Equals(StateMachine.WalkingMarioState))
                    Mario.ChangeActionState(StateMachine.IdleMarioState);
            }
        }
    }
}
