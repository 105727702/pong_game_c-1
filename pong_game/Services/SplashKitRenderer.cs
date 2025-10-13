using SplashKitSDK;

namespace PongGame.Services
{
    /// <summary>
    /// SplashKit implementation of IRenderer
    /// Adapter Pattern - adapts SplashKit rendering API to our IRenderer interface
    /// </summary>
    public class SplashKitRenderer : IRenderer
    {
        public void DrawCircle(Color color, float centerX, float centerY, float radius)
        {
            SplashKit.FillCircle(color, centerX, centerY, radius);
        }

        public void DrawRectangle(Color color, float x, float y, float width, float height)
        {
            SplashKit.FillRectangle(color, x, y, width, height);
        }

        public void DrawText(string text, Color color, float x, float y, int fontSize)
        {
            SplashKit.DrawText(text, color, x, y);
        }

        public void DrawText(string text, Color color, string fontName, int fontSize, float x, float y)
        {
            SplashKit.DrawText(text, color, fontName, fontSize, x, y);
        }

        public int GetTextWidth(string text, string fontName, int fontSize)
        {
            return SplashKit.TextWidth(text, fontName, fontSize);
        }

        public int GetTextHeight(string text, string fontName, int fontSize)
        {
            return SplashKit.TextHeight(text, fontName, fontSize);
        }
    }
}
