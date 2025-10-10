using PongGame.Services;
using PongGame.Factories;

namespace PongGame.Core.State
{
    /// <summary>
    /// Play state - handles active gameplay
    /// Uses Observer Pattern for score management
    /// </summary>
    public class PlayState : IGameState
    {
        private readonly GameContext _context;
        private InputHandler? _inputHandler;
        private const int WINDOW_WIDTH = 1200;
        private const int WINDOW_HEIGHT = 800;
        private const int MIN_WALL_DISTANCE = 60;

        public PlayState(GameContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            // Initialize InputHandler with Command Pattern
            _inputHandler = new InputHandler(_context.LeftPaddle, _context.RightPaddle);
            
            // Clear power-ups and effects when starting/restarting game
            _context.PowerUpManager?.Clear();
            _context.ActiveEffectManager?.ClearAllEffects();
            
            // Start the scoreboard timer using Observer Pattern
            _context.ScoreSubject.Start();
        }

        public void Update(float deltaTime)
        {
            // Update game objects
            _context.Ball.Move();
            if (_inputHandler != null)
            {
                _inputHandler.UpdatePaddleMovement(_context.LeftPaddle, _context.RightPaddle);
            }

            // Update power-ups (remove expired ones)
            _context.PowerUpManager?.Update();

            // Update active effects (remove expired ones and restore state)
            _context.ActiveEffectManager?.Update();

            // Update walls based on current score for progressive difficulty
            GameManager.Instance.UpdateWallsBasedOnScore(MIN_WALL_DISTANCE);

            // Handle collisions using Factory pattern
            CollisionHandler.HandleCollisions(
                _context.Ball, 
                _context.LeftPaddle, 
                _context.RightPaddle,
                _context.Walls, 
                WINDOW_WIDTH, 
                WINDOW_HEIGHT, 
                _context.SoundManager,
                _context.PowerUpManager
            );

            // Check power-up collisions
            if (_context.PowerUpManager != null)
            {
                var collectedPowerUp = _context.PowerUpManager.CheckCollisions(_context.Ball);
                if (collectedPowerUp != null)
                {
                    _context.PowerUpManager.ApplyPowerUpEffect(
                        collectedPowerUp,
                        _context.ActiveEffectManager,
                        _context.SoundManager
                    );
                }
            }

            // Check for scoring using Observer Pattern (ScoreSubject)
            int winner = _context.ScoreSubject.CheckBallOutOfBounds();
            if (winner > 0)
            {
                GameManager.Instance.GameOver = true;
            }
        }

        public void HandleInput()
        {
            if (_inputHandler != null)
            {
                _inputHandler.HandleKeyInput(_context.LeftPaddle, _context.RightPaddle);
            }
        }

        public void Exit()
        {
            // Pause or stop gameplay-related activities
        }
    }
}
