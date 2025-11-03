using System;

namespace PongGame.ValueObjects
{
    public readonly struct Vector2D
    {
        public float X { get; init; }
        public float Y { get; init; }
        public Vector2D(float x = 0, float y = 0)
        {
            X = x;
            Y = y;
        }
        public Vector2D Add(Vector2D other)
        {
            return new Vector2D(X + other.X, Y + other.Y);
        }
        public Vector2D Subtract(Vector2D other)
        {
            return new Vector2D(X - other.X, Y - other.Y);
        }

        public Vector2D Multiply(float scalar)
        {
            return new Vector2D(X * scalar, Y * scalar);
        }
        public float Magnitude => (float)Math.Sqrt(X * X + Y * Y);

        public Vector2D Normalize()
        {
            float mag = Magnitude;
            if (mag > 0)
            {
                return new Vector2D(X / mag, Y / mag);
            }
            return this;
        }
        public Vector2D Limit(float max)
        {
            if (Magnitude > max)
            {
                return Normalize().Multiply(max);
            }
            return this;
        }

        public float DotProduct(Vector2D other)
        {
            return X * other.X + Y * other.Y;
        }
        public Vector2D Copy()
        {
            return new Vector2D(X, Y);
        }
    }
}
