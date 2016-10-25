﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static MarioGame.Entities.Mario;
using MarioGame.Entities;
using System;
using static MarioGame.Entities.Entity;
using MarioGame.States;

namespace MarioGame.Sprites
{
    public class MarioSprite : AnimatedSprite
    {
        public enum Frames
        {
            //frames are all facing left. Except DeadMario who is facing the computer user.
            SwimmingMarioEnd = 1,
            SwimmingMarioBeforeEnd = 2,
            SwimmingMarioMiddle = 3,
            SwimmingMarioAfterStart = 4,
            SwimmingMarioStart = 5,
            SittingMario2 = 6, //I know he's not sitting . . .I'm just not sure exactly what that section of the sprite sheet is
            SittingMario1 = 7,
            JumpingMario = 8,
            FallingMario = 8,
            DashingMario = 9,
            MovingMario3 = 10, //moving is the same sprites for Running and Walking action states
            MovingMario2 = 11,
            MovingMario1 = 12,
            CrouchingMario = 13,
            StandingMario = 14,
            DeadMario = 14
        }

        public enum Rows
        {
            Super = 0,
            Standard = 1,
            SuperLuigi = 2,
            Luigi = 3,
            Fire = 4,
            Dead = 5
        }

        //power up states - standard(small), super(big), fire ,star (invincible), Dead

        public MarioSprite(ContentManager content, Entity entity) : base(content, entity)
        {
            AssetName = "characters_transparent";
            NumberOfFramesPerRow = 15;
            //Each state has a frameSet
            
            FrameSets = new Dictionary<int, List<int>> {
                { MarioActionStateEnum.Idle.GetHashCode(), new List<int> { Frames.StandingMario.GetHashCode() } },
                { MarioActionStateEnum.Walking.GetHashCode(), new List<int> {Frames.MovingMario1.GetHashCode(), Frames.MovingMario2.GetHashCode(), Frames.MovingMario3.GetHashCode(), Frames.MovingMario2.GetHashCode() } },//TODO: instead of {1, 2, 3} may have to do {1, 2, 3, 2} or something like that
                { MarioActionStateEnum.Running.GetHashCode(), new List<int> {Frames.MovingMario1.GetHashCode(), Frames.MovingMario2.GetHashCode(), Frames.MovingMario3.GetHashCode(), Frames.MovingMario2.GetHashCode() } },//TODO: instead of {1, 2, 3} may have to do {1, 2, 3, 2} or something like that
                { MarioActionStateEnum.Crouching.GetHashCode(), new List<int> {Frames.CrouchingMario.GetHashCode() } },
                { MarioActionStateEnum.Jumping.GetHashCode(), new List<int> {Frames.JumpingMario.GetHashCode() } },
                { MarioActionStateEnum.Falling.GetHashCode(), new List<int> {Frames.FallingMario.GetHashCode() } },
                { MarioActionStateEnum.Sitting.GetHashCode(), new List<int> {Frames.SittingMario1.GetHashCode(), Frames.SittingMario2.GetHashCode() } },
                { MarioActionStateEnum.Swimming.GetHashCode(), new List<int> {Frames.SwimmingMarioStart.GetHashCode(), Frames.SwimmingMarioAfterStart.GetHashCode(), Frames.SwimmingMarioMiddle.GetHashCode(), Frames.SwimmingMarioBeforeEnd.GetHashCode(), Frames.SwimmingMarioEnd.GetHashCode() }},
                { MarioActionStateEnum.Dead.GetHashCode(), new List<int> {Frames.DeadMario.GetHashCode() } }
            };

            RowSets = new Dictionary<int, List<int>>
            {
                {MarioPowerUpStateEnum.Standard.GetHashCode(), new List<int> {Rows.Standard.GetHashCode() } },
                {MarioPowerUpStateEnum.Super.GetHashCode(), new List<int> {Rows.Super.GetHashCode() } },
                {MarioPowerUpStateEnum.Fire.GetHashCode(), new List<int> {Rows.Fire.GetHashCode() } },
                {MarioPowerUpStateEnum.SuperStar.GetHashCode(), new List<int> {Rows.Fire.GetHashCode(), Rows.SuperLuigi.GetHashCode(), Rows.Super.GetHashCode() } },  //Cycle between various types of mario sprite to give the flashing feel of invincibility
                {MarioPowerUpStateEnum.StandardStar.GetHashCode(), new List<int> {Rows.Luigi.GetHashCode(), Rows.Standard.GetHashCode() } },  //Cycle between various types of mario sprite to give the flashing feel of invincibility
                {MarioPowerUpStateEnum.FireStar.GetHashCode(), new List<int> {Rows.Fire.GetHashCode(), Rows.SuperLuigi.GetHashCode(), Rows.Super.GetHashCode() } },  //Cycle between various types of mario sprite to give the flashing feel of invincibility
                {MarioPowerUpStateEnum.Dead.GetHashCode(), new List<int> {Rows.Dead.GetHashCode() } }
            };

            FrameSet = FrameSets[MarioActionStateEnum.Idle.GetHashCode()];
            FrameSetPosition = 0;

            // Begin with sprite facing right
            Flipped = SpriteEffects.FlipHorizontally;

            RowSet = RowSets[MarioPowerUpStateEnum.Standard.GetHashCode()];
            RowSetPosition = 0;
        }

        public override void Load(int framesPerSecond = 5)
        {
            base.Load(framesPerSecond);
            FrameHeight = 40;
        }
        public void ChangeActionState(MarioActionState marioActionState)
        {
            base.ChangeActionState(marioActionState);
            FrameSet = FrameSets[marioActionState.actionState.GetHashCode()];
        }
        public void ChangePowerUp(MarioPowerUpState marioPowerUpState)
        {
            base.ChangePowerUp(marioPowerUpState);
            // Because on the sprite sheet, dead state is a frame set, not a row set
            if (marioPowerUpState.PowerUpState == MarioPowerUpStateEnum.Dead)
            {
                FrameSet = FrameSets[MarioActionStateEnum.Dead.GetHashCode()];
                FrameSetPosition = 0;
            }
           
            RowSet = RowSets[marioPowerUpState.PowerUpState.GetHashCode()];
        }
    }
}
