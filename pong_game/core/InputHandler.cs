using SplashKitSDK;
using PongGame.Entities;
using PongGame.Commands;
using System.Collections.Generic;

namespace PongGame.Core
{
    /// <summary>
    /// Handles input events for the game using Command Pattern
    /// </summary>
    public class InputHandler
    {
        private readonly Dictionary<KeyCode, ICommand> _keyBindings;
        private readonly List<ICommand> _commandHistory;
        private ICommand? _leftPaddleCommand;
        private ICommand? _rightPaddleCommand;

        public InputHandler(Paddle leftPaddle, Paddle rightPaddle)
        {
            _keyBindings = new Dictionary<KeyCode, ICommand>();
            _commandHistory = new List<ICommand>();

            // Bind keys to commands
            _keyBindings[KeyCode.WKey] = new MoveUpCommand(leftPaddle);
            _keyBindings[KeyCode.SKey] = new MoveDownCommand(leftPaddle);
            _keyBindings[KeyCode.UpKey] = new MoveUpCommand(rightPaddle);
            _keyBindings[KeyCode.DownKey] = new MoveDownCommand(rightPaddle);
        }

        /// <summary>
        /// Handle keyboard input for paddle movement using Command Pattern
        /// </summary>
        public void HandleKeyInput(Paddle leftPaddle, Paddle rightPaddle)
        {
            bool leftPaddleMoving = false;
            bool rightPaddleMoving = false;

            // Left paddle controls (W/S)
            if (SplashKit.KeyDown(KeyCode.WKey))
            {
                _leftPaddleCommand = _keyBindings[KeyCode.WKey];
                _leftPaddleCommand.Execute();
                leftPaddleMoving = true;
            }
            else if (SplashKit.KeyDown(KeyCode.SKey))
            {
                _leftPaddleCommand = _keyBindings[KeyCode.SKey];
                _leftPaddleCommand.Execute();
                leftPaddleMoving = true;
            }

            if (!leftPaddleMoving)
            {
                var stopCommand = new StopPaddleCommand(leftPaddle);
                stopCommand.Execute();
            }

            // Right paddle controls (Up/Down arrows)
            if (SplashKit.KeyDown(KeyCode.UpKey))
            {
                _rightPaddleCommand = _keyBindings[KeyCode.UpKey];
                _rightPaddleCommand.Execute();
                rightPaddleMoving = true;
            }
            else if (SplashKit.KeyDown(KeyCode.DownKey))
            {
                _rightPaddleCommand = _keyBindings[KeyCode.DownKey];
                _rightPaddleCommand.Execute();
                rightPaddleMoving = true;
            }

            if (!rightPaddleMoving)
            {
                var stopCommand = new StopPaddleCommand(rightPaddle);
                stopCommand.Execute();
            }
        }

        /// <summary>
        /// Update paddle movement based on currently pressed keys
        /// </summary>
        public void UpdatePaddleMovement(Paddle leftPaddle, Paddle rightPaddle)
        {
            HandleKeyInput(leftPaddle, rightPaddle);
        }

        /// <summary>
        /// Undo the last command (for replay or debugging)
        /// </summary>
        public void UndoLastCommand()
        {
            if (_commandHistory.Count > 0)
            {
                var lastCommand = _commandHistory[_commandHistory.Count - 1];
                lastCommand.Undo();
                _commandHistory.RemoveAt(_commandHistory.Count - 1);
            }
        }
    }
}
