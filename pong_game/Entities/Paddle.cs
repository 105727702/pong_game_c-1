using SplashKitSDK;
using PongGame.Components;

namespace PongGame.Entities
{
    public class Paddle : GameObjectComponent
    {
        private const float BASE_SPEED = 1.5f;
        private const float MAX_SPEED = 15.0f;
        private const float FORCE = 1f;

        private readonly int _windowHeight;
        private float _speed;

        public float Speed 
        { 
            get => _speed;
            set => _speed = value;
        }
        public float StartX { get; private set; }
        public float StartY { get; private set; }

        public new int Width 
        { 
            get => (int)base.Width;
            private set => base.Width = value; 
        }
        
        public new int Height 
        { 
            get => (int)base.Height;
            set => base.Height = value;
        }

        public Paddle(float x, float y, int windowHeight)
        {
            _windowHeight = windowHeight;
            
            var transform = new TransformComponent(x, y, 20, 100);
            var render = new RenderComponent(transform, Color.White, isCircle: false);
            
            AddComponent(transform);
            AddComponent(render);
            
            Speed = BASE_SPEED;
            StartX = x;
            StartY = y;
        }

        public void MoveUp()
        {
            Y -= Speed;
            if (Y < 0) Y = 0;
            IncreaseSpeed();
        }

        public void MoveDown()
        {
            Y += Speed;
            if (Y > _windowHeight - Height) 
                Y = _windowHeight - Height;
            IncreaseSpeed();
        }

        public void ResetSpeed()
        {
            Speed = BASE_SPEED; 
        }

        private void IncreaseSpeed()
        {
            if (Speed < MAX_SPEED)
                Speed += FORCE; 
        }

        public void ResetPosition()
        {
            X = StartX;
            Y = StartY;
        }
 
        public Rectangle CreateRectangle()
        {
            return _transform?.CreateRectangle() ?? new Rectangle();
        }

        public override void Draw()
        {
            _render?.Update(0);
        }
    }
}
