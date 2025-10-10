using System;
using System.Collections.Generic;
using PongGame.Entities;
using PongGame.Services;

namespace PongGame.Factories
{
    /// <summary>
    /// Manages power-up spawning, collision detection, and effects
    /// Replaces old PotionEffectManager with Factory Pattern
    /// </summary>
    public class PowerUpManager
    {
        private readonly List<IPowerUp> _activePowerUps;
        private readonly Random _random;
        private readonly int _windowWidth;
        private readonly int _windowHeight;
        private const int MAX_POWERUPS = 3; // Maximum number of power-ups on screen
        private const double POWERUP_LIFETIME = 8.0; // Power-ups disappear after 8 seconds

        public PowerUpManager(int windowWidth, int windowHeight)
        {
            _activePowerUps = new List<IPowerUp>();
            _random = new Random();
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
        }

        /// <summary>
        /// Spawn a random power-up at a random location
        /// </summary>
        public void SpawnRandomPowerUp(SoundManager? soundManager = null)
        {
            // Don't spawn if we've reached the maximum
            if (_activePowerUps.Count >= MAX_POWERUPS)
            {
                return;
            }

            // Random position in the middle area of the screen
            float x = _random.Next(200, _windowWidth - 200);
            float y = _random.Next(100, _windowHeight - 100);

            // Random power-up type
            PowerUpType[] types = { PowerUpType.SpeedBoost, PowerUpType.SpeedReduction, PowerUpType.SizeBoost };
            PowerUpType randomType = types[_random.Next(types.Length)];

            IPowerUp powerUp = PowerUpFactory.CreatePowerUp(randomType, x, y);
            _activePowerUps.Add(powerUp);

            soundManager?.PlayEffect(SoundType.PotionEffect);
        }

        /// <summary>
        /// Spawn multiple random power-ups
        /// </summary>
        public void SpawnMultiplePowerUps(int count, SoundManager? soundManager = null)
        {
            for (int i = 0; i < count; i++)
            {
                // Only spawn if we haven't reached the limit
                if (_activePowerUps.Count >= MAX_POWERUPS)
                {
                    break;
                }
                SpawnRandomPowerUp(soundManager);
            }
        }

        /// <summary>
        /// Update power-ups - remove expired ones
        /// </summary>
        public void Update()
        {
            // Remove expired power-ups
            for (int i = _activePowerUps.Count - 1; i >= 0; i--)
            {
                if (_activePowerUps[i].IsExpired())
                {
                    _activePowerUps.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Check collisions between ball and power-ups
        /// Returns the power-up that was collected, or null
        /// </summary>
        public IPowerUp? CheckCollisions(Ball ball)
        {
            for (int i = _activePowerUps.Count - 1; i >= 0; i--)
            {
                if (_activePowerUps[i].IsColliding(ball))
                {
                    IPowerUp collected = _activePowerUps[i];
                    _activePowerUps.RemoveAt(i);
                    return collected;
                }
            }
            return null;
        }

        /// <summary>
        /// Apply power-up effect based on type
        /// </summary>
        public void ApplyPowerUpEffect(IPowerUp powerUp, ActiveEffectManager? activeEffectManager, SoundManager? soundManager = null)
        {
            if (activeEffectManager != null)
            {
                activeEffectManager.ApplyEffect(powerUp.Type, duration: 5.0);
                soundManager?.PlayEffect(SoundType.PotionEffect);
            }
        }

        /// <summary>
        /// Draw all active power-ups
        /// </summary>
        public void Draw()
        {
            foreach (var powerUp in _activePowerUps)
            {
                powerUp.Draw();
            }
        }

        /// <summary>
        /// Clear all active power-ups
        /// </summary>
        public void Clear()
        {
            _activePowerUps.Clear();
        }

        /// <summary>
        /// Get count of active power-ups
        /// </summary>
        public int Count => _activePowerUps.Count;

        /// <summary>
        /// Get maximum allowed power-ups
        /// </summary>
        public int MaxPowerUps => MAX_POWERUPS;
    }
}
