using SplashKitSDK;

namespace PongGame.Effects
{
    /// <summary>
    /// Factory for creating power-up items
    /// Simplified - no need for subclasses
    /// </summary>
    public class PowerUpFactory
    {
        /// <summary>
        /// Create a power-up based on type with appropriate color
        /// </summary>
        public static IPowerUp CreatePowerUp(PowerUpType type, float x, float y)
        {
            Color color = GetColorForType(type);
            return new PowerUp(type, x, y, color);
        }

        /// <summary>
        /// Get the display color for each power-up type
        /// </summary>
        private static Color GetColorForType(PowerUpType type)
        {
            switch (type)
            {
                case PowerUpType.SpeedBoost:
                    return Color.Yellow;
                case PowerUpType.SpeedReduction:
                    return Color.Blue;
                case PowerUpType.SizeBoost:
                    return Color.Green;
                default:
                    return Color.White;
            }
        }
    }
}
