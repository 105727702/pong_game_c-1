using SplashKitSDK;
using PongGame.Entities;

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

        public Rectangle GetBounds()
        {
            return SplashKit.RectangleFrom(X, Y, Width, Height);
        }

        public void Update(float deltaTime)
        {
            // Transform doesn't need to update by default
        }
    }

    /// <summary>
    /// Movement component - handles velocity and movement
    /// </summary>
    public class MovementComponent : IComponent
    {
        private readonly TransformComponent _transform;
        public PongGame.Entities.Vector2D Velocity { get; set; }
        public float Speed { get; set; }

        public MovementComponent(TransformComponent transform, float speed)
        {
            _transform = transform;
            Velocity = new PongGame.Entities.Vector2D(0, 0);
            Speed = speed;
        }

        public void Update(float deltaTime)
        {
            _transform.X += Velocity.X;
            _transform.Y += Velocity.Y;
        }

        public void SetVelocity(float x, float y)
        {
            Velocity = new PongGame.Entities.Vector2D(x, y);
        }
    }

    /// <summary>
    /// Render component - handles visual rendering
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
            // Render doesn't need update
        }

        public void Draw()
        {
            if (IsCircle)
            {
                SplashKit.FillCircle(Color, _transform.X + _transform.Width / 2, 
                    _transform.Y + _transform.Height / 2, _transform.Width / 2);
            }
            else
            {
                SplashKit.FillRectangle(Color, _transform.X, _transform.Y, 
                    _transform.Width, _transform.Height);
            }
        }
    }

    /// <summary>
    /// Collision component - handles collision detection
    /// </summary>
    public class CollisionComponent : IComponent
    {
        private readonly TransformComponent _transform;
        public bool IsSolid { get; set; }

        public CollisionComponent(TransformComponent transform, bool isSolid = true)
        {
            _transform = transform;
            IsSolid = isSolid;
        }

        public void Update(float deltaTime)
        {
            // Collision detection is handled externally
        }

        public Rectangle GetBounds()
        {
            return _transform.GetBounds();
        }

        public bool CheckCollision(CollisionComponent other)
        {
            return SplashKit.RectanglesIntersect(GetBounds(), other.GetBounds());
        }
    }
}
