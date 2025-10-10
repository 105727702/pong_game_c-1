using SplashKitSDK;
using System.Collections.Generic;
using System;
using PongGame.Components;

namespace PongGame.Entities
{
    /// <summary>
    /// Wall entity using Component Pattern
    /// Inherits from GameObject for consistent entity structure
    /// </summary>
    public class Wall : GameObject
    {
        public const int WALL_WIDTH = 10;
        public const int WALL_HEIGHT = 100;

        // Components
        private TransformComponent _transform;
        private MovementComponent _movement;
        private RenderComponent _render;

        // Wall-specific properties
        private readonly int _windowHeight;
        private readonly Random _random = new Random();

        // Public properties for external access
        public float X 
        { 
            get => _transform.X;
            set => _transform.X = value;
        }
        public float Y 
        { 
            get => _transform.Y;
            set => _transform.Y = value;
        }
        public int Width => WALL_WIDTH;
        public int Height => WALL_HEIGHT;
        public Color Color 
        { 
            get => _render.Color;
            set => _render.Color = value;
        }
        public float YSpeed 
        { 
            get => _movement.Velocity.Y;
            set => _movement.SetVelocity(0, value);
        }

        public Wall(float x, float y, int windowHeight, float speedMultiplier = 1.0f)
        {
            _windowHeight = windowHeight;
            
            // Initialize components
            _transform = new TransformComponent(x, y, WALL_WIDTH, WALL_HEIGHT);
            _movement = new MovementComponent(_transform, 0);
            _render = new RenderComponent(_transform, Color.Gray, isCircle: false);
            
            // Add components to GameObject
            AddComponent(_transform);
            AddComponent(_movement);
            AddComponent(_render);
            
            // Set initial speed
            float baseSpeed = _random.Next(1, 2) * speedMultiplier;
            float speed = Math.Max(baseSpeed, 2.0f);
            _movement.SetVelocity(0, speed);
        }

        public void Move()
        {
            // Update position via component
            base.Update(0);
            
            // Bounce off boundaries
            if (_transform.Y <= 0 || _transform.Y + Height >= _windowHeight)
            {
                var currentVelocity = _movement.Velocity;
                _movement.SetVelocity(0, -currentVelocity.Y);
            }
        }

        /// <summary>
        /// Check if the new wall position is valid (doesn't overlap with existing walls)
        /// </summary>
        public static bool IsValidPosition(float newX, float newY, List<Wall> existingWalls, int minDistance)
        {
            foreach (Wall wall in existingWalls)
            {
                if (newX < wall.X + wall.Width + minDistance &&
                    newX + WALL_WIDTH + minDistance > wall.X &&
                    newY < wall.Y + wall.Height + minDistance &&
                    newY + WALL_HEIGHT + minDistance > wall.Y)
                {
                    return false; // Overlapping or too close
                }
            }
            return true; // Valid position
        }

        /// <summary>
        /// Get the rectangle bounds of the wall for collision detection
        /// </summary>
        public Rectangle GetBounds()
        {
            return _transform.GetBounds();
        }

        /// <summary>
        /// Draw the wall - overrides GameObject.Draw()
        /// </summary>
        public override void Draw()
        {
            _render.Draw();
        }
    }
}
