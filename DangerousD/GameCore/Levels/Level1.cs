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
            var Трава = new GrassBlock(new Vector2(0, 128));
            var Death = new TestAnimationDeath(new Vector2(128, 128));
            var Zombie = new Zombie(new Vector2(256, 128));
        }
    }
}
