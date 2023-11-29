using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;



namespace DangerousD.GameCore.GameObjects.Entities.Items
{
    internal class Ammo:Entity
    {
        Random random = new Random();
        public Ammo(Vector2 position) : base(position)
        {
            int scal = 20;
            Width = scal;
            Height = scal;
            GraphicsComponent.StartAnimation("Ammo");
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new GraphicsComponent(new List<string>() { "Ammo" }, "Ammo");
        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                AppManager.Instance.GameManager.Remove(this);
                //new ScoreText(Pos, ScoreText.scores[random.Next(0, ScoreText.scores.Length)]);
                (gameObject as Player).Bullets=5;
                
                AppManager.Instance.SoundManager.StartSound("reloading", Pos, Pos);
            }
            base.OnCollision(gameObject);
        }
    }
}
