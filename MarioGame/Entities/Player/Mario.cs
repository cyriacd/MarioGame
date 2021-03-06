﻿using Microsoft.Xna.Framework;
using MarioGame.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MarioGame.States;
using MarioGame.Core;
using MarioGame.Theming;
using System;
using System.Collections.Generic;
using MarioGame.Entities.Items;
using MarioGame.Entities.Player;
using MarioGame.Theming.Scenes;

namespace MarioGame.Entities
{
    public class Mario : PowerUpEntity
    {
        //private float _secondsOfInvincibilityRemaining = 0.0f;
        // Could be useful for casting in certain circumstances
        public MarioPowerUpState MarioPowerUpState => (MarioPowerUpState)PState;
        public MarioActionState MarioActionState => (MarioActionState)AState;
        // TODO: maybe we don't have to give the casted variable a new name, but rather just use the new keyword and the subclass type
        protected MarioSprite MarioSprite => (MarioSprite)Sprite;
        internal static Scoreboard Scoreboard = new Scoreboard();

        // Velocity variables
        private static readonly Vector2 JumpingVelocity = new Vector2(0, VelocityConstant * -2);
        private static readonly int FireballXSpeed = VelocityConstant*5;
        private const int SuperBoundingBoxWidth = 18;
        private const int SuperBoundingBoxHeight = 36;

        private const int StandardBoundingBoxWidth = 14;
        private const int StandardBoundingBoxHeight = 20;
        private bool IsMovingUp => VelocityY <= 0;
        private bool IsLarge => MarioPowerUpState is SuperState || MarioPowerUpState is SuperStarState || MarioPowerUpState is FireState || MarioPowerUpState is FireStarState;
        public bool CanBreakBricks => IsLarge && IsMovingUp;
        public bool Invincible => _secondsOfInvincibilityRemaining > 0 || PState is FireStarState || PState is StandardStarState || PState is SuperStarState;

        public bool IsStarState
            =>
            MarioPowerUpState is StandardStarState || MarioPowerUpState is SuperStarState ||
            MarioPowerUpState is FireStarState;
        public bool CanKillPirahna => IsStarState;
        public bool CanRunOverEnemies => IsStarState;
        private enum SpaceBarAction
        {
            Walk,
            Run
        }

        private SpaceBarAction _spaceBarAction;
        private Vector2 _currentCheckpointPosition;
        private bool _checkpointReached = false; //set true for testing
        public Action EnterHiddenRoom { get; private set; }

        public Action ExitHiddenRoom { get; private set; }
        public static Action EnterGameOver { get; private set; }
        public static Action EnterMainMenu { get; private set; }

        private readonly MarioActionStateMachine _marioActionStateMachine;
        private readonly MarioPowerUpStateMachine _marioPowerUpStateMachine;

        private readonly ContentManager _content;
        private bool onScreen = true;
        public Mario(Vector2 position, ContentManager content, Action<Entity> addToScriptEntities) : base(position, content, addToScriptEntities)
        {
            _marioActionStateMachine = new MarioActionStateMachine(this);
            _marioPowerUpStateMachine = new MarioPowerUpStateMachine(this);
            AState = _marioActionStateMachine.IdleMarioState; //TODO: make marioActionState a casted getter of aState?
            PState = _marioPowerUpStateMachine.StandardState;
            Direction = Directions.Right;
            _spaceBarAction = SpaceBarAction.Run;
            onScreen = true;
            _content = content;
            
        }

        internal override void Init(JEntity e, Game1 game)
        {
            SetGameOver(game.EnterGameOver);
        }
        
        protected override void SetUpBoundingBoxProperties()//TODO: All bounding box logic should really be in sprite classes, not entity classes
        {
            int JumpingBoundingBoxWidth = StandardBoundingBoxWidth + 3;
            int boxWidth = StandardBoundingBoxWidth, boxHeight = StandardBoundingBoxHeight;
            int sideMargin = 0;
            var topBottomMargin = 0;
            if (IsLarge && Game1.playAsMario==true)//TODO: This logic perhaps should be handled by the state
            {
                boxWidth = SuperBoundingBoxWidth;
                boxHeight = SuperBoundingBoxHeight;
                sideMargin = 2;
            }
            else
            {
                if (Game1.playAsMario == true)
                {
                    BoundingBoxSize = new Point(StandardBoundingBoxWidth, StandardBoundingBoxHeight);
                    if (MarioActionState is JumpingMarioState || MarioActionState is FallingMarioState)
                    {
                        boxWidth = JumpingBoundingBoxWidth;
                    }
                    else
                    {
                        sideMargin = 3;
                    }
                    topBottomMargin = 16;
                }
                else
                {
                    topBottomMargin = -2;
                    sideMargin = 0;
                }
            }
            BoundingBoxSize = new Point(boxWidth, boxHeight);
            BoundingBoxOffset = new Point(sideMargin, topBottomMargin);
        }

        internal void SetPosition(Vector2 newPosition)
        {
            Position = newPosition*GlobalConstants.GridHeight;
        }

