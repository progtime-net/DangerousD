using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using DangerousD.GameCore.GameObjects.MapObjects;
using Microsoft.Xna.Framework;
using DangerousD.GameCore.GameObjects;
using System.Globalization;
using DangerousD.GameCore.GameObjects.Entities;
using DangerousD.GameCore.GameObjects.LivingEntities;

namespace DangerousD.GameCore.Managers;

    
    public class MapManager
    {
        private int _scale;
        private List<TileSet> _tileSets = new();

        public MapManager(int scale)
        {
            _scale = scale;
        }
        
        private void LoadTileSets(XmlNodeList tileSets)
        {
            foreach (XmlNode tileSet in tileSets)
            {
                string source = tileSet.Attributes["source"].Value;
                string filePath = $"../../../Content/{source}";

                XmlDocument tsx = new();
                tsx.Load(filePath);
                XmlNode root = tsx.DocumentElement;

                int firstGid = int.Parse(tileSet.Attributes["firstgid"].Value);
                int tileWidth = int.Parse(root.Attributes["tilewidth"].Value);
                int tileHeight = int.Parse(root.Attributes["tileheight"].Value);
                int tileCount = int.Parse(root.Attributes["tilecount"].Value);
                int columns = int.Parse(root.Attributes["columns"].Value);

                string tileSource = root.FirstChild.Attributes["source"].Value;

                _tileSets.Add(new TileSet(tileSource, firstGid, new Point(tileWidth, tileHeight), tileCount / columns, columns));
            }
        }

        
        private TileSet GetTileSet(long gid)
        {
            if (_tileSets == null || _tileSets.Count == 0)
            {
                return null;
            }
            var tileSet = _tileSets.FirstOrDefault(t => gid >= t.FirstGid && (_tileSets.IndexOf(t) == _tileSets.Count - 1 || gid < _tileSets[_tileSets.IndexOf(t) + 1].FirstGid));
            return tileSet;
        }
        //Level
        public void LoadLevel(string map)
        {
            XmlDocument xml = new();
            xml.Load($"../../../Content/{map}.tmx");
            
            LoadTileSets(xml.DocumentElement.SelectNodes("tileset"));
            
            foreach (XmlNode layer in xml.DocumentElement.SelectNodes("layer"))
            {
                InstantiateTiles(layer);
            }
            
            foreach (XmlNode layer in xml.DocumentElement.SelectNodes("objectgroup"))
            {
                InstantiateEntities(layer);
            }
        }

        private float GetAttr(XmlNode node, string attributeName)
        {
            return node.Attributes[attributeName] != null ? float.Parse(node.Attributes[attributeName].Value) : 0;
        }
        
        private void InstantiateTiles(XmlNode layer)
        {
            string tileType = layer.Attributes["class"].Value;
            float offsetX = GetAttr(layer, "offsetx");
            float offsetY = GetAttr(layer, "offsety");

            
            foreach (XmlNode chunk in layer.SelectNodes("data/chunk"))
            {
                int chunkW = int.Parse(chunk.Attributes["width"].Value);
                int chunkX = int.Parse(chunk.Attributes["x"].Value);
                int chunkY = int.Parse(chunk.Attributes["y"].Value);


                string[] tiles = chunk.InnerText.Split(',');
                for (int i = 0; i < tiles.Length; i++)
                {
                    long gid = long.Parse(tiles[i]);

                    if (gid == 0) continue;
                    
                    TileSet tileSet = GetTileSet(gid);
                    Vector2 pos = new(((chunkX + i % chunkW) * tileSet.TileSize.X  + offsetX) * _scale,
                        ((chunkY + i / chunkW) * tileSet.TileSize.Y + offsetY) * _scale);
                    pos *= _scale;
                    
                    
                    Rectangle sourceRect = new(new Point((int)(gid - tileSet.FirstGid) % tileSet.Columns, (int)(gid - tileSet.FirstGid) / tileSet.Columns) * tileSet.TileSize, tileSet.TileSize);
                    Type type = Type.GetType($"DangerousD.GameCore.GameObjects.{tileType}");
                    Activator.CreateInstance(type, pos, tileSet.TileSize.ToVector2() * _scale, sourceRect);
                }
            }
        }

        
        private void InstantiateEntities(XmlNode layer)
        {
            string entityType = layer.Attributes["class"] is not null ? "." + layer.Attributes["class"].Value : "";
            float offsetX = GetAttr(layer, "offsetx");;
            float offsetY = GetAttr(layer, "offsety");;
            
            foreach (XmlNode entity in layer.ChildNodes)
            {
                string finalEntityType = entityType + (entity.Attributes["type"] is not null ? "." + entity.Attributes["type"].Value : "");
                Type type = Type.GetType($"DangerousD.GameCore.GameObjects{finalEntityType}");
                
                Vector2 pos =
                    new Vector2(float.Parse(entity.Attributes["x"].Value) + offsetX,
                        float.Parse(entity.Attributes["y"].Value) + offsetY) * _scale;

                Entity inst = null;
                if (typeof(Door).IsAssignableFrom(type))
                {
                    long gid = entity.Attributes["gid"] is not null ? int.Parse(entity.Attributes["gid"].Value) : 0;
                    TileSet tileSet = GetTileSet(gid);
                    Vector2 objectSize = new(int.Parse(entity.Attributes["width"].Value), int.Parse(entity.Attributes["height"].Value));
                    
                    /// TODO: idk what to do with this, maybe fix api
                    if (type == typeof(TeleportingDoor))
                    {
                        XmlNode level = entity.SelectSingleNode("properties/property[@name = 'level']");
                    
                        if (level is not null)  // Teleport to level
                        {
                            inst = new TeleportingDoor(pos, objectSize,
                                new Rectangle(new Point((int)(gid - tileSet.FirstGid) * tileSet.TileSize.X, 0),
                                    tileSet.TileSize), () => {AppManager.Instance.ChangeMap(level.Attributes["value"].Value, GetStartCoordinates(level.Attributes["value"].Value));});
                        }
                        else  // Teleport to location
                        {
                            XmlNode destination = entity.SelectSingleNode("properties/property[@name = 'destination']");
                            string target = destination is not null ? destination.Attributes["value"].Value : "0";
                            XmlNode dest = layer.SelectSingleNode($"object[@id = '{target}']");
                        
                            
                            inst = new TeleportingDoor(pos, objectSize, new Rectangle(new Point((int)(gid - tileSet.FirstGid) * tileSet.TileSize.X, 0), tileSet.TileSize),
                                new Vector2(float.Parse(dest.Attributes["x"].Value) + offsetX,
                                    float.Parse(dest.Attributes["y"].Value) + offsetY) * _scale);
                        }
                    } 
                    else
                        inst = new Door(pos, objectSize, new Rectangle(new Point((int)(gid - tileSet.FirstGid) * tileSet.TileSize.X, 0), tileSet.TileSize));
                }
                else if (!type.Equals(typeof(Player)) || (type.Equals(typeof(Player)) && AppManager.Instance.GameManager.players.Count == 0))
                {
                    inst = (Entity)Activator.CreateInstance(type, pos);
                }

                if (inst is not null)
                {
                    inst.SetPosition(new Vector2(inst.Pos.X, inst.Pos.Y - inst.Height));
                }
            }
        }

        private Vector2 GetStartCoordinates(string map)
        {
            XmlDocument xml = new();
            xml.Load($"../../../Content/{map}.tmx");

            XmlNode player = xml.DocumentElement.SelectSingleNode("//objectgroup[@class = 'LivingEntities.Player']").FirstChild;

            return new Vector2(float.Parse(player.Attributes["x"].Value),
                float.Parse(player.Attributes["y"].Value));
        }
    }

    class TileSet
    {
        public string Texture;
        public int FirstGid;
        public Point TileSize;
        public int Rows;
        public int Columns;

        public TileSet(string texture, int firstGid, Point tileSize, int rows, int columns)
        {
            Texture = texture;
            FirstGid = firstGid;
            TileSize = tileSize;
            Rows = rows;
            Columns = columns;
        }
    }

