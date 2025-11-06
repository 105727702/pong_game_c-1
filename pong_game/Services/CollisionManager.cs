using SplashKitSDK;
using System.Collections.Generic;
using PongGame.Entities;
using Vector2D = PongGame.ValueObjects.Vector2D;

namespace PongGame.Services
{
    public class CollisionHandler
    {
        private readonly System.Random _random = new();
        private readonly SoundManager? _soundManager;
        private readonly PowerUpManager? _powerUpManager;

        public CollisionHandler(SoundManager? soundManager = null, PowerUpManager? powerUpManager = null)
        {
            _soundManager = soundManager;
            _powerUpManager = powerUpManager;
        }

        private void Play(SoundType type) => _soundManager?.PlayEffect(type);

        public bool CheckCollision(Rectangle a, Rectangle b)
            => SplashKit.RectanglesIntersect(a, b);

        public void ResolveCollision(Ball ball, Rectangle obj, Vector2D normal = default)
        {
            normal = (normal.X != 0 || normal.Y != 0) ? normal : new Vector2D(0, 0);

            float ballCenterX = ball.X + ball.Size * 0.5f;
            float ballCenterY = ball.Y + ball.Size * 0.5f;

            float objCenterX = (float)obj.X + (float)obj.Width * 0.5f;
            float objCenterY = (float)obj.Y + (float)obj.Height * 0.5f;

            float dx = ballCenterX - objCenterX;
            float dy = ballCenterY - objCenterY;

            float overlapX = ball.Size * 0.5f + (float)obj.Width * 0.5f - System.Math.Abs(dx);
            float overlapY = ball.Size * 0.5f + (float)obj.Height * 0.5f - System.Math.Abs(dy);


            if (overlapX < overlapY)
            {
                float newX = ball.X + (dx < 0 ? -overlapX : overlapX);
                ball.SetPosition(newX, ball.Y);
                normal = new Vector2D(dx < 0 ? 1 : -1, 0);
            }
            else
            {
                float newY = ball.Y + (dy < 0 ? -overlapY : overlapY);
                ball.SetPosition(ball.X, newY);
                normal = new Vector2D(0, dy < 0 ? 1 : -1);
            }

            ball.Bounce(normal);
        }

        public void HandleCollisions(
            Ball ball, Paddle leftPaddle, Paddle rightPaddle, 
            IReadOnlyList<Wall> walls, int winW, int winH)
        {
            if (ball.Y <= 0)
            {
                ball.SetPosition(ball.X, 0);
                ball.Bounce(new Vector2D(0, 1));
                Play(SoundType.WallHit);
            }
            else if (ball.Y + ball.Size >= winH)
            {
                ball.SetPosition(ball.X, winH - ball.Size);
                ball.Bounce(new Vector2D(0, -1));
                Play(SoundType.WallHit);
            }

            bool spawned = false;

            Rectangle ballRect = ball.CreateRectangle();

            void HandlePaddleHit(Paddle paddle, Vector2D bounceDir)
            {
                ResolveCollision(ball, paddle.CreateRectangle(), bounceDir);
                
                float speedDiff = paddle.Speed - 1.5f;
                
                if (speedDiff > 0)
                {
                    ball.Accelerate(bounceDir.X * speedDiff * 0.15f, 0);
                }
                else
                {
                    ball.Accelerate(bounceDir.X * speedDiff * 0.1f, 0);
                }
                
                ball.LimitSpeed(5);
                Play(SoundType.PaddleHit);

                if (!spawned && _powerUpManager != null && _random.NextDouble() < 0.2)
                {
                    _powerUpManager.SpawnRandomPowerUp(_soundManager);
                    spawned = true;
                }
            }

            if (CheckCollision(ballRect, leftPaddle.CreateRectangle()))
                HandlePaddleHit(leftPaddle, new Vector2D(1, 0));
            else if (CheckCollision(ballRect, rightPaddle.CreateRectangle()))
                HandlePaddleHit(rightPaddle, new Vector2D(-1, 0));

            foreach (var wall in walls)
            {
                wall.Move();
                if (!CheckCollision(ballRect, wall.CreateRectangle())) continue;

                ResolveCollision(ball, wall.CreateRectangle());
                Play(SoundType.BallHitWall);

                if (_powerUpManager != null && _random.NextDouble() < 0.7)
                {
                    int count = _random.Next(1, 3);
                    _powerUpManager.SpawnMultiplePowerUps(count, _soundManager);
                }
            }
        }
    }
}