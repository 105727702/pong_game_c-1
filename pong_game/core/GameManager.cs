using PongGame.Entities;
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

        public bool GameOver { get; set; }
        private bool _gameStarted;

        // Factory
        private readonly IGameEntityFactory _factory;

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

            Context!.SoundManager = soundManager;
            Context!.PowerUpManager = powerUpManager;
            Context!.ActiveEffectManager = activeEffectManager;

            // Initialize ScoreSubject dependencies
            Context.InitializeScoreSubject();

            // Generate initial walls using Factory Pattern
            Context.Walls = _factory.CreateWalls(NUM_WALLS, MIN_WALL_DISTANCE, WINDOW_HEIGHT);

            // Set initial state
            _gameStarted = false;
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
            
            GameOver = false;
        }

        /// <summary>
        /// Start the game
        /// </summary>
        public void StartGame()
        {
            Context?.Scoreboard.Start();
        }

        /// <summary>
        /// Restart the game with a specified number of walls and a minimum distance between them
        /// </summary>
        public void RestartGame(int numWalls, int minDistance)
        {
            Context?.ScoreSubject.Reset();
            Context?.Ball.ResetPosition();
            Context?.LeftPaddle.ResetPosition();
            Context?.RightPaddle.ResetPosition();
            
            if (Context != null)
            {
                Context.Walls.Clear();
                Context.Walls = _factory.CreateWalls(numWalls, minDistance, Context.WindowHeight);
            }
            GameOver = false;
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
            StateMachine?.Update(deltaTime);

            // Sync UI state with game state when game over occurs
            if (_gameStarted && GameOver && _gameUI != null)
            {
                int winner = Context!.Scoreboard.LeftScore >= 1 ? 1 : 2;
                _gameUI.Winner = winner;
                _gameUI.CurrentState = GameState.GameOver;
                _gameStarted = false;
            }
        }

        /// <summary>
        /// Handle user input
        /// </summary>
        public void HandleInput()
        {
            // Handle UI interactions (menu clicks)
            HandleMenuInput();

            // Delegate gameplay input to current state
            if (_gameStarted && _gameUI?.CurrentState == GameState.Playing)
            {
                PlayState?.HandleInput();
            }
        }

        /// <summary>
        /// Handle menu input
        /// </summary>
        private void HandleMenuInput()
        {
            if (_gameUI == null) return;

            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Point2D mousePos = SplashKit.MousePosition();
                if (_gameUI.HandleMouseClick((float)mousePos.X, (float)mousePos.Y))
                {
                    if (_gameUI.CurrentState == GameState.MainMenu)
                    {
                        // Start new game from menu
                        float ballSpeed = GetBallSpeedFromDifficulty();
                        MenuState?.StartNewGame(ballSpeed);
                        _gameUI.CurrentState = GameState.Playing;
                        _gameStarted = true;
                    }
                    else if (_gameUI.CurrentState == GameState.GameOver)
                    {
                        // Restart game from game over
                        GameOverState?.RestartGame();
                        _gameUI.CurrentState = GameState.Playing;
                        _gameStarted = true;
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
            if (_gameStarted && _gameUI.CurrentState == GameState.Playing)
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
            Context.PowerUpManager?.Draw();
        }

        /// <summary>
        /// Change to a different game state
        /// </summary>
        public void ChangeState(string stateName)
        {
            StateMachine?.ChangeState(stateName);
        }
    }
}

