using PongGame.Decorators;
using SplashKitSDK;

namespace PongGame.Factories
{
    /// <summary>
    /// Size Boost Power-up - Increases entity size
    /// </summary>
    public class SizeBoostPowerUp : BasePowerUp
    {
        public override PowerUpType Type => PowerUpType.SizeBoost;

        public SizeBoostPowerUp(float x, float y) : base(x, y, Color.Green)
        {
        }

        public override void Apply(IGameEntity entity)
        {
            // Applied through decorator pattern
            // Example: new SizeBoostDecorator(entity, 2.0f, 5.0)
        }
    }
}
