﻿using MarioGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioGame.States.BlockStates.PowerUpStates
{
    public class VisibleState : BlockPowerUpState
    {
        public VisibleState(Block block, BlockPowerUpStateMachine stateMachine): base(block, stateMachine)
        {
            powerUpStateEnum = BlockPowerUpStateEnum.Visible;
        }
        public override void Begin(IState prevState)
        {
            block.ChangeBlockPowerUpState(stateMachine.VisibleState);
        }
    }
}