using System;
using SplashKitSDK;
using PongGame.Services;
using PongGame.Components;
using Vector2D = PongGame.ValueObjects.Vector2D;

namespace PongGame.Entities
{
    public class Ball : GameObjectComponent
    {
        private readonly int _windowWidth;
        private readonly int _windowHeight;
        private readonly Random _random = new();
        private float _baseSpeed;
        
        public int Size => (int)Width;

        public Ball(int windowWidth, int windowHeight)
        {
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;

            int startX = windowWidth / 2 + 120;
            int startY = _random.Next(100, windowHeight - 100);
            int size = 10;

            var transform = new TransformComponent(startX, startY, size, size);
            var movement = new MovementComponent(transform, 4f);
            var render = new RenderComponent(transform, Color.White, isCircle: false);

            AddComponent(transform);
            AddComponent(movement);
            AddComponent(render);

            Velocity = new Vector2D(4, 4);
            Speed = Velocity.Magnitude;
            _baseSpeed = Speed;
        }

        public void Bounce(Vector2D normal)
        {
            Vector2D v = Velocity;
            float dot = v.DotProduct(normal);
            Velocity = v.Subtract(normal.Multiply(2 * dot));
        }

        public void SetPosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void ResetPosition()
        {
            X = _windowWidth / 2;
            Y = _random.Next(100, _windowHeight - 100);

            int dir = _random.Next(2) == 0 ? 1 : -1;
            Velocity = new Vector2D(4 * dir, 4 * dir);
            Speed = _baseSpeed;
            NormalizeVelocity();
        }

        public void Accelerate(float ax, float ay)
            => Velocity = Velocity.Add(new Vector2D(ax, ay));

        public void LimitSpeed(float max)
            => Velocity = Velocity.Limit(max);

        public void SetBaseSpeed(float speed)
        {
            _baseSpeed = speed;
            Speed = speed;
            NormalizeVelocity();
        }

        public void ResetSpeed() => Speed = _baseSpeed;

        public void NormalizeVelocity()
        {
            float m = Velocity.Magnitude;
            if (m > 0)
                Velocity = new Vector2D(Velocity.X / m * Speed, Velocity.Y / m * Speed);
        }

        public Rectangle CreateRectangle() 
        {
            return _transform?.CreateRectangle() ?? new Rectangle(); 
        }

        public override void Draw()
        {
            _render?.Update(0);
        }
    }
}