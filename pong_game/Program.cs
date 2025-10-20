using System;
using SplashKitSDK;
using PongGame.Core;

namespace PongGame
{
    /// <summary>
    /// Main entry point - Initializes and runs the game
    /// Game logic is delegated to GameManager (Singleton + State Pattern)
    /// </summary>
    internal static class Program
    {
        static void Main()
        {            
            try
            {
                // Initialize SplashKit window
                SplashKit.OpenWindow("Pong Game - Design Patterns Demo", 1200, 800);
                
                // Initialize GameManager (Singleton Pattern)
                // This will create all game entities using Factory Pattern
                GameManager.Instance.InitializeGame();
                
                // Main game loop
                while (!SplashKit.WindowCloseRequested("Pong Game - Design Patterns Demo"))
                {
                    SplashKit.ProcessEvents();
                    
                    // Delegate to GameManager
                    // GameManager uses State Pattern to handle different game states
                    // Input is handled using Command Pattern
                    GameManager.Instance.HandleMenuInput();
                    GameManager.Instance.Update(0.016f); // ~60 FPS
                    
                    SplashKit.ClearScreen(Color.Black);
                    GameManager.Instance.Render();
                    SplashKit.RefreshScreen(60);
                }
                
                // Cleanup - SplashKit manages all resources automatically
                SplashKit.CloseAllWindows();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}
