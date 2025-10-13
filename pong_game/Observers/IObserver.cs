using PongGame.Entities;

namespace PongGame.Observers
{
    /// <summary>
    /// Observer Pattern - Observer interface
    /// Observers are notified when ScoreSubject changes
    /// Note: ISubject interface removed - only ScoreSubject exists (no need for abstraction)
    /// </summary>
    public interface IObserver
    {
        void Update(ScoreSubject subject);
    }
}
