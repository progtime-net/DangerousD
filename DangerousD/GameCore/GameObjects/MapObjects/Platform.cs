using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DangerousD.GameCore.GameObjects.MapObjects;

public class Platform : MapObject
{
    public override bool IsColliderOn { get; protected set; } = true;
    public Platform(Vector2 position, Vector2 size, Rectangle sourceRectangle) : base(position, size, sourceRectangle)
    {
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        //debug
        //spriteBatch.Draw(debugTexture, new Rectangle(Rectangle.X - GraphicsComponent.CameraPosition.X, Rectangle.Y - GraphicsComponent.CameraPosition.Y, Rectangle.Width, Rectangle.Height), Color.Blue);

    }

}