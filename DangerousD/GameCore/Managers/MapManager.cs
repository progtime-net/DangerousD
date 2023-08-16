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
        //Level
        public void LoadLevel(string level)
        {
            XmlDocument xml = new();
            xml.Load($"{level}.tmx");
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
            
            foreach (XmlNode chunk in layer.ChildNodes)
            {
                Vector2 chunkSize = new(int.Parse(chunk.Attributes["width"].Value),
                    int.Parse(chunk.Attributes["height"].Value));
                Vector2 chunkPos = new(int.Parse(chunk.Attributes["x"].Value), int.Parse(chunk.Attributes["y"].Value));
                
                
                List<int> tiles = chunk.Value.Split(',').Select(int.Parse).ToList();
                for (int i = 0; i < tiles.Count; i++)
                {
                    Vector2 pos = new((chunkPos.Y + i % chunkSize.X) * tileSize.Y,
                        (chunkPos.Y + i / chunkSize.X) * tileSize.Y);
                    Rectangle sourceRect = new(pos.ToPoint(), tileSize.ToPoint());

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
    }
}
