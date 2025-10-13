using System;
using System.Collections.Generic;
using PongGame.Entities;
using SplashKitSDK;

namespace PongGame.Effects
{
    /// <summary>
    /// Strategy Pattern - Factory that creates and manages effect strategies
    /// Uses polymorphism to replace switch statements with strategy objects
    /// Follows Open/Closed Principle - open for extension, closed for modification
    /// </summary>
    public class EffectFactory
    {
        private readonly Dictionary<PowerUpType, IEffect> _effectStrategies;

        public EffectFactory()
        {
            // Initialize strategy map - easy to extend with new effects
            _effectStrategies = new Dictionary<PowerUpType, IEffect>
            {
                { PowerUpType.SpeedBoost, new SpeedBoostEffect() },
                { PowerUpType.SpeedReduction, new SpeedReductionEffect() },
                { PowerUpType.SizeBoost, new SizeBoostEffect() }
            };
        }

        /// <summary>
        /// Get effect strategy for the given power-up type
        /// </summary>
        public IEffect GetEffect(PowerUpType type)
        {
            if (_effectStrategies.TryGetValue(type, out var effect))
            {
                return effect;
            }

            throw new ArgumentException($"Unknown power-up type: {type}");
        }

        /// <summary>
        /// Apply effect using Strategy Pattern - delegates to appropriate strategy
        /// </summary>
        public void ApplyEffect(PowerUpType type, Ball ball, Paddle leftPaddle, Paddle rightPaddle)
        {
            var effect = GetEffect(type);
            effect.Apply(ball, leftPaddle, rightPaddle);
        }

        /// <summary>
        /// Remove effect using Strategy Pattern - delegates to appropriate strategy
        /// </summary>
        public void RemoveEffect(PowerUpType type, Ball ball, Paddle leftPaddle, Paddle rightPaddle, int originalPaddleHeight, float originalBallSpeed)
        {
            var effect = GetEffect(type);
            effect.Remove(ball, leftPaddle, rightPaddle, originalPaddleHeight);
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
    }
}
