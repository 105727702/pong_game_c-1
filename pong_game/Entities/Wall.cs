using SplashKitSDK;
using System.Collections.Generic;
using System;
using PongGame.Components;
using Vector2D = PongGame.ValueObjects.Vector2D;

namespace PongGame.Entities
{
    public class Wall : GameObjectComponent
    {
        public const int WALL_WIDTH = 10;
        public const int WALL_HEIGHT = 100;

        private readonly int _windowHeight;
        private readonly Random _random = new Random();

        public new int Width => WALL_WIDTH;
        public new int Height => WALL_HEIGHT;
        
        public float YSpeed
        {
            get => Velocity.Y;
            set 
            {
                Velocity = new Vector2D(0, value);
            }
        }

        public Wall(float x, float y, int windowHeight, float speedMultiplier = 1.0f)
        {
            _windowHeight = windowHeight;

            var transform = new TransformComponent(x, y, WALL_WIDTH, WALL_HEIGHT);
            var movement = new MovementComponent(transform, 0);
            var render = new RenderComponent(transform, Color.Gray, isCircle: false);

            AddComponent(transform);
            AddComponent(movement);
            AddComponent(render);

            float baseSpeed = _random.Next(1, 2) * speedMultiplier;
            float speed = Math.Max(baseSpeed, 2.0f);
            YSpeed = speed;
        }

        public void Move()
        {
            base.Update(0);

            if (Y <= 0 || Y + Height >= _windowHeight)
            {
                YSpeed = -Velocity.Y;
            }
        }

        public List<Wall> CreateWalls(int numWalls, int minDistance, int windowHeight)
        {
            var newWalls = new List<Wall>();
            var _random = new Random();

            float speedMultiplier = 1.0f + (numWalls - 4) * 0.1f;

            float playableStartX = 60f;
            float playableEndX = 1140f;
            float playableWidth = playableEndX - playableStartX;
            float colWidth = playableWidth / numWalls;
            var xPositions = new List<float>();

            for (int i = 0; i < numWalls; i++)
            {
                float x = playableStartX + i * colWidth + colWidth / 2f;
                xPositions.Add(x);
            }

            foreach (float x in xPositions)
            {
                int attempts = 0;
                float y;

                do
                {
                    y = _random.Next(80, windowHeight - Wall.WALL_HEIGHT - 80);
                    attempts++;
                } while (newWalls.Any(w => Math.Abs(w.Y - y) < minDistance) && attempts <= 100);

                if (attempts <= 100)
                {
                    newWalls.Add(new Wall(x, y, windowHeight, speedMultiplier));
                }
            }

            return newWalls;
        }

        public int CalculateWallCount(int totalScore, int baseWalls = 4)
        {
            int additionalWalls = totalScore / 4;
            return Math.Min(baseWalls + additionalWalls, 6);
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

