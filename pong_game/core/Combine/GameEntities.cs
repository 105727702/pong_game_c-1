using PongGame.Entities;
using System.Collections.Generic;

namespace PongGame.Combine
{
    public class GameEntities
    {
        public Ball Ball { get; set; }
        public Paddle LeftPaddle { get; set; }
        public Paddle RightPaddle { get; set; }
        public List<Wall> Walls { get; set; }

        public GameEntities(Ball ball, Paddle leftPaddle, Paddle rightPaddle)
        {
            Ball = ball;
            LeftPaddle = leftPaddle;
            RightPaddle = rightPaddle;
            Walls = new List<Wall>();
        }
        public void ResetPositions()
        {
            Ball.ResetPosition();
            LeftPaddle.ResetPosition();
            RightPaddle.ResetPosition();
        }
    }
}
