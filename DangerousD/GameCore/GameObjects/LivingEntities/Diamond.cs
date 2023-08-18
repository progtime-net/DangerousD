using System;
using System.Collections.Generic;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
	public class Diamond : Entity
	{

		protected override GraphicsComponent GraphicsComponent { get; } = new GraphicsComponent(new List<string>() { "spriteDiamond" }, "spriteDiamond");

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

