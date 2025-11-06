using PongGame.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PongGame.Combine
{
    public class GameEntities
    {
        public Ball Ball { get; private set; }
        public Paddle LeftPaddle { get; private set; }
        public Paddle RightPaddle { get; private set; }
        public Wall WallTemplate { get; private set; }
        
        private readonly List<Wall> _walls = new List<Wall>();
        public IReadOnlyList<Wall> Walls => _walls.AsReadOnly();

        public GameEntities(Ball ball, Paddle leftPaddle, Paddle rightPaddle, Wall wallTemplate)
        {
            Ball = ball;
            LeftPaddle = leftPaddle;
            RightPaddle = rightPaddle;
            WallTemplate = wallTemplate;
        }
        
        public void UpdateWalls(List<Wall> newWalls)
        {
            _walls.Clear();
            _walls.AddRange(newWalls);
        }
        public void ResetPositions()
        {
            Ball.ResetPosition();
            LeftPaddle.ResetPosition();
            RightPaddle.ResetPosition();
        }
    }
}
