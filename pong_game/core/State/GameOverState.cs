using PongGame.Services;

namespace PongGame.Core.State
{
    /// <summary>
    /// Game Over state - handles game over screen and restart logic
    /// </summary>
    public class GameOverState : IGameState
    {
        private readonly GameContext _context;
        private const int NUM_WALLS = 4;
        private const int MIN_WALL_DISTANCE = 60;

        public GameOverState(GameContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            // Game over music is already playing (started by ScoreManager.HandleGameOver)
            // Set game over flag
            GameManager.Instance.GameOver = true;
            
            // Could add additional effects here like screen shake, final score calculation, etc.
        }

        public void Update(float deltaTime)
        {
            // Game over screen doesn't need per-frame updates
            // Input for restart/menu is handled separately in MainGameForm
            // Could add animations here like score counting, confetti, etc.
        }

        /// <summary>
        /// Restart the game from game over screen
        /// </summary>
        public void RestartGame()
        {
            // Reset game state using ScoreSubject (Observer Pattern)
            _context.ScoreSubject.Reset();
            
            // Reset all entity positions using GameEntities wrapper
            _context.Entities.ResetPositions();
            
            // Regenerate walls using Factory Pattern
            _context.Walls.Clear();
            var factory = new PongGame.Factories.GameEntityFactory();
            _context.Walls = factory.CreateWalls(NUM_WALLS, MIN_WALL_DISTANCE, _context.WindowHeight);
            
            // Reset game over flag
            GameManager.Instance.GameOver = false;
            
            // Transition to Play state
            GameManager.Instance.ChangeState("Play");
        }

        /// <summary>
        /// Return to main menu
        /// </summary>
        public void ReturnToMenu()
        {
            // Reset game over flag
            GameManager.Instance.GameOver = false;
            
            // Transition to Menu state
            GameManager.Instance.ChangeState("Menu");
        }

        public void Exit()
        {
            // Stop game over music when leaving this state (restarting or returning to menu)
            _context.Services.SoundManager?.StopMusic();
            
            // Reset game over flag when exiting
            GameManager.Instance.GameOver = false;
        }
    }
}