        private void OnInvincibilityEnded()
        {//TODO: This method's logic should be handled in each power up state's End method
            if(MarioPowerUpState is FireStarState)
            {
               ChangeToFireState();
            }
            else if (MarioPowerUpState is StandardStarState)
            {
                ChangeToStandardState();
            }
            else if (MarioPowerUpState is SuperStarState)
            {
                ChangeToSuperState();
            }
        }
        private void UpdateInvincibilityStatus()
        {
            if (!(_secondsOfInvincibilityRemaining > 0)) return;
            _secondsOfInvincibilityRemaining -= (GlobalConstants.MillisecondsPerFrame / 1000);
            if (_secondsOfInvincibilityRemaining <= 0)
            {
                OnInvincibilityEnded();
            }
        }
        public override void Update(Viewport viewport, int elapsedMilliseconds)
        {
            //if (Game1.playAsMario == false)
            //Delete();
            base.Update(viewport, elapsedMilliseconds);
            MarioPowerUpState.UpdateEntity(elapsedMilliseconds);
            MarioActionState.UpdateEntity(elapsedMilliseconds);
            UpdateInvincibilityStatus();
            MarioActionState.CheckForLevelEdges();
            SetXVelocity(Vector2.Zero);
            CheckFallOff(viewport);
            Scoreboard.UpdateTimer(elapsedMilliseconds);
        }
        private void CheckFallOff(Viewport viewport)
        {
            if(onScreen == true && PositionY>viewport.Height)
            {
                ChangeToDeadState();
                onScreen = false;
            }
        }

        
        public void ChangeActionState(MarioActionState state)
        {
            base.ChangeActionState(state);
            LoadBoundingBox();
            MarioSprite.ChangeActionState(state);
        }
        //TODO: move this logic into each state class
        public void ChangePowerUpState(MarioPowerUpState state)
        {
            base.ChangePowerUpState(state);
            PlaySoundForNewPowerUpState(state);
            LoadBoundingBox();
            MarioSprite.ChangePowerUp(state);//TODO: can we push _marioSprite.changePowerUp inside of base.ChangePowerUpState, or will doing so lose the polymorphism (e.g. will it call AnimatedSprite.changePowerUp rather than _marioSprite.changePowerUp
        }

        public void PlaySoundForNewPowerUpState(MarioPowerUpState newState)
        {
                if (!(PState is SuperStarState || PState is FireStarState || PState is StandardStarState))
                {
                    if (newState is SuperState || newState is FireState)
                    {
                        Script.Announce(EventTypes.Powerup);
                    }
                    else if (newState is StandardState)
                    {
                        Script.Announce(EventTypes.Powerdown);
                    }
                }
        }
        public void Jump()
        {
            //TODO: factor this logic somehow into marioPowerUpState so that mario doesn't have to keep track of what power up state he is inn
            if (!(MarioPowerUpState is DeadState))
            {
                MarioActionState.Jump();
            }
        }
        public void Crouch()
        {
            //TODO: make actionState take a StateFactory so the way we check pState and action State below can be consistent
            if (!(MarioPowerUpState is DeadState))
            {
                MarioActionState.Crouch();
            }
        }
        public void MoveLeft()
        {
            if (!(MarioPowerUpState is DeadState))
            {
                MarioActionState.MoveLeft();
            }
        }
        public void MoveRight()
        {
            if (!(MarioPowerUpState is DeadState))
            {
                MarioActionState.MoveRight();
            }
        }
        public void ChangeToFireState()
        {
            MarioPowerUpState.ChangeToFire();
        }
        public void ChangeToStandardState()
        {
            MarioPowerUpState.ChangeToStandard();
        }
        public void ChangeToSuperState()
        {
            MarioPowerUpState.ChangeToSuper();
        }
        public void ChangeToStarState()
        {
            MarioPowerUpState.ChangeToStar();
        }
        public void ChangeToDeadState()
        {
            MarioPowerUpState.ChangeToDead();
        }
        //Todo: I don't like that we have a method called DashOrThrowFireball. I think we should have a Dash() method and a throwFireball() method, and which one gets called gets handled in the State classes
        public void DashOrThrowFireball()
        {
            MarioPowerUpState.DashOrThrowFireball();
        }

        public void Dash()
        {
            switch (_spaceBarAction)
            {
                case SpaceBarAction.Walk:
                    Velocity = Velocity / 2;
                    _spaceBarAction = SpaceBarAction.Run;
                    break;
                case SpaceBarAction.Run:
                    Velocity = Velocity * 2;
                    _spaceBarAction = SpaceBarAction.Walk;
                    break;
            }
        }
        
        public void SetVelocityToJumping()
        {
            SetYVelocity(JumpingVelocity);
        }
        public override void Halt()
        {
            HaltX();
            HaltY();
            MarioActionState.Halt();
        }
        public override void HaltX()
        {
            PositionX -= VelocityX;
            base.HaltX();
        }
        public override void HaltY()
        {
            PositionY -= VelocityY;
            base.HaltY();
        }

