
using DangerousD.GameCore.Managers;
using DangerousD.GameCore.GameObjects.LivingEntities;
using System;
using Microsoft.Xna.Framework.Content;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
	public class Diamond : Entity
	{
		public PhysicsManager PhysicsManager;
        protected override GraphicsComponent GraphicsComponent => throw new NotImplementedException();
		public Texture2D texture;
		public Vector2 Position;

		public Diamond(Vector2 pos) : base(pos)
		{
			PhysicsManager = new PhysicsManager();
			Position = pos;
		}
		public void Update(Player player)
		{
			/*if (Rectangle.Intersects())
			{

			}*/
		}
    }
}

