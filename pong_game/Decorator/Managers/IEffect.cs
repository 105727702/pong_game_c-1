using PongGame.Entities;

namespace PongGame.Decorator
{
    /// <summary>
    /// Strategy Pattern - Interface for power-up effects
    /// Allows different effect behaviors to be applied and removed polymorphically
    /// </summary>
    public interface IEffect
    {
        /// <summary>
        /// Apply the effect to game entities
        /// </summary>
        void Apply(Ball ball, Paddle leftPaddle, Paddle rightPaddle);

        /// <summary>
        /// Remove the effect from game entities
        /// </summary>
        void Remove(Ball ball, Paddle leftPaddle, Paddle rightPaddle, int originalPaddleHeight);
    }
}
