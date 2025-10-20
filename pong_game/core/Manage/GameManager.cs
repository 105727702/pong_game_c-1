using PongGame.Entities;
using PongGame.Models;
using PongGame.Core.State;
using PongGame.Services;
using PongGame.UI;
using PongGame.Factories;
using SplashKitSDK;

namespace PongGame.Core
{
    /// <summary>
    /// Singleton Game Manager using State Pattern
    /// Manages the overall game flow and delegates to appropriate states
    /// </summary>
    public class GameManager
    {
        private static GameManager? _instance;
        private static readonly object _lock = new object();

        // Constants
        private const int WINDOW_WIDTH = 1200;
        private const int WINDOW_HEIGHT = 800;
        private const int NUM_WALLS = 4;
        private const int MIN_WALL_DISTANCE = 60;

        public GameContext? Context { get; private set; }
        public StateMachine? StateMachine { get; private set; }

        // States
        public MenuState? MenuState { get; private set; }
        public PlayState? PlayState { get; private set; }
        public GameOverState? GameOverState { get; private set; }

        // UI
        private GameUI? _gameUI;

        // Factory
        private readonly GameEntityFactory _factory;

        // Private constructor for Singleton
        private GameManager()
        {
            _factory = new GameEntityFactory();
        }

        /// <summary>
        /// Get the singleton instance of GameManager
        /// </summary>
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new GameManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initialize the game manager - called once at startup
        /// </summary>
        public void InitializeGame()
        {
            // Use Factory Pattern to create game entities
            Ball ball = _factory.CreateBall(WINDOW_WIDTH, WINDOW_HEIGHT);
            Paddle leftPaddle = _factory.CreatePaddle(30, 250, WINDOW_HEIGHT);
            Paddle rightPaddle = _factory.CreatePaddle(WINDOW_WIDTH - 50, 250, WINDOW_HEIGHT);
            Scoreboard scoreboard = _factory.CreateScoreboard();

            // Initialize game UI
            _gameUI = new GameUI(WINDOW_WIDTH, WINDOW_HEIGHT);

            // Initialize context and services
            Initialize(ball, leftPaddle, rightPaddle, scoreboard, WINDOW_WIDTH, WINDOW_HEIGHT);

            // Initialize and attach services
            var soundManager = new SoundManager();
            var powerUpManager = new PowerUpManager(WINDOW_WIDTH, WINDOW_HEIGHT);
            var activeEffectManager = new ActiveEffectManager(ball, leftPaddle, rightPaddle);
            if (Context != null)
            {
                Context.Services.SoundManager = soundManager;
                Context.Services.PowerUpManager = powerUpManager;
                Context.Services.ActiveEffectManager = activeEffectManager;
                Context.InitializeScoreSubject();
                Context.Walls = _factory.CreateWalls(NUM_WALLS, MIN_WALL_DISTANCE, WINDOW_HEIGHT);
            }
             _gameUI.CurrentState = GameState.MainMenu;
        }

        /// <summary>
        /// Initialize the game manager with game objects
        /// </summary>
        public void Initialize(Ball ball, Paddle leftPaddle, Paddle rightPaddle, 
            Scoreboard scoreboard, int windowWidth, int windowHeight)
        {
            Context = new GameContext(ball, leftPaddle, rightPaddle, scoreboard, windowWidth, windowHeight);
            StateMachine = new StateMachine();
            
            // Initialize states
            MenuState = new MenuState(Context);
            PlayState = new PlayState(Context);
            GameOverState = new GameOverState(Context);

            // Register states with state machine
            StateMachine.AddState("Menu", MenuState);
            StateMachine.AddState("Play", PlayState);
            StateMachine.AddState("GameOver", GameOverState);

            // Start with menu state
            StateMachine.ChangeState("Menu");
        }

        /// <summary>
        /// Update walls based on current score for progressive difficulty
        /// </summary>
        public void UpdateWallsBasedOnScore(int minDistance)
        {
            if (Context == null) return;

            int totalScore = Context.Scoreboard.LeftScore + Context.Scoreboard.RightScore;
            int newWallCount = _factory.CalculateWallCount(totalScore);
            
            // Only regenerate if wall count should change
            if (Context.Walls.Count != newWallCount)
            {
                Context.Walls = _factory.CreateWalls(newWallCount, minDistance, Context.WindowHeight);
            }
        }

