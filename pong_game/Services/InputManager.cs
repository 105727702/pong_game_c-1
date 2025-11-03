using SplashKitSDK;
using PongGame.Entities;
using PongGame.Commands;

namespace PongGame.Services
{
    public class InputHandler
    {
        private readonly Dictionary<KeyCode, ICommand> _keyBindings;

        public InputHandler(Paddle leftPaddle, Paddle rightPaddle)
        {
            _keyBindings = new Dictionary<KeyCode, ICommand>
            {
                { KeyCode.WKey, new MoveUpCommand(leftPaddle) },
                { KeyCode.SKey, new MoveDownCommand(leftPaddle) },
                { KeyCode.UpKey, new MoveUpCommand(rightPaddle) },
                { KeyCode.DownKey, new MoveDownCommand(rightPaddle) }
            };

        }
        public void HandleKeyInput()
        {
            foreach (var binding in _keyBindings)
            {
                if (SplashKit.KeyDown(binding.Key))
                    binding.Value.Execute();
            }
        }
    }
}
