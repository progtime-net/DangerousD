using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;



namespace DangerousD.GameCore.GameObjects.Entities.Items
{
    internal class Ammo : Entity
    {
        Random random = new Random();
        int scal; //scale of an object
        double del = 0; //time from creation
        public Ammo(Vector2 position) : base(position)
        {
            scal = 10;
            Width = scal;
            Height = scal;
            GraphicsComponent.StartAnimation("Ammo");
        }
        Vector2 targetPosition;
        public override void SetPosition(Vector2 position)
        {
            targetPosition = position;
            base.SetPosition(position);
        }
        public override void Update(GameTime gameTime)
        {
            del = Math.Min(1, del + gameTime.ElapsedGameTime.TotalSeconds);//find dt of scale animation
            Width = (int)(scal * easeOutElastic(del / 1)); //apply easeOut
            Height = (int)(scal * easeOutElastic(del / 1)); //apply easeOut
            _pos = targetPosition + new Vector2(scal / 2, scal) - new Vector2(Width / 2, Height); // set position to draw correctly
            base.Update(gameTime);
        }
        public double easeOutElastic(double x)
        {

            double c1 = 1.70158;
            double c2 = c1 * 1.525;
            double c4 = (2 * Math.PI) / 3;
            return 0.5 + 2 * (Math.Pow(2, -10 * x) * Math.Sin((x * 10 - 0.75) * c4) + 1);
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new GraphicsComponent(new List<string>() { "Ammo" }, "Ammo");
        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                AppManager.Instance.GameManager.Remove(this);
                //new ScoreText(Pos, ScoreText.scores[random.Next(0, ScoreText.scores.Length)]);
                (gameObject as Player).Bullets = 5;

                AppManager.Instance.SoundManager.StartSound("reloading", Pos, Pos);
            }
            base.OnCollision(gameObject);
        }
    }
}
