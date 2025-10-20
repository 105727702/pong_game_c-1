# UML Diagrams - Pong Game Project

## 1. Class Diagram - Overall Architecture

```mermaid
classDiagram
    %% Core Classes
    class Program {
        <<static>>
        +Main()
    }
    
    class GameManager {
        <<Singleton>>
        -_instance: GameManager
        -_lock: object
        -_factory: GameEntityFactory
        +Context: GameContext
        +StateMachine: StateMachine
        +Instance: GameManager
        +InitializeGame()
        +Update(deltaTime)
        +Render()
        +HandleMenuInput()
    }
    
    class GameContext {
        +Entities: GameEntities
        +Services: GameServices
        +ScoreSubject: ScoreSubject
        +WindowWidth: int
        +WindowHeight: int
        +InitializeScoreSubject()
    }
    
    %% Combine Pattern - Aggregation Classes
    class GameEntities {
        +Ball: Ball
        +LeftPaddle: Paddle
        +RightPaddle: Paddle
        +Walls: List~Wall~
        +ResetPositions()
    }
    
    class GameServices {
        +SoundManager: SoundManager
        +PowerUpManager: PowerUpManager
        +ActiveEffectManager: ActiveEffectManager
        +CollisionHandler: CollisionHandler
        +InputHandler: InputHandler
        +ClearAll()
    }
    
    class StateMachine {
        -_currentState: IGameState
        +CurrentState: IGameState
        +ChangeState(newState)
        +Update(deltaTime)
    }
    
    %% State Pattern
    class IGameState {
        <<interface>>
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    class MenuState {
        -_context: GameContext
        -_stateMachine: StateMachine
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    class PlayState {
        -_context: GameContext
        -_stateMachine: StateMachine
        -_collisionHandler: CollisionHandler
        -_inputHandler: InputHandler
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    class GameOverState {
        -_context: GameContext
        -_stateMachine: StateMachine
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    %% Component Pattern
    class IComponent {
        <<interface>>
        +Update(deltaTime)
    }
    
    class GameObject {
        <<abstract>>
        #_components: List~IComponent~
        +AddComponent(component)
        +RemoveComponent(component)
        +GetComponent~T~()
        +Update(deltaTime)
        +Draw()*
    }
    
    class TransformComponent {
        +X: float
        +Y: float
        +Width: float
        +Height: float
        +Update(deltaTime)
    }
    
    class MovementComponent {
        -_transform: TransformComponent
        -_speed: float
        +Update(deltaTime)
    }
    
    class RenderComponent {
        -_transform: TransformComponent
        -_color: Color
        -_isCircle: bool
        +Update(deltaTime)
    }
    
    %% Entities
    class Ball {
        -_transform: TransformComponent
        -_movement: MovementComponent
        -_render: RenderComponent
        -_velocity: Vector2D
        -_speed: float
        +X: float
        +Y: float
        +Size: int
        +Move()
        +Bounce(surfaceNormal)
        +ResetPosition()
        +Draw()
    }
    
    class Paddle {
        -_transform: TransformComponent
        -_render: RenderComponent
        -_speed: float
        +X: float
        +Y: float
        +Width: int
        +Height: int
        +MoveUp()
        +MoveDown()
        +ResetSpeed()
        +Draw()
    }
    
    class Wall {
        +X: float
        +Y: float
        +Width: float
        +Height: float
        +GetNormal(): Vector2D
        +IsColliding(ball): bool
        +Draw()
    }
    
    %% Command Pattern
    class ICommand {
        <<interface>>
        +Execute()
        +Undo()
    }
    
    class MoveUpCommand {
        -_paddle: Paddle
        +Execute()
        +Undo()
    }
    
    class MoveDownCommand {
        -_paddle: Paddle
        +Execute()
        +Undo()
    }
    
    %% Factory Pattern
    class GameEntityFactory {
        +CreateBall(width, height): Ball
        +CreatePaddle(x, y, height): Paddle
        +CreateScoreboard(): Scoreboard
        +CreateWalls(num, distance, height): List~Wall~
    }
    
    %% Observer Pattern
    class ScoreSubject {
        -_observers: List~UIScoreObserver~
        -_scoreboard: Scoreboard
        -_ball: Ball
        -_soundManager: SoundManager
        -_activeEffectManager: ActiveEffectManager
        -_powerUpManager: PowerUpManager
        +LeftScore: int
        +RightScore: int
        +GameStarted: bool
        +Attach(observer)
        +Detach(observer)
        +Notify()
        +LeftPoint()
        +RightPoint()
        +Start()
        +Reset()
        +CheckBallOutOfBounds(): int
        +InjectDependencies(ball, soundManager, effectManager, powerUpManager)
    }
    
    class UIScoreObserver {
        <<abstract>>
        +Update(subject: ScoreSubject)*
    }
    
    class ScoreDisplayObserver {
        +Update(subject)
    }
    
    class GameStatusObserver {
        +Update(subject)
    }
    
    %% Decorator Pattern
    class IEffect {
        <<interface>>
        +Apply()
        +Remove()
        +IsExpired(): bool
        +Update(deltaTime)
    }
    
    class SpeedBoostEffect {
        -_ball: Ball
        -_startTime: DateTime
        -_duration: double
        +Apply()
        +Remove()
        +IsExpired(): bool
        +Update(deltaTime)
    }
    
    class PaddleExtendEffect {
        -_paddle: Paddle
        -_startTime: DateTime
        -_duration: double
        +Apply()
        +Remove()
        +IsExpired(): bool
        +Update(deltaTime)
    }
    
    class PowerUp {
        +X: float
        +Y: float
        +Type: PowerUpType
        -_color: Color
        +Draw()
        +IsColliding(ball): bool
        +IsExpired(): bool
    }
    
    class PowerUpFactory {
        +CreatePowerUp(type, x, y): PowerUp
    }
    
    class EffectFactory {
        +CreateEffect(type, ball, paddle): IEffect
    }
    
    %% Services
    class CollisionHandler {
        -_context: GameContext
        +CheckCollisions()
    }
    
    class InputHandler {
        -_leftPaddle: Paddle
        -_rightPaddle: Paddle
        +HandleInput()
    }
    
    class SoundManager {
        +PlayPaddleHit()
        +PlayWallHit()
        +PlayScore()
        +PlayPowerUpCollect()
    }
    
    class PowerUpManager {
        -_powerUps: List~PowerUp~
        -_factory: PowerUpFactory
        +SpawnPowerUp()
        +UpdatePowerUps(deltaTime)
        +DrawPowerUps()
        +CheckCollisions(ball): PowerUp?
    }
    
    class ActiveEffectManager {
        -_activeEffects: List~IEffect~
        -_factory: EffectFactory
        +AddEffect(effect)
        +UpdateEffects(deltaTime)
        +ClearEffects()
    }
    
    %% Value Object
    class Vector2D {
        <<ValueObject>>
        +X: float
        +Y: float
        +Magnitude: float
        +Add(other): Vector2D
        +Subtract(other): Vector2D
        +Multiply(scalar): Vector2D
        +DotProduct(other): float
        +Normalize(): Vector2D
    }
    
    class Scoreboard {
        +LeftScore: int
        +RightScore: int
        +IncrementLeftScore()
        +IncrementRightScore()
        +Reset()
    }
    
    %% Relationships
    Program --> GameManager : uses
    GameManager --> GameContext : has
    GameManager --> StateMachine : has
    GameManager --> GameEntityFactory : uses
    GameManager --> MenuState : creates
    GameManager --> PlayState : creates
    GameManager --> GameOverState : creates
    
    StateMachine --> IGameState : manages
    IGameState <|.. MenuState : implements
    IGameState <|.. PlayState : implements
    IGameState <|.. GameOverState : implements
    
    MenuState --> GameContext : uses
    PlayState --> GameContext : uses
    PlayState --> CollisionHandler : uses
    PlayState --> InputHandler : uses
    GameOverState --> GameContext : uses
    
    GameContext --> GameEntities : aggregates
    GameContext --> GameServices : aggregates
    GameContext --> ScoreSubject : aggregates
    
    GameEntities --> Ball : contains
    GameEntities --> Paddle : contains
    GameEntities --> Wall : contains
    
    GameServices --> SoundManager : contains
    GameServices --> PowerUpManager : contains
    GameServices --> ActiveEffectManager : contains
    GameServices --> CollisionHandler : contains
    GameServices --> InputHandler : contains
    
    ScoreSubject --> Scoreboard : manages
    ScoreSubject --> Ball : depends on
    ScoreSubject --> SoundManager : uses
    ScoreSubject --> ActiveEffectManager : uses
    ScoreSubject --> PowerUpManager : uses
    
    GameObject <|-- Ball : extends
    GameObject <|-- Paddle : extends
    GameObject --> IComponent : contains
    IComponent <|.. TransformComponent : implements
    IComponent <|.. MovementComponent : implements
    IComponent <|.. RenderComponent : implements
    
    Ball --> TransformComponent : uses
    Ball --> MovementComponent : uses
    Ball --> RenderComponent : uses
    Ball --> Vector2D : uses
    Paddle --> TransformComponent : uses
    Paddle --> RenderComponent : uses
    
    ICommand <|.. MoveUpCommand : implements
    ICommand <|.. MoveDownCommand : implements
    MoveUpCommand --> Paddle : controls
    MoveDownCommand --> Paddle : controls
    
    GameEntityFactory --> Ball : creates
    GameEntityFactory --> Paddle : creates
    GameEntityFactory --> Wall : creates
    GameEntityFactory --> Scoreboard : creates
    
    ScoreSubject --> UIScoreObserver : notifies
    UIScoreObserver <|-- ScoreDisplayObserver : extends
    UIScoreObserver <|-- GameStatusObserver : extends
    
    IEffect <|.. SpeedBoostEffect : implements
    IEffect <|.. PaddleExtendEffect : implements
    SpeedBoostEffect --> Ball : modifies
    PaddleExtendEffect --> Paddle : modifies
    
    PowerUpFactory --> PowerUp : creates
    EffectFactory --> IEffect : creates
    PowerUpManager --> PowerUpFactory : uses
    PowerUpManager --> PowerUp : manages
    ActiveEffectManager --> EffectFactory : uses
    ActiveEffectManager --> IEffect : manages
    
    CollisionHandler --> GameContext : uses
    InputHandler --> Paddle : controls
```

