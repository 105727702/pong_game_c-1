using PongGame.Entities;
using PongGame.Services;
using System.Collections.Generic;
using PongGame.Combine;

namespace PongGame.Core
{
    public class GameContext
    {
        public GameEntities Entities { get; private set; }
        public GameServices Services { get; private set; }
        public ScoreSubject ScoreSubject { get; private set; }
        
        public Scoreboard Scoreboard => ScoreSubject.GetScoreboard();
        public SoundManager? SoundManager => Services.SoundManager;
        public PowerUpManager? PowerUpManager => Services.PowerUpManager;
        public ActiveEffectManager? ActiveEffectManager => Services.ActiveEffectManager;

        public GameContext(
            Ball ball, 
            Paddle leftPaddle, 
            Paddle rightPaddle,
            Wall wallTemplate,
            Scoreboard scoreboard, 
            SoundManager? soundManager = null,
            PowerUpManager? powerUpManager = null,
            ActiveEffectManager? activeEffectManager = null)
        {
            Entities = new GameEntities(ball, leftPaddle, rightPaddle, wallTemplate);
            Services = new GameServices(soundManager, powerUpManager, activeEffectManager);
            ScoreSubject = new ScoreSubject(scoreboard, ball, soundManager, activeEffectManager, powerUpManager);
        }
    }
}
