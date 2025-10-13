# Pong Game - UML Class Diagram

```mermaid
classDiagram
    %% ============================================
    %% CORE GAME MANAGEMENT (Singleton + State Pattern)
    %% ============================================
    
    class GameManager {
        -GameManager _instance
        -object _lock
        -IGameEntityFactory _factory
        -GameUI _gameUI
        -bool _gameStarted
        +GameContext Context
        +StateMachine StateMachine
        +MenuState MenuState
        +PlayState PlayState
        +GameOverState GameOverState
        +bool GameOver
        +Instance GameManager
        +InitializeGame()
        +Initialize()
        +StartGame()
        +RestartGame()
        +ChangeState()
    }
    
    class StateMachine {
        -IGameState _currentState
        -Dictionary~string, IGameState~ _states
        +AddState()
        +ChangeState()
        +Update()
        +GetCurrentState()
    }
    
    class IGameState {
        <<interface>>
        +Enter()
        +Update()
        +Exit()
    }
    style IGameState fill:#e1f5ff,stroke:#0066cc,stroke-width:3px
    
    class MenuState {
        -GameContext _context
        +Enter()
        +Update()
        +StartNewGame()
        +Exit()
    }
    style MenuState fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class PlayState {
        -GameContext _context
        -InputHandler _inputHandler
        +Enter()
        +Update()
        +HandleInput()
        +Exit()
    }
    style PlayState fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class GameOverState {
        -GameContext _context
        +Enter()
        +Update()
        +RestartGame()
        +ReturnToMenu()
        +Exit()
    }
    style GameOverState fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    %% ============================================
    %% GAME CONTEXT AND CONTAINERS
    %% ============================================
    
    class GameContext {
        +GameEntities Entities
        +GameServices Services
        +ScoreSubject ScoreSubject
        +int WindowWidth
        +int WindowHeight
        +Ball Ball
        +Paddle LeftPaddle
        +Paddle RightPaddle
        +List~Wall~ Walls
        +Scoreboard Scoreboard
        +SoundManager SoundManager
        +PowerUpManager PowerUpManager
        +ActiveEffectManager ActiveEffectManager
        +InitializeScoreSubject()
    }
    
    class GameEntities {
        +Ball Ball
        +Paddle LeftPaddle
        +Paddle RightPaddle
        +List~Wall~ Walls
        +ResetPositions()
    }
    
    class GameServices {
        +SoundManager SoundManager
        +PowerUpManager PowerUpManager
        +ActiveEffectManager ActiveEffectManager
        +ClearAll()
    }
    
    %% ============================================
    %% COMPONENT PATTERN
    %% ============================================
    
    class IComponent {
        <<interface>>
        +Update()
    }
    style IComponent fill:#e1f5ff,stroke:#0066cc,stroke-width:3px
    
    class GameObject {
        <<abstract>>
        #List~IComponent~ _components
        +AddComponent()
        +RemoveComponent()
        +GetComponent()
        +Update()
        +Draw()*
    }
    style GameObject fill:#fff4e1,stroke:#ff9800,stroke-width:3px
    
    class TransformComponent {
        +float X
        +float Y
        +float Width
        +float Height
        +GetBounds()
        +Update()
    }
    style TransformComponent fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class MovementComponent {
        -TransformComponent _transform
        +Vector2D Velocity
        +float Speed
        +Update()
        +SetVelocity()
    }
    style MovementComponent fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class RenderComponent {
        -TransformComponent _transform
        +Color Color
        +bool IsCircle
        +Update()
        +Draw()
    }
    style RenderComponent fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class CollisionComponent {
        -TransformComponent _transform
        +bool IsSolid
        +Update()
        +GetBounds()
        +CheckCollision()
    }
    style CollisionComponent fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    %% ============================================
    %% GAME ENTITIES
    %% ============================================
    
    class Ball {
        -TransformComponent _transform
        -MovementComponent _movement
        -RenderComponent _render
        -int _windowWidth
        -int _windowHeight
        -float _baseSpeed
        -Vector2D _velocity
        +float X
        +float Y
        +int Size
        +Color Color
        +Vector2D Velocity
        +float Speed
        +Move()
        +Bounce()
        +ResetPosition()
        +Accelerate()
        +LimitSpeed()
        +SetBaseSpeed()
        +ResetSpeed()
        +NormalizeVelocity()
        +GetBounds()
        +Draw()
    }
    style Ball fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class Paddle {
        -TransformComponent _transform
        -RenderComponent _render
        -int _windowHeight
        +float Speed
        +float StartX
        +float StartY
        +float X
        +float Y
        +int Width
        +int Height
        +Color Color
        +MoveUp()
        +MoveDown()
        +ResetSpeed()
        +ResetPosition()
        +GetBounds()
        +Draw()
    }
    style Paddle fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class Wall {
        -TransformComponent _transform
        -MovementComponent _movement
        -RenderComponent _render
        -int _windowHeight
        +float X
        +float Y
        +int Width
        +int Height
        +Color Color
        +float YSpeed
        +Move()
        +IsValidPosition()$
    }
    style Wall fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class Vector2D {
        +float X
        +float Y
        +Add()
        +Subtract()
        +Multiply()
        +Magnitude float
        +Normalize()
        +Limit()
        +DotProduct()
        +Copy()
    }
    
    class Scoreboard {
        +int LeftScore
        +int RightScore
        +bool GameStarted
        +LeftPoint()
        +RightPoint()
        +Start()
        +Reset()
    }
    
    %% ============================================
    %% OBSERVER PATTERN
    %% ============================================
    
    class ScoreSubject {
        -List~IObserver~ _observers
        -Scoreboard _scoreboard
        -Ball _ball
        -SoundManager _soundManager
        -ActiveEffectManager _activeEffectManager
        +int LeftScore
        +int RightScore
        +bool GameStarted
        +InjectDependencies()
        +Attach()
        +Detach()
        +Notify()
        +LeftPoint()
        +RightPoint()
        +Start()
        +Reset()
        +CheckBallOutOfBounds()
    }
    
    class IObserver {
        <<interface>>
        +Update(ScoreSubject)
    }
    style IObserver fill:#e1f5ff,stroke:#0066cc,stroke-width:3px
    
    class UIScoreObserver {
        -int _leftScore
        -int _rightScore
        -bool _gameStarted
        +int LeftScore
        +int RightScore
        +bool GameStarted
        +Update()
        +DrawScore()
    }
    style UIScoreObserver fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class ConsoleScoreObserver {
        +Update()
    }
    style ConsoleScoreObserver fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    %% ============================================
    %% COMMAND PATTERN
    %% ============================================
    
    class ICommand {
        <<interface>>
        +Execute()
        +Undo()
    }
    style ICommand fill:#e1f5ff,stroke:#0066cc,stroke-width:3px
    
    class MoveUpCommand {
        -Paddle _paddle
        -float _previousY
        +Execute()
        +Undo()
    }
    style MoveUpCommand fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class MoveDownCommand {
        -Paddle _paddle
        -float _previousY
        +Execute()
        +Undo()
    }
    style MoveDownCommand fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class StopPaddleCommand {
        -Paddle _paddle
        -float _previousSpeed
        +Execute()
        +Undo()
    }
    style StopPaddleCommand fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class InputHandler {
        -Dictionary~KeyCode, ICommand~ _keyBindings
        -ICommand _stopLeftPaddleCommand
        -ICommand _stopRightPaddleCommand
        +InputHandler(leftPaddle, rightPaddle)
        +HandleKeyInput()
        +UpdatePaddleMovement()
    }
    style InputHandler fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    %% ============================================
    %% FACTORY PATTERN
    %% ============================================
    
    class IGameEntityFactory {
        <<interface>>
        +CreateBall()
        +CreatePaddle()
        +CreateWall()
        +CreateScoreboard()
        +CreateWalls()
        +CalculateWallCount()
    }
    style IGameEntityFactory fill:#e1f5ff,stroke:#0066cc,stroke-width:3px
    
    class GameEntityFactory {
        -Random _random
        +CreateBall()
        +CreatePaddle()
        +CreateWall()
        +CreateScoreboard()
        +CreateWalls()
        +CalculateWallCount()
    }
    style GameEntityFactory fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class PowerUpFactory {
        +CreatePowerUp(type, x, y)$
        -GetColorForType(type)$
    }
    
    class EffectFactory {
        +ApplyEffect()
        +RemoveEffect()
        +ResetAllEffects()
        -ApplySpeedBoost()
        -ApplySpeedReduction()
        -ApplySizeBoost()
        -ResetSpeed()
        -ResetSize()
    }
    
    %% ============================================
    %% POWER-UP SYSTEM (Effects Layer)
    %% ============================================
    
    class PowerUpType {
        <<enumeration>>
        SpeedBoost
        SpeedReduction
        SizeBoost
    }
    
    class IPowerUp {
        <<interface>>
        +float X
        +float Y
        +PowerUpType Type
        +Draw()
        +IsColliding()
        +IsExpired()
        +GetRemainingLifetime()
    }
    style IPowerUp fill:#e1f5ff,stroke:#0066cc,stroke-width:3px
    
    class PowerUp {
        -float X
        -float Y
        -PowerUpType Type
        -Color _color
        -DateTime _spawnTime
        -double _lifetime
        +PowerUp(type, x, y, color, lifetime)
        +Draw()
        +IsColliding()
        +IsExpired()
        +GetRemainingLifetime()
    }
    style PowerUp fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class PowerUpManager {
        -List~IPowerUp~ _activePowerUps
        -Random _random
        -int _windowWidth
        -int _windowHeight
        +int Count
        +int MaxPowerUps
        +SpawnRandomPowerUp()
        +SpawnMultiplePowerUps()
        +Update()
        +CheckCollisions()
        +ApplyPowerUpEffect()
        +Draw()
        +Clear()
    }
    
    class ActiveEffect {
        +PowerUpType Type
        +DateTime StartTime
        +double Duration
        +IsExpired()
        +GetRemainingTime()
    }
    
    class ActiveEffectManager {
        -List~ActiveEffect~ _activeEffects
        -Ball _ball
        -Paddle _leftPaddle
        -Paddle _rightPaddle
        -int _originalPaddleHeight
        -float _originalBallSpeed
        -EffectFactory _effectFactory
        +int Count
        +ApplyEffect()
        +Update()
        -ActivateEffect()
        -DeactivateEffect()
        +ClearAllEffects()
        +GetActiveEffects()
    }
    
    %% ============================================
    %% SERVICES
    %% ============================================
    
    class SoundType {
        <<enumeration>>
        WallHit
        PaddleHit
        BallHitWall
        BallOut
        MenuMusic
        GameOverMusic
        PotionEffect
    }
    
    class SoundManager {
        -Dictionary~SoundType, SoundEffect~ _soundEffects
        -Music _currentMusic
        +PlayEffect()
        +PlayMusic()
        +StopMusic()
        -InitializeSounds()
        -LoadCustomSounds()
        -TryLoadSound()
    }
    
    class CollisionHandler {
        <<static>>
        +CheckCollision()$
        +ResolveCollision()$
        +HandleCollisions()$
    }
    
    %% ============================================
    %% UI SYSTEM
    %% ============================================
    
    class GameState {
        <<enumeration>>
        MainMenu
        DifficultyMenu
        Playing
        GameOver
    }
    
    class Difficulty {
        <<enumeration>>
        Easy
        Medium
        Hard
    }
    
    class GameUI {
        -UIRenderer _uiRenderer
        +GameState CurrentState
        +Difficulty SelectedDifficulty
        +int Winner
        +RenderGameplay()
        +Draw()
        +HandleMouseClick()
    }
    
    class UIRenderer {
        -int _windowWidth
        -int _windowHeight
        -int _trueCenterX
        -Font _titleFont
        -Font _buttonFont
        -Font _scoreFont
        +GameState CurrentState
        +Difficulty SelectedDifficulty
        +int Winner
        +Draw()
        -DrawMainMenu()
        -DrawDifficultyMenu()
        -DrawScore()
        -DrawGameOver()
        +HandleMouseClick()
        -InitializeButtonRects()
    }
    
    %% ============================================
    %% RELATIONSHIPS
    %% ============================================
    
    %% Singleton and State Pattern
    GameManager --> StateMachine
    GameManager --> GameContext
    GameManager --> IGameEntityFactory
    GameManager --> MenuState
    GameManager --> PlayState
    GameManager --> GameOverState
    GameManager --> GameUI
    
    StateMachine --> IGameState
    IGameState <|.. MenuState
    IGameState <|.. PlayState
    IGameState <|.. GameOverState
    
    MenuState --> GameContext
    PlayState --> GameContext
    PlayState --> InputHandler
    GameOverState --> GameContext
    
    %% Game Context
    GameContext --> GameEntities
    GameContext --> GameServices
    GameContext --> ScoreSubject
    
    GameEntities --> Ball
    GameEntities --> Paddle
    GameEntities --> Wall
    
    GameServices --> SoundManager
    GameServices --> PowerUpManager
    GameServices --> ActiveEffectManager
    
    %% Component Pattern
    IComponent <|.. TransformComponent
    IComponent <|.. MovementComponent
    IComponent <|.. RenderComponent
    IComponent <|.. CollisionComponent
    
    GameObject o-- IComponent
    GameObject <|-- Ball
    GameObject <|-- Paddle
    GameObject <|-- Wall
    
    Ball --> TransformComponent
    Ball --> MovementComponent
    Ball --> RenderComponent
    Ball --> Vector2D
    
    Paddle --> TransformComponent
    Paddle --> RenderComponent
    
    Wall --> TransformComponent
    Wall --> MovementComponent
    Wall --> RenderComponent
    
    MovementComponent --> TransformComponent
    MovementComponent --> Vector2D
    RenderComponent --> TransformComponent
    CollisionComponent --> TransformComponent
    
    %% Observer Pattern
    ScoreSubject o-- IObserver
    ScoreSubject --> Scoreboard
    ScoreSubject --> Ball
    ScoreSubject --> SoundManager
    ScoreSubject --> ActiveEffectManager
    
    IObserver <|.. UIScoreObserver
    IObserver <|.. ConsoleScoreObserver
    
    %% Command Pattern
    ICommand <|.. MoveUpCommand
    ICommand <|.. MoveDownCommand
    ICommand <|.. StopPaddleCommand
    
    MoveUpCommand --> Paddle
    MoveDownCommand --> Paddle
    StopPaddleCommand --> Paddle
    
    InputHandler o-- ICommand
    InputHandler --> MoveUpCommand
    InputHandler --> MoveDownCommand
    InputHandler --> StopPaddleCommand
    
    %% Factory Pattern
    IGameEntityFactory <|.. GameEntityFactory
    
    GameEntityFactory ..> Ball
    GameEntityFactory ..> Paddle
    GameEntityFactory ..> Wall
    GameEntityFactory ..> Scoreboard
    
    PowerUpFactory ..> IPowerUp
    PowerUpFactory --> PowerUpType
    
    EffectFactory --> PowerUpType
    EffectFactory --> Ball
    EffectFactory --> Paddle
    
    %% Power-Up System
    IPowerUp <|.. PowerUp
    
    PowerUp --> PowerUpType
    PowerUp --> Ball
    
    PowerUpManager o-- IPowerUp
    PowerUpManager --> PowerUpFactory
    PowerUpManager --> Ball
    PowerUpManager --> SoundManager
    PowerUpManager --> ActiveEffectManager
    
    ActiveEffectManager o-- ActiveEffect
    ActiveEffectManager --> Ball
    ActiveEffectManager --> Paddle
    ActiveEffectManager --> EffectFactory
    
    ActiveEffect --> PowerUpType
    
    %% Services
    CollisionHandler --> Ball
    CollisionHandler --> Paddle
    CollisionHandler --> Wall
    CollisionHandler --> SoundManager
    CollisionHandler --> PowerUpManager
    
    SoundManager --> SoundType
    
    %% UI System
    GameUI --> UIRenderer
    GameUI --> Scoreboard
    UIRenderer --> GameState
    UIRenderer --> Difficulty
    UIRenderer --> Scoreboard
```

