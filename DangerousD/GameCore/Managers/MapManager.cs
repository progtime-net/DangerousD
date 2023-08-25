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
using DangerousD.GameCore.GameObjects.LivingEntities.Monsters;

namespace DangerousD.GameCore.Managers
{
    public class MapManager
    {
        private int _scale;
        private List<TileSet> _tileSets = new List<TileSet>();

        public MapManager(int scale)
        {
            _scale = scale;
        }

        private void LoadTileSets(XmlNodeList tileSets)
        {
            foreach (XmlNode tileSet in tileSets)
            {
                XmlDocument tsx = new();
                tsx.Load($"../../../Content/{tileSet.Attributes["source"].Value}");
                XmlNode root = tsx.DocumentElement;

                Point tileSize = new Point(int.Parse(root.Attributes["tilewidth"].Value),
                    int.Parse(root.Attributes["tileheight"].Value));

                int columns = int.Parse(root.Attributes["columns"].Value);
                _tileSets.Add(new TileSet(root.FirstChild.Attributes["source"].Value,
                    int.Parse(tileSet.Attributes["firstgid"].Value),
                    tileSize,
                    int.Parse(root.Attributes["tilecount"].Value) / columns,
                    columns));
            }
        }

        private TileSet GetTileSet(int gid)
        {
            if (_tileSets == null)
            {
                return null;
            }

            for (var i = 0; i < _tileSets.Count; i++)
            {
                if (i < _tileSets.Count - 1)
                {
                    int gid1 = _tileSets[i].FirstGid;
                    int gid2 = _tileSets[i + 1].FirstGid;

                    if (gid >= gid1 && gid < gid2)
                    {
                        return _tileSets[i];
                    }
                }
                else
                {
                    return _tileSets[i];
                }
            }
        
            return null;
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

        private void InstantiateTiles(XmlNode layer)
        {
            string tileType = layer.Attributes["class"].Value;
            float offsetX = layer.Attributes["offsetx"] is not null ? float.Parse(layer.Attributes["offsetx"].Value, CultureInfo.InvariantCulture) : 0;
            float offsetY = layer.Attributes["offsety"] is not null ? float.Parse(layer.Attributes["offsety"].Value, CultureInfo.InvariantCulture) : 0;

            
            foreach (XmlNode chunk in layer.SelectNodes("data/chunk"))
            {
                int chunkW = int.Parse(chunk.Attributes["width"].Value);
                int chunkX = int.Parse(chunk.Attributes["x"].Value);
                int chunkY = int.Parse(chunk.Attributes["y"].Value);


                string[] tiles = chunk.InnerText.Split(',');
                for (int i = 0; i < tiles.Length; i++)
                {
                    int gid = int.Parse(tiles[i]);

                    if (gid == 0) continue;
                    
                    TileSet tileSet = GetTileSet(gid);
                    Vector2 pos = new(((chunkX + i % chunkW) * tileSet.TileSize.X  + offsetX) * _scale,
                        ((chunkY + i / chunkW) * tileSet.TileSize.Y + offsetY) * _scale);
                    pos *= _scale;
                    
                    
                    Rectangle sourceRect = new(new Point((gid - tileSet.FirstGid) % tileSet.Columns, (gid - tileSet.FirstGid) / tileSet.Columns) * tileSet.TileSize, tileSet.TileSize);
                    Type type = Type.GetType($"DangerousD.GameCore.GameObjects.MapObjects.{tileType}");
                    Activator.CreateInstance(type, pos, tileSet.TileSize.ToVector2() * _scale, sourceRect);
                }
            }
        }

        private void InstantiateEntities(XmlNode layer)
        {
            string entityType = layer.Attributes["class"] is not null ? "." + layer.Attributes["class"].Value : "";
            float offsetX = layer.Attributes["offsetx"] is not null ? float.Parse(layer.Attributes["offsetx"].Value) : 0;
            float offsetY = layer.Attributes["offsety"] is not null ? float.Parse(layer.Attributes["offsety"].Value) : 0;
            
            foreach (XmlNode entity in layer.ChildNodes)
            {
                string finalEntityType = entityType + (entity.Attributes["type"] is not null ? "." + entity.Attributes["type"].Value : "");
                Type type = Type.GetType($"DangerousD.GameCore.GameObjects{finalEntityType}");
                
                Vector2 pos =
                    new Vector2(float.Parse(entity.Attributes["x"].Value, CultureInfo.InvariantCulture) + offsetX,
                        float.Parse(entity.Attributes["y"].Value, CultureInfo.InvariantCulture) + offsetY) * _scale;

                Entity inst;
                if (type.Equals(typeof(Door)))
                {
                    int gid = entity.Attributes["gid"] is not null ? int.Parse(entity.Attributes["gid"].Value) : 0;
                    TileSet tileSet = GetTileSet(gid);
                    Vector2 objectSize = new(int.Parse(entity.Attributes["width"].Value), int.Parse(entity.Attributes["height"].Value));
                    
                    /// TODO: wtf is tileSize   
                    inst = (Entity)Activator.CreateInstance(type, pos, objectSize, new Rectangle(new Point((gid - tileSet.FirstGid) * tileSet.TileSize.X, 0), tileSet.TileSize));
                    inst.SetPosition(new Vector2(inst.Pos.X, inst.Pos.Y - inst.Height));
                }
                else if (type.Equals(typeof(TeleportingDoor)))
                {
                    int gid = entity.Attributes["gid"] is not null ? int.Parse(entity.Attributes["gid"].Value) : 0;
                    Vector2 objectSize = new(int.Parse(entity.Attributes["width"].Value), int.Parse(entity.Attributes["height"].Value));
                    TileSet tileSet = GetTileSet(gid);
                    
                    XmlNode level = entity.SelectSingleNode("properties/property[@name = 'level']");
                    
                    if (level is not null)  // Teleport to level
                    {
                        inst = (Entity)Activator.CreateInstance(type, pos, objectSize,
                            new Rectangle(new Point((gid - tileSet.FirstGid) * tileSet.TileSize.X, 0),
                                tileSet.TileSize), () => {AppManager.Instance.ChangeMap(level.Attributes["value"].Value, GetStartCoordinates(level.Attributes["value"].Value));});
                    }
                    else  // Teleport to location
                    {
                        XmlNode destination = entity.SelectSingleNode("properties/property[@name = 'destination']");
                        string target = destination is not null ? destination.Attributes["value"].Value : "0";
                        XmlNode dest = layer.SelectSingleNode($"object[@id = '{target}']");
                        
                        if (dest is null)
                        {
                            throw new ArgumentNullException($"Door with id: {entity.Attributes["id"]} has invalid destination set");
                        }
                        
                        inst = (Entity)Activator.CreateInstance(type,pos, objectSize, new Rectangle(new Point((gid - tileSet.FirstGid) * tileSet.TileSize.X, 0), tileSet.TileSize),
                            new Vector2(float.Parse(dest.Attributes["x"].Value, CultureInfo.InvariantCulture) + offsetX,
                                float.Parse(dest.Attributes["y"].Value, CultureInfo.InvariantCulture) + offsetY) * _scale);
                    }
                    inst.SetPosition(new Vector2(inst.Pos.X, inst.Pos.Y - inst.Height));
                }
                else if (!type.Equals(typeof(Player)) || (type.Equals(typeof(Player)) && AppManager.Instance.GameManager.players.Count == 0))
                {
                    inst = (Entity)Activator.CreateInstance(type, pos);
                    inst.SetPosition(new Vector2(inst.Pos.X, inst.Pos.Y - inst.Height));
                }
            }   
        }

        private Vector2 GetStartCoordinates(string map)
        {
            XmlDocument xml = new();
            xml.Load($"../../../Content/{map}.tmx");

            XmlNode player = xml.DocumentElement.SelectSingleNode("//objectgroup[@class = 'LivingEntities.Player']").FirstChild;

            return new Vector2(float.Parse(player.Attributes["x"].Value, CultureInfo.InvariantCulture),
                float.Parse(player.Attributes["y"].Value, CultureInfo.InvariantCulture));
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
}
