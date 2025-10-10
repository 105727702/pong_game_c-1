# UML Class Diagram - Pong Game Project (Mermaid)

## Design Patterns Overview

This project implements 7 core design patterns: Singleton, State, Component, Decorator, Factory, Observer, and Command.

---

## 1. Singleton Pattern - GameManager

```mermaid
classDiagram
    class GameManager {
        <<singleton>>
        -GameManager _instance
        -object _lock
        +GameManager Instance
        -GameManager()
        +InitializeGame()
        +Run()
        +Update(deltaTime)
        +HandleInput()
    }
```

---

## 2. State Pattern

```mermaid
classDiagram
    class IGameState {
        <<interface>>
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    class StateMachine {
        -IGameState _currentState
        -Dictionary~string,IGameState~ _states
        +AddState(name, state)
        +ChangeState(stateName)
        +Update(deltaTime)
        +GetCurrentState() IGameState
    }
    
    class MenuState {
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    class PlayState {
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    class GameOverState {
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    IGameState <|.. MenuState
    IGameState <|.. PlayState
    IGameState <|.. GameOverState
    StateMachine o-- IGameState
```

---

## 3. Component Pattern

```mermaid
classDiagram
    class IComponent {
        <<interface>>
        +Update(deltaTime)
    }
    
    class GameObject {
        <<abstract>>
        #List~IComponent~ _components
        +AddComponent(component)
        +RemoveComponent(component)
        +GetComponent~T~() T
        +Update(deltaTime)
        +Draw()*
    }
    
    class TransformComponent {
        +float X
        +float Y
        +float Width
        +float Height
        +GetBounds() Rectangle
        +Update(deltaTime)
    }
    
    class MovementComponent {
        +Vector2D Velocity
        +float Speed
        +Update(deltaTime)
        +SetVelocity(x, y)
    }
    
    class RenderComponent {
        +Color Color
        +bool IsCircle
        +Update(deltaTime)
        +Draw()
    }
    
    class CollisionComponent {
        +CheckCollision(other) bool
        +Update(deltaTime)
    }
    
    class Ball {
        +float X
        +float Y
        +int Size
        +Vector2D Velocity
        +Move()
        +Bounce(normal)
        +ResetPosition()
    }
    
    class Paddle {
        +float X
        +float Y
        +int Width
        +int Height
        +float Speed
        +MoveUp()
        +MoveDown()
        +ResetSpeed()
    }
    
    class Wall {
        +float X
        +float Y
        +int Width
        +int Height
        +Move()
        +Draw()
    }
    
    IComponent <|.. TransformComponent
    IComponent <|.. MovementComponent
    IComponent <|.. RenderComponent
    IComponent <|.. CollisionComponent
    GameObject *-- IComponent
    GameObject <|-- Ball
    GameObject <|-- Paddle
    GameObject <|-- Wall
```

---

## 4. Decorator Pattern

```mermaid
classDiagram
    class IGameEntity {
        <<interface>>
        +GetSpeed() float
        +GetSize() float
        +Update(deltaTime)
    }
    
    class EntityDecorator {
        <<abstract>>
        #IGameEntity _wrappedEntity
        +GetSpeed() float
        +GetSize() float
        +Update(deltaTime)
    }
    
    class SpeedBoostDecorator {
        -float _speedMultiplier
        -DateTime _startTime
        -double _duration
        +GetSpeed() float
        +IsActive() bool
    }
    
    class SpeedReductionDecorator {
        -float _speedMultiplier
        -DateTime _startTime
        -double _duration
        +GetSpeed() float
        +IsActive() bool
    }
    
    class SizeBoostDecorator {
        -float _sizeMultiplier
        -DateTime _startTime
        -double _duration
        +GetSize() float
        +IsActive() bool
    }
    
    IGameEntity <|.. EntityDecorator
    IGameEntity <|.. Ball
    IGameEntity <|.. Paddle
    EntityDecorator <|-- SpeedBoostDecorator
    EntityDecorator <|-- SpeedReductionDecorator
    EntityDecorator <|-- SizeBoostDecorator
    EntityDecorator o-- IGameEntity
```

---

## 5. Factory Pattern

```mermaid
classDiagram
    class IGameEntityFactory {
        <<interface>>
        +CreateBall(windowWidth, windowHeight) Ball
        +CreatePaddle(x, y, windowHeight) Paddle
        +CreateWall(x, y, windowHeight, speedMultiplier) Wall
        +CreateScoreboard() Scoreboard
        +CreateWalls(numWalls, minDistance, windowHeight) List~Wall~
        +CalculateWallCount(totalScore, baseWalls) int
    }
    
    class GameEntityFactory {
        +CreateBall(windowWidth, windowHeight) Ball
        +CreatePaddle(x, y, windowHeight) Paddle
        +CreateWall(x, y, windowHeight, speedMultiplier) Wall
        +CreateScoreboard() Scoreboard
        +CreateWalls(numWalls, minDistance, windowHeight) List~Wall~
        +CalculateWallCount(totalScore, baseWalls) int
    }
    
    IGameEntityFactory <|.. GameEntityFactory
```

