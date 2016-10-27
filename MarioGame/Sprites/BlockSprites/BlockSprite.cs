﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MarioGame.Entities;
using System.Collections.Generic;
using System.Collections;
using MarioGame.States;

namespace MarioGame.Sprites
{
    public class BlockSprite : HidableSprite
    {
        public enum Frames
        {
            BrickBlock = 0,
            UsedBlock = 1,
            //BrickBlock = 2
            GroundBlock = 3,
            StepBlock = 4,
            SilverBlock = 5,
            QuestionBlock1 =6,
            QuestionBlock2 = 7,
            QuestionBlock3 = 8,
        }
        public enum Rows
        {
            Visible = 0,
            Hidden = 1
        }

        public BlockSprite(ContentManager content, Entity entity) : base(content, entity)
        {
            AssetName = "blocks";
            NumberOfFramesPerRow = 9;
            //Each state has a frameSet
            FrameSets = new Dictionary<int, List<int>> {
                { BlockActionStateEnum.BrickBlock.GetHashCode(), new List<int> { Frames.BrickBlock.GetHashCode() } },
                { BlockActionStateEnum.UsedBlock.GetHashCode(), new List<int> { Frames.UsedBlock.GetHashCode() } },
                { BlockActionStateEnum.GroundBlock.GetHashCode(), new List<int> { Frames.GroundBlock.GetHashCode() } },
                { BlockActionStateEnum.StepBlock.GetHashCode(), new List<int> { Frames.StepBlock.GetHashCode() } },
                { BlockActionStateEnum.SilverBlock.GetHashCode(), new List<int> { Frames.SilverBlock.GetHashCode() } },
                { BlockActionStateEnum.QuestionBlock.GetHashCode(), new List<int> {Frames.QuestionBlock1.GetHashCode(), Frames.QuestionBlock2.GetHashCode(), Frames.QuestionBlock3.GetHashCode() } },
            };
            RowSets = new Dictionary<int, List<int>>
            {
                {Rows.Visible.GetHashCode(), new List<int> {Rows.Visible.GetHashCode() } }
            };
            RowSet = RowSets[Rows.Visible.GetHashCode()];
            FrameSet = FrameSets[BlockActionStateEnum.BrickBlock.GetHashCode()];

        }
        public override void Load(int framesPerSecond = 5)
        {
            base.Load(framesPerSecond);
            FrameHeight = 16;
        }
        public override void Draw(SpriteBatch batch)
        {
            if (isVisible)
            {
                base.Draw(batch);
            }
        }
        public void ChangeActionState(BlockActionState actionState)
        {
            base.ChangeActionState(actionState);
            FrameSet = FrameSets[actionState.BState.GetHashCode()];
        }

    }
}