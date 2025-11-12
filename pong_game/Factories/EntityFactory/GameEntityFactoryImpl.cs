using PongGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PongGame.Factories
{
    public class GameEntityFactory 
    {
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
    }
}
