using PongGame.Entities;

namespace PongGame.UI
{
    public class GameUI
    {
        private readonly UIRenderer _uiRenderer;

        public GameState CurrentState 
        { 
            get => _uiRenderer.CurrentState; 
            set => _uiRenderer.CurrentState = value; 
        }

        public Difficulty SelectedDifficulty 
        { 
            get => _uiRenderer.SelectedDifficulty; 
        }

        public int Winner 
        { 
            get => _uiRenderer.Winner; 
            set => _uiRenderer.Winner = value; 
        }

        public GameUI(int windowWidth, int windowHeight)
        {
            _uiRenderer = new UIRenderer(windowWidth, windowHeight);
        }

        public void RenderGameplay(Scoreboard scoreboard)
        {
            _uiRenderer.Draw(scoreboard);
        }

        public void Draw(Scoreboard scoreboard)
        {
            _uiRenderer.Draw(scoreboard);
        }

        public bool HandleMouseClick(float x, float y)
        {
            return _uiRenderer.HandleMouseClick(x, y);
        }
    }
}
