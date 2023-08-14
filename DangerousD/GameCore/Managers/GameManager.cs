using DangerousD.GameCore.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DangerousD.GameCore
{
    static class GameManager
    {
        static List<LivingEntity> livingEntities;
        static List<MapObject> MapObjects;
        internal static void Register(GameObject gameObject)
        {
            if (gameObject is LivingEntity)
                livingEntities.Add(gameObject as LivingEntity);
            if (gameObject is MapObject)
                MapObjects.Add(gameObject as MapObject);
        }
        public static void Start()
        {
            livingEntities = new List<LivingEntity>();
            MapObjects = new List<MapObject>();
        }

        public static void Draw(SpriteBatch _spriteBatch)
        {

        }
        public static void Update(GameTime gameTime)
        {

        }
    }
}
