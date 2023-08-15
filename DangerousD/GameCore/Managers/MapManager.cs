using DangerousD.GameCore.GameObjects;
using DangerousD.GameCore.GameObjects.MapObjects;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.Managers
{
    interface ILevel
    {
        void InitLevel();
    }
    public class Level1 : ILevel
    {
        public void InitLevel()
        {
            var Трава = new GrassBlock(new Vector2(0,128));
        }
    }
    public class MapManager
    {
        ILevel Level;
        public void Init()
        {
            Level = new Level1();
        }
        //Level
        public void LoadLevel(string level)
        {
            Level.InitLevel();
        }
    }
}
