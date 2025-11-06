using PongGame.Services;

namespace PongGame.Combine
{
    public class GameServices
    {
        public SoundManager? SoundManager { get; private set; }
        public PowerUpManager? PowerUpManager { get; private set; }
        public ActiveEffectManager? ActiveEffectManager { get; private set; }
        public CollisionHandler? CollisionHandler { get; set; }
        public InputHandler? InputHandler { get; set; }

        public GameServices(
            SoundManager? soundManager = null,
            PowerUpManager? powerUpManager = null,
            ActiveEffectManager? activeEffectManager = null,
            CollisionHandler? collisionHandler = null,
            InputHandler? inputHandler = null
            )
        {
            SoundManager = soundManager;
            PowerUpManager = powerUpManager;
            ActiveEffectManager = activeEffectManager;
            CollisionHandler = collisionHandler;
            InputHandler = inputHandler;
        }

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
