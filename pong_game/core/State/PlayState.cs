using PongGame.Services;
using PongGame.Decorator;
using PongGame.Factories;

namespace PongGame.Core.State
{
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
            _context.Services.InputHandler = new InputHandler(_context.Entities.LeftPaddle, _context.Entities.RightPaddle);
            
            _context.Services.CollisionHandler = new CollisionHandler(_context.Services.SoundManager, _context.Services.PowerUpManager);
            
            _context.Services.ClearAll();
            
            _context.ScoreSubject.Start();
        }

        public void Update(float deltaTime)
        {
            _context.Entities.Ball.Update(deltaTime);
            
            if (_context.Services.InputHandler != null)
            {
                _context.Services.InputHandler.HandleKeyInput();
            }

            if (_context.Services.PowerUpManager != null)
            {
                _context.Services.PowerUpManager.Update();
            }

            if (_context.Services.ActiveEffectManager != null)
            {
                _context.Services.ActiveEffectManager.Update();
            }

            GameManager.Instance.UpdateWallsBasedOnScore(MIN_WALL_DISTANCE);

            if (_context.Services.CollisionHandler != null)
            {
                _context.Services.CollisionHandler.HandleCollisions(
                    _context.Entities.Ball, 
                    _context.Entities.LeftPaddle, 
                    _context.Entities.RightPaddle,
                    _context.Entities.Walls, 
                    WINDOW_WIDTH, 
                    WINDOW_HEIGHT
                );
            }
            if (_context.Services.PowerUpManager != null)
            {
                var collectedPowerUp = _context.Services.PowerUpManager.CheckCollisions(_context.Entities.Ball);
                if (collectedPowerUp != null)
                {
                    _context.Services.PowerUpManager.ApplyPowerUpEffect(
                        collectedPowerUp,
                        _context.Services.ActiveEffectManager,
                        _context.Services.SoundManager
                    );
                }
            }

            int winner = _context.ScoreSubject.CheckBallOutOfBounds();
            if (winner > 0)
            {
                GameManager.Instance.ChangeState("GameOver");
            }
        }

        public void Exit()
        {
        }
    }
}
