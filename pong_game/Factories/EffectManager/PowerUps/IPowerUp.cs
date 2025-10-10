using PongGame.Entities;
using PongGame.Decorators;

namespace PongGame.Factories
{
    /// <summary>
    /// Interface for power-up items
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
        void Apply(IGameEntity entity);
    }
}
