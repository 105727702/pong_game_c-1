using System;
using SplashKitSDK;
using PongGame.Entities;

namespace PongGame.Observers
{
    /// <summary>
    /// UI Observer - updates UI when score changes
    /// </summary>
    public class UIScoreObserver : IObserver
    {
        private int _leftScore;
        private int _rightScore;
        private bool _gameStarted;

        public int LeftScore => _leftScore;
        public int RightScore => _rightScore;
        public bool GameStarted => _gameStarted;

        public void Update(ISubject subject)
        {
            if (subject is ScoreSubject scoreSubject)
            {
                _leftScore = scoreSubject.LeftScore;
                _rightScore = scoreSubject.RightScore;
                _gameStarted = scoreSubject.GameStarted;
                
                // Log score change for debugging
                Console.WriteLine($"[UI Observer] Score updated: Left {_leftScore} - Right {_rightScore}");
            }
        }

        public void DrawScore(int windowWidth, int windowHeight)
        {
            string leftScoreText = $"Left: {_leftScore}";
            string rightScoreText = $"Right: {_rightScore}";

            SplashKit.DrawText(leftScoreText, Color.White, "Arial", 24, 20, 20);
            SplashKit.DrawText(rightScoreText, Color.White, "Arial", 24, windowWidth - 120, 20);
        }
    }

    /// <summary>
    /// Console Observer - logs score changes to console
    /// </summary>
    public class ConsoleScoreObserver : IObserver
    {
        public void Update(ISubject subject)
        {
            if (subject is ScoreSubject scoreSubject)
            {
                Console.WriteLine($"[Console Log] Score Update - Left: {scoreSubject.LeftScore}, Right: {scoreSubject.RightScore}");
            }
        }
    }
}
