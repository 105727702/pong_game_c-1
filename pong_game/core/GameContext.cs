using PongGame.Entities;
using PongGame.Services;
using PongGame.Decorator;
using System.Collections.Generic;

namespace PongGame.Core
{
    /// <summary>
    /// Context object containing all game objects and services
    /// Simplified and organized with wrapper classes
    /// Uses Observer Pattern for score management
    /// </summary>
    public class GameContext
    {
        // Organized game entities
        public GameEntities Entities { get; private set; }
        
        // Organized game services
        public GameServices Services { get; private set; }
        
        // Score management with Observer Pattern
        public ScoreSubject ScoreSubject { get; private set; }
        
        // Window configuration
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        // Backward compatibility properties
        public Ball Ball => Entities.Ball;
        public Paddle LeftPaddle => Entities.LeftPaddle;
        public Paddle RightPaddle => Entities.RightPaddle;
        public List<Wall> Walls 
        { 
            get => Entities.Walls;
            set => Entities.Walls = value;
        }
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
            // Initialize organized containers
            Entities = new GameEntities(ball, leftPaddle, rightPaddle);
            Services = new GameServices();
            
            // Create ScoreSubject with Observer Pattern
            ScoreSubject = new ScoreSubject(scoreboard);
            
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
        }

        /// <summary>
        /// Initialize score subject dependencies after services are set
        /// </summary>
        public void InitializeScoreSubject()
        {
            ScoreSubject.InjectDependencies(
                Entities.Ball, 
                Services.SoundManager, 
                Services.ActiveEffectManager
            );
        }
    }
}
