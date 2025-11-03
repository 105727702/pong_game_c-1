using SplashKitSDK;
using PongGame.Services;


namespace PongGame.Core.State
{
    public class MenuState : IGameState
    {
        private readonly GameContext _context;

        public MenuState(GameContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            _context.SoundManager?.PlayMusic(SoundType.MenuMusic);
        }

        public void Update(float deltaTime)
        {
        }

        public void Exit()
        {
            _context.SoundManager?.StopMusic();
        }
    }
}
