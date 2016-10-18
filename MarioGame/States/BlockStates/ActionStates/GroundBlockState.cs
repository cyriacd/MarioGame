﻿using MarioGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioGame.States
{
    public class GroundBlockState : BlockState
    {
        public GroundBlockState(Block block, BlockActionStateMachine stateMachine): base(block, stateMachine)
        {
            bState = BlockStateEnum.GroundBlock;
        }
    }
}