## 2. Sequence Diagram - Game Initialization

```mermaid
sequenceDiagram
    participant Main as Program.Main()
    participant GM as GameManager
    participant Factory as GameEntityFactory
    participant Context as GameContext
    participant SM as StateMachine
    participant MS as MenuState
    
    Main->>GM: Instance (Singleton)
    activate GM
    GM-->>Main: return instance
    deactivate GM
    
    Main->>GM: InitializeGame()
    activate GM
    
    GM->>Factory: CreateBall(width, height)
    Factory-->>GM: Ball
    
    GM->>Factory: CreatePaddle(x1, y1)
    Factory-->>GM: LeftPaddle
    
    GM->>Factory: CreatePaddle(x2, y2)
    Factory-->>GM: RightPaddle
    
    GM->>Factory: CreateScoreboard()
    Factory-->>GM: Scoreboard
    
    GM->>Context: new GameContext(entities, services)
    Context-->>GM: context
    
    GM->>Context: InitializeScoreSubject()
    activate Context
    Context->>Context: Attach observers
    deactivate Context
    
    GM->>Factory: CreateWalls(num, distance, height)
    Factory-->>GM: List<Wall>
    
    GM->>SM: new StateMachine()
    SM-->>GM: stateMachine
    
    GM->>MS: new MenuState(context, stateMachine)
    MS-->>GM: menuState
    
    GM->>SM: ChangeState(menuState)
    activate SM
    SM->>MS: Enter()
    deactivate SM
    
    deactivate GM
```

