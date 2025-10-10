using SplashKitSDK;
using PongGame.Entities;

namespace PongGame.Core
{
    /// <summary>
    /// Handles input events for the game - simplified direct approach
    /// Removed Command Pattern as it was over-engineering for simple paddle movement
    /// </summary>
    public class InputHandler
    {
        /// <summary>
        /// Handle keyboard input for paddle movement
        /// </summary>
        public void HandleKeyInput(Paddle leftPaddle, Paddle rightPaddle)
        {
            bool leftPaddleMoving = false;
            bool rightPaddleMoving = false;

            // Left paddle controls (W/S)
            if (SplashKit.KeyDown(KeyCode.WKey))
            {
                leftPaddle.MoveUp();
                leftPaddleMoving = true;
            }
            else if (SplashKit.KeyDown(KeyCode.SKey))
            {
                leftPaddle.MoveDown();
                leftPaddleMoving = true;
            }

            if (!leftPaddleMoving)
            {
                leftPaddle.ResetSpeed();
            }

            // Right paddle controls (Up/Down arrows)
            if (SplashKit.KeyDown(KeyCode.UpKey))
            {
                rightPaddle.MoveUp();
                rightPaddleMoving = true;
            }
            else if (SplashKit.KeyDown(KeyCode.DownKey))
            {
                rightPaddle.MoveDown();
                rightPaddleMoving = true;
            }

            if (!rightPaddleMoving)
            {
                rightPaddle.ResetSpeed();
            }
        }

        /// <summary>
        /// Update paddle movement based on currently pressed keys
        /// </summary>
        public void UpdatePaddleMovement(Paddle leftPaddle, Paddle rightPaddle)
        {
            HandleKeyInput(leftPaddle, rightPaddle);
        }
    }
}
