using SplashKitSDK;
using PongGame.Components;

namespace PongGame.Entities
{
    /// <summary>
    /// Paddle entity using Component Pattern
    /// Inherits from GameObject
    /// </summary>
    public class Paddle : GameObject
    {
        private const float BASE_SPEED = 3.5f;
        private const float MAX_SPEED = 6f;
        private const float FORCE = 0.15f;

        // Components
        private TransformComponent _transform;
        private RenderComponent _render;

        // Paddle-specific properties
        private readonly int _windowHeight;
        private float _speed;

        public float Speed 
        { 
            get => _speed;
            internal set => _speed = value; // ✅ Internal setter - for effects and commands
        }
        public float StartX { get; private set; }
        public float StartY { get; private set; }

        // Public properties for external access - read-only where possible
        public float X 
        { 
            get => _transform.X;
            private set => _transform.X = value; // ✅ Private setter
        }
        public float Y 
        { 
            get => _transform.Y;
            internal set => _transform.Y = value; // ✅ Internal - for commands
        }
        public int Width 
        { 
            get => (int)_transform.Width;
            private set => _transform.Width = value; // ✅ Private setter
        }
        public int Height 
        { 
            get => (int)_transform.Height;
            internal set => _transform.Height = value; // ✅ Internal - only for effects
        }
        public Color Color 
        { 
            get => _render.Color;
            internal set => _render.Color = value; // ✅ Internal - for effects
        }

        public Paddle(float x, float y, int windowHeight)
        {
            _windowHeight = windowHeight;
            
            // Initialize components
            _transform = new TransformComponent(x, y, 20, 100);
            _render = new RenderComponent(_transform, Color.White, isCircle: false);
            
            // Add components to GameObject
            AddComponent(_transform);
            AddComponent(_render);
            
            // Initialize paddle properties
            Speed = BASE_SPEED;
            StartX = x;
            StartY = y;
        }

        public void MoveUp()
        {
            _transform.Y -= Speed;
            if (_transform.Y < 0) _transform.Y = 0;
            IncreaseSpeed();
        }

        public void MoveDown()
        {
            _transform.Y += Speed;
            if (_transform.Y > _windowHeight - Height) 
                _transform.Y = _windowHeight - Height;
            IncreaseSpeed();
        }

        public void ResetSpeed()
        {
            Speed = BASE_SPEED; // ✅ Use private setter through this
        }

        private void IncreaseSpeed()
        {
            if (Speed < MAX_SPEED)
                Speed += FORCE; // ✅ Use private setter through this
        }

        public void ResetPosition()
        {
            _transform.X = StartX;
            _transform.Y = StartY;
        }

        /// <summary>
        /// Get the rectangle bounds of the paddle for collision detection
        /// </summary>
        public Rectangle GetBounds()
        {
            return _transform.GetBounds();
        }

        /// <summary>
        /// Draw the paddle - overrides GameObject.Draw()
        /// </summary>
        public override void Draw()
        {
            _render.Draw();
        }
    }
}