## 3. Sequence Diagram - Game Loop (Play State)

```mermaid
sequenceDiagram
    participant Main as Program.Main()
    participant GM as GameManager
    participant SM as StateMachine
    participant PS as PlayState
    participant IH as InputHandler
    participant CH as CollisionHandler
    participant PM as PowerUpManager
    participant AEM as ActiveEffectManager
    participant Ball as Ball
    participant Paddle as Paddle
    
    loop Game Loop
        Main->>GM: Update(deltaTime)
        activate GM
        
        GM->>SM: Update(deltaTime)
        activate SM
        
        SM->>PS: Update(deltaTime)
        activate PS
        
        PS->>IH: HandleInput()
        activate IH
        IH->>Paddle: MoveUp() / MoveDown()
        deactivate IH
        
        PS->>Ball: Move()
        
        PS->>CH: CheckCollisions()
        activate CH
        CH->>Ball: Bounce(normal)
        deactivate CH
        
        PS->>PM: UpdatePowerUps(deltaTime)
        activate PM
        PM->>PM: CheckCollisions(ball)
        alt PowerUp collected
            PM->>AEM: AddEffect(effect)
        end
        deactivate PM
        
        PS->>AEM: UpdateEffects(deltaTime)
        activate AEM
        AEM->>AEM: Remove expired effects
        deactivate AEM
        
        deactivate PS
        deactivate SM
        
        GM->>GM: Render()
        
        deactivate GM
    end
```

