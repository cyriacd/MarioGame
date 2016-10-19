﻿using MarioGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioGame.States
{
    public abstract class State: IState
    {
        public IState _prevState { get; set; }
        protected IEntity _entity;
        public State(IEntity entity)
        {
            _entity = entity;
        }

        public virtual void Begin(IState prevState)
        {
            //_prevState.End();
            _prevState = prevState;
        }
        public virtual void End() {}
    }
}
