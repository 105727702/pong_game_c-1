using PongGame.Entities;
using PongGame.Services;
using PongGame.Factories;
using System.Collections.Generic;

namespace PongGame.Core
{
    /// <summary>
    /// Context object containing all game objects and services
    /// Uses Observer Pattern for score management
    /// </summary>
    public class GameContext
    {
        public Ball Ball { get; set; }
        public Paddle LeftPaddle { get; set; }
        public Paddle RightPaddle { get; set; }
        
        // Use ScoreSubject instead of Scoreboard (Observer Pattern)
        public ScoreSubject ScoreSubject { get; set; }
        
        // Keep backward compatibility
        public Scoreboard Scoreboard => ScoreSubject.GetScoreboard();
        
        public List<Wall> Walls { get; set; }
        public SoundManager? SoundManager { get; set; }
        public PowerUpManager? PowerUpManager { get; set; }
        public ActiveEffectManager? ActiveEffectManager { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        public GameContext(Ball ball, Paddle leftPaddle, Paddle rightPaddle, 
            Scoreboard scoreboard, int windowWidth, int windowHeight)
        {
            Ball = ball;
            LeftPaddle = leftPaddle;
            RightPaddle = rightPaddle;
            
            // Create ScoreSubject with Observer Pattern
            ScoreSubject = new ScoreSubject(scoreboard);
            
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            Walls = new List<Wall>();
        }

        /// <summary>
        /// Initialize score subject dependencies
        /// </summary>
        public void InitializeScoreSubject()
        {
            ScoreSubject.InjectDependencies(Ball, SoundManager, ActiveEffectManager);
        }
    }
}
