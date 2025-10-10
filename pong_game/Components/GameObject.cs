using System.Collections.Generic;

namespace PongGame.Components
{
    /// <summary>
    /// Component Pattern - Base interface for all components
    /// </summary>
    public interface IComponent
    {
        void Update(float deltaTime);
    }

    /// <summary>
    /// Base GameObject class that can have multiple components
    /// </summary>
    public abstract class GameObject
    {
        protected List<IComponent> _components = new List<IComponent>();

        public void AddComponent(IComponent component)
        {
            _components.Add(component);
        }

        public void RemoveComponent(IComponent component)
        {
            _components.Remove(component);
        }

        public T? GetComponent<T>() where T : class, IComponent
        {
            foreach (var component in _components)
            {
                if (component is T typedComponent)
                    return typedComponent;
            }
            return null;
        }

        public virtual void Update(float deltaTime)
        {
            foreach (var component in _components)
            {
                component.Update(deltaTime);
            }
        }

        public abstract void Draw();
    }
}
