using DangerousD.GameCore.GameObjects;
using DangerousD.GameCore.Graphics;
using DangerousD.GameCore.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DangerousD.GameCore.GameObjects.MapObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DangerousD.GameCore.Managers
{
    public class MapManager
    {
        private Texture2D _texture;
        public void Init()
        {
        }
        //Level
        public void LoadLevel(string level)
        {
            XmlDocument xml = new();
            xml.Load($"{level}.tmx");
            XmlNode mapNode = xml.DocumentElement[].SelectSingleNode("//layer[@type='collidable']");
            Vector2 tileSize = new(int.Parse(xml.DocumentElement.Attributes["tilewidth"].Value),
                int.Parse(xml.DocumentElement.Attributes["tileheight"].Value));
            
            foreach (XmlNode chunk in mapNode.ChildNodes)
            {
                Vector2 chunkSize = new(int.Parse(chunk.Attributes["width"].Value), int.Parse(chunk.Attributes["height"].Value))
                Vector2 chunkPos = new(int.Parse(chunk.Attributes["x"].Value), int.Parse(chunk.Attributes["y"].Value));
                
                
                List<int> tiles = chunk.Value.Split(',').Select(int.Parse).ToList();
                for (int i = 0; i < tiles.Count; i++)
                {
                    new StopTile(chunk)
                }
            }
        }

        private void CreateTiles(T d)
        {
            
        }
    }
}
