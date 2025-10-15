namespace PongGame.Models
{
    /// <summary>
    /// MODEL OBJECT - Represents game state and score tracking
    /// Not a game entity - has no visual representation or physics
    /// Used by Observer Pattern (Subject) and displayed by UI
    /// </summary>
    public class Scoreboard
    {
        public int LeftScore { get; private set; }
        public int RightScore { get; private set; }
        public bool GameStarted { get; set; }

        public Scoreboard()
        {
            Reset();
        }

        public void LeftPoint()
        {
            LeftScore++;
        }

        public void RightPoint()
        {
            RightScore++;
        }

        public void Start()
        {
            GameStarted = true;
        }

        public void Reset()
        {
            LeftScore = 0;
            RightScore = 0;
            GameStarted = false;
        }
    }
}
