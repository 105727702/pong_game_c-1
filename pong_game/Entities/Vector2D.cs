using System;

namespace PongGame.Entities
{
    /// <summary>
    /// Immutable readonly struct for 2D vectors
    /// Follows value semantics - all operations return new instances
    /// Thread-safe and prevents accidental mutations
    /// </summary>
    public readonly struct Vector2D
    {
        public float X { get; init; }
        public float Y { get; init; }

        /// <summary>
        /// Initialize the vector with x and y coordinates
        /// </summary>
        public Vector2D(float x = 0, float y = 0)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Add another vector and return a new vector (immutable operation)
        /// </summary>
        public Vector2D Add(Vector2D other)
        {
            return new Vector2D(X + other.X, Y + other.Y);
        }

        /// <summary>
        /// Subtract another vector and return a new vector (immutable operation)
        /// </summary>
        public Vector2D Subtract(Vector2D other)
        {
            return new Vector2D(X - other.X, Y - other.Y);
        }

        /// <summary>
        /// Multiply by a scalar and return a new vector (immutable operation)
        /// </summary>
        public Vector2D Multiply(float scalar)
        {
            return new Vector2D(X * scalar, Y * scalar);
        }

        /// <summary>
        /// Calculate the magnitude (length) of the vector
        /// </summary>
        public float Magnitude => (float)Math.Sqrt(X * X + Y * Y);

        /// <summary>
        /// Normalize the vector to unit length and return a new vector (immutable operation)
        /// </summary>
        public Vector2D Normalize()
        {
            float mag = Magnitude;
            if (mag > 0)
            {
                return new Vector2D(X / mag, Y / mag);
            }
            return this;
        }

        /// <summary>
        /// Limit the magnitude to a maximum value and return a new vector (immutable operation)
        /// </summary>
        public Vector2D Limit(float max)
        {
            if (Magnitude > max)
            {
                return Normalize().Multiply(max);
            }
            return this;
        }

        /// <summary>
        /// Calculate the dot product with another vector
        /// </summary>
        public float DotProduct(Vector2D other)
        {
            return X * other.X + Y * other.Y;
        }

        /// <summary>
        /// Create a copy of this vector (unnecessary for structs, but kept for API compatibility)
        /// </summary>
        public Vector2D Copy()
        {
            return new Vector2D(X, Y);
        }

        /// <summary>
        /// Operator overloads for natural vector math syntax
        /// </summary>
        public static Vector2D operator +(Vector2D a, Vector2D b) => a.Add(b);
        public static Vector2D operator -(Vector2D a, Vector2D b) => a.Subtract(b);
        public static Vector2D operator *(Vector2D v, float scalar) => v.Multiply(scalar);
        public static Vector2D operator *(float scalar, Vector2D v) => v.Multiply(scalar);
    }
}