---

## 6. Observer Pattern

```mermaid
classDiagram
    class ISubject {
        <<interface>>
        +Attach(observer)
        +Detach(observer)
        +Notify()
    }
    
    class IObserver {
        <<interface>>
        +Update(subject)
    }
    
    class ScoreSubject {
        -List~IObserver~ _observers
        -Scoreboard _scoreboard
        +Attach(observer)
        +Detach(observer)
        +Notify()
        +IncrementLeftScore()
        +IncrementRightScore()
        +GetScoreboard() Scoreboard
    }
    
    class SoundScoreObserver {
        -SoundManager _soundManager
        +Update(subject)
    }
    
    class WallScoreObserver {
        -GameContext _context
        -IGameEntityFactory _factory
        +Update(subject)
    }
    
    class EffectScoreObserver {
        -ActiveEffectManager _effectManager
        +Update(subject)
    }
    
    ISubject <|.. ScoreSubject
    IObserver <|.. SoundScoreObserver
    IObserver <|.. WallScoreObserver
    IObserver <|.. EffectScoreObserver
    ScoreSubject o-- IObserver
```

---

## 7. Command Pattern

```mermaid
classDiagram
    class ICommand {
        <<interface>>
        +Execute()
        +Undo()
    }
    
    class MoveUpCommand {
        -Paddle _paddle
        +Execute()
        +Undo()
    }
    
    class MoveDownCommand {
        -Paddle _paddle
        +Execute()
        +Undo()
    }
    
    ICommand <|.. MoveUpCommand
    ICommand <|.. MoveDownCommand
```

---

## Complete System Architecture

```mermaid
classDiagram
    class GameManager {
        <<singleton>>
        +GameContext Context
        +StateMachine StateMachine
        +MenuState MenuState
        +PlayState PlayState
        +GameOverState GameOverState
        +InitializeGame()
        +Run()
    }
    
    class GameContext {
        +Ball Ball
        +Paddle LeftPaddle
        +Paddle RightPaddle
        +ScoreSubject ScoreSubject
        +List~Wall~ Walls
        +SoundManager SoundManager
        +PowerUpManager PowerUpManager
        +ActiveEffectManager ActiveEffectManager
        +InitializeScoreSubject()
    }
    
    class StateMachine {
        -IGameState _currentState
        -Dictionary~string,IGameState~ _states
        +ChangeState(stateName)
        +Update(deltaTime)
    }
    
    class Ball {
        +Move()
        +Bounce(normal)
        +ResetPosition()
    }
    
    class Paddle {
        +MoveUp()
        +MoveDown()
        +ResetPosition()
    }
    
    class Wall {
        +Move()
        +Draw()
    }
    
    class ScoreSubject {
        +IncrementLeftScore()
        +IncrementRightScore()
        +Notify()
    }
    
    class Scoreboard {
        +int LeftScore
        +int RightScore
        +IncrementLeft()
        +IncrementRight()
        +GetTotalScore() int
    }
    
    GameManager *-- GameContext
    GameManager *-- StateMachine
    GameContext *-- Ball
    GameContext *-- Paddle
    GameContext *-- Wall
    GameContext *-- ScoreSubject
    ScoreSubject *-- Scoreboard
```

---

## Core Entities

```mermaid
classDiagram
    class Ball {
        +float X
        +float Y
        +int Size
        +Vector2D Velocity
        +float Speed
        +Color Color
        +Move()
        +Bounce(surfaceNormal)
        +ResetPosition()
        +Update(deltaTime)
        +Draw()
    }
    
    class Paddle {
        +float X
        +float Y
        +int Width
        +int Height
        +float Speed
        +Color Color
        +MoveUp()
        +MoveDown()
        +ResetSpeed()
        +ResetPosition()
        +Update(deltaTime)
        +Draw()
    }
    
    class Wall {
        +float X
        +float Y
        +int Width
        +int Height
        +float Speed
        +Color Color
        +Move()
        +Update(deltaTime)
        +Draw()
    }
    
    class Scoreboard {
        +int LeftScore
        +int RightScore
        +IncrementLeft()
        +IncrementRight()
        +GetTotalScore() int
        +Reset()
    }
    
    class Vector2D {
        +float X
        +float Y
        +float Magnitude
        +Normalize() Vector2D
        +DotProduct(other) float
        +Copy() Vector2D
        +Multiply(scalar) Vector2D
        +Subtract(other) Vector2D
    }
    
    Ball ..> Vector2D
    Paddle ..> Vector2D
```

---

## Services & Managers

