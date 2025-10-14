using System;
using PongGame.Entities;
using SplashKitSDK;

namespace PongGame.Decorator
{
    /// <summary>
    /// Concrete Strategy - Speed Boost Effect
    /// Increases ball speed and changes ball color to yellow
    /// </summary>
    public class SpeedBoostEffect : IEffect
    {
        private const float SPEED_INCREASE = 3f;

        public void Apply(Ball ball, Paddle leftPaddle, Paddle rightPaddle)
        {
            ball.SetSpeed(ball.Speed + SPEED_INCREASE);
            ball.NormalizeVelocity();
            ball.SetColor(Color.Yellow);
        }

        public void Remove(Ball ball, Paddle leftPaddle, Paddle rightPaddle, int originalPaddleHeight)
        {
            ball.ResetSpeed();
            ball.NormalizeVelocity();
            ball.SetColor(Color.White);
        }
    }

    /// <summary>
    /// Concrete Strategy - Speed Reduction Effect
    /// Decreases ball speed and changes ball color to blue
    /// </summary>
    public class SpeedReductionEffect : IEffect
    {
        private const float SPEED_DECREASE = 3f;
        private const float MIN_SPEED = 3f;

        public void Apply(Ball ball, Paddle leftPaddle, Paddle rightPaddle)
        {
            ball.SetSpeed(Math.Max(MIN_SPEED, ball.Speed - SPEED_DECREASE));
            ball.NormalizeVelocity();
            ball.SetColor(Color.Blue);
        }

        public void Remove(Ball ball, Paddle leftPaddle, Paddle rightPaddle, int originalPaddleHeight)
        {
            ball.ResetSpeed();
            ball.NormalizeVelocity();
            ball.SetColor(Color.White);
        }
    }

    /// <summary>
    /// Concrete Strategy - Size Boost Effect
    /// Increases paddle size and changes paddle color to green
    /// </summary>
    public class SizeBoostEffect : IEffect
    {
        private const int BOOSTED_HEIGHT = 150;

        public void Apply(Ball ball, Paddle leftPaddle, Paddle rightPaddle)
        {
            leftPaddle.Height = BOOSTED_HEIGHT;
            rightPaddle.Height = BOOSTED_HEIGHT;
            leftPaddle.Color = Color.Green;
            rightPaddle.Color = Color.Green;
        }

        public void Remove(Ball ball, Paddle leftPaddle, Paddle rightPaddle, int originalPaddleHeight)
        {
            leftPaddle.Height = originalPaddleHeight;
            rightPaddle.Height = originalPaddleHeight;
            leftPaddle.Color = Color.White;
            rightPaddle.Color = Color.White;
        }
    }
}