## 4. State Machine Diagram

```mermaid
stateDiagram-v2
    [*] --> MenuState: Game Start
    
    MenuState --> PlayState: Press SPACE
    MenuState --> MenuState: Arrow Keys (Select Difficulty)
    
    PlayState --> PlayState: Game Running
    PlayState --> GameOverState: Player Reaches 5 Points
    PlayState --> MenuState: Press ESC
    
    GameOverState --> MenuState: Press SPACE
    GameOverState --> GameOverState: Display Winner
    
    MenuState --> [*]: Close Window
    
    note right of MenuState
        - Display Menu
        - Select Difficulty
        - Handle Input
    end note
    
    note right of PlayState
        - Update Ball
        - Handle Input
        - Check Collisions
        - Update PowerUps
        - Update Effects
        - Check Scoring
    end note
    
    note right of GameOverState
        - Display Winner
        - Display Final Score
        - Wait for Input
    end note
```

## 5. Component Pattern Structure

```mermaid
classDiagram
    class IComponent {
        <<interface>>
        +Update(deltaTime)
    }
    
    class GameObject {
        <<abstract>>
        #_components: List~IComponent~
        +AddComponent(component)
        +RemoveComponent(component)
        +GetComponent~T~()
        +Update(deltaTime)
        +Draw()*
    }
    
    class TransformComponent {
        +X: float
        +Y: float
        +Width: float
        +Height: float
        +Update(deltaTime)
    }
    
    class MovementComponent {
        -_transform: TransformComponent
        -_speed: float
        +Speed: float
        +Update(deltaTime)
    }
    
    class RenderComponent {
        -_transform: TransformComponent
        -_color: Color
        -_isCircle: bool
        +Color: Color
        +Update(deltaTime)
    }
    
    class Ball {
        -_transform: TransformComponent
        -_movement: MovementComponent
        -_render: RenderComponent
        -_velocity: Vector2D
        +Move()
        +Bounce(normal)
        +Draw()
    }
    
    class Paddle {
        -_transform: TransformComponent
        -_render: RenderComponent
        +MoveUp()
        +MoveDown()
        +Draw()
    }
    
    GameObject --> IComponent : contains
    IComponent <|.. TransformComponent
    IComponent <|.. MovementComponent
    IComponent <|.. RenderComponent
    GameObject <|-- Ball
    GameObject <|-- Paddle
    Ball --> TransformComponent
    Ball --> MovementComponent
    Ball --> RenderComponent
    Paddle --> TransformComponent
    Paddle --> RenderComponent
```

## 6. Observer Pattern - Score System

```mermaid
classDiagram
    class ScoreSubject {
        -_observers: List~UIScoreObserver~
        -_scoreboard: Scoreboard
        -_ball: Ball
        -_soundManager: SoundManager
        -_activeEffectManager: ActiveEffectManager
        -_powerUpManager: PowerUpManager
        +LeftScore: int
        +RightScore: int
        +GameStarted: bool
        +Attach(observer)
        +Detach(observer)
        +Notify()
        +LeftPoint()
        +RightPoint()
        +Start()
        +Reset()
        +CheckBallOutOfBounds(): int
        +InjectDependencies(ball, soundManager, effectManager, powerUpManager)
    }
    
    class UIScoreObserver {
        <<abstract>>
        +Update(subject: ScoreSubject)*
    }
    
    class ScoreDisplayObserver {
        +Update(subject)
    }
    
    class GameStatusObserver {
        +Update(subject)
    }
    
    class Scoreboard {
        +LeftScore: int
        +RightScore: int
        +LeftPoint()
        +RightPoint()
        +Start()
        +Reset()
    }
    
    ScoreSubject --> UIScoreObserver : notifies
    ScoreSubject --> Scoreboard : manages
    UIScoreObserver <|-- ScoreDisplayObserver : extends
    UIScoreObserver <|-- GameStatusObserver : extends
```

## 7. Combine Pattern - GameContext Aggregation

