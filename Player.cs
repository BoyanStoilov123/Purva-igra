using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


public class Player
{
    private const float _gravity = 9.8f;
    private const float _jumpForce = 350f;

    private float _movementSpeed;
    private Texture2D _texture;

    public Rectangle Collider;
    public Vector2 Position;
    public Vector2 Size;

    public Vector2 Velocity;

    public Player(Vector2 position, Vector2 size)
    {
        Position = position;
        Size = size;

        _movementSpeed = 300;
        Collider = new Rectangle(Position.ToPoint(), Size.ToPoint());
    }

    public void LoadContent(Texture2D texture)
    {
        _texture = texture;
    }

    public void Update(float dt)
    {
        Velocity.Y += _gravity;

        Position.X += Velocity.X * _movementSpeed * dt;
        Position.Y += Velocity.Y * dt;

        Collider.X = (int)Position.X;
        Collider.Y = (int)Position.Y;
    }

    public void Jump()
    {
        Velocity.Y -= _jumpForce;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            _texture,
            new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)Size.X,
                (int)Size.Y),
            Color.Beige);
    }

    public void SetDirection(Vector2 direction)
    {
        Velocity.X = direction.X;
    }
}