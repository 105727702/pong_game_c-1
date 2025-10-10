using PongGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PongGame.Factories
{
    /// <summary>
    /// Concrete factory for creating game entities
    /// Includes wall generation logic (previously in WallGenerator)
    /// </summary>
    public class GameEntityFactory : IGameEntityFactory
    {
        private readonly Random _random = new Random();

        public Ball CreateBall(int windowWidth, int windowHeight)
        {
            return new Ball(windowWidth, windowHeight);
        }

        public Paddle CreatePaddle(float x, float y, int windowHeight)
        {
            return new Paddle(x, y, windowHeight);
        }

        public Wall CreateWall(float x, float y, int windowHeight, float speedMultiplier = 1.0f)
        {
            return new Wall(x, y, windowHeight, speedMultiplier);
        }

        public Scoreboard CreateScoreboard()
        {
            return new Scoreboard();
        }

        /// <summary>
        /// Generate walls with a minimum distance between them
        /// </summary>
        public List<Wall> CreateWalls(int numWalls, int minDistance, int windowHeight)
        {
            var newWalls = new List<Wall>();
            
            // Calculate speed multiplier based on number of walls
            float speedMultiplier = 1.0f + (numWalls - 4) * 0.1f;
            
            float playableStartX = 60f;
            float playableEndX = 1140f;
            float playableWidth = playableEndX - playableStartX;
            float colWidth = playableWidth / numWalls;
            var xPositions = new List<float>();

            for (int i = 0; i < numWalls; i++)
            {
                float x = playableStartX + i * colWidth + colWidth / 2f;
                xPositions.Add(x);
            }

            foreach (float x in xPositions)
            {
                int attempts = 0;
                float y;
                
                do
                {
                    y = _random.Next(80, windowHeight - Wall.WALL_HEIGHT - 80);
                    attempts++;
                } while (newWalls.Any(w => Math.Abs(w.Y - y) < minDistance) && attempts <= 100);

                if (attempts <= 100)
                {
                    newWalls.Add(CreateWall(x, y, windowHeight, speedMultiplier));
                }
            }

            return newWalls;
        }

        /// <summary>
        /// Calculate number of walls based on current score for progressive difficulty
        /// </summary>
        public int CalculateWallCount(int totalScore, int baseWalls = 4)
        {
            int additionalWalls = totalScore / 4;
            return Math.Min(baseWalls + additionalWalls, 6); // Max 6 walls
        }
    }
}
