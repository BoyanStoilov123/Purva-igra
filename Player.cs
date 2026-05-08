using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

public class Player
{
    private const float _gravity = 9.8f;
    private const float _jumpForce = 300f;
    
    
    private float _movementSpeed;
    
    public Vector2 Position;
    public Vector2 Size;

    public Vector2 Velocity;

    public Player(Vector2 position, Vector2 size)
    {
        Position = position;
        Size = size;

        _movementSpeed = 300;
    }

    public void Update(float dt)
    {
        Velocity.Y += _gravity;

        Position.X += Velocity.X * _movementSpeed * dt;
        Position.Y += Velocity.Y * dt;
    }

    
    public void Jump()
    {
        Velocity.Y -= _jumpForce;
    }
    
    public void Draw()
    {
    }
    
    public void SetDirection(Vector2 direction)
    {
        Velocity.X = direction.X;
    }
}

