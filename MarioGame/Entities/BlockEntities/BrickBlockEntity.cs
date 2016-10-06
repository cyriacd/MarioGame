﻿
using MarioGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MarioGame.Sprites.BlockSprites;
using MarioGame.Entities.BlockEntities;

namespace MarioGame.Entities.BlockEntities
{
    public class BrickBlockEntity : BlockEntity
    {
        public BrickBlockEntity(Vector2 position, BrickBlockSprite sprite) : base(position, sprite)
        {
            boundingBox = new Rectangle((int)_position.X, (int)_position.Y, 18, 18);
            boxColor = Color.Blue;
        }
    }
}
