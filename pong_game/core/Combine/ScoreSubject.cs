using System.Collections.Generic;
using PongGame.Entities;
using PongGame.Services;

namespace PongGame.Combine
{

    public class ScoreSubject
    {
        private readonly Scoreboard _scoreboard;
        private readonly Ball _ball;
        private readonly SoundManager? _soundManager;
        private readonly ActiveEffectManager? _activeEffectManager;
        private readonly PowerUpManager? _powerUpManager;
        private readonly GameEntities? _entities;
        
        private const int WINNING_SCORE = 10;

        public int LeftScore => _scoreboard.LeftScore;
        public int RightScore => _scoreboard.RightScore;
        public bool GameStarted => _scoreboard.GameStarted;

        public ScoreSubject(
            Scoreboard scoreboard, 
            Ball ball, 
            SoundManager? soundManager = null, 
            ActiveEffectManager? activeEffectManager = null, 
            PowerUpManager? powerUpManager = null,
            GameEntities? entities = null)
        {
            _scoreboard = scoreboard;
            _ball = ball;
            _soundManager = soundManager;
            _activeEffectManager = activeEffectManager;
            _powerUpManager = powerUpManager;
            _entities = entities;
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
            if (_ball.X < 20)
            {
                _soundManager?.PlayEffect(SoundType.BallOut);
                RightPoint();
                _entities?.ResetPositions();
                _activeEffectManager?.ClearAllEffects();
                _powerUpManager?.Clear();

                if (RightScore >= WINNING_SCORE)
                {
                    HandleGameOver(2);
                    return 2; 
                }
            }
            else if (_ball.X > 1180)
            {
                _soundManager?.PlayEffect(SoundType.BallOut);
                LeftPoint();
                _entities?.ResetPositions();
                _activeEffectManager?.ClearAllEffects();
                _powerUpManager?.Clear();

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
            _soundManager?.StopMusic();
            _soundManager?.PlayMusic(SoundType.GameOverMusic);
        }

        public Scoreboard GetScoreboard()
        {
            return _scoreboard;
        }
    }
}
