using PongGame.Services;

namespace PongGame.Combine
{
    /// <summary>
    /// Container for all game services - improves GameContext organization
    /// Manages all game services and their interactions
    /// </summary>
    public class GameServices
    {
        public SoundManager? SoundManager { get; set; }
        public PowerUpManager? PowerUpManager { get; set; }
        public ActiveEffectManager? ActiveEffectManager { get; set; }
        public CollisionHandler? CollisionHandler { get; set; }
        public InputHandler? InputHandler { get; set; }

        public GameServices(SoundManager? soundManager = null, 
            PowerUpManager? powerUpManager = null, 
            ActiveEffectManager? activeEffectManager = null,
            CollisionHandler? collisionHandler = null,
            InputHandler? inputHandler = null)
        {
            SoundManager = soundManager;
            PowerUpManager = powerUpManager;
            ActiveEffectManager = activeEffectManager;
            CollisionHandler = collisionHandler;
            InputHandler = inputHandler;
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
