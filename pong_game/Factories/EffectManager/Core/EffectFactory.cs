using System;
using PongGame.Entities;
using SplashKitSDK;

namespace PongGame.Factories
{
    /// <summary>
    /// Factory Pattern - Creates and applies power-up effects
    /// Separates effect creation logic from effect management
    /// </summary>
    public class EffectFactory
    {
        /// <summary>
        /// Create and apply effect to game entities
        /// </summary>
        public void ApplyEffect(PowerUpType type, Ball ball, Paddle leftPaddle, Paddle rightPaddle)
        {
            switch (type)
            {
                case PowerUpType.SpeedBoost:
                    ApplySpeedBoost(ball);
                    break;

                case PowerUpType.SpeedReduction:
                    ApplySpeedReduction(ball);
                    break;

                case PowerUpType.SizeBoost:
                    ApplySizeBoost(leftPaddle, rightPaddle);
                    break;
            }
        }

        /// <summary>
        /// Remove effect from game entities
        /// </summary>
        public void RemoveEffect(PowerUpType type, Ball ball, Paddle leftPaddle, Paddle rightPaddle, int originalPaddleHeight, float originalBallSpeed)
        {
            switch (type)
            {
                case PowerUpType.SpeedBoost:
                case PowerUpType.SpeedReduction:
                    ResetSpeed(ball);
                    break;

                case PowerUpType.SizeBoost:
                    ResetSize(leftPaddle, rightPaddle, originalPaddleHeight);
                    break;
            }
        }

        /// <summary>
        /// Reset all effects on game entities
        /// </summary>
        public void ResetAllEffects(Ball ball, Paddle leftPaddle, Paddle rightPaddle, int originalPaddleHeight)
        {
            // Reset ball
            ball.ResetSpeed();
            ball.NormalizeVelocity();
            ball.SetColor(Color.White);

            // Reset paddles
            leftPaddle.Height = originalPaddleHeight;
            rightPaddle.Height = originalPaddleHeight;
            leftPaddle.Color = Color.White;
            rightPaddle.Color = Color.White;
        }

        #region Private Effect Application Methods

        private void ApplySpeedBoost(Ball ball)
        {
            ball.SetSpeed(ball.Speed + 3f);
            ball.NormalizeVelocity();
            ball.SetColor(Color.Yellow);
        }

        private void ApplySpeedReduction(Ball ball)
        {
            ball.SetSpeed(Math.Max(3f, ball.Speed - 3f));
            ball.NormalizeVelocity();
            ball.SetColor(Color.Blue);
        }

        private void ApplySizeBoost(Paddle leftPaddle, Paddle rightPaddle)
        {
            leftPaddle.Height = 150;
            rightPaddle.Height = 150;
            leftPaddle.Color = Color.Green;
            rightPaddle.Color = Color.Green;
        }

        private void ResetSpeed(Ball ball)
        {
            ball.ResetSpeed();
            ball.NormalizeVelocity();
            ball.SetColor(Color.White);
        }

        private void ResetSize(Paddle leftPaddle, Paddle rightPaddle, int originalHeight)
        {
            leftPaddle.Height = originalHeight;
            rightPaddle.Height = originalHeight;
            leftPaddle.Color = Color.White;
            rightPaddle.Color = Color.White;
        }

        #endregion
    }
}
