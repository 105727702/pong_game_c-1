using SplashKitSDK;
using Vector2D = PongGame.ValueObjects.Vector2D;
using PongGame.Services;

namespace PongGame.Components
{
    /// <summary>
    /// Transform component - handles position and size
    /// </summary>
    public class TransformComponent : IComponent
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public TransformComponent(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle CreateRectangle()
        {
            return SplashKit.RectangleFrom(X, Y, Width, Height);
        }

        public void Update(float deltaTime)
        {
        }
    }

    /// <summary>
    /// Movement component - handles velocity and movement
    /// </summary>
    public class MovementComponent : IComponent
    {
        private readonly TransformComponent _transform;
        public Vector2D Velocity { get; set; }
        public float Speed { get; set; }

        public MovementComponent(TransformComponent transform, float speed)
        {
            _transform = transform;
            Velocity = new Vector2D(0, 0);
            Speed = speed;
        }

        public void Update(float deltaTime)
        {
            _transform.X += Velocity.X;
            _transform.Y += Velocity.Y;
        }

        public void SetVelocity(float x, float y)
        {
            Velocity = new Vector2D(x, y);
        }
    }

    /// <summary>
    /// Render component - handles visual rendering
    /// Uses IRenderer abstraction to decouple from SplashKit (Dependency Inversion)
    /// </summary>
    public class RenderComponent : IComponent
    {
        private readonly TransformComponent _transform;
        public Color Color { get; set; }
        public bool IsCircle { get; set; }

        public RenderComponent(TransformComponent transform, Color color, bool isCircle = false)
        {
            _transform = transform;
            Color = color;
            IsCircle = isCircle;
        }

        public void Update(float deltaTime)
        {
            if (IsCircle)
            {
                SplashKit.DrawCircle(Color,
                    _transform.X + _transform.Width / 2,
                    _transform.Y + _transform.Height / 2,
                    _transform.Width / 2);
                SplashKit.FillCircle(Color,
                    _transform.X + _transform.Width / 2,
                    _transform.Y + _transform.Height / 2,
                    _transform.Width / 2);
            }
            else
            {
                SplashKit.DrawRectangle(Color,
                    _transform.X, _transform.Y,
                    _transform.Width, _transform.Height);
                SplashKit.FillRectangle(Color,
                    _transform.X, _transform.Y,
                    _transform.Width, _transform.Height);
            }
        }
    }
}
