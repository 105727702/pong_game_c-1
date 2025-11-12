using PongGame.Entities;
using SplashKitSDK;
using System;

namespace PongGame.Strategy
{
    public class PowerUp
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public PowerUpType Type { get; private set; }
        private const int Size = 20;
        private readonly Color _color;
        private readonly DateTime _spawnTime;
        private readonly double _lifetime;

        public PowerUp(PowerUpType type, float x, float y, Color color, double lifetime = 10.0)
        {
            Type = type;
            X = x;
            Y = y;
            _color = color;
            _spawnTime = DateTime.Now;
            _lifetime = lifetime;
        }

        public void Draw()
        {
            double timeRemaining = GetRemainingLifetime();
            if (timeRemaining < 5.0)
            {
                double blinkSpeed = timeRemaining < 1.5 ? 5.0 : 3.0;
                byte alpha = (byte)(128 + 127 * Math.Sin(DateTime.Now.Millisecond / 1000.0 * Math.PI * blinkSpeed));
                Color pulseColor = SplashKit.RGBAColor(_color.R, _color.G, _color.B, alpha);
                SplashKit.FillCircle(pulseColor, X, Y, Size);
            }
            else
            {
                SplashKit.FillCircle(_color, X, Y, Size);
            }
            
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
    }
}
