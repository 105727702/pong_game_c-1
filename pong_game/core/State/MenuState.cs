using SplashKitSDK;
using PongGame.Services;

namespace PongGame.Core.State
{
    /// <summary>
    /// Menu state - handles menu interactions and game initialization
    /// </summary>
    public class MenuState : IGameState
    {
        private readonly GameContext _context;
        private const int NUM_WALLS = 4;
        private const int MIN_WALL_DISTANCE = 60;

        public MenuState(GameContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            // Play menu music when entering menu
            _context.SoundManager?.PlayMusic(SoundType.MenuMusic);
            
            // Reset game over flag
            GameManager.Instance.GameOver = false;
        }

        public void Update(float deltaTime)
        {
            // Menu doesn't need per-frame updates
            // Input is handled separately in MainGameForm
        }

        /// <summary>
        /// Start a new game with specified difficulty
        /// </summary>
        public void StartNewGame(float ballSpeed)
        {
            // Set ball speed based on difficulty
            _context.Ball.SetBaseSpeed(ballSpeed);

            // Reset game state using ScoreSubject (Observer Pattern)
            _context.ScoreSubject.Reset();
            _context.Ball.ResetPosition();
            _context.LeftPaddle.ResetPosition();
            _context.RightPaddle.ResetPosition();
            
            // Regenerate walls using Factory Pattern
            _context.Walls.Clear();
            var factory = new PongGame.Factories.GameEntityFactory();
            _context.Walls = factory.CreateWalls(NUM_WALLS, MIN_WALL_DISTANCE, _context.WindowHeight);
            
            // Reset game over flag
            GameManager.Instance.GameOver = false;
            
            // Transition to Play state
            GameManager.Instance.ChangeState("Play");
        }

        public void Exit()
        {
            // Stop menu music when leaving menu (going to game or difficulty selection)
            _context.SoundManager?.StopMusic();
        }
    }
}
