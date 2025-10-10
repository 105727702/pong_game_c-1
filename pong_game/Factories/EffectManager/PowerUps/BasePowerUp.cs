using PongGame.Entities;
using PongGame.Decorators;
using SplashKitSDK;
using System;

namespace PongGame.Factories
{
    /// <summary>
    /// Base power-up implementation
    /// </summary>
    public abstract class BasePowerUp : IPowerUp
    {
        public float X { get; protected set; }
        public float Y { get; protected set; }
        public abstract PowerUpType Type { get; }
        protected const int Size = 20;
        protected Color _color;
        private readonly DateTime _spawnTime;
        private readonly double _lifetime;

        protected BasePowerUp(float x, float y, Color color, double lifetime = 10.0)
        {
            X = x;
            Y = y;
            _color = color;
            _spawnTime = DateTime.Now;
            _lifetime = lifetime;
        }

        public virtual void Draw()
        {
            // Add pulsing effect based on remaining lifetime
            double timeRemaining = GetRemainingLifetime();
            if (timeRemaining < 3.0)
            {
                // Blink faster when about to expire
                double blinkSpeed = timeRemaining < 1.5 ? 5.0 : 3.0;
                byte alpha = (byte)(128 + 127 * Math.Sin(DateTime.Now.Millisecond / 1000.0 * Math.PI * blinkSpeed));
                Color pulseColor = SplashKit.RGBAColor(_color.R, _color.G, _color.B, alpha);
                SplashKit.FillCircle(pulseColor, X, Y, Size);
            }
            else
            {
                SplashKit.FillCircle(_color, X, Y, Size);
            }
            
            // Draw outline
            SplashKit.DrawCircle(Color.White, X, Y, Size);
        }

        public bool IsColliding(Ball ball)
        {
            float dx = ball.X - X;
            float dy = ball.Y - Y;
            float distance = (float)System.Math.Sqrt(dx * dx + dy * dy);
            return distance < (ball.Size / 2 + Size);
        }

        public bool IsExpired()
        {
            return (DateTime.Now - _spawnTime).TotalSeconds >= _lifetime;
        }

        public double GetRemainingLifetime()
        {
            double elapsed = (DateTime.Now - _spawnTime).TotalSeconds;
            return Math.Max(0, _lifetime - elapsed);
        }

        public abstract void Apply(IGameEntity entity);
    }
}
