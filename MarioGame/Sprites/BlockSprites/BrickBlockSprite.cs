﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarioGame.Entities;
using Microsoft.Xna.Framework.Content;
using MarioGame.States;

namespace MarioGame.Sprites
{
    public class BrickBlockSprite : BlockSprite
    {
        public enum Frames
        {
            BrickBlock = 0,
            UsedBlock = 1
        }
        public BrickBlockSprite(ContentManager content, Entity entity) : base(content, entity)
        {
            AssetName = "brickblock";
            NumberOfFramesPerRow = 2;
            //Each state has a frameSet
            FrameSets = new Dictionary<int, List<int>> {
                { BlockActionStateEnum.Standard.GetHashCode(), new List<int> { Frames.BrickBlock.GetHashCode() } },
                { BlockActionStateEnum.Bumping.GetHashCode(), new List<int> { Frames.BrickBlock.GetHashCode() } },
                { BlockActionStateEnum.Used.GetHashCode(), new List<int> { Frames.UsedBlock.GetHashCode() } }
            };
            FrameSet = FrameSets[BlockActionStateEnum.Standard.GetHashCode()];
        }
    }
}