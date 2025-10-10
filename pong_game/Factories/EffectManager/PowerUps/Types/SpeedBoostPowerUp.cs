using PongGame.Decorators;
using SplashKitSDK;

namespace PongGame.Factories
{
    /// <summary>
    /// Speed Boost Power-up - Increases entity speed
    /// </summary>
    public class SpeedBoostPowerUp : BasePowerUp
    {
        public override PowerUpType Type => PowerUpType.SpeedBoost;

        public SpeedBoostPowerUp(float x, float y) : base(x, y, Color.Yellow)
        {
        }

        public override void Apply(IGameEntity entity)
        {
            // Applied through decorator pattern
            // Example: new SpeedBoostDecorator(entity, 1.5f, 5.0)
        }
    }
}
