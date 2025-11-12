namespace PongGame.Core.State
{

    public class GameOverState : IGameState
    {
        private readonly GameContext _context;

        public GameOverState(GameContext context)
        {
            _context = context;
        }

        public void Enter()
        {
        }

        public void Update(float deltaTime)
        {
        }

        public void Exit()
        {
            _context.Services.SoundManager?.StopMusic();
        }
    }
}
