namespace PongGame.Decorators
{
    /// <summary>
    /// Decorator Pattern - Base interface for game entities that can be decorated
    /// </summary>
    public interface IGameEntity
    {
        float GetSpeed();
        float GetSize();
        void Update(float deltaTime);
    }

    /// <summary>
    /// Base decorator class
    /// </summary>
    public abstract class EntityDecorator : IGameEntity
    {
        protected IGameEntity _wrappedEntity;

        protected EntityDecorator(IGameEntity entity)
        {
            _wrappedEntity = entity;
        }

        public virtual float GetSpeed()
        {
            return _wrappedEntity.GetSpeed();
        }

        public virtual float GetSize()
        {
            return _wrappedEntity.GetSize();
        }

        public virtual void Update(float deltaTime)
        {
            _wrappedEntity.Update(deltaTime);
        }
    }
}