```mermaid
classDiagram
    class GameContext {
        +Entities: GameEntities
        +Services: GameServices
        +ScoreSubject: ScoreSubject
        +WindowWidth: int
        +WindowHeight: int
        +InitializeScoreSubject()
    }
    
    class GameEntities {
        +Ball: Ball
        +LeftPaddle: Paddle
        +RightPaddle: Paddle
        +Walls: List~Wall~
        +ResetPositions()
    }
    
    class GameServices {
        +SoundManager: SoundManager
        +PowerUpManager: PowerUpManager
        +ActiveEffectManager: ActiveEffectManager
        +CollisionHandler: CollisionHandler
        +InputHandler: InputHandler
        +ClearAll()
    }
    
    class ScoreSubject {
        -_observers: List~UIScoreObserver~
        -_scoreboard: Scoreboard
        +LeftScore: int
        +RightScore: int
        +Notify()
        +CheckBallOutOfBounds(): int
    }
    
    class Ball
    class Paddle
    class Wall
    class SoundManager
    class PowerUpManager
    class ActiveEffectManager
    class CollisionHandler
    class InputHandler
    
    GameContext *-- GameEntities : aggregates
    GameContext *-- GameServices : aggregates
    GameContext *-- ScoreSubject : aggregates
    
    GameEntities o-- Ball : contains
    GameEntities o-- Paddle : contains
    GameEntities o-- Wall : contains
    
    GameServices o-- SoundManager : contains
    GameServices o-- PowerUpManager : contains
    GameServices o-- ActiveEffectManager : contains
    GameServices o-- CollisionHandler : contains
    GameServices o-- InputHandler : contains
    
    note for GameContext "Aggregates all game components\nfrom Combine folder:\n- GameEntities\n- GameServices\n- ScoreSubject"
    
    note for GameEntities "Container for all game entities\nProvides ResetPositions() method"
    
    note for GameServices "Container for all game services\nProvides ClearAll() method"
```

## 8. Factory Pattern Hierarchy

```mermaid
classDiagram
    class GameEntityFactory {
        +CreateBall(width, height): Ball
        +CreatePaddle(x, y, height): Paddle
        +CreateScoreboard(): Scoreboard
        +CreateWalls(num, distance, height): List~Wall~
    }
    
    class PowerUpFactory {
        -_random: Random
        +CreatePowerUp(type, x, y): PowerUp
        +CreateRandomPowerUp(windowWidth, windowHeight): PowerUp
    }
    
    class EffectFactory {
        +CreateEffect(type, ball, leftPaddle, rightPaddle): IEffect
    }
    
    class Ball {
        +X: float
        +Y: float
        +Move()
        +Bounce(normal)
    }
    
    class Paddle {
        +X: float
        +Y: float
        +MoveUp()
        +MoveDown()
    }
    
    class Wall {
        +X: float
        +Y: float
        +IsColliding(ball): bool
    }
    
    class Scoreboard {
        +LeftScore: int
        +RightScore: int
    }
    
    class PowerUp {
        +Type: PowerUpType
        +X: float
        +Y: float
        +IsColliding(ball): bool
    }
    
    class IEffect {
        <<interface>>
        +Apply()
        +Remove()
        +IsExpired(): bool
    }
    
    GameEntityFactory ..> Ball : creates
    GameEntityFactory ..> Paddle : creates
    GameEntityFactory ..> Wall : creates
    GameEntityFactory ..> Scoreboard : creates
    PowerUpFactory ..> PowerUp : creates
    EffectFactory ..> IEffect : creates
```

## 9. Command Pattern Structure

```mermaid
classDiagram
    class ICommand {
        <<interface>>
        +Execute()
        +Undo()
    }
    
    class MoveUpCommand {
        -_paddle: Paddle
        +Execute()
        +Undo()
    }
    
    class MoveDownCommand {
        -_paddle: Paddle
        +Execute()
        +Undo()
    }
    
    class InputHandler {
        -_leftPaddle: Paddle
        -_rightPaddle: Paddle
        -_leftUpCommand: ICommand
        -_leftDownCommand: ICommand
        -_rightUpCommand: ICommand
        -_rightDownCommand: ICommand
        +HandleInput()
    }
    
    class Paddle {
        +MoveUp()
        +MoveDown()
        +ResetSpeed()
    }
    
    ICommand <|.. MoveUpCommand : implements
    ICommand <|.. MoveDownCommand : implements
    MoveUpCommand --> Paddle : controls
    MoveDownCommand --> Paddle : controls
    InputHandler --> ICommand : uses
    InputHandler --> Paddle : manages
```

