namespace PongGame.Commands
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
