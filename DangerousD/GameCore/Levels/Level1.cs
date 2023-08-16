﻿using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.GameObjects.MapObjects;
using Microsoft.Xna.Framework;
using DangerousD.GameCore.GameObjects.LivingEntities.Monsters;


namespace DangerousD.GameCore.Levels
{
    public class Level1 : ILevel
    {
        public void InitLevel()
        {
            new Player(new Vector2(0,0));

            var Zombie = new Zombie(new Vector2(300, 64));
            var Frank = new Frank(new Vector2(100, 64));

            new GrassBlock(new Vector2(0, 224));
            for (int i = 0; i < 50; i++)
            {
                new GrassBlock(new Vector2(i*32, 256));
            }
            new GrassBlock(new Vector2(500, 224));
        }
    }
}