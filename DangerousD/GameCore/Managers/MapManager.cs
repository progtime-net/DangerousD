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

namespace DangerousD.GameCore.Managers
{
    public class MapManager
    {
        private int _columns;
        private int _scale;
        
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
            float offsetX = layer.Attributes["offsetx"] is not null ? float.Parse(layer.Attributes["offsetx"].Value) : 0;
            float offsetY = layer.Attributes["offsety"] is not null ? float.Parse(layer.Attributes["offsety"].Value) : 0;

            
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
                        Vector2 pos = new((chunkX+ i % chunkW) * tileSize.X * _scale + offsetX,
                            (chunkY + i / chunkW) * tileSize.Y * _scale + offsetY);
                        //pos *= _scale;
                        Rectangle sourceRect = new(new Point((tiles[i] -1) % _columns, (tiles[i] -1) / _columns) * tileSize.ToPoint(), tileSize.ToPoint());
                        Type type = Type.GetType($"DangerousD.GameCore.GameObjects.MapObjects.{tileType}");
                        Activator.CreateInstance(type, pos, tileSize * _scale, sourceRect);

                        /*switch (tileType)
                        {
                            case "collidable":
                                new StopTile(pos, tileSize * _scale, sourceRect);
                                break;
                            case "platform":
                                new Platform(pos, tileSize * _scale, sourceRect);
                                break;
                            case "non_collidable":
                                new Tile(pos, tileSize * _scale, sourceRect);
                                break;
                        }*/
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
            string entityType = group.Attributes["class"].Value;
            foreach (XmlNode entity in group.ChildNodes)
            {
                Type type = Type.GetType($"DangerousD.GameCore.GameObjects.{entityType}");
                Activator.CreateInstance(type, new Vector2(float.Parse(entity.Attributes["x"].Value), float.Parse(entity.Attributes["y"].Value)));
            }
        }
    }
}
