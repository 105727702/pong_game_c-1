using SplashKitSDK;

namespace PongGame.Strategy
{
    public class PowerUpFactory
    {
        public PowerUp CreatePowerUp(PowerUpType type, float x, float y)
        {
            Color color = GetColorForType(type);
            return new PowerUp(type, x, y, color);
        }

        private Color GetColorForType(PowerUpType type)
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
