using PongGame.Entities;

namespace PongGame.Commands
{
    /// <summary>
    /// Command for moving paddle up
    /// </summary>
    public class MoveUpCommand : ICommand
    {
        private readonly Paddle _paddle;
        private float _previousY;

        public MoveUpCommand(Paddle paddle)
        {
            _paddle = paddle;
        }

        public void Execute()
        {
            _previousY = _paddle.Y;
            _paddle.MoveUp();
        }

        public void Undo()
        {
            _paddle.Y = _previousY;
        }
    }

    /// <summary>
    /// Command for moving paddle down
    /// </summary>
    public class MoveDownCommand : ICommand
    {
        private readonly Paddle _paddle;
        private float _previousY;

        public MoveDownCommand(Paddle paddle)
        {
            _paddle = paddle;
        }

        public void Execute()
        {
            _previousY = _paddle.Y;
            _paddle.MoveDown();
        }

        public void Undo()
        {
            _paddle.Y = _previousY;
        }
    }

    /// <summary>
    /// Command for stopping paddle movement
    /// </summary>
    public class StopPaddleCommand : ICommand
    {
        private readonly Paddle _paddle;
        private float _previousSpeed;

        public StopPaddleCommand(Paddle paddle)
        {
            _paddle = paddle;
        }

        public void Execute()
        {
            _previousSpeed = _paddle.Speed;
            _paddle.ResetSpeed();
        }

        public void Undo()
        {
            _paddle.Speed = _previousSpeed;
        }
    }
}
