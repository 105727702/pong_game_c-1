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
        private List<IComponent> _components = new List<IComponent>();
        private TransformComponent? _transform;
        private RenderComponent? _render;
        private MovementComponent? _movement;

        public float X 
        { 
            get 
            {
                if (_transform != null)
                    return _transform.X;
                return 0;
            }
            set 
            { 
                if (_transform != null) 
                    _transform.X = value; 
            }
        }
        
        public float Y 
        { 
            get 
            {
                if (_transform != null)
                    return _transform.Y;
                return 0;
            }
            set 
            { 
                if (_transform != null) 
                    _transform.Y = value; 
            }
        }

        public float Width
        {
            get 
            {
                if (_transform != null)
                    return _transform.Width;
                return 0;
            }
            set
            {
                if (_transform != null)
                    _transform.Width = value;
            }
        }

        public float Height
        {
            get 
            {
                if (_transform != null)
                    return _transform.Height;
                return 0;
            }
            set
            {
                if (_transform != null)
                    _transform.Height = value;
            }
        }
        
        public Color Color 
        { 
            get
            {
                if (_render != null)
                    return _render.Color;

                return Color.White;
            }
            set 
            { 
                if (_render != null) 
                    _render.Color = value; 
            }
        }

        public Vector2D Velocity
        {
            get
            {
                if (_movement != null)
                    return _movement.Velocity;

                return new Vector2D(0, 0);
            }
            set
            {
                if (_movement != null)
                    _movement.Velocity = value;
            }
        }

        public float Speed
        {
            get
            {
                if (_movement != null)
                    return _movement.Speed;
                return 0;
            }
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
