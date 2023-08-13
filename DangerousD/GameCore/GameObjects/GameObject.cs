using System;
using System.Collections.Generic;
using System.Text;

namespace DangerousD.GameCore
{
    class GameObject
    {
        public GameObject()
        {
            GameManager.Register(this);
        }
    }
}