## 10. Decorator Pattern - PowerUps and Effects

```mermaid
classDiagram
    class IEffect {
        <<interface>>
        +Apply()
        +Remove()
        +IsExpired(): bool
        +Update(deltaTime)
    }
    
    class SpeedBoostEffect {
        -_ball: Ball
        -_originalSpeed: float
        -_startTime: DateTime
        -_duration: double
        +Apply()
        +Remove()
        +IsExpired(): bool
    }
    
    class SlowDownEffect {
        -_ball: Ball
        -_originalSpeed: float
        -_startTime: DateTime
        -_duration: double
        +Apply()
        +Remove()
        +IsExpired(): bool
    }
    
    class PaddleExtendEffect {
        -_paddle: Paddle
        -_originalHeight: int
        -_startTime: DateTime
        -_duration: double
        +Apply()
        +Remove()
        +IsExpired(): bool
    }
    
    class PaddleShrinkEffect {
        -_paddle: Paddle
        -_originalHeight: int
        -_startTime: DateTime
        -_duration: double
        +Apply()
        +Remove()
        +IsExpired(): bool
    }
    
    class PowerUp {
        +Type: PowerUpType
        +X: float
        +Y: float
        -_color: Color
        -_spawnTime: DateTime
        +Draw()
        +IsColliding(ball): bool
        +IsExpired(): bool
    }
    
    class PowerUpType {
        <<enumeration>>
        SPEED_BOOST
        SLOW_DOWN
        PADDLE_EXTEND
        PADDLE_SHRINK
        MULTI_BALL
    }
    
    class EffectFactory {
        +CreateEffect(type, ball, leftPaddle, rightPaddle): IEffect
    }
    
    class ActiveEffectManager {
        -_activeEffects: List~IEffect~
        -_ball: Ball
        -_leftPaddle: Paddle
        -_rightPaddle: Paddle
        +AddEffect(effect)
        +UpdateEffects(deltaTime)
        +ClearEffects()
    }
    
    IEffect <|.. SpeedBoostEffect
    IEffect <|.. SlowDownEffect
    IEffect <|.. PaddleExtendEffect
    IEffect <|.. PaddleShrinkEffect
    PowerUp --> PowerUpType : has
    EffectFactory ..> IEffect : creates
    ActiveEffectManager --> IEffect : manages
    SpeedBoostEffect --> Ball : modifies
    SlowDownEffect --> Ball : modifies
    PaddleExtendEffect --> Paddle : modifies
    PaddleShrinkEffect --> Paddle : modifies
```

## 11. Package/Module Diagram

