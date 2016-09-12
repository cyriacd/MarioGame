﻿using System;
using System.Collections.Generic;
using CSE3902_Sprint0.Theming;
using CSE3902_Sprint0.Theming.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CSE3902_Sprint0.Core
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly int _scene;
        private readonly List<Scene> _scenes;

        public Game1()
        {
            Stage stage = new Stage(this);

            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _scenes = new List<Scene>();
            _scenes.Add(new Scene(stage));
            _scene = 1;
            Console.Out.WriteLine(_scenes.Count.ToString());
        }

        public Scene Scene
        {
            get { return _scenes[_scene - 1]; }
        }

        public GraphicsDeviceManager Graphics { get; private set; }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _scenes[_scene - 1].Initialize();

            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _scenes[_scene - 1].LoadContent();
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _scenes[_scene - 1].Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _scenes[_scene - 1].Draw(gameTime);

            base.Draw(gameTime);
        }

        public void ExitCommand()
        {
            Exit();
        }
    }
}