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
            ScoreSubject = new ScoreSubject(scoreboard, ball, soundManager, activeEffectManager, powerUpManager, Entities);
        }

        public void DrawAllEntities()
        {
            Entities.Ball.Draw();
            Entities.LeftPaddle.Draw();
            Entities.RightPaddle.Draw();
            foreach (var wall in Entities.Walls)
            {
                wall.Draw();
            }
        }

        public void DrawPowerUps()
        {
            PowerUpManager?.Draw();
        }

        public void ResetGame()
        {
            ScoreSubject.Reset();
            Entities.ResetPositions();
        }

        public void SetBallSpeed(float speed)
        {
            Entities.Ball.SetBaseSpeed(speed);
        }

        public void UpdateWalls(int numWalls, int minDistance, int windowHeight)
        {
            var newWalls = Entities.WallTemplate.CreateWalls(numWalls, minDistance, windowHeight);
            Entities.UpdateWalls(newWalls);
        }

        public int CalculateWallCount(int baseWalls = 4)
        {
            int totalScore = Scoreboard.LeftScore + Scoreboard.RightScore;
            return Entities.WallTemplate.CalculateWallCount(totalScore, baseWalls);
        }

        public int GetWallCount()
        {
            return Entities.Walls.Count;
        }

        public int GetLeftScore()
        {
            return Scoreboard.LeftScore;
        }

        public int GetRightScore()
        {
            return Scoreboard.RightScore;
        }
    }
}
