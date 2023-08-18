using System;

using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
	public class Diamond : Entity
	{

        protected override GraphicsComponent GraphicsComponent => throw new NotImplementedException();

        public Diamond(Vector2 position) : base(position)
		{
			
		}
		public void Update(Player player)
		{
			if (Rectangle.Intersects(player.Rectangle))
			{
				player.score++;
			}
		}
	}
}

