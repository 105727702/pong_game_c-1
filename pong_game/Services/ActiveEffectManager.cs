using System;
using System.Collections.Generic;
using System.Linq;
using PongGame.Entities;
using SplashKitSDK;
using PongGame.Strategy;

namespace PongGame.Services
{
    public class ActiveEffectManager
    {
        private readonly Dictionary<PowerUpType, (DateTime StartTime, double Duration)> _activeEffects;
        private readonly Ball _ball;
        private readonly Paddle _leftPaddle;
        private readonly Paddle _rightPaddle;
        private readonly int _originalPaddleHeight;
        private readonly float _originalBallSpeed;
        private readonly EffectFactory _effectFactory;

        public ActiveEffectManager(Ball ball, Paddle leftPaddle, Paddle rightPaddle)
        {
            _activeEffects = new Dictionary<PowerUpType, (DateTime, double)>();
            _ball = ball;
            _leftPaddle = leftPaddle;
            _rightPaddle = rightPaddle;
            _originalPaddleHeight = leftPaddle.Height;
            _originalBallSpeed = ball.Speed;
            _effectFactory = new EffectFactory();
        }

        public void ApplyEffect(PowerUpType type, double duration = 5.0)
        {
            _activeEffects[type] = (DateTime.Now, duration);
            ActivateEffect(type);
        }

        public void Update()
        {
            var expiredEffects = _activeEffects
                .Where(kvp => IsExpired(kvp.Value.StartTime, kvp.Value.Duration))
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var type in expiredEffects)
            {
                DeactivateEffect(type);
                _activeEffects.Remove(type);
            }
        }

        private bool IsExpired(DateTime startTime, double duration)
        {
            return (DateTime.Now - startTime).TotalSeconds >= duration;
        }

        private void ActivateEffect(PowerUpType type)
        {
            _effectFactory.ApplyEffect(type, _ball, _leftPaddle, _rightPaddle);
        }

        private void DeactivateEffect(PowerUpType type)
        {
            bool shouldReset = true;

            if (shouldReset)
            {
                _effectFactory.RemoveEffect(type, _ball, _leftPaddle, _rightPaddle, _originalPaddleHeight, _originalBallSpeed);
            }
        }

        public void ClearAllEffects()
        {
            _activeEffects.Clear();
            _effectFactory.ResetAllEffects(_ball, _leftPaddle, _rightPaddle, _originalPaddleHeight);
        }

    }
}
