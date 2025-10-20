using System;
using SplashKitSDK;
using PongGame.Combine;

namespace PongGame.Observers
{
    /// <summary>
    /// UI Observer - updates UI when score changes
    /// </summary>
    public class UIScoreObserver
    {
        private int _leftScore;
        private int _rightScore;
        private bool _gameStarted;

        public int LeftScore => _leftScore;
        public int RightScore => _rightScore;
        public bool GameStarted => _gameStarted;

        public void Update(ScoreSubject subject)
        {
            _leftScore = subject.LeftScore;
            _rightScore = subject.RightScore;
            _gameStarted = subject.GameStarted;
            
            // Log score change for debugging
            Console.WriteLine($"[UI Observer] Score updated: Left {_leftScore} - Right {_rightScore}");
        }

        public void DrawScore(int windowWidth, int windowHeight)
        {
            string leftScoreText = $"Left: {_leftScore}";
            string rightScoreText = $"Right: {_rightScore}";

            SplashKit.DrawText(leftScoreText, Color.White, "Arial", 24, 20, 20);
            SplashKit.DrawText(rightScoreText, Color.White, "Arial", 24, windowWidth - 120, 20);
        }
    }
}
