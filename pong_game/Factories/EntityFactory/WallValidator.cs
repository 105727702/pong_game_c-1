using System.Collections.Generic;
using PongGame.Entities;

namespace PongGame.Factories
{
    /// <summary>
    /// Interface for wall position validation
    /// Follows Single Responsibility Principle - separates validation from entity
    /// </summary>
    public interface IWallValidator
    {
        /// <summary>
        /// Check if a wall position is valid (not too close to existing walls)
        /// </summary>
        bool IsValidPosition(float x, float y, List<Wall> existingWalls, int minDistance);
    }

    /// <summary>
    /// Validates wall positions to ensure proper spacing
    /// </summary>
    public class WallValidator : IWallValidator
    {
        private const int WALL_WIDTH = 10;
        private const int WALL_HEIGHT = 80;

        /// <summary>
        /// Check if a wall position is valid (doesn't overlap with existing walls)
        /// </summary>
        public bool IsValidPosition(float x, float y, List<Wall> existingWalls, int minDistance)
        {
            foreach (Wall wall in existingWalls)
            {
                if (x < wall.X + wall.Width + minDistance &&
                    x + WALL_WIDTH + minDistance > wall.X &&
                    y < wall.Y + wall.Height + minDistance &&
                    y + WALL_HEIGHT + minDistance > wall.Y)
                {
                    return false; // Overlapping or too close
                }
            }
            return true; // Valid position
        }
    }
}
