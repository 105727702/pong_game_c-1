using PongGame.Decorators;
using SplashKitSDK;

namespace PongGame.Factories
{
    /// <summary>
    /// Speed Reduction Power-up - Decreases entity speed
    /// </summary>
    public class SpeedReductionPowerUp : BasePowerUp
    {
        public override PowerUpType Type => PowerUpType.SpeedReduction;

        public SpeedReductionPowerUp(float x, float y) : base(x, y, Color.Blue)
        {
        }

        public override void Apply(IGameEntity entity)
        {
            // Applied through decorator pattern
            // Example: new SpeedReductionDecorator(entity, 0.5f, 5.0)
        }
    }
}
