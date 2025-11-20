using SplashKitSDK;
using PongGame.Components;
using System;

namespace PongGame.Entities
{
    public class Paddle : GameObjectComponent
    {
        private const float BASE_SPEED = 1.5f;
        private const float MAX_SPEED = 15.0f;
        private const float MIN_SPEED = 0f;
        private const float FORCE = 1f;

        private readonly int _windowHeight;
        private float _speed;

        public new float Speed 
        { 
            get => _speed;
            private set => _speed = Math.Clamp(value, MIN_SPEED, MAX_SPEED);
        }
        public float StartX { get; private set; }
        public float StartY { get; private set; }
        
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
        
        internal void SetY(float y)
        {
            Y = y;
        }
 
        public Rectangle CreateRectangle()
        {
            return SplashKit.RectangleFrom(X, Y, Width, Height);
        }

        public override void Draw()
        {
            base.Update(0);
        }
    }
}
