﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MarioGame.Entities
{
    public abstract class ContainableHidableEntity : Entity, IContainable, IHidable
    {
        public ContainableHidableEntity(Vector2 position, ContentManager content, float xVelocity = 0, float yVelocity = 0) : base(position, content, xVelocity, yVelocity)
        {
        }

        public abstract void Hide();
        public abstract void leaveContainer();
        public abstract void Show();
    }
}
