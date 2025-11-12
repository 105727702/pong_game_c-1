using System;
using System.Collections.Generic;
using PongGame.Entities;
using PongGame.Strategy;

namespace PongGame.Services
{
    public class PowerUpManager
    {
        private readonly List<PowerUp> _activePowerUps;
        private readonly Random _random;
        private readonly int _windowWidth;
        private readonly int _windowHeight;
        private readonly PowerUpFactory _powerUpFactory;
        private const int MAX_POWERUPS = 5; 
        private const double POWERUP_LIFETIME = 8.0; 

        public PowerUpManager(int windowWidth, int windowHeight, PowerUpFactory? powerUpFactory = null)
        {
            _activePowerUps = new List<PowerUp>();
            _random = new Random();
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
            _powerUpFactory = powerUpFactory ?? new PowerUpFactory(); 
        }
        public void SpawnRandomPowerUp(SoundManager? soundManager = null)
        {
            float x = _random.Next(200, _windowWidth - 200);
            float y = _random.Next(100, _windowHeight - 100);

            PowerUpType[] types = { PowerUpType.SpeedBoost, PowerUpType.SpeedReduction, PowerUpType.SizeBoost };
            PowerUpType randomType = types[_random.Next(types.Length)];

            PowerUp powerUp = _powerUpFactory.CreatePowerUp(randomType, x, y);
            _activePowerUps.Add(powerUp);

            if (soundManager != null)
            {
                soundManager.PlayEffect(SoundType.PotionEffect);
            }
        }
        public void SpawnMultiplePowerUps(int count, SoundManager? soundManager = null)
        {
            for (int i = 0; i < count; i++)
            {
                if (_activePowerUps.Count >= MAX_POWERUPS)
                {
                    break;
                }
                SpawnRandomPowerUp(soundManager);
            }
        }

        public void Update()
        {
            for (int i = _activePowerUps.Count - 1; i >= 0; i--)
            {
                if (_activePowerUps[i].IsExpired())
                {
                    _activePowerUps.RemoveAt(i);
                }
            }
        }

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

        public void Draw()
        {
            foreach (var powerUp in _activePowerUps)
            {
                powerUp.Draw();
            }
        }

        public void Clear()
        {
            _activePowerUps.Clear();
        }
    }
}
