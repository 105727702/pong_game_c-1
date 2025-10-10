using PongGame.Entities;
using System.Collections.Generic;

namespace PongGame.Core
{
    /// <summary>
    /// Container for all game entities - improves GameContext organization
    /// </summary>
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

        /// <summary>
        /// Reset all entities to their starting positions
        /// </summary>
        public void ResetPositions()
        {
            Ball.ResetPosition();
            LeftPaddle.ResetPosition();
            RightPaddle.ResetPosition();
        }
    }
}
