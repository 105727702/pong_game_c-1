using System;
using System.Collections.Generic;
using PongGame.Entities;
using SplashKitSDK;

namespace PongGame.Factories
{
    /// <summary>
    /// Manages active power-up effects with duration
    /// </summary>
    public class ActiveEffect
    {
        public PowerUpType Type { get; set; }
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }

        public bool IsExpired()
        {
            return (DateTime.Now - StartTime).TotalSeconds >= Duration;
        }

        public double GetRemainingTime()
        {
            return Math.Max(0, Duration - (DateTime.Now - StartTime).TotalSeconds);
        }
    }

    /// <summary>
    /// Manages active effects and their durations
    /// Delegates effect application to EffectFactory (Separation of Concerns)
    /// </summary>
    public class ActiveEffectManager
    {
        private readonly List<ActiveEffect> _activeEffects;
        private readonly Ball _ball;
        private readonly Paddle _leftPaddle;
        private readonly Paddle _rightPaddle;
        private readonly int _originalPaddleHeight;
        private readonly float _originalBallSpeed;
        private readonly EffectFactory _effectFactory;

        public ActiveEffectManager(Ball ball, Paddle leftPaddle, Paddle rightPaddle)
        {
            _activeEffects = new List<ActiveEffect>();
            _ball = ball;
            _leftPaddle = leftPaddle;
            _rightPaddle = rightPaddle;
            _originalPaddleHeight = leftPaddle.Height;
            _originalBallSpeed = ball.Speed;
            _effectFactory = new EffectFactory();
        }

        /// <summary>
        /// Apply a power-up effect with duration
        /// </summary>
        public void ApplyEffect(PowerUpType type, double duration = 5.0)
        {
            // Don't apply the same effect twice
            if (_activeEffects.Exists(e => e.Type == type))
            {
                return;
            }

            var effect = new ActiveEffect
            {
                Type = type,
                StartTime = DateTime.Now,
                Duration = duration
            };

            _activeEffects.Add(effect);
            ActivateEffect(type);
        }

        /// <summary>
        /// Update effects and remove expired ones
        /// </summary>
        public void Update()
        {
            for (int i = _activeEffects.Count - 1; i >= 0; i--)
            {
                if (_activeEffects[i].IsExpired())
                {
                    DeactivateEffect(_activeEffects[i].Type);
                    _activeEffects.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Activate an effect using EffectFactory
        /// </summary>
        private void ActivateEffect(PowerUpType type)
        {
            _effectFactory.ApplyEffect(type, _ball, _leftPaddle, _rightPaddle);
        }

        /// <summary>
        /// Deactivate an effect and restore original state using EffectFactory
        /// </summary>
        private void DeactivateEffect(PowerUpType type)
        {
            // Check if we should reset (no other conflicting effects active)
            bool shouldReset = true;
            
            if (type == PowerUpType.SpeedBoost || type == PowerUpType.SpeedReduction)
            {
                // Only reset if no other speed effects are active
                shouldReset = !_activeEffects.Exists(e => 
                    e.Type != type && 
                    (e.Type == PowerUpType.SpeedBoost || e.Type == PowerUpType.SpeedReduction) &&
                    !e.IsExpired()
                );
            }

            if (shouldReset)
            {
                _effectFactory.RemoveEffect(type, _ball, _leftPaddle, _rightPaddle, _originalPaddleHeight, _originalBallSpeed);
            }
        }

        /// <summary>
        /// Clear all active effects and restore original state using EffectFactory
        /// </summary>
        public void ClearAllEffects()
        {
            _activeEffects.Clear();
            _effectFactory.ResetAllEffects(_ball, _leftPaddle, _rightPaddle, _originalPaddleHeight);
        }

        /// <summary>
        /// Get all active effects
        /// </summary>
        public List<ActiveEffect> GetActiveEffects()
        {
            return new List<ActiveEffect>(_activeEffects);
        }

        /// <summary>
        /// Get count of active effects
        /// </summary>
        public int Count => _activeEffects.Count;
    }
}
