using System;
using System.Collections.Generic;
using PongGame.Entities;
using SplashKitSDK;

namespace PongGame.Strategy
{
    public class EffectFactory
    {
        private readonly Dictionary<PowerUpType, IEffect> _effectStrategies;

        public EffectFactory()
        {
            _effectStrategies = new Dictionary<PowerUpType, IEffect>
            {
                { PowerUpType.SpeedBoost, new SpeedBoostEffect() },
                { PowerUpType.SpeedReduction, new SpeedReductionEffect() },
                { PowerUpType.SizeBoost, new SizeBoostEffect() }
            };
        }

        public IEffect GetEffect(PowerUpType type)
        {
            if (_effectStrategies.TryGetValue(type, out var effect))
            {
                return effect;
            }

            throw new ArgumentException($"Unknown power-up type: {type}");
        }

        public void ApplyEffect(PowerUpType type, Ball ball, Paddle leftPaddle, Paddle rightPaddle)
        {
            var effect = GetEffect(type);
            effect.Apply(ball, leftPaddle, rightPaddle);
        }

        public void ResetAllEffects(Ball ball, Paddle leftPaddle, Paddle rightPaddle, int originalPaddleHeight)
        {
            ball.ResetSpeed();
            ball.NormalizeVelocity();
            ball.Color = Color.White;

            leftPaddle.Height = originalPaddleHeight;
            rightPaddle.Height = originalPaddleHeight;
            leftPaddle.Color = Color.White;
            rightPaddle.Color = Color.White;
        }
    }
}
