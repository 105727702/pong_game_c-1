namespace PongGame.Effects
{
    /// <summary>
    /// Interface for PowerUp factory
    /// Follows Dependency Inversion Principle
    /// </summary>
    public interface IPowerUpFactory
    {
        /// <summary>
        /// Create a power-up based on type with appropriate visual properties
        /// </summary>
        IPowerUp CreatePowerUp(PowerUpType type, float x, float y);
    }
}