## Mô tả các Design Pattern được sử dụng

### 1. **Singleton Pattern** - GameManager
- Đảm bảo chỉ có một instance của GameManager trong suốt vòng đời ứng dụng
- Quản lý trạng thái game toàn cục

### 2. **State Pattern** - Game States
- **IGameState**: Interface cho các trạng thái game
- **MenuState**, **PlayState**, **GameOverState**: Các trạng thái cụ thể
- StateMachine quản lý chuyển đổi giữa các trạng thái

### 3. **Observer Pattern** - Score Management
- **ScoreSubject**: Subject thông báo thay đổi điểm số
- **IObserver**: Interface cho observers
- **UIScoreObserver**, **ConsoleScoreObserver**: Các observer cụ thể

### 4. **Component Pattern** - Game Entities
- **IComponent**: Interface chung cho tất cả components
- **TransformComponent**, **MovementComponent**, **RenderComponent**, **CollisionComponent**
- GameObject chứa danh sách components và quản lý chúng

### 5. **Command Pattern** - Input Handling
- **ICommand**: Interface cho commands
- **MoveUpCommand**, **MoveDownCommand**, **StopPaddleCommand**: Các command cụ thể
- **InputHandler**: Sử dụng Command Pattern để tách biệt input detection và action execution
- Hỗ trợ Undo/Redo và command queuing

