using SplashKitSDK;
using System.Collections.Generic;
using PongGame.Entities;
using Vector2D = PongGame.Entities.Vector2D; // ✅ Alias to avoid ambiguity

namespace PongGame.Factories
{
    /// <summary>
    /// Interface for collision detection and handling
    /// Follows Dependency Inversion Principle - depend on abstraction not concrete implementation
    /// </summary>
    public interface ICollisionHandler
    {
        /// <summary>
        /// Check if two rectangles collide (AABB collision)
        /// </summary>
        bool CheckCollision(Rectangle rect1, Rectangle rect2);

        /// <summary>
        /// Resolve collision between ball and object
        /// </summary>
        void ResolveCollision(Ball ball, Rectangle objectBounds, Vector2D normal = default);

        /// <summary>
        /// Handle all collisions in the game
        /// </summary>
        void HandleCollisions(Ball ball, Paddle leftPaddle, Paddle rightPaddle, 
            List<Wall> walls, int windowWidth, int windowHeight);
    }
}
