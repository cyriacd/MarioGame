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
        public static Texture2D RectanglePixel;
        private SpriteBatch _spriteBatch;
        public Camera Camera;
        private Vector2 _camPos;

        public Scene(Stage stage)
        {
            Stage = stage;
            Script = new Script(this);
        }

        private List<Layer> Layers { get; set; }
        public Stage Stage { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Initialize()
        {
            Stage.Initialize();
            Script.Initialize();
            Layers = new List<Layer>();
            var middle = new Vector2(Stage.Game1.GraphicsDevice.Viewport.Width/2f,
                Stage.Game1.GraphicsDevice.Viewport.Height/2f);

            LevelLoader.AddTileMapToScript("Level1.json", Script, Stage.Game1.Content);
            Camera = new Camera(Stage.Game1.GraphicsDevice.Viewport);
            _camPos = Camera.Position;

        }

        public void LoadContent()
        {
            _spriteBatch = new SpriteBatch(Stage.Game1.GraphicsDevice);

            Stage.LoadContent();
            Layers.Add(new Layer(Camera, new Vector2(0.5f, 1.0f)));
            Layers.Add(new Layer(Camera, new Vector2(1.0f, 1.0f)));
            foreach (Entity e in Script.Entities)
            {
                if(e is Cloud)
                {
                    Layers[0].Add(e.Sprite);
                }
                else
                {
                    Layers[1].Add(e.Sprite);
                }
            }

            Layers.ForEach(l => l.Load());
            Script.Entities.ForEach(e => e.LoadBoundingBox());
            //Allows for bounding boxes to be drawn in different colors
            RectanglePixel = new Texture2D(Stage.Game1.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            RectanglePixel.SetData(new[] { Color.White });

        }

        public void Update(GameTime gameTime)
        {
            Stage.Update(gameTime);
            Script.Update(gameTime);
            // TODO Should we update the sprites in script? That way we are only doing updates from one location
            Layers.ForEach(l => l.Sprites.ForEach(s => s.Update((float)gameTime.ElapsedGameTime.TotalSeconds)));
            //camera.Position = new Vector2(camera.Position.X + 1, camera.Position.Y);
            //camera.LookAt(_script.mario.position);
        }

        public void Draw(GameTime gameTime)
        {
            Stage.Draw(gameTime, _spriteBatch);
            Layers.ForEach(l => l.Draw(_spriteBatch));
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.GetViewMatrix(new Vector2(1.0f)));
            Script.Entities.FindAll(e => (!e.Sprite.Visible)&& !(e is Cloud)).//TODO: make it so that bounding boxes are handled in the specific entities sprite's draw method
                ForEach(e => DrawRectangleBorder(_spriteBatch, e.BoundingBox, 1, e.BoxColor));
            _spriteBatch.End();
        }

        public void DrawRectangleBorder(SpriteBatch batch, Rectangle toDraw, int borderThickness, Color borderColor)
        {
            // Draw top line
            batch.Draw(Scene.RectanglePixel, new Rectangle((toDraw.X), toDraw.Y, toDraw.Width, 1), borderColor);
            // Draw left line
            batch.Draw(Scene.RectanglePixel, new Rectangle(toDraw.X, toDraw.Y, 1, toDraw.Height), borderColor);
            // Draw right line
            batch.Draw(Scene.RectanglePixel, new Rectangle((toDraw.X + toDraw.Width - 1), toDraw.Y, 1, toDraw.Height), borderColor);
            // Draw bottom line
            batch.Draw(Scene.RectanglePixel, new Rectangle(toDraw.X, toDraw.Y + toDraw.Height - 1, toDraw.Width, 1), borderColor);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _spriteBatch.Dispose();
        }

        public Script Script { get; }
    }
}
