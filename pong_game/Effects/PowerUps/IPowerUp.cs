using PongGame.Entities;

namespace PongGame.Effects
{
    /// <summary>
    /// Interface for power-up items
    /// Power-ups are visual collectibles that trigger effects through EffectFactory
    /// </summary>
    public interface IPowerUp
    {
        float X { get; }
        float Y { get; }
        PowerUpType Type { get; }
        void Draw();
        bool IsColliding(Ball ball);
        bool IsExpired();
        double GetRemainingLifetime();
    }
}
