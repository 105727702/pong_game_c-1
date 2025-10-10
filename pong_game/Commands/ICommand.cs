namespace PongGame.Commands
{
    /// <summary>
    /// Command Pattern - Base interface for all commands
    /// </summary>
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