```mermaid
classDiagram
    class SoundManager {
        +PlayBounce()
        +PlayScore()
        +PlayPowerUp()
        +PlayGameOver()
    }
    
    class PowerUpManager {
        -List~PowerUp~ _activePowerUps
        +Update(deltaTime)
        +Draw()
        +SpawnPowerUp()
        +CheckCollision(ball) PowerUp
    }
    
    class ActiveEffectManager {
        -Ball _ball
        -Paddle _leftPaddle
        -Paddle _rightPaddle
        +ApplyEffect(type, target)
        +Update(deltaTime)
        +HasActiveEffect() bool
        +ClearAllEffects()
    }
    
    class InputHandler {
        +HandleInput(context)
        +ProcessCommands()
    }
    
    class CollisionHandler {
        +CheckBallPaddleCollision(ball, paddle) bool
        +CheckBallWallCollision(ball, wall) bool
        +HandleCollision(ball, surface)
    }
    
    class UIRenderer {
        +RenderScore(scoreboard)
        +RenderMenu()
        +RenderGameOver(scoreboard)
        +RenderPowerUps()
    }
```

---

## Sequence Diagram - Game Flow

```mermaid
sequenceDiagram
    participant Player
    participant GameManager
    participant StateMachine
    participant IGameState
    participant GameContext
    
    Player->>GameManager: Start Game
    GameManager->>GameManager: InitializeGame()
    GameManager->>StateMachine: ChangeState("Menu")
    StateMachine->>IGameState: Enter() [MenuState]
    
    Player->>GameManager: Press Start
    GameManager->>StateMachine: ChangeState("Play")
    StateMachine->>IGameState: Exit() [MenuState]
    StateMachine->>IGameState: Enter() [PlayState]
    
    loop Game Loop
        Player->>GameManager: Input
        GameManager->>StateMachine: Update(deltaTime)
        StateMachine->>IGameState: Update(deltaTime) [PlayState]
        IGameState->>GameContext: Access Ball, Paddles, Walls
        GameContext-->>IGameState: Return Entities
        IGameState->>IGameState: Process Game Logic
    end
    
    IGameState->>GameManager: Game Over Condition
    GameManager->>StateMachine: ChangeState("GameOver")
    StateMachine->>IGameState: Exit() [PlayState]
    StateMachine->>IGameState: Enter() [GameOverState]
```

---

## State Transition Diagram

```mermaid
stateDiagram-v2
    [*] --> Menu: Game Start
    Menu --> Play: Press Start
    Play --> GameOver: Win Condition
    GameOver --> Menu: Press Restart
    GameOver --> [*]: Exit
    Menu --> [*]: Exit
```

---

## Component Composition

```mermaid
graph TB
    A[GameObject] --> B[TransformComponent]
    A --> C[MovementComponent]
    A --> D[RenderComponent]
    A --> E[CollisionComponent]
    
    F[Ball] -.inherits.-> A
    G[Paddle] -.inherits.-> A
    H[Wall] -.inherits.-> A
    
    B --> B1[Position X, Y]
    B --> B2[Size Width, Height]
    
    C --> C1[Velocity]
    C --> C2[Speed]
    
    D --> D1[Color]
    D --> D2[Shape Type]
    
    E --> E1[Bounds Checking]
```

---

## Observer Pattern Flow

```mermaid
sequenceDiagram
    participant Ball
    participant ScoreSubject
    participant SoundObserver
    participant WallObserver
    participant EffectObserver
    
    Ball->>ScoreSubject: IncrementScore()
    ScoreSubject->>ScoreSubject: Update Score
    ScoreSubject->>SoundObserver: Notify()
    SoundObserver->>SoundObserver: PlayScoreSound()
    ScoreSubject->>WallObserver: Notify()
    WallObserver->>WallObserver: SpawnNewWall()
    ScoreSubject->>EffectObserver: Notify()
    EffectObserver->>EffectObserver: ClearEffects()
```

---

## Design Patterns Summary

| Pattern | Purpose | Key Classes |
|---------|---------|-------------|
| **Singleton** | Single game instance | `GameManager` |
| **State** | Game state management | `IGameState`, `StateMachine`, `MenuState`, `PlayState`, `GameOverState` |
| **Component** | Modular entity composition | `IComponent`, `GameObject`, `TransformComponent`, `MovementComponent`, `RenderComponent` |
| **Decorator** | Dynamic ability addition | `IGameEntity`, `EntityDecorator`, `SpeedBoostDecorator`, `SizeBoostDecorator` |
| **Factory** | Entity creation | `IGameEntityFactory`, `GameEntityFactory` |
| **Observer** | Event notification | `IObserver`, `ISubject`, `ScoreSubject`, `SoundScoreObserver`, `WallScoreObserver` |
| **Command** | Input encapsulation | `ICommand`, `MoveUpCommand`, `MoveDownCommand` |

---

**Generated**: October 10, 2025  
**Project**: C# Pong Game with Design Patterns  
**Framework**: .NET 8.0 with SplashKit  
**Diagrams**: Mermaid (GitHub Compatible)
