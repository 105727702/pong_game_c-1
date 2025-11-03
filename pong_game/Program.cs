using System;
using SplashKitSDK;
using PongGame.Core;

namespace PongGame
{
    internal static class Program
    {
        static void Main()
        {            
            try
            {
                SplashKit.OpenWindow("Pong Game - Design Patterns Demo", 1200, 800);
                GameManager.Instance.InitializeGame();
                while (!SplashKit.WindowCloseRequested("Pong Game - Design Patterns Demo"))
                {
                    SplashKit.ProcessEvents();
                    
                    GameManager.Instance.HandleMenuInput();
                    GameManager.Instance.Update(0.016f);
                    
                    SplashKit.ClearScreen(Color.Black);
                    GameManager.Instance.Render();
                    SplashKit.RefreshScreen(60);
                }

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
