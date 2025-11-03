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

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        public Wall Wall { get; private set; } 
        public Scoreboard Scoreboard => ScoreSubject.GetScoreboard();

        public SoundManager? SoundManager 
        { 
            get => Services.SoundManager;
            set => Services.SoundManager = value;
        }
        public PowerUpManager? PowerUpManager 
        { 
            get => Services.PowerUpManager;
            set => Services.PowerUpManager = value;
        }
        public ActiveEffectManager? ActiveEffectManager 
        { 
            get => Services.ActiveEffectManager;
            set => Services.ActiveEffectManager = value;
        }

        public GameContext(Ball ball, Paddle leftPaddle, Paddle rightPaddle,
            Scoreboard scoreboard, int windowWidth, int windowHeight)
        {
            Entities = new GameEntities(ball, leftPaddle, rightPaddle);
            Services = new GameServices();
            ScoreSubject = new ScoreSubject(scoreboard);
            Wall = new Wall(0, 0, windowHeight);

            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
        }

        public void InitializeScoreSubject()
        {
            ScoreSubject.InjectDependencies(
                Entities.Ball,
                Services.SoundManager,
                Services.ActiveEffectManager,
                Services.PowerUpManager
            );
        }
    }
}
