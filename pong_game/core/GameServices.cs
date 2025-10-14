using PongGame.Services;
using PongGame.Decorator;

namespace PongGame.Core
{
    /// <summary>
    /// Container for all game services - improves GameContext organization
    /// </summary>
    public class GameServices
    {
        public SoundManager? SoundManager { get; set; }
        public PowerUpManager? PowerUpManager { get; set; }
        public ActiveEffectManager? ActiveEffectManager { get; set; }

        public GameServices(SoundManager? soundManager = null, 
            PowerUpManager? powerUpManager = null, 
            ActiveEffectManager? activeEffectManager = null)
        {
            SoundManager = soundManager;
            PowerUpManager = powerUpManager;
            ActiveEffectManager = activeEffectManager;
        }

        /// <summary>
        /// Clear all active effects and power-ups
        /// </summary>
        public void ClearAll()
        {
            if (PowerUpManager != null)
            {
                PowerUpManager.Clear();
            }
            if (ActiveEffectManager != null)
            {
                ActiveEffectManager.ClearAllEffects();
            }
        }
    }
}
