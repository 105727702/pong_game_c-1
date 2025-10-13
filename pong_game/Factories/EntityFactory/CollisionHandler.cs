using SplashKitSDK;
using System.Collections.Generic;
using PongGame.Entities;
using PongGame.Effects;
using PongGame.Services;
using Vector2D = PongGame.Entities.Vector2D; // ✅ Alias to avoid ambiguity

namespace PongGame.Factories
{
    /// <summary>
    /// Collision detection and handling implementation
    /// Converted from static class to instance-based for better OOP design
    /// Supports dependency injection and testability
    /// </summary>
    public class CollisionHandler : ICollisionHandler
    {
        private readonly System.Random _random;
        private readonly Services.SoundManager? _soundManager;
        private readonly PowerUpManager? _powerUpManager;

        public CollisionHandler(Services.SoundManager? soundManager = null, PowerUpManager? powerUpManager = null)
        {
            _random = new System.Random();
            _soundManager = soundManager;
            _powerUpManager = powerUpManager;
        }

        /// <summary>
        /// Check if two rectangles collide (AABB collision)
        /// Algorithm: Compare the position and size of the two rectangles on both the X and Y axes.
        /// </summary>
        public bool CheckCollision(Rectangle rect1, Rectangle rect2)
        {
            return SplashKit.RectanglesIntersect(rect1, rect2);
        }

        /// <summary>
        /// Resolve collision between ball and object
        /// Idea: Calculate the overlap on each axis, then push the ball away from the object in the direction of the least overlap.
        /// Then, reflect the ball's velocity based on the normal vector of the collision surface.
        /// </summary>
        public void ResolveCollision(Ball ball, Rectangle objectBounds, Vector2D normal = default)
        {
            // Use default value for struct (0, 0)
            if (normal.X == 0 && normal.Y == 0)
                normal = new Vector2D(0, 0);

            // Calculate the distance between the centers of the ball and the object
            float dx = (ball.GetX() + ball.GetSize() / 2f) - ((float)objectBounds.X + (float)objectBounds.Width / 2f);
            float dy = (ball.GetY() + ball.GetSize() / 2f) - ((float)objectBounds.Y + (float)objectBounds.Height / 2f);

            // Calculate the overlap on each axis
            float halfBallWidth = ball.GetSize() / 2f;
            float halfBallHeight = ball.GetSize() / 2f;
            float halfObjectWidth = (float)objectBounds.Width / 2f;
            float halfObjectHeight = (float)objectBounds.Height / 2f;

            float overlapX = halfBallWidth + halfObjectWidth - System.Math.Abs(dx);
            float overlapY = halfBallHeight + halfObjectHeight - System.Math.Abs(dy);

            // Push the ball away from the object along the axis with a smaller overlap.
            if (overlapX >= overlapY) // Resolve vertical collision
            {
                if (dy < 0)
                {
                    ball.SetY(ball.GetY() - overlapY); // Reflection along the Y axis
                    normal = new Vector2D(0, 1);  // Immutable: create new vector
                }
                else
                {
                    ball.SetY(ball.GetY() + overlapY);
                    normal = new Vector2D(0, -1);  // Immutable: create new vector
                }
            }
            else // Resolve horizontal collision
            {
                if (dx < 0)
                {
                    ball.SetX(ball.GetX() - overlapX); // Reflection along the X axis
                    normal = new Vector2D(1, 0);  // Immutable: create new vector
                }
                else
                {
                    ball.SetX(ball.GetX() + overlapX);
                    normal = new Vector2D(-1, 0);  // Immutable: create new vector
                }
            }
            ball.Bounce(normal);
        }

        /// <summary>
        /// Handle all collisions in the game
        /// Includes: collision with the window frame, paddle, moving wall
        /// </summary>
        public void HandleCollisions(Ball ball, Paddle leftPaddle, Paddle rightPaddle, 
            List<Wall> walls, int windowWidth, int windowHeight)
        {
            // Collisions with window boundaries
            if (ball.Y <= 0)
            {
                ball.SetY(0);
                ball.Bounce(new Vector2D(0, 1)); // Reflection along the Y axis
                _soundManager?.PlayEffect(Services.SoundType.WallHit);
            }
            else if (ball.Y + ball.Size >= windowHeight)
            {
                ball.SetY(windowHeight - ball.Size);
                ball.Bounce(new Vector2D(0, -1));
                _soundManager?.PlayEffect(Services.SoundType.WallHit);
            }

            // Collisions with paddles
            if (CheckCollision(ball.GetBounds(), leftPaddle.GetBounds()))
            {
                ResolveCollision(ball, leftPaddle.GetBounds(), new Vector2D(1, 0));
                ball.Accelerate(leftPaddle.Speed * 0.1f, 0);
                ball.LimitSpeed(20);
                _soundManager?.PlayEffect(Services.SoundType.PaddleHit);
                
                // 20% chance to spawn power-up when hitting paddle
                if (_powerUpManager != null && _random.NextDouble() < 0.2)
                {
                    _powerUpManager.SpawnRandomPowerUp(_soundManager);
                }
            }
            else if (CheckCollision(ball.GetBounds(), rightPaddle.GetBounds()))
            {
                ResolveCollision(ball, rightPaddle.GetBounds(), new Vector2D(-1, 0));
                ball.Accelerate(-rightPaddle.Speed * 0.1f, 0);
                ball.LimitSpeed(20);
                _soundManager?.PlayEffect(Services.SoundType.PaddleHit);
                
                // 20% chance to spawn power-up when hitting paddle
                if (_powerUpManager != null && _random.NextDouble() < 0.2)
                {
                    _powerUpManager.SpawnRandomPowerUp(_soundManager);
                }
            }

            // Collisions with walls
            foreach (Wall wall in walls)
            {
                wall.Move();
                if (CheckCollision(ball.GetBounds(), wall.GetBounds()))
                {
                    ResolveCollision(ball, wall.GetBounds());
                    _soundManager?.PlayEffect(Services.SoundType.BallHitWall);
                    
                    // 70% chance to spawn 1-2 power-ups when hitting wall
                    if (_powerUpManager != null && _random.NextDouble() < 0.7)
                    {
                        int numPowerUps = _random.Next(1, 3); // 1 or 2 power-ups
                        _powerUpManager.SpawnMultiplePowerUps(numPowerUps, _soundManager);
                    }
                }
            }
        }
    }
}
