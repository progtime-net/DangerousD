using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using DangerousD.GameCore.GameObjects.MapObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            
            XmlNodeList layers = xml.DocumentElement.SelectNodes("layer");
            Debug.Write(layers.Count);
            foreach (XmlNode layer in layers)
            {
                InstantiateTiles(layer, tileSize);
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
                    
                        switch (tileType)
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
                        }
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
    }
}
