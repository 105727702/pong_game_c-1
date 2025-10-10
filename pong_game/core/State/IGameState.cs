namespace PongGame.Core
{
    /// <summary>
    /// Interface for game states
    /// </summary>
    public interface IGameState
    {
        void Enter();
        void Update(float deltaTime);
        void Exit();
    }
}
