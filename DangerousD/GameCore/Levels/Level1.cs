using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.GameObjects.MapObjects;
using Microsoft.Xna.Framework;
using DangerousD.GameCore.GameObjects.LivingEntities.Monsters;
using System.Collections.Generic;

namespace DangerousD.GameCore.Levels
{
    public class Level1 : ILevel
    {
        public void InitLevel()
        {

            new Player(new Vector2(350,0));
            var Zombie = new Zombie(new Vector2(100, 128));

            //var Frank = new Frank(new Vector2(384, 128));
            //var Spider = new Spider(new Vector2(112, 0));
            //var FlameSkull = new FlameSkull(new Vector2(512, 0));
            //var Werewolf = new Werewolf(new Vector2(640, 0));
            //var Ghost = new Ghost(new Vector2(730, 0));
            //var FrankBalls = new FrankBalls(new Vector2(Frank.Pos.X, Frank.Pos.Y));     
            //var SilasHand = new SilasHands(new Vector2(200,64));
            //var SilasMaster = new SilasMaster(new Vector2(400, 64));

            var HunchMan = new Hunchman(new Vector2(300, 100));

            new GrassBlock(new Vector2(0, 224));
            for (int i = 0; i < 50; i++)
            {
                new GrassBlock(new Vector2(i*32, 256));
            }
            new GrassBlock(new Vector2(500, 224));
        }
    }
}
