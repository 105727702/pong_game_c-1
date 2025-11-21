using PongGame.Entities;
using PongGame.Core.State;
using PongGame.Services;
using PongGame.UI;
using PongGame.Factories;
using SplashKitSDK;

namespace PongGame.Core
{
    public class GameManager
    {
        private const int WINDOW_WIDTH = 1200;
        private const int WINDOW_HEIGHT = 800;
        private const int NUM_WALLS = 4;
        private const int MIN_WALL_DISTANCE = 60;
        private static GameManager? _instance;
        private GameContext? _Context;
        private StateMachine? _StateMachine;
        private MenuState? _MenuState;
        private PlayState? _PlayState;
        private GameOverState? _GameOverState;
        private GameUI? _gameUI;
        private readonly GameEntityFactory _factory;

        private GameManager()
        {
            _factory = new GameEntityFactory();
        }

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }

        public void InitializeGame()
        {
            Ball ball = _factory.CreateBall(WINDOW_WIDTH, WINDOW_HEIGHT);
            Paddle leftPaddle = _factory.CreatePaddle(30, 250, WINDOW_HEIGHT);
            Paddle rightPaddle = _factory.CreatePaddle(WINDOW_WIDTH - 50, 250, WINDOW_HEIGHT);
            Scoreboard scoreboard = _factory.CreateScoreboard();
            Wall wallTemplate = _factory.CreateWall(0, 0, WINDOW_HEIGHT);

            _gameUI = new GameUI(WINDOW_WIDTH, WINDOW_HEIGHT);

            var soundManager = new SoundManager();
            var powerUpManager = new PowerUpManager(WINDOW_WIDTH, WINDOW_HEIGHT);
            var activeEffectManager = new ActiveEffectManager(ball, leftPaddle, rightPaddle);

            _Context = new GameContext(
                ball, 
                leftPaddle, 
                rightPaddle, 
                wallTemplate,
                scoreboard, 
                soundManager,
                powerUpManager,
                activeEffectManager);
            
            _StateMachine = new StateMachine();

            _MenuState = new MenuState(_Context);
            _PlayState = new PlayState(_Context);
            _GameOverState = new GameOverState(_Context);

            _StateMachine.AddState("Menu", _MenuState);
            _StateMachine.AddState("Play", _PlayState);
            _StateMachine.AddState("GameOver", _GameOverState);
            _StateMachine.ChangeState("Menu");

            _Context.UpdateWalls(NUM_WALLS, MIN_WALL_DISTANCE, WINDOW_HEIGHT);

            _gameUI.CurrentState = GameState.MainMenu;
        }


        public void UpdateWallsBasedOnScore(int minDistance)
        {
            if (_Context == null) return;

            int newWallCount = _Context.CalculateWallCount();
            
            if (_Context.GetWallCount() != newWallCount)
            {
                _Context.UpdateWalls(newWallCount, minDistance, WINDOW_HEIGHT);
            }
        }

        public void Update(float deltaTime)
        {
            if (_StateMachine != null)
            {
                _StateMachine.Update(deltaTime);
            }
            
            bool isGameOver = _StateMachine?.GetCurrentState() == _GameOverState;
            if (isGameOver && _gameUI != null && _gameUI.CurrentState != GameState.GameOver)
            {
                _gameUI.CurrentState = GameState.GameOver;
            }
        }

        public void HandleMenuInput()
        {
            if (_gameUI == null || _Context == null) return;

            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Point2D mousePos = SplashKit.MousePosition();
                
                if (_gameUI.CurrentState == GameState.GameOver)
                {
                    var stateBeforeClick = _gameUI.CurrentState;
                    bool clickedPlayAgain = _gameUI.HandleMouseClick((float)mousePos.X, (float)mousePos.Y);
                    
                    if (clickedPlayAgain)
                    {
                        _Context.ResetGame();
                        _Context.UpdateWalls(NUM_WALLS, MIN_WALL_DISTANCE, WINDOW_HEIGHT);
                        
                        ChangeState("Play");
                        _gameUI.CurrentState = GameState.Playing;
                    }
                    else if (stateBeforeClick == GameState.GameOver && _gameUI.CurrentState == GameState.MainMenu)
                    {
                        ChangeState("Menu");
                    }
                }
                else if (_gameUI.HandleMouseClick((float)mousePos.X, (float)mousePos.Y))
                {
                    if (_gameUI.CurrentState == GameState.MainMenu)
                    {
                        float ballSpeed = _gameUI.SelectedDifficulty switch
                        {
                            Difficulty.Easy => 4f,
                            Difficulty.Medium => 5f,
                            Difficulty.Hard => 6f,
                            _ => 5f
                        };                        
                        _Context.SetBallSpeed(ballSpeed);
                        _Context.ResetGame();
                        _Context.UpdateWalls(NUM_WALLS, MIN_WALL_DISTANCE, WINDOW_HEIGHT);

                        ChangeState("Play");
                        _gameUI.CurrentState = GameState.Playing;
                    }

                }
            }
        }

        public void Render()
        {
            if (_gameUI == null || _Context == null) return;

            _gameUI.Draw(_Context.Scoreboard);

            bool isPlaying = _StateMachine?.GetCurrentState() == _PlayState;
            if (isPlaying && _gameUI.CurrentState == GameState.Playing)
            {
                _Context.DrawAllEntities();
                _Context.DrawPowerUps();
            }
        }

        public void ChangeState(string stateName)
        {
            if (_StateMachine != null)
            {
                _StateMachine.ChangeState(stateName);
            }
        }
    }
}

