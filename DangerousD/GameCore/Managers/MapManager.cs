using System.Collections.Generic;
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
        
        //Level
        public void LoadLevel(string level)
        {
            LoadTilesData();
            
            XmlDocument xml = new();
            xml.Load($"../../../Content/{level}.tmx");
            Vector2 tileSize = new(int.Parse(xml.DocumentElement.Attributes["tilewidth"].Value),
                int.Parse(xml.DocumentElement.Attributes["tileheight"].Value));
            
            XmlNodeList layers = xml.DocumentElement.SelectNodes("//layer");

            foreach (XmlNode layer in layers)
            {
                InstantiateTiles(layer, tileSize);
            }
        }

        private void InstantiateTiles(XmlNode layer, Vector2 tileSize)
        {
            string tileType = layer.Attributes["class"].Value;
            
            foreach (XmlNode chunk in layer.SelectNodes("//chunk"))
            {
                int chunkW = int.Parse(chunk.Attributes["width"].Value);
                int chunkX = int.Parse(chunk.Attributes["x"].Value);
                int chunkY = int.Parse(chunk.Attributes["y"].Value);
                
                
                List<int> tiles = chunk.InnerText.Split(',').Select(int.Parse).ToList();
                for (int i = 0; i < tiles.Count; i++)
                {
                    if (tiles[i] == 0) continue;
                    
                    Vector2 pos = new((chunkX + i % chunkW) * tileSize.X,
                        (chunkY + i / chunkW) * tileSize.Y);
                    Rectangle sourceRect = new(new Point(tiles[i] % _columns, tiles[i] / _columns), tileSize.ToPoint());

                    switch (tileType)
                    {
                        case "collidable":
                            new StopTile(pos, sourceRect);
                            break;
                        case "platform":
                            new Platform(pos, sourceRect);
                            break;
                        case "non_collidable":
                            new Tile(pos, sourceRect);
                            break;
                    }}
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
