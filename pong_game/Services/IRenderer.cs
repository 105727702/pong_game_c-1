using SplashKitSDK;

namespace PongGame.Services
{
    /// <summary>
    /// Abstraction layer for rendering - decouples game entities from SplashKit
    /// Follows Dependency Inversion Principle - high-level entities don't depend on low-level rendering details
    /// Makes the game engine-agnostic and easier to test
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Draw a filled circle
        /// </summary>
        void DrawCircle(Color color, float centerX, float centerY, float radius);

        /// <summary>
        /// Draw a filled rectangle
        /// </summary>
        void DrawRectangle(Color color, float x, float y, float width, float height);

        /// <summary>
        /// Draw text
        /// </summary>
        void DrawText(string text, Color color, float x, float y, int fontSize);

        /// <summary>
        /// Draw text with specified font
        /// </summary>
        void DrawText(string text, Color color, string fontName, int fontSize, float x, float y);

        /// <summary>
        /// Get text width for layout calculations
        /// </summary>
        int GetTextWidth(string text, string fontName, int fontSize);

        /// <summary>
        /// Get text height for layout calculations
        /// </summary>
        int GetTextHeight(string text, string fontName, int fontSize);
    }
}
