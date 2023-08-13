using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DangerousD.GameCore.HUD
{
    interface IHUD
    {
        void Update();
        void Draw(SpriteBatch _spriteBatch);
    }
}
