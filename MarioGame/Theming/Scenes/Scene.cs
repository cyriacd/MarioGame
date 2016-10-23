﻿using System;
using System.Collections.Generic;
using MarioGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarioGame.Entities;
using Microsoft.Xna.Framework.Content;

namespace MarioGame.Theming.Scenes
{
    public class Scene : IDisposable
    {
        //Texture in order to draw bounding boxes on screen from sprint2
        public static Texture2D rectanglePixel;
        private readonly Script _script;
        private SpriteBatch _spriteBatch;

        public Scene(Stage stage)
        {
            Stage = stage;
            _script = new Script(this);
        }

        private List<ISprite> Sprites { get; set; }
        public Stage Stage { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Initialize()
        {
            Stage.Initialize();
            _script.Initialize();
            Sprites = new List<ISprite>();
            var middle = new Vector2(Stage.Game1.GraphicsDevice.Viewport.Width/2f,
                Stage.Game1.GraphicsDevice.Viewport.Height/2f);

            LevelLoader.addTileMapToScript("Level1.json", _script, Stage.Game1.Content);

        }

        public void LoadContent()
        {
            _spriteBatch = new SpriteBatch(Stage.Game1.GraphicsDevice);

            Stage.LoadContent();

            _script._entities.ForEach(e => Sprites.Add(e._sprite));
            Sprites.ForEach(s => s.Load());
            //Allows for bounding boxes to be drawn in different colors
            rectanglePixel = new Texture2D(Stage.Game1.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            rectanglePixel.SetData(new[] { Color.White });

        }

        public void Update(GameTime gameTime)
        {
            Stage.Update(gameTime);
            _script.Update(gameTime);
            // TODO Should we update the sprites in script? That way we are only doing updates from one location
            Sprites.ForEach(s => s.Update((float)gameTime.ElapsedGameTime.TotalSeconds));
        }

        public void Draw(GameTime gameTime)
        {
            Stage.Draw(gameTime, _spriteBatch);

            _spriteBatch.Begin();
            Sprites.ForEach(s => s.Draw(_spriteBatch));
            _script._entities.FindAll(e => !e._sprite.Visible).ForEach(e => drawRectangleBorder(_spriteBatch, e.boundingBox, 1, e.BoxColor));
            _spriteBatch.End();
        }

        public void drawRectangleBorder(SpriteBatch batch, Rectangle toDraw, int borderThickness, Color borderColor)
        {
            // Draw top line
            batch.Draw(Scene.rectanglePixel, new Rectangle((toDraw.X), toDraw.Y, toDraw.Width, 1), borderColor);
            // Draw left line
            batch.Draw(Scene.rectanglePixel, new Rectangle(toDraw.X, toDraw.Y, 1, toDraw.Height), borderColor);
            // Draw right line
            batch.Draw(Scene.rectanglePixel, new Rectangle((toDraw.X + toDraw.Width - 1), toDraw.Y, 1, toDraw.Height), borderColor);
            // Draw bottom line
            batch.Draw(Scene.rectanglePixel, new Rectangle(toDraw.X, toDraw.Y + toDraw.Height - 1, toDraw.Width, 1), borderColor);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _spriteBatch.Dispose();
        }

        public Script getScript()
        {
            return this._script;
        }

    }
}
