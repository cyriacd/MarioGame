﻿using MarioGame.Core;
using MarioGame.Entities;
using MarioGame.States;
using MarioGame.Theming.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MarioGame.Sprites
{
    public class GoombaSprite : HidableSprite
    {
        public enum Frames
        {
            //frames are all facing left. 
            Walk = 0,
            Walk1 = 1,
            Dead = 2,
        }
        protected override void DefineFrameSets()
        {
            base.DefineFrameSets();
            NumberOfFramesPerRow = Enum.GetNames(typeof(Frames)).Length;
            FrameSets = new Dictionary<int, Collection<int>> {
            {EnemyActionStateEnum.Walking.GetHashCode(), new Collection<int>{Frames.Walk.GetHashCode(), Frames.Walk1.GetHashCode()} },
            {EnemyActionStateEnum.Dead.GetHashCode(), new Collection<int> { Frames.Dead.GetHashCode() } }
            };
        }
        public GoombaSprite(ContentManager content, Entity entity) : base(content, entity)
        {
            AssetName = "regulargoomba";
        }
        public override void Load(int framesPerSecond)
        {
            base.Load(framesPerSecond);
            if (Game1.playAsMario == false)
            {
                FrameHeight = 15;
                AssetName = "mariorunningright21";
                NumberOfFramesPerRow = 3;
                FrameSets = new Dictionary<int, Collection<int>> {
                { MarioActionStateEnum.Walking.GetHashCode(), new Collection<int> {MarioSprite.Frames.MovingMario1.GetHashCode(), MarioSprite.Frames.MovingMario2.GetHashCode(), MarioSprite.Frames.MovingMario3.GetHashCode(), MarioSprite.Frames.MovingMario2.GetHashCode() } },//TODO: instead of {1, 2, 3} may have to do {1, 2, 3, 2} or something like that
                { MarioActionStateEnum.Dead.GetHashCode(), new Collection<int> {MarioSprite.Frames.DeadMario.GetHashCode() } }
                };
            }
            else
                FrameHeight = 15;
        }

        public void ChangeActionState(GoombaActionState goombaActionState)
        {
            if (goombaActionState.GetHashCode() == EnemyActionStateEnum.Dead.GetHashCode())
            {
                if (Game1.playAsMario == false)
                    FrameSet = FrameSets[goombaActionState.EnemyState.GetHashCode()];
                else
                    FrameSet = FrameSets[MarioActionStateEnum.Dead.GetHashCode()];
            }
            FrameSetPosition = 0;
        }
    }
}