        protected override void OnCollideHiddenBlock(Block block, Sides side, Sides blockSide)
        {
            if (side == Sides.Top && blockSide == Sides.Bottom && Velocity.Y < 0)
            {
                Halt();
            }
        }

        private void OnCollideEnemy(Enemy enemy, Sides side)
        {
            if (Invincible) return;//TODO: Invincible and seconds of invincibility remaining should be handled by mario's state classes.
            if (enemy.IsVisible && side != Sides.Bottom )
            {
                if (enemy._secondsOfInvincibilityRemaining <= 0)
                {
                    MarioPowerUpState.OnHitByEnemy();
                }
            }
            else
            {
                Halt();
                Position -= new Vector2(0, 15);
            }
        }
        protected override void OnBlockSideCollision(Sides side)
        {
            if (side == Sides.Right)
            {
                OnBlockLeftCollision();
            }
            else
            {
                OnBlockRightCollision();
            }
            
        }

        private void OnBlockRightCollision()
        {
            if (VelocityX < 0)
            {
                PositionX -= VelocityX;
            }
        }

        private void OnBlockLeftCollision()
        {
            if (VelocityX > 0)
            {
                PositionX -= VelocityX;
            }
        }

        protected override void OnBlockBottomCollision(Block block)
        {
            base.OnBlockBottomCollision();
            if (block is GreenPipe)
            {
                MarioActionState.HitTopOfGreenPipe(((GreenPipe)block).SceneTransport, ((GreenPipe)block).TransportPosition);
            }
        }
        private void OnCollideItem(Item item)
        {
            if (!item.IsVisible) return;
            if (item is PowerUp) Scoreboard.AddPoint(1000);
            if (item is Coin)
            {
                Script.Announce(EventTypes.Coin);
                Scoreboard.AddCoin();
            }
            else if (item is Star)
            {
                ChangeToStarState();
            }
            else if (item is FireFlower)
            {
                ChangeToFireState();
            }
            else if (item is Mushroom1Up)
            {
                Script.Announce(EventTypes.OneUp);
                Scoreboard.AddLife();
            }
            else if (item is MushroomSuper)
            {
                ChangeToSuperState();
            }
            else if (item is Checkpoint)
            {
                _currentCheckpointPosition = item.Position;
                _checkpointReached = true;
            }
        }
        
        public override void OnCollide(IEntity otherObject, Sides side, Sides otherSide)
        {
            base.OnCollide(otherObject, side, otherSide);
            if (otherObject is Enemy)
            {
                OnCollideEnemy((Enemy)otherObject, side);
            }
            else if (otherObject is Item)
            {
                OnCollideItem((Item)otherObject);
            }
       }
        public void SetInvincible(float seconds)
        {
            _secondsOfInvincibilityRemaining = seconds;
        }
        internal void SetHiddenRoomEntry(Action enterHiddenScene)
        {
            EnterHiddenRoom = enterHiddenScene;
        }
        internal void SetHiddenRoomDeparture(Action exitHiddenScene)
        {
            ExitHiddenRoom = exitHiddenScene;
        }
        public void GoToHiddenRoom()
        {
            EnterHiddenRoom();
        }
        protected void LeaveHiddenRooom()
        {
            ExitHiddenRoom();
        }
        public static void GoToGameOver()
        {
            EnterGameOver();
        }
        internal static void SetGameOver(Action enterGameOver)
        {
            EnterGameOver = enterGameOver;
        }
        public static void GoToMainMenu()
        {
            EnterMainMenu();
        }
        public static void SetMainMenu(Action enterMainMenu)
        {
            EnterMainMenu = enterMainMenu;
        }
        public void RespawnOrGameOver()
        {
            if (Scoreboard.HasLives)
            {
                if (_checkpointReached)
                {
                    Respawn();
                }
                else
                {
                    Script.Reset();
                }
            }
            else
            {
                //TODO: End GAME 
                GoToGameOver();
            }
        }

        public void Respawn()
        {
            MoveToLocation(_currentCheckpointPosition);
            ChangeActionState(_marioActionStateMachine.IdleMarioState);
            ChangePowerUpState(_marioPowerUpStateMachine.StandardState);
        }

        private void MoveToLocation(Vector2 xPosition)
        {
            xPosition.Y -= 15f;
            Position = xPosition; //*GlobalConstants.GridWidth;
        }

        public void ThrowFireball()
        {
            Script.Announce(EventTypes.Fireball);
            var fireballXDistanceFromMario = FacingRight ? GlobalConstants.GridWidth : -1*GlobalConstants.GridWidth;
            var fireballYDistanceFromMario = GlobalConstants.GridHeight;
            var fireballXVelocity = FacingRight ? FireballXSpeed : -1*FireballXSpeed;
            var fireball = new Fireball(Position + new Vector2(fireballXDistanceFromMario, fireballYDistanceFromMario), _content, AddToScriptEntities, fireballXVelocity);
            AddToScriptEntities(fireball);
            fireball.Sprite.Load();
            fireball.LoadBoundingBox();
        }
    }

}
