using System;
using System.Collections.Generic;
using PongGame.Entities;
using PongGame.Decorator;

namespace PongGame.Services
{
    /// <summary>
    /// Manages power-up spawning, collision detection, and effects
    /// Replaces old PotionEffectManager with Factory Pattern
    /// Uses dependency injection for PowerUpFactory
    /// </summary>
    public class PowerUpManager
    {
        private readonly List<PowerUp> _activePowerUps;
        private readonly Random _random;
        private readonly int _windowWidth;
        private readonly int _windowHeight;
        private readonly PowerUpFactory _powerUpFactory;
        private const int MAX_POWERUPS = 5; // Maximum number of power-ups on screen
        private const double POWERUP_LIFETIME = 8.0; // Power-ups disappear after 8 seconds

        public PowerUpManager(int windowWidth, int windowHeight, PowerUpFactory? powerUpFactory = null)
        {
            _activePowerUps = new List<PowerUp>();
            _random = new Random();
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
            _powerUpFactory = powerUpFactory ?? new PowerUpFactory(); // Default if not provided
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

            PowerUp powerUp = _powerUpFactory.CreatePowerUp(randomType, x, y);
            _activePowerUps.Add(powerUp);

            if (soundManager != null)
            {
                soundManager.PlayEffect(SoundType.PotionEffect);
            }
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
        public PowerUp? CheckCollisions(Ball ball)
        {
            for (int i = _activePowerUps.Count - 1; i >= 0; i--)
            {
                if (_activePowerUps[i].IsColliding(ball))
                {
                    PowerUp? collected = _activePowerUps[i];
                    _activePowerUps.RemoveAt(i);
                    return collected;
                }
            }
            return null;
        }

        /// <summary>
        /// Apply power-up effect based on type
        /// </summary>
        public void ApplyPowerUpEffect(PowerUp powerUp, ActiveEffectManager? activeEffectManager, SoundManager? soundManager = null)
        {
            if (activeEffectManager != null)
            {
                activeEffectManager.ApplyEffect(powerUp.Type, duration: 5.0);
                if (soundManager != null)
                {
                    soundManager.PlayEffect(SoundType.PotionEffect);
                }
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
