﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MarioGame.States;
using MarioGame.Core;
using MarioGame.Theming;

namespace MarioGame.Entities
{
    class BrickBlock : BumpableContainerBlock
    {
        private List<BlockPiece> _blockPieces;
        private readonly ContentManager _content;
        public BrickBlock(Vector2 position, ContentManager content, Action<Entity> addToScriptEntities) : base(position, content, addToScriptEntities)
        {
            _content = content;
        }

        private void CreateBlockPieces(ContentManager content)
        {
            _blockPieces = new List<BlockPiece>
            {
                new BlockPiece(Position, content, AddToScriptEntities, Partitions.TopLeft),
                new BlockPiece(Position + new Vector2(Width/2, 0), content, AddToScriptEntities, Partitions.TopRight),
                new BlockPiece(Position + new Vector2(0, Height/2), content, AddToScriptEntities, Partitions.BottomLeft),
                new BlockPiece(Position + new Vector2(Width/2, Height/2), content, AddToScriptEntities, Partitions.BottomRight)
            };
            _blockPieces.ForEach(b => AddToScriptEntities(b));
            _blockPieces.ConvertAll(b => b.Sprite).ForEach(s => s.Load());
        }

        public override void OnCollide(IEntity otherObject, Sides side, Sides otherSide)
        {
            var mario = otherObject as Mario;
            if (mario == null || side != Sides.Bottom) return;

            if (mario.CanBreakBricks && IsVisible)
            {
                BState.Break();
            }
            else if (mario.Velocity.Y <= 0)
            {
                Bump();
            }
        }

        public virtual void Break()
        {
            BState.Break();
        }

        public virtual void BreakIntoPieces()
        {
            Script.Announce(EventTypes.Breakblock);
            CreateBlockPieces(_content);
            Delete();
        }
    }
}
