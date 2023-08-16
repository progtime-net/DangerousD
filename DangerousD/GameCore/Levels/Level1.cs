using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.GameObjects.MapObjects;
using Microsoft.Xna.Framework;
using DangerousD.GameCore.GameObjects.LivingEntities.Monsters;


namespace DangerousD.GameCore.Levels
{
    public class Level1 : ILevel
    {
        public void InitLevel()
        {
<<<<<<< HEAD
            var Трава = new GrassBlock(new Vector2(0, 128));
            var Death = new TestAnimationDeath(new Vector2(128, 128));
            //var Zombie = new Zombie(new Vector2(256, 128));
            var Frank = new Frank(new Vector2(384, 128));
            var FlameSkull = new FlameSkull(new Vector2(512, 128));
=======
            new Player(new Vector2(0,0));

            var Zombie = new Zombie(new Vector2(256, 128));
            var Frank = new Frank(new Vector2(384, 128));

            new GrassBlock(new Vector2(0, 224));
            for (int i = 0; i < 50; i++)
            {
                new GrassBlock(new Vector2(i*32, 256));
            }
            new GrassBlock(new Vector2(500, 224));
>>>>>>> livingEntities
        }
    }
}
