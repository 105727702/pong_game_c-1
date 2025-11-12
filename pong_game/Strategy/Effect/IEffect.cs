using PongGame.Entities;

namespace PongGame.Strategy
{
    public interface IEffect
    {
        void Apply(Ball ball, Paddle leftPaddle, Paddle rightPaddle);
        void Remove(Ball ball, Paddle leftPaddle, Paddle rightPaddle, int originalPaddleHeight);
    }
}
