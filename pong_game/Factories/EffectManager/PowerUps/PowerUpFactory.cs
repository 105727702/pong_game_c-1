namespace PongGame.Factories
{
    /// <summary>
    /// Factory for creating power-up items
    /// </summary>
    public class PowerUpFactory
    {
        public static IPowerUp CreatePowerUp(PowerUpType type, float x, float y)
        {
            switch (type)
            {
                case PowerUpType.SpeedBoost:
                    return new SpeedBoostPowerUp(x, y);
                case PowerUpType.SpeedReduction:
                    return new SpeedReductionPowerUp(x, y);
                case PowerUpType.SizeBoost:
                    return new SizeBoostPowerUp(x, y);
                default:
                    return new SpeedBoostPowerUp(x, y);
            }
        }
    }
}
