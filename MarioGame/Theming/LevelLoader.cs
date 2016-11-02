﻿using MarioGame.Entities;
using MarioGame.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioGame.Theming
{
    internal static class LevelLoader
    {
        public static Entity CreateEntity(string klass, Vector2 location, ContentManager content, Action<Entity> addToEntities)
        {
            var type = Type.GetType(typeof(Entity).Namespace + "." + klass);
            Debug.Assert(type != null, "type != null");
            return (Entity)Activator.CreateInstance(type, location * GlobalConstants.GridWidth, content, addToEntities);
        }

        public static void AddTileMapToScript(string tileMapFile, Script script, ContentManager content)
        {

            var json = File.ReadAllText(tileMapFile);
            var level = JsonConvert.DeserializeObject<Level>(json);
            script.LevelWidth = level.width;
            level.entities.FindAll(e => e.position != null).ForEach(e =>
            {
                e.position.ForEach(rc =>
                {
                rc.columns.ForEach(c => {
                    var entity = CreateEntity(e.type, new Vector2(c, rc.row), content, script.AddEntity);
                    if(entity is Mario)
                    {
                        ((Mario)entity).LevelWidth = level.width*GlobalConstants.GridWidth;
                    }
                    if(entity is BackgroundItem)
                    {
                        ((BackgroundItem)entity).Layer = e.backgroundlayer;
                    }
                    if (e.actionState != null)
                    {
                        //TODO: make it so that we dont have to check what type each entity is 
                        if (entity is Block)
                        {
                            ((Block)entity).SetBlockActionState(e.actionState);
                        }
                    }
                    if (e.visibility != null)
                    {
                        if (entity is Block)
                        {//TODO: get rid of check for block in case we want to init mario to a certain power up state. also get rid of block power up states.
                            if (e.visibility == "Hidden")
                            {
                                ((Block)entity).Hide();
                            }
                            else
                            {
                                ((Block)entity).Show();
                            }
                        }
                    }
                    else
                    {
                        if (entity is Block)
                        {
                            ((Block)entity).Show();
                        }
                    }
                    script.AddEntity(entity);
                });
                });
            });

            level.entities.FindAll(e => e.positionWithHiddenItems != null).ForEach(e =>
            {
                e.positionWithHiddenItems.ForEach(instance =>
                {
                    var entity = CreateEntity(e.type, new Vector2(instance.column, instance.row), content, script.AddEntity);
                    script.AddEntity(entity);
                    if (e.actionState != null)
                    {
                        //TODO: make it so that we dont have to check what type each entity is 
                        if (entity is Block)
                        {
                            ((Block)entity).SetBlockActionState(e.actionState);
                        }
                    }
                    if (e.visibility != null)
                    {
                        if (entity is Block)
                        {//TODO: get rid of check for block in case we want to init mario to a certain power up state. also get rid of block power up states.
                            if (e.visibility == "Hidden")
                            {
                                ((Block)entity).Hide();
                            }
                            else
                            {
                                ((Block)entity).Show();
                            }
                        }
                    }
                    else
                    {
                        if (entity is Block)
                        {
                            ((Block)entity).Show();
                        }
                    }
                    instance.hiddenItems.ForEach(h =>
                    {
                        while (h.amount-- > 0)
                        {
                            var hiddenItem = (ContainableHidableEntity)CreateEntity(h.type, new Vector2(instance.column, instance.row), content, script.AddEntity);
                            ((IContainer)entity).AddContainedItem(hiddenItem);
                            script.AddEntity(hiddenItem);
                            hiddenItem.Hide();
                        }
                    });

                });
            });

        }
    }

    public class Position
    {
        public float row { get; set; }
        public List<float> columns { get; set; }
    }

    public class HiddenItem
    {
        public string type { get; set; }
        public int amount { get; set; }
    }

    public class positionWithHiddenItem
    {
        public float row { get; set; }
        public float column { get; set; }
        public List<HiddenItem> hiddenItems { get; set; }
    }

    public class JEntity
    {
        public string type { get; set; }
        public List<Position> position { get; set; }
        public List<positionWithHiddenItem> positionWithHiddenItems { get; set; }
        public string visibility { get; set; }
        public string actionState { get; set; }
        public string backgroundtype { get; set; }
        public int backgroundlayer { get; set; }
    }

    public class Level
    {
        public int width { get; set; }
        public int height { get; set; }
        public Collection<int> checkpoints { get; set; }
        public List<JEntity> entities { get; set; }
    }
}
