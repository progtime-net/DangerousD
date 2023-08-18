using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using DangerousD.GameCore.GameObjects.MapObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using DangerousD.GameCore.GameObjects;
using System.Globalization;
using System.IO;
using DangerousD.GameCore.GameObjects.Entities;
using DangerousD.GameCore.GameObjects.LivingEntities;

namespace DangerousD.GameCore.Managers
{
    public class MapManager
    {

        private int _scale;
        private int _columns;

        public MapManager(int scale)
        {
            _scale = scale;
        }
        
        //Level
        public void LoadLevel(string level)
        {
            LoadTilesData();
            
            XmlDocument xml = new();
            xml.Load($"../../../Content/{level}.tmx");
            Vector2 tileSize = new(int.Parse(xml.DocumentElement.Attributes["tilewidth"].Value),
                int.Parse(xml.DocumentElement.Attributes["tileheight"].Value));
            //tileSize *= _scale;
            
            foreach (XmlNode layer in xml.DocumentElement.SelectNodes("layer"))
            {
                InstantiateTiles(layer, tileSize);
            }

            foreach (XmlNode layer in xml.DocumentElement.SelectNodes("objectgroup"))
            {
                InstantiateEntities(layer);
            }
        }

        private void InstantiateTiles(XmlNode layer, Vector2 tileSize)
        {
            string tileType = layer.Attributes["class"].Value;
            float offsetX = layer.Attributes["offsetx"] is not null ? float.Parse(layer.Attributes["offsetx"].Value, CultureInfo.InvariantCulture) : 0;
            float offsetY = layer.Attributes["offsety"] is not null ? float.Parse(layer.Attributes["offsety"].Value, CultureInfo.InvariantCulture) : 0;

            
            Debug.Write(layer.SelectNodes("data/chunk").Count);
            foreach (XmlNode chunk in layer.SelectNodes("data/chunk"))
            {
                int chunkW = int.Parse(chunk.Attributes["width"].Value);
                int chunkX = int.Parse(chunk.Attributes["x"].Value);
                int chunkY = int.Parse(chunk.Attributes["y"].Value);
                
                
                List<int> tiles = chunk.InnerText.Split(',').Select(int.Parse).ToList();
                for (int i = 0; i < tiles.Count; i++)
                {
                    if (tiles[i] != 0)
                    {
                        Vector2 pos = new(((chunkX+ i % chunkW) * tileSize.X  + offsetX) * _scale,
                            ((chunkY + i / chunkW) * tileSize.Y + offsetY) * _scale);
                        //pos *= _scale;
                        Rectangle sourceRect = new(new Point((tiles[i] -1) % _columns, (tiles[i] -1) / _columns) * tileSize.ToPoint(), tileSize.ToPoint());
                        Type type = Type.GetType($"DangerousD.GameCore.GameObjects.MapObjects.{tileType}");
                        Activator.CreateInstance(type, pos, tileSize * _scale, sourceRect);
                    }
                }
            }
        }

        private void LoadTilesData()
        {
            
            XmlDocument xml = new();
            xml.Load($"../../../Content/map.tsx");
            XmlNode root = xml.DocumentElement;
            
            _columns = int.Parse(root.Attributes["columns"].Value);
        }

        private void InstantiateEntities(XmlNode group)
        {
            string entityGroup = group.Attributes["class"] is not null ? group.Attributes["class"].Value : "";
            float offsetX = group.Attributes["offsetx"] is not null ? float.Parse(group.Attributes["offsetx"].Value) : 0;
            float offsetY = group.Attributes["offsety"] is not null ? float.Parse(group.Attributes["offsety"].Value) : 0;
            foreach (XmlNode entity in group.ChildNodes)
            {
                string entityType = entity.Attributes["type"] is not null ? "." + entity.Attributes["type"].Value : "";
                Type type = Type.GetType($"DangerousD.GameCore.GameObjects.{entityGroup}{entityType}");
                Vector2 pos =
                    new Vector2(float.Parse(entity.Attributes["x"].Value, CultureInfo.InvariantCulture) + offsetX,
                        float.Parse(entity.Attributes["y"].Value, CultureInfo.InvariantCulture) + offsetY) * _scale;
                Entity inst;
                if (type.Equals(typeof(Player)))
                {
                    inst = (Entity)Activator.CreateInstance(type, pos, false);
                }
                else if (type.Equals(typeof(Door)))
                {
                    int gid =  entity.Attributes["gid"] is not null ? int.Parse(entity.Attributes["gid"].Value) : 0;
                    inst = (Entity)Activator.CreateInstance(type, pos, new Vector2(32, 48), new Rectangle((gid - 872)*32, 0, 32, 48));
                }
                else
                {
                    inst = (Entity)Activator.CreateInstance(type, pos);
                }
                inst.SetPosition(new Vector2(inst.Pos.X, inst.Pos.Y - inst.Height));
                inst.Height *= _scale;
                inst.Width *= _scale;
            }   
        }
    }
}