```mermaid
graph TB
    subgraph Core["Core (Singleton + State Pattern)"]
        GM[GameManager]
        GC[GameContext]
        SM[StateMachine]
        MS[MenuState]
        PS[PlayState]
        GOS[GameOverState]
    end
    
    subgraph Combine["Combine (Aggregation Pattern)"]
        GE[GameEntities]
        GS[GameServices]
        SS[ScoreSubject]
    end
    
    subgraph Entities["Entities (Component Pattern)"]
        Ball[Ball]
        Paddle[Paddle]
        Wall[Wall]
    end
    
    subgraph Components["Components"]
        TC[TransformComponent]
        MC[MovementComponent]
        RC[RenderComponent]
        GO[GameObject]
    end
    
    subgraph Commands["Commands (Command Pattern)"]
        IC[ICommand]
        MUC[MoveUpCommand]
        MDC[MoveDownCommand]
    end
    
    subgraph Factories["Factories (Factory Pattern)"]
        GEF[GameEntityFactory]
        PUF[PowerUpFactory]
        EF[EffectFactory]
    end
    
    subgraph Observers["Observers (Observer Pattern)"]
        USO[UIScoreObserver]
        SDO[ScoreDisplayObserver]
        GSO[GameStatusObserver]
    end
    
    subgraph Decorators["Decorators (Decorator/Strategy)"]
        PU[PowerUp]
        IE[IEffect]
        SBE[SpeedBoostEffect]
        PEE[PaddleExtendEffect]
    end
    
    subgraph Services["Services"]
        CH[CollisionHandler]
        IH[InputHandler]
        SMG[SoundManager]
        PUM[PowerUpManager]
        AEM[ActiveEffectManager]
    end
    
    subgraph Models["Models & ValueObjects"]
        SB[Scoreboard]
        V2D[Vector2D]
    end
    
    subgraph UI["UI"]
        GU[GameUI]
        UR[UIRenderer]
    end
    
    Core --> Combine
    Core --> Factories
    Core --> UI
    
    Combine --> Entities
    Combine --> Services
    Combine --> Models
    
    GC -.->|aggregates| GE
    GC -.->|aggregates| GS
    GC -.->|aggregates| SS
    
    GE --> Ball
    GE --> Paddle
    GE --> Wall
    
    GS --> SMG
    GS --> PUM
    GS --> AEM
    GS --> CH
    GS --> IH
    
    SS --> SB
    SS --> Observers
    
    Entities --> Components
    Entities --> Models
    
    Services --> Entities
    Services --> Decorators
    Services --> Commands
    Services --> Factories
    
    Observers --> Models
    
    Factories --> Entities
    Factories --> Decorators
    Factories --> Models
    
    Commands --> Entities
    
    style Core fill:#e1f5ff
    style Combine fill:#fffacd
    style Entities fill:#fff4e1
    style Components fill:#f0f0f0
    style Commands fill:#e8f5e9
    style Factories fill:#fff3e0
    style Observers fill:#f3e5f5
    style Decorators fill:#fce4ec
    style Services fill:#e0f2f1
    style Models fill:#f9fbe7
    style UI fill:#ede7f6
```
    
    Factories --> Entities
    Factories --> Decorators
    Factories --> Models
    
    Commands --> Entities
    
    style Core fill:#e1f5ff
    style Combine fill:#fffacd
    style Entities fill:#fff4e1
    style Components fill:#f0f0f0
    style Commands fill:#e8f5e9
    style Factories fill:#fff3e0
    style Observers fill:#f3e5f5
    style Decorators fill:#fce4ec
    style Services fill:#e0f2f1
    style Models fill:#f9fbe7
    style UI fill:#ede7f6
```

## Design Patterns Summary

### 1. **Singleton Pattern**
- `GameManager` - Ensures only one instance exists

### 2. **State Pattern**
- `IGameState` interface
- States: `MenuState`, `PlayState`, `GameOverState`
- Managed by `StateMachine`

### 3. **Observer Pattern**
- `ScoreSubject` notifies observers when score changes
- Observers: `UIScoreObserver` (abstract base class)
  - `ScoreDisplayObserver` - Updates UI display
  - `GameStatusObserver` - Handles game status changes

### 4. **Component Pattern**
- `GameObject` base class with `IComponent` list
- Components: `TransformComponent`, `MovementComponent`, `RenderComponent`
- Used by `Ball` and `Paddle`

### 5. **Command Pattern**
- `ICommand` interface
- Commands: `MoveUpCommand`, `MoveDownCommand`
- Executed by `InputHandler`

### 6. **Factory Pattern**
- `GameEntityFactory` - Creates game entities
- `PowerUpFactory` - Creates power-ups
- `EffectFactory` - Creates effects

### 7. **Strategy Pattern**
- `IEffect` interface with different strategies
- Effects: `SpeedBoostEffect`, `SlowDownEffect`, `PaddleExtendEffect`, `PaddleShrinkEffect`

### 8. **Decorator Pattern**
- Power-ups temporarily modify entity behavior
- Effects are applied/removed dynamically

### 9. **Dependency Injection**
- Services injected into `GameContext`
- Dependencies passed through constructors

### 10. **Value Object Pattern**
- `Vector2D` - Immutable value object for 2D vectors
- Provides mathematical operations

### 11. **Combine/Aggregation Pattern** ⭐
- `GameContext` aggregates all game components from **Combine** folder
- **`GameEntities`** - Container for all game entities (Ball, Paddles, Walls)
  - Provides `ResetPositions()` method
- **`GameServices`** - Container for all game services (Managers, Handlers)
  - Provides `ClearAll()` method
- **`ScoreSubject`** - Observer Pattern implementation for score management
  - Manages scoreboard and notifies observers
  - Integrates with game services
- **Benefits:**
  - Better organization and separation of concerns
  - Cleaner `GameContext` interface
  - Easier to maintain and extend
  - Clear responsibility boundaries

