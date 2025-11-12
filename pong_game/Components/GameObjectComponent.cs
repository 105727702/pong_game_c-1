using System.Collections.Generic;
using SplashKitSDK;
using Vector2D = PongGame.ValueObjects.Vector2D;

namespace PongGame.Components
{

    public interface IComponent
    {
        void Update(float deltaTime);
    }

    public abstract class GameObjectComponent
    {
        public List<IComponent> _components = new List<IComponent>();
        public TransformComponent? _transform;
        public RenderComponent? _render;
        public MovementComponent? _movement;

        public float X 
        { 
            get => _transform?.X ?? 0;
            set 
            { 
                if (_transform != null) 
                    _transform.X = value; 
            }
        }
        
        public float Y 
        { 
            get => _transform?.Y ?? 0;
            set 
            { 
                if (_transform != null) 
                    _transform.Y = value; 
            }
        }

        public float Width
        {
            get => _transform?.Width ?? 0;
            set
            {
                if (_transform != null)
                    _transform.Width = value;
            }
        }

        public float Height
        {
            get => _transform?.Height ?? 0;
            set
            {
                if (_transform != null)
                    _transform.Height = value;
            }
        }
        
        public Color Color 
        { 
            get => _render?.Color ?? Color.White;
            set 
            { 
                if (_render != null) 
                    _render.Color = value; 
            }
        }

        public Vector2D Velocity
        {
            get => _movement?.Velocity ?? new Vector2D(0, 0);
            set
            {
                if (_movement != null)
                    _movement.Velocity = value;
            }
        }

        public float Speed
        {
            get => _movement?.Speed ?? 0;
            set
            {
                if (_movement != null)
                    _movement.Speed = value;
            }
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component);
            
            if (component is TransformComponent transform)
                _transform = transform;
            else if (component is RenderComponent render)
                _render = render;
            else if (component is MovementComponent movement)
                _movement = movement;
        }

        public void RemoveComponent(IComponent component)
        {
            _components.Remove(component);
            
            if (component is TransformComponent)
                _transform = null;
            else if (component is RenderComponent)
                _render = null;
            else if (component is MovementComponent)
                _movement = null;
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