### 6. **Factory Pattern** - Entity Creation
- **IGameEntityFactory**: Interface cho factory
- **GameEntityFactory**: Tạo các game entities (Ball, Paddle, Wall, Scoreboard)
- Chịu trách nhiệm tạo đối tượng game, không quản lý effects

### 7. **Effect System** - Power-Up Management (Tách riêng khỏi Factory)
- **PowerUpFactory**: Tạo power-up collectibles
- **EffectFactory**: Áp dụng effects trực tiếp lên entities
- **ActiveEffectManager**: Quản lý lifecycle và duration của effects
- **PowerUpManager**: Quản lý spawning, collision và lifecycle của power-ups
- Hệ thống effects độc lập, không nằm trong Factory Pattern

### 8. **Strategy Pattern** (Implicit) - Collision Handling
- CollisionHandler xử lý collision với các strategies khác nhau

## Cấu trúc chính

1. **Core Layer**: GameManager, StateMachine, States
2. **Entity Layer**: Ball, Paddle, Wall với Component Pattern
3. **Effects Layer**: PowerUp system, Effect managers (tách riêng)
4. **Factory Layer**: Entity creation only (không bao gồm effects)
5. **Service Layer**: SoundManager, InputHandler, CollisionHandler
6. **Observer Layer**: Score tracking và notification
7. **UI Layer**: GameUI, UIRenderer với state management
