using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

public class Player
{
    private const float _gravity = 9.8f;
    
    private float _movementSpeed;
    
    public Vector2 Position;
    public Vector2 Size;

    private Vector2 _velocity;

    public Player(Vector2 position, Vector2 size)
    {
        Position = position;
        Size = size;

        _movementSpeed = 300;
    }

    public void Update(float dt)
    {
        _velocity.Y += _gravity;

        Position.X += _velocity.X * _movementSpeed * dt;
        Position.Y += _velocity.Y * _gravity * dt;
    }

    public void Draw()
    {
    }
    
    public void SetDirection(Vector2 direction)
    {
        _velocity.X += direction.X;
    }
}

