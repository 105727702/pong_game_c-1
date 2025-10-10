namespace PongGame.Observers
{
    /// <summary>
    /// Observer Pattern - Subject interface
    /// </summary>
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    /// <summary>
    /// Observer Pattern - Observer interface
    /// </summary>
    public interface IObserver
    {
        void Update(ISubject subject);
    }
}
