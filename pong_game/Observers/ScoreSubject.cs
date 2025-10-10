using System.Collections.Generic;
using PongGame.Observers;
using PongGame.Services;
using PongGame.Factories;

namespace PongGame.Entities
{
    /// <summary>
    /// Scoreboard with Observer Pattern - notifies observers when score changes
    /// Integrates ScoreManager functionality (boundary checking and game over logic)
    /// Improved with better dependency management
    /// </summary>
    public class ScoreSubject : ISubject
    {
        private readonly List<IObserver> _observers = new List<IObserver>();
        private readonly Scoreboard _scoreboard;
        private const int WINNING_SCORE = 10;

        // Game context dependencies
        private Ball? _ball;
        private SoundManager? _soundManager;
        private ActiveEffectManager? _activeEffectManager;

        public int LeftScore => _scoreboard.LeftScore;
        public int RightScore => _scoreboard.RightScore;
        public bool GameStarted => _scoreboard.GameStarted;

        public ScoreSubject(Scoreboard scoreboard)
        {
            _scoreboard = scoreboard;
        }

        /// <summary>
        /// Inject dependencies for score management
        /// Called during GameContext initialization
        /// </summary>
        public void InjectDependencies(Ball ball, SoundManager? soundManager, ActiveEffectManager? activeEffectManager = null)
        {
            _ball = ball;
            _soundManager = soundManager;
            _activeEffectManager = activeEffectManager;
        }

        public void Attach(IObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }

        public void LeftPoint()
        {
            _scoreboard.LeftPoint();
            Notify();
        }

        public void RightPoint()
        {
            _scoreboard.RightPoint();
            Notify();
        }

        public void Start()
        {
            _scoreboard.Start();
            Notify();
        }

        public void Reset()
        {
            _scoreboard.Reset();
            Notify();
        }

        /// <summary>
        /// Check if the ball is out of bounds and handle scoring
        /// Returns: 0 = no winner, 1 = left player wins, 2 = right player wins
        /// </summary>
        public int CheckBallOutOfBounds()
        {
            if (_ball == null) return 0;

            // Left boundary - behind left paddle (x=30, width=20)
            if (_ball.X < 30)
            {
                _soundManager?.PlayEffect(SoundType.BallOut);
                RightPoint();
                _ball.ResetPosition();
                
                // Clear all active effects when ball goes out
                _activeEffectManager?.ClearAllEffects();

                if (RightScore >= WINNING_SCORE)
                {
                    HandleGameOver(2);
                    return 2; // Right player wins
                }
            }
            // Right boundary - based on actual display area (after x=1200)
            else if (_ball.X > 1180)
            {
                _soundManager?.PlayEffect(SoundType.BallOut);
                LeftPoint();
                _ball.ResetPosition();
                
                // Clear all active effects when ball goes out
                _activeEffectManager?.ClearAllEffects();

                if (LeftScore >= WINNING_SCORE)
                {
                    HandleGameOver(1);
                    return 1; // Left player wins
                }
            }

            return 0; // No winner yet
        }

        /// <summary>
        /// Handle game over state
        /// </summary>
        public void HandleGameOver(int winner)
        {
            _soundManager?.StopMusic();
            _soundManager?.PlayMusic(SoundType.GameOverMusic);
        }

        public Scoreboard GetScoreboard()
        {
            return _scoreboard;
        }
    }
}
