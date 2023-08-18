using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

namespace DangerousD.GameCore.GameObjects.Entities.Items;

public class Diamond : Entity
{
    Random random = new Random();
    public Diamond(Vector2 position) : base(position)
    {
        int scal = random.Next(3, 12);
        Width = scal;
        Height = scal;
        GraphicsComponent.StartAnimation("Diamond");
    }

    protected override GraphicsComponent GraphicsComponent { get; } = new GraphicsComponent(new List<string>() { "Diamond" }, "Diamond");
    public override void OnCollision(GameObject gameObject)
    {
        if (gameObject is Player)
        {
            AppManager.Instance.GameManager.Remove(this);
            new ScoreText(Pos, ScoreText.scores[random.Next(0, ScoreText.scores.Length)]);
            AppManager.Instance.SoundManager.StartSound("collected_coins", Pos, Pos);
        }
        base.OnCollision(gameObject);   
    }
}