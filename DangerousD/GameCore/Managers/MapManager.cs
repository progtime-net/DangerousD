using DangerousD.GameCore.GameObjects;
using DangerousD.GameCore.Graphics;
using DangerousD.GameCore.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.Managers
{
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
