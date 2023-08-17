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
        private int _tilesCount;
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
            float offsetX = layer.Attributes["offsetx"] is not null
                ? float.Parse(layer.Attributes["offsetx"].Value)
                : 0;
            float offsetY = layer.Attributes["offsety"] is not null
                ? float.Parse(layer.Attributes["offsety"].Value)
                : 0;


            foreach (XmlNode chunk in layer.SelectNodes("data/chunk"))
            {
                int chunkW = int.Parse(chunk.Attributes["width"].Value);
                int chunkH = int.Parse(chunk.Attributes["height"].Value);
                int chunkX = int.Parse(chunk.Attributes["x"].Value);
                int chunkY = int.Parse(chunk.Attributes["y"].Value);


                int[] parse = chunk.InnerText.Split(',').Select(int.Parse).ToArray();
                int[,] arr = new int[chunkW, chunkH];
                for (int i = 0; i < chunkH; i++)
                {
                    for (int j = 0; j < chunkW; j++)
                    {
                        arr[i, j] = parse[i * chunkH + j];
                    }
                }

                for (int i = 1; i < _tilesCount; i++)
                {
                    List < Rectangle > rects = GenerateRectangles(arr, i);
                    foreach (Rectangle rect in rects)
                    {
                        Vector2 pos = new(((chunkX + rect.X % chunkW) * tileSize.X + offsetX) * _scale,
                            ((chunkY + rect.X / chunkW) * tileSize.Y + offsetY) * _scale);
                        Rectangle sourceRect =
                            new(new Point((i - 1) % _columns, (i - 1) / _columns) * tileSize.ToPoint(),
                                tileSize.ToPoint());
                        Type type = Type.GetType($"DangerousD.GameCore.GameObjects.MapObjects.{tileType}");
                        Activator.CreateInstance(type, pos, new Vector2(rect.Width * tileSize.X, rect.Height * tileSize.Y) * _scale, sourceRect);
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
            _tilesCount = int.Parse(root.Attributes["tilecount"].Value);
        }

        private void InstantiateEntities(XmlNode group)
        {
            string entityType = group.Attributes["class"].Value;
            float offsetX = group.Attributes["offsetx"] is not null
                ? float.Parse(group.Attributes["offsetx"].Value)
                : 0;
            float offsetY = group.Attributes["offsety"] is not null
                ? float.Parse(group.Attributes["offsety"].Value)
                : 0;
            foreach (XmlNode entity in group.ChildNodes)
            {
                Type type = Type.GetType($"DangerousD.GameCore.GameObjects.{entityType}");
                Entity inst = (Entity)Activator.CreateInstance(type,
                    new Vector2(float.Parse(entity.Attributes["x"].Value) + offsetX,
                        float.Parse(entity.Attributes["y"].Value) + offsetY) * _scale);
                inst.SetPosition(new Vector2(inst.Pos.X, inst.Pos.Y - inst.Height));
                inst.Height *= _scale;
                inst.Width *= _scale;
            }
        }
        Point FindEnd(int i, int j, int[,] arr, Rectangle rect, int tile)
        {
            int x = arr.GetLength(0);
            int y = arr.GetLength(1);
            int flagc = 0;
            int flagr = 0;
            int n = 0, m = 0;


            for (m = i; m < x; m++)
            {
                if (arr[m, j] != tile)
                {
                    flagr = 1;
                    break;
                }
                if (arr[m, j] == -1) continue;

                for (n = j; n < y; n++)
                {
                    if (arr[m, n] != tile)
                    {
                        flagc = 1;
                        break;
                    }
                    arr[m, n] = -1;
                }
            }

            Point res = new Point();
            if (flagr == 1)
                res.X = rect.X + (m - 1);
            else
                res.X = rect.X + m;
            if (flagc == 1)
                res.Y = rect.Y + (n - 1);
            else
                res.Y = rect.Y + n;
            return res;
        }

        List<Rectangle> GenerateRectangles(int[,] arr, int tile)
        {
            int arrSize = arr.GetLength(0);
            List<Rectangle> res = new List<Rectangle>();
            
            for (int i = 0; i < arrSize; i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (arr[i, j] == tile)
                    {
                        Rectangle rect = new Rectangle(i, j, 0, 0);
                        rect.Size = FindEnd(i, j, arr, rect, tile);
                        res.Add(rect);
                    }
                }
            }

            return res;
        }
    }
}
