using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.Entities.Items
{
    internal class ScoreText : Entity
    {
        public static int[] scores = new int[] { 100, 600};
        public ScoreText(Vector2 position, int score) : base(position)
        {
            Width = 32;
            Height = 32;
            GraphicsComponent.StartAnimation("score"+ score);
            GraphicsComponent.actionOfAnimationEnd += (a) => { AppManager.Instance.GameManager.Remove(this); };
        }
        public override void Initialize()
        {
        }
        public override void Update(GameTime gameTime)
        {
            _pos.Y -= 1.0f; 
            base.Update(gameTime); 
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new GraphicsComponent(new List<string>() { "score100", "score600" }, "score100");
         
    }
}
