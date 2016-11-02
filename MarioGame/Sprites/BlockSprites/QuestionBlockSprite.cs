﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarioGame.Entities;
using Microsoft.Xna.Framework.Content;
using MarioGame.States;

namespace MarioGame.Sprites
{
    public class QuestionBlockSprite : BlockSprite
    {
        public enum Frames
        {
            QuestionBlock1 = 0,
            QuestionBlock2 = 1,
            QuestionBlock3 = 2,
            UsedBlock = 3
        }
        public QuestionBlockSprite(ContentManager content, Entity entity) : base(content, entity)
        {
            AssetName = "questionblock";
            NumberOfFramesPerRow = 4;
            //Each state has a frameSet
            FrameSets = new Dictionary<int, Collection<int>> {
                { BlockActionStateEnum.Standard.GetHashCode(), new Collection<int> {Frames.QuestionBlock1.GetHashCode(), Frames.QuestionBlock2.GetHashCode(), Frames.QuestionBlock3.GetHashCode() } },
                { BlockActionStateEnum.Bumping.GetHashCode(), new Collection<int> {Frames.QuestionBlock1.GetHashCode(), Frames.QuestionBlock2.GetHashCode(), Frames.QuestionBlock3.GetHashCode() } },
                { BlockActionStateEnum.Used.GetHashCode(), new Collection<int> { Frames.UsedBlock.GetHashCode() } }

            };
            FrameSet = FrameSets[BlockActionStateEnum.Standard.GetHashCode()];
        }
    }
}
