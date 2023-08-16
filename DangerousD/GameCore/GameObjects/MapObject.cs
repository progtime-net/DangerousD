﻿using Microsoft.Xna.Framework;
using System.Security.Cryptography.X509Certificates;

namespace DangerousD.GameCore.GameObjects;

public abstract class MapObject : GameObject
{
    public bool IsColliderOn;
    public MapObject(Vector2 position) : base(position)
    {
        
    }
}