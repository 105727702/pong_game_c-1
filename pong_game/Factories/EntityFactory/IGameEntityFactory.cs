using PongGame.Entities;
using PongGame.Models;
using System.Collections.Generic;

namespace PongGame.Factories
{
    /// <summary>
    /// Factory Pattern - Interface for creating game entities
    /// </summary>
    public interface IGameEntityFactory
    {
        Ball CreateBall(int windowWidth, int windowHeight);
        Paddle CreatePaddle(float x, float y, int windowHeight);
        Wall CreateWall(float x, float y, int windowHeight, float speedMultiplier = 1.0f);
        Scoreboard CreateScoreboard();
        List<Wall> CreateWalls(int numWalls, int minDistance, int windowHeight);
        int CalculateWallCount(int totalScore, int baseWalls = 4);
    }
}
