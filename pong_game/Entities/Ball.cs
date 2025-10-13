using System;
using SplashKitSDK;
using PongGame.Services;
using PongGame.Components;

namespace PongGame.Entities
{
    /// <summary>
    /// Ball entity using Component Pattern
    /// Inherits from GameObject
    /// </summary>
    public class Ball : GameObject
    {
        // Components
        private TransformComponent _transform;
        private MovementComponent _movement;
        private RenderComponent _render;

        // Ball-specific properties
        private readonly int _windowWidth;
        private readonly int _windowHeight;
        private readonly Random _random = new Random();
        private float _baseSpeed;
        private Vector2D _velocity;

        // Public properties for external access
        public float X { get => _transform.X; }
        public float Y { get => _transform.Y; }
        public int Size { get => (int)_transform.Width; }
        public Color Color 
        { 
            get => _render.Color;
            set => _render.Color = value;
        }
        public Vector2D Velocity { get => _velocity; }
        public float Speed { get; private set; }

        // Backward compatibility methods
        public float GetX() => _transform.X;
        public float GetY() => _transform.Y;
        public int GetSize() => (int)_transform.Width;
        public Color GetColor() => _render.Color;
        public Vector2D GetVelocity() => _velocity;
        public float GetSpeed() => Speed;
        
        // Controlled setters
        public void SetX(float x) => _transform.X = x;
        public void SetY(float y) => _transform.Y = y;
        public void SetSpeed(float speed) => Speed = speed;
        public void SetColor(Color color) => _render.Color = color;

        public Ball(int windowWidth, int windowHeight)
        {
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
            
            // Calculate initial position
            int trueCenterX = windowWidth / 2 + 120;
            int randomY = _random.Next(100, windowHeight - 100);
            int size = 10;
            
            // Initialize components
            _transform = new TransformComponent(trueCenterX, randomY, size, size);
            _movement = new MovementComponent(_transform, 4f);
            _render = new RenderComponent(_transform, Color.White, isCircle: false);
            
            // Add components to GameObject
            AddComponent(_transform);
            AddComponent(_movement);
            AddComponent(_render);
            
            // Initialize velocity
            _velocity = new Vector2D(4, 4);
            Speed = _velocity.Magnitude;
            _baseSpeed = Speed;
        }

        public void Move()
        {
            _transform.X += _velocity.X;
            _transform.Y += _velocity.Y;
        }

        public void Bounce(Vector2D surfaceNormal)
        {
            Vector2D incidentDirection = _velocity.Copy();
            float dotProduct = incidentDirection.DotProduct(surfaceNormal);
            Vector2D reflection = surfaceNormal.Copy().Multiply(2 * dotProduct);
            _velocity.Subtract(reflection);
        }

        public void ResetPosition()
        {
            int trueCenterX = _windowWidth / 2;
            _transform.X = trueCenterX;
            _transform.Y = _random.Next(100, _windowHeight - 100);
            
            int direction = _random.Next(0, 2) == 0 ? 1 : -1;
            _velocity = new Vector2D(4 * direction, 4 * direction);
            Speed = _baseSpeed;
            NormalizeVelocity();
        }

        public void Accelerate(float ax, float ay)
        {
            _velocity.Add(new Vector2D(ax, ay));
        }

        /// <summary>
        /// Limit the speed of the ball to a maximum value
        /// </summary>
        public void LimitSpeed(float maxSpeed)
        {
            _velocity.Limit(maxSpeed);
        }

        /// <summary>
        /// Set the ball's base speed (used for difficulty settings)
        /// </summary>
        public void SetBaseSpeed(float speed)
        {
            _baseSpeed = speed;
            Speed = speed;
            NormalizeVelocity();
        }

        /// <summary>
        /// Reset speed to base speed (used when effects expire)
        /// </summary>
        public void ResetSpeed()
        {
            Speed = _baseSpeed;
        }

        /// <summary>
        /// Normalize velocity to maintain current speed
        /// </summary>
        public void NormalizeVelocity()
        {
            float magnitude = _velocity.Magnitude;
            if (magnitude > 0)
            {
                _velocity.X = (_velocity.X / magnitude) * Speed;
                _velocity.Y = (_velocity.Y / magnitude) * Speed;
            }
        }

        /// <summary>
        /// Get the rectangle bounds of the ball for collision detection
        /// </summary>
        public Rectangle GetBounds()
        {
            return _transform.GetBounds();
        }

        /// <summary>
        /// Draw the ball - overrides GameObject.Draw()
        /// </summary>
        public override void Draw()
        {
            _render.Draw();
        }
    }
}

