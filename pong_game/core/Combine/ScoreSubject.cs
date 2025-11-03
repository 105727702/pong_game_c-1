using System.Collections.Generic;
using PongGame.Entities;
using PongGame.Services;

namespace PongGame.Combine
{

    public class ScoreSubject
    {
        private readonly Scoreboard _scoreboard;
        private const int WINNING_SCORE = 1;

        private Ball? _ball;
        private SoundManager? _soundManager;
        private ActiveEffectManager? _activeEffectManager;
        private PowerUpManager? _powerUpManager;

        public int LeftScore => _scoreboard.LeftScore;
        public int RightScore => _scoreboard.RightScore;
        public bool GameStarted => _scoreboard.GameStarted;

        public ScoreSubject(Scoreboard scoreboard)
        {
            _scoreboard = scoreboard;
        }

        public void InjectDependencies(Ball ball, SoundManager? soundManager, ActiveEffectManager? activeEffectManager = null, PowerUpManager? powerUpManager = null)
        {
            _ball = ball;
            _soundManager = soundManager;
            _activeEffectManager = activeEffectManager;
            _powerUpManager = powerUpManager;
        }

        public void LeftPoint()
        {
            _scoreboard.LeftPoint();
        }

        public void RightPoint()
        {
            _scoreboard.RightPoint();
        }

        public void Start()
        {
            _scoreboard.Start();
        }

        public void Reset()
        {
            _scoreboard.Reset();
        }

        public int CheckBallOutOfBounds()
        {
            if (_ball == null) return 0;

            if (_ball.X < 30)
            {
                if (_soundManager != null)
                {
                    _soundManager.PlayEffect(SoundType.BallOut);
                }
                RightPoint();
                _ball.ResetPosition();

                if (_activeEffectManager != null)
                {
                    _activeEffectManager.ClearAllEffects();
                }

                if (_powerUpManager != null)
                {
                    _powerUpManager.Clear();
                }

                if (RightScore >= WINNING_SCORE)
                {
                    HandleGameOver(2);
                    return 2; 
                }
            }
            else if (_ball.X > 1180)
            {
                if (_soundManager != null)
                {
                    _soundManager.PlayEffect(SoundType.BallOut);
                }
                LeftPoint();
                _ball.ResetPosition();

                if (_activeEffectManager != null)
                {
                    _activeEffectManager.ClearAllEffects();
                }
                
                if (_powerUpManager != null)
                {
                    _powerUpManager.Clear();
                }

                if (LeftScore >= WINNING_SCORE)
                {
                    HandleGameOver(1);
                    return 1; 
                }
            }

            return 0; 
        }

        public void HandleGameOver(int winner)
        {
            if (_soundManager != null)
            {
                _soundManager.StopMusic();
                _soundManager.PlayMusic(SoundType.GameOverMusic);
            }
        }

        public Scoreboard GetScoreboard()
        {
            return _scoreboard;
        }
    }
}
