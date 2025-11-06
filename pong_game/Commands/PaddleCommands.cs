using PongGame.Entities;

namespace PongGame.Commands
{
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
            _paddle.SetY(_previousY);
        }
    }

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
            _paddle.SetY(_previousY);
        }
    }
}
