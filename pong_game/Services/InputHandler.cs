using SplashKitSDK;
using PongGame.Entities;
using PongGame.Commands;

namespace PongGame.Services
{
    /// <summary>
    /// Handles input events for the game using Command Pattern
    /// Separates input detection from action execution
    /// Provides flexibility for undo/redo and command queuing
    /// </summary>
    public class InputHandler
    {
        private readonly Dictionary<KeyCode, ICommand> _keyBindings;
        private readonly ICommand _stopLeftPaddleCommand;
        private readonly ICommand _stopRightPaddleCommand;

        /// <summary>
        /// Initialize InputHandler with paddle command bindings
        /// </summary>
        public InputHandler(Paddle leftPaddle, Paddle rightPaddle)
        {
            // Create commands for each paddle
            _keyBindings = new Dictionary<KeyCode, ICommand>
            {
                { KeyCode.WKey, new MoveUpCommand(leftPaddle) },
                { KeyCode.SKey, new MoveDownCommand(leftPaddle) },
                { KeyCode.UpKey, new MoveUpCommand(rightPaddle) },
                { KeyCode.DownKey, new MoveDownCommand(rightPaddle) }
            };

            _stopLeftPaddleCommand = new StopPaddleCommand(leftPaddle);
            _stopRightPaddleCommand = new StopPaddleCommand(rightPaddle);
        }

        /// <summary>
        /// Handle keyboard input and execute corresponding commands
        /// </summary>
        public void HandleKeyInput()
        {
            bool leftPaddleMoving = false;
            bool rightPaddleMoving = false;

            // Check left paddle controls (W/S)
            if (SplashKit.KeyDown(KeyCode.WKey))
            {
                _keyBindings[KeyCode.WKey].Execute();
                leftPaddleMoving = true;
            }
            else if (SplashKit.KeyDown(KeyCode.SKey))
            {
                _keyBindings[KeyCode.SKey].Execute();
                leftPaddleMoving = true;
            }

            if (!leftPaddleMoving)
            {
                _stopLeftPaddleCommand.Execute();
            }

            // Check right paddle controls (Up/Down arrows)
            if (SplashKit.KeyDown(KeyCode.UpKey))
            {
                _keyBindings[KeyCode.UpKey].Execute();
                rightPaddleMoving = true;
            }
            else if (SplashKit.KeyDown(KeyCode.DownKey))
            {
                _keyBindings[KeyCode.DownKey].Execute();
                rightPaddleMoving = true;
            }

            if (!rightPaddleMoving)
            {
                _stopRightPaddleCommand.Execute();
            }
        }
    }
}
