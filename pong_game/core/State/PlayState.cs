using PongGame.Services;
using PongGame.Decorator;
using PongGame.Factories;

namespace PongGame.Core.State
{
    /// <summary>
    /// Play state - handles active gameplay
    /// Uses Observer Pattern for score management
    /// Uses instance-based CollisionHandler (Dependency Injection)
    /// </summary>
    public class PlayState : IGameState
    {
        private readonly GameContext _context;
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
            _context.Services.InputHandler = new InputHandler(_context.LeftPaddle, _context.RightPaddle);
            
            // Create collision handler with dependencies - MOVED HERE so services are available
            _context.Services.CollisionHandler = new CollisionHandler(_context.Services.SoundManager, _context.Services.PowerUpManager);
            
            // Clear power-ups and effects using GameServices wrapper
            _context.Services.ClearAll();
            
            // Start the scoreboard timer using Observer Pattern
            _context.ScoreSubject.Start();
        }

        public void Update(float deltaTime)
        {
            // Update game objects
            _context.Ball.Update(deltaTime);
            
            // Update paddle movement through InputHandler
            if (_context.Services.InputHandler != null)
            {
                _context.Services.InputHandler.HandleKeyInput();
            }

            // Update power-ups (remove expired ones)
            if (_context.Services.PowerUpManager != null)
            {
                _context.Services.PowerUpManager.Update();
            }

            // Update active effects (remove expired ones and restore state)
            if (_context.Services.ActiveEffectManager != null)
            {
                _context.Services.ActiveEffectManager.Update();
            }

            // Update walls based on current score for progressive difficulty
            GameManager.Instance.UpdateWallsBasedOnScore(MIN_WALL_DISTANCE);

            // Handle collisions using instance-based handler (OOP improvement)
            if (_context.Services.CollisionHandler != null)
            {
                _context.Services.CollisionHandler.HandleCollisions(
                    _context.Ball, 
                    _context.LeftPaddle, 
                    _context.RightPaddle,
                    _context.Walls, 
                    WINDOW_WIDTH, 
                    WINDOW_HEIGHT
                );
            }

            // Check power-up collisions
            if (_context.Services.PowerUpManager != null)
            {
                var collectedPowerUp = _context.Services.PowerUpManager.CheckCollisions(_context.Ball);
                if (collectedPowerUp != null)
                {
                    _context.Services.PowerUpManager.ApplyPowerUpEffect(
                        collectedPowerUp,
                        _context.Services.ActiveEffectManager,
                        _context.Services.SoundManager
                    );
                }
            }

            // Check for scoring using Observer Pattern (ScoreSubject)
            int winner = _context.ScoreSubject.CheckBallOutOfBounds();
            if (winner > 0)
            {
                GameManager.Instance.ChangeState("GameOver");
            }
        }

        public void Exit()
        {
            // Pause or stop gameplay-related activities
        }
    }
}
