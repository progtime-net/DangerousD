using System;
using System.Collections.Generic;
using System.Text;

namespace DangerousD.GameCore
{
    class GameObject
    {
        GraphicsComponent graphicsComponent;
        public GameObject()
        {
            GameManager.Register(this);
        }
    }
}
