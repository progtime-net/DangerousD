using DangerousD.GameCore.GameObjects;
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
            //new MapObject(new Vector2(0,128));
        }
    }
    public class MapManager
    {
        public void Init()
        {

        }
        //Level
        public void LoadLevel(string level)
        {

        }
    }
}
