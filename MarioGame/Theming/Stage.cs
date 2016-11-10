﻿using System.Collections.Generic;
using MarioGame.Commands;
using MarioGame.Controllers;
using MarioGame.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MarioGame.Commands.BlockCommands;
using MarioGame.Theming.Scenes;

namespace MarioGame.Theming
{
    public class Stage
    {
        private readonly List<IController> _controllers;

        public Stage(Game1 game1)
        {
            Game1 = game1;
            _controllers = new List<IController>();
        }

        public Game1 Game1 { get; set; }

        public GraphicsDeviceManager GraphicsDevice => Game1.Graphics;

        public void Initialize()
        {
            _controllers.Add(new KeyboardController());
            _controllers.Add(new GamepadController());
            _controllers[0].AddCommand((int)Keys.Q, new QuitCommand(Game1));
            //Reset Command
            _controllers[0].AddCommand((int)Keys.R, new ResetCommand(Game1));
            _controllers[1].AddCommand((int) Buttons.Start, new QuitCommand(Game1));

            // Adding jump command to controllers
            _controllers[0].AddCommand((int)Keys.W, new JumpCommand(Game1.Scene.Script));
            _controllers[0].AddHeldCommand((int)Keys.W, new JumpCommand(Game1.Scene.Script));
            _controllers[0].AddCommand((int)Keys.Up, new JumpCommand(Game1.Scene.Script));
            _controllers[0].AddHeldCommand((int)Keys.Up, new JumpCommand(Game1.Scene.Script));

            // Adding movement commands (Needs updated with actual commands)
            _controllers[0].AddCommand((int)Keys.Left, new MoveLeftCommand(Game1.Scene.Script));
            _controllers[0].AddHeldCommand((int)Keys.Left, new MoveLeftCommand(Game1.Scene.Script));
            _controllers[0].AddCommand((int)Keys.A, new MoveLeftCommand(Game1.Scene.Script));
            _controllers[0].AddHeldCommand((int)Keys.A, new MoveLeftCommand(Game1.Scene.Script));
            _controllers[0].AddCommand((int)Keys.Right, new MoveRightCommand(Game1.Scene.Script));
            _controllers[0].AddHeldCommand((int)Keys.Right, new MoveRightCommand(Game1.Scene.Script));
            _controllers[0].AddCommand((int)Keys.D, new MoveRightCommand(Game1.Scene.Script));
            _controllers[0].AddHeldCommand((int)Keys.D, new MoveRightCommand(Game1.Scene.Script));
            _controllers[0].AddCommand((int)Keys.Down, new CrouchCommand(Game1.Scene.Script));
            _controllers[0].AddCommand((int)Keys.S, new CrouchCommand(Game1.Scene.Script));
            _controllers[0].AddCommand((int)Keys.Space, new DashOrThrowFireballCommand(Game1.Scene.Script));

            // Add power up state commands
            _controllers[0].AddCommand((int)Keys.Y, new StandardStateCommand(Game1.Scene.Script));
            _controllers[0].AddCommand((int)Keys.U, new SuperStateCommand(Game1.Scene.Script));
            _controllers[0].AddCommand((int)Keys.I, new FireStateCommand(Game1.Scene.Script));
            _controllers[0].AddCommand((int)Keys.O, new DeadStateCommand(Game1.Scene.Script));

            // Add block state commands
            _controllers[0].AddCommand((int)Keys.B, new BumpOrBreakBrick(Game1.Scene.Script));
            _controllers[0].AddCommand((int)Keys.H, new ShowHiddenBlock(Game1.Scene.Script));
            _controllers[0].AddCommand((int)Keys.X, new ChangeQuestionToUsedState(Game1.Scene.Script));

            // Adds commands to game controller
            _controllers[1].AddCommand((int)Buttons.DPadLeft, new MoveLeftCommand(Game1.Scene.Script));
            _controllers[1].AddCommand((int)Buttons.DPadDown, new CrouchCommand(Game1.Scene.Script));
            _controllers[1].AddCommand((int)Buttons.DPadRight, new MoveRightCommand(Game1.Scene.Script));
            _controllers[1].AddCommand((int)Buttons.B, new DashOrThrowFireballCommand(Game1.Scene.Script));
            _controllers[1].AddCommand((int)Buttons.A, new JumpCommand(Game1.Scene.Script));

            //Adds command to show bounding boxes
            _controllers[0].AddCommand((int)Keys.C, new DrawBoundingBoxes(Game1.Scene.Script));

            // Add pause commands
            _controllers[0].AddCommand((int)Keys.P, new PauseCommand(Game1));
        }

        public void Update()
        {
            _controllers.ForEach(c => c.UpdateInput());
        }

        public void Draw()
        {
            GraphicsDevice.GraphicsDevice.Clear(Color.CornflowerBlue);
        }
    }
}