namespace PongGame.Core
{
    public interface IGameState
    {
        void Enter();
        void Update(float deltaTime);
        void Exit();
    }
}