        /// <summary>
        /// Update the game state
        /// </summary>
        public void Update(float deltaTime)
        {
            if (StateMachine != null) 
            {
                StateMachine.Update(deltaTime);
            }
            // StateMachine?.Update(deltaTime);

            // Sync UI state with game state when game over occurs
            bool isGameOver = StateMachine?.GetCurrentState() == GameOverState;
            if (isGameOver && _gameUI != null && _gameUI.CurrentState != GameState.GameOver)
            {
                int winner = Context!.Scoreboard.LeftScore >= 1 ? 1 : 2;
                _gameUI.Winner = winner;
                _gameUI.CurrentState = GameState.GameOver;
            }
        }
        /// <summary>
        /// Handle menu input
        /// </summary>
        public void HandleMenuInput()
        {
            if (_gameUI == null) return;

            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Point2D mousePos = SplashKit.MousePosition();
                
                // Check if user clicked "Back to Menu" button from GameOver state
                if (_gameUI.CurrentState == GameState.GameOver)
                {
                    // Store current state before HandleMouseClick changes it
                    var stateBeforeClick = _gameUI.CurrentState;
                    bool clickedPlayAgain = _gameUI.HandleMouseClick((float)mousePos.X, (float)mousePos.Y);
                    
                    if (clickedPlayAgain)
                    {
                        // Restart game from game over
                        if (GameOverState != null)
                        {
                            GameOverState.RestartGame();
                        }
                        _gameUI.CurrentState = GameState.Playing;
                    }
                    else if (stateBeforeClick == GameState.GameOver && _gameUI.CurrentState == GameState.MainMenu)
                    {
                        // User clicked "Back to Menu" - return to menu
                        if (GameOverState != null)
                        {
                            GameOverState.ReturnToMenu();
                        }
                    }
                }
                else if (_gameUI.HandleMouseClick((float)mousePos.X, (float)mousePos.Y))
                {
                    if (_gameUI.CurrentState == GameState.MainMenu)
                    {
                        // Start new game from menu
                        float ballSpeed = GetBallSpeedFromDifficulty();
                        if (MenuState != null)
                        {
                            MenuState.StartNewGame(ballSpeed);
                        }
                        _gameUI.CurrentState = GameState.Playing;
                    }
                }
            }
        }

        /// <summary>
        /// Get ball speed based on selected difficulty
        /// </summary>
        private float GetBallSpeedFromDifficulty()
        {
            if (_gameUI == null) return 5f;

            return _gameUI.SelectedDifficulty switch
            {
                Difficulty.Easy => 4f,
                Difficulty.Medium => 5f,
                Difficulty.Hard => 6f,
                _ => 5f
            };
        }

        /// <summary>
        /// Render the game
        /// </summary>
        public void Render()
        {
            if (_gameUI == null || Context == null) return;

            // Render UI
            _gameUI.Draw(Context.Scoreboard);

            // Render game objects only during active gameplay
            bool isPlaying = StateMachine?.GetCurrentState() == PlayState;
            if (isPlaying && _gameUI.CurrentState == GameState.Playing)
            {
                RenderGameObjects();
            }
        }

        /// <summary>
        /// Render all game objects
        /// </summary>
        private void RenderGameObjects()
        {
            if (Context == null) return;

            Context.Ball.Draw();
            Context.LeftPaddle.Draw();
            Context.RightPaddle.Draw();

            foreach (Wall wall in Context.Walls)
            {
                wall.Draw();
            }

            // Draw power-ups
            if (Context.PowerUpManager != null)
            {
                Context.PowerUpManager.Draw();
            }
        }

        /// <summary>
        /// Change to a different game state
        /// </summary>
        public void ChangeState(string stateName)
        {
            if (StateMachine != null)
            {
                StateMachine.ChangeState(stateName);
            }
        }
    }
}

