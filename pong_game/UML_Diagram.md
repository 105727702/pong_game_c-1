# UML Class Diagram - Pong Game Project

## Design Patterns Implementation

### 1. Singleton Pattern
```
┌─────────────────────────────┐
│      GameManager            │
│    <<singleton>>            │
├─────────────────────────────┤
│ - _instance: GameManager    │
│ - _lock: object             │
│ + Instance: GameManager     │
├─────────────────────────────┤
│ - GameManager()             │
│ + InitializeGame()          │
│ + Run()                     │
│ + Update(deltaTime)         │
│ + HandleInput()             │
└─────────────────────────────┘
```

### 2. State Pattern
```
                    ┌──────────────────┐
                    │   IGameState     │
                    │  <<interface>>   │
                    ├──────────────────┤
                    │ + Enter()        │
                    │ + Update(float)  │
                    │ + Exit()         │
                    └──────────────────┘
                            △
                            │
            ┌───────────────┼───────────────┐
            │               │               │
    ┌───────┴────────┐ ┌───┴──────┐ ┌─────┴──────────┐
    │   MenuState    │ │PlayState │ │ GameOverState  │
    ├────────────────┤ ├──────────┤ ├────────────────┤
    │ + Enter()      │ │+ Enter() │ │ + Enter()      │
    │ + Update()     │ │+ Update()│ │ + Update()     │
    │ + Exit()       │ │+ Exit()  │ │ + Exit()       │
    └────────────────┘ └──────────┘ └────────────────┘

┌─────────────────────────────────────────┐
│         StateMachine                    │
├─────────────────────────────────────────┤
│ - _currentState: IGameState             │
│ - _states: Dictionary<string, IGameState>│
├─────────────────────────────────────────┤
│ + AddState(name, state)                 │
│ + ChangeState(stateName)                │
│ + Update(deltaTime)                     │
│ + GetCurrentState(): IGameState         │
└─────────────────────────────────────────┘
```

### 3. Component Pattern
```
        ┌──────────────────┐
        │   IComponent     │
        │  <<interface>>   │
        ├──────────────────┤
        │ + Update(float)  │
        └──────────────────┘
                △
                │
    ┌───────────┼──────────────────┬─────────────────┐
    │           │                  │                 │
┌───┴────────┐ ┌┴────────────┐ ┌──┴──────────┐ ┌───┴──────────┐
│Transform   │ │Movement     │ │  Render     │ │  Collision   │
│Component   │ │Component    │ │  Component  │ │  Component   │
├────────────┤ ├─────────────┤ ├─────────────┤ ├──────────────┤
│+ X: float  │ │+ Velocity   │ │+ Color      │ │+ CheckColl() │
│+ Y: float  │ │+ Speed      │ │+ IsCircle   │ │              │
│+ Width     │ │+ Update()   │ │+ Draw()     │ │              │
│+ Height    │ │             │ │             │ │              │
└────────────┘ └─────────────┘ └─────────────┘ └──────────────┘

        ┌─────────────────────────────┐
        │      GameObject             │
        │    <<abstract>>             │
        ├─────────────────────────────┤
        │ # _components: List<IComp>  │
        ├─────────────────────────────┤
        │ + AddComponent(comp)        │
        │ + RemoveComponent(comp)     │
        │ + GetComponent<T>(): T      │
        │ + Update(deltaTime)         │
        │ + Draw() <<abstract>>       │
        └─────────────────────────────┘
                     △
                     │
        ┌────────────┼────────────┐
        │            │            │
    ┌───┴───┐   ┌───┴────┐  ┌───┴────┐
    │ Ball  │   │ Paddle │  │  Wall  │
    └───────┘   └────────┘  └────────┘
```

### 4. Decorator Pattern
```
        ┌──────────────────────┐
        │   IGameEntity        │
        │   <<interface>>      │
        ├──────────────────────┤
        │ + GetSpeed(): float  │
        │ + GetSize(): float   │
        │ + Update(deltaTime)  │
        └──────────────────────┘
                △
                │
        ┌───────┴──────────────────┐
        │                           │
┌───────┴────────┐     ┌───────────┴──────────┐
│  Ball, Paddle  │     │  EntityDecorator     │
│                │     │   <<abstract>>       │
└────────────────┘     ├──────────────────────┤
                       │ # _wrappedEntity     │
                       ├──────────────────────┤
                       │ + GetSpeed()         │
                       │ + GetSize()          │
                       │ + Update()           │
                       └──────────────────────┘
                                △
                                │
                ┌───────────────┼───────────────┐
                │               │               │
    ┌───────────┴────┐  ┌──────┴──────┐  ┌────┴─────────┐
    │ SpeedBoost     │  │SpeedReduc.  │  │ SizeBoost    │
    │ Decorator      │  │Decorator    │  │ Decorator    │
    ├────────────────┤  ├─────────────┤  ├──────────────┤
    │- _speedMult    │  │- _speedMult │  │- _sizeMult   │
    │- _duration     │  │- _duration  │  │- _duration   │
    ├────────────────┤  ├─────────────┤  ├──────────────┤
    │+ GetSpeed()    │  │+ GetSpeed() │  │+ GetSize()   │
    │+ IsActive()    │  │+ IsActive() │  │+ IsActive()  │
    └────────────────┘  └─────────────┘  └──────────────┘
```

### 5. Factory Pattern
```
┌──────────────────────────────────────────┐
│      IGameEntityFactory                  │
│        <<interface>>                     │
├──────────────────────────────────────────┤
│ + CreateBall(): Ball                     │
│ + CreatePaddle(x, y): Paddle             │
│ + CreateWall(x, y): Wall                 │
│ + CreateScoreboard(): Scoreboard         │
│ + CreateWalls(num): List<Wall>           │
│ + CalculateWallCount(score): int         │
└──────────────────────────────────────────┘
                    △
                    │
        ┌───────────┴──────────────┐
        │  GameEntityFactory       │
        ├──────────────────────────┤
        │ + CreateBall()           │
        │ + CreatePaddle()         │
        │ + CreateWall()           │
        │ + CreateScoreboard()     │
        │ + CreateWalls()          │
        │ + CalculateWallCount()   │
        └──────────────────────────┘
```

### 6. Observer Pattern
```
┌─────────────────────┐           ┌──────────────────┐
│    ISubject         │           │   IObserver      │
│   <<interface>>     │           │  <<interface>>   │
├─────────────────────┤           ├──────────────────┤
│ + Attach(observer)  │           │ + Update(subject)│
│ + Detach(observer)  │           └──────────────────┘
│ + Notify()          │                    △
└─────────────────────┘                    │
         △                                 │
         │                    ┌────────────┼──────────────┐
         │                    │            │              │
┌────────┴──────────┐  ┌──────┴────┐ ┌────┴──────┐ ┌────┴─────────┐
│  ScoreSubject     │  │SoundScore │ │WallScore  │ │EffectScore   │
├───────────────────┤  │Observer   │ │Observer   │ │Observer      │
│- _observers: List │  ├───────────┤ ├───────────┤ ├──────────────┤
│- _scoreboard      │  │+ Update() │ │+ Update() │ │+ Update()    │
├───────────────────┤  └───────────┘ └───────────┘ └──────────────┘
│+ Attach()         │
│+ Detach()         │
│+ Notify()         │
│+ IncrementScore() │
└───────────────────┘
```

### 7. Command Pattern
```
        ┌──────────────────┐
        │   ICommand       │
        │  <<interface>>   │
        ├──────────────────┤
        │ + Execute()      │
        │ + Undo()         │
        └──────────────────┘
                △
                │
        ┌───────┴──────────┐
        │                  │
┌───────┴────────┐  ┌──────┴──────────┐
│MoveUpCommand   │  │MoveDownCommand  │
├────────────────┤  ├─────────────────┤
│- _paddle       │  │- _paddle        │
├────────────────┤  ├─────────────────┤
│+ Execute()     │  │+ Execute()      │
│+ Undo()        │  │+ Undo()         │
└────────────────┘  └─────────────────┘
```

## Complete Class Relationships

```
┌───────────────────────────────────────────────────────────────┐
│                      GameManager (Singleton)                  │
│                                                               │
│  ┌────────────┐    ┌──────────────┐    ┌────────────────┐   │
│  │StateMachine│───▶│ IGameState   │◁───│MenuState       │   │
│  └────────────┘    └──────────────┘    │PlayState       │   │
│                                         │GameOverState   │   │
│  ┌──────────────┐                      └────────────────┘   │
│  │ GameContext  │                                            │
│  └──────────────┘                                            │
│       │                                                       │
│       ├──▶ Ball ───────────────┐                            │
│       ├──▶ Paddle (Left)       │                            │
│       ├──▶ Paddle (Right)      ├──▶ GameObject ──▶ IGameEntity
│       ├──▶ Wall (List)         │         │                  │
│       ├──▶ ScoreSubject ───────┘         │                  │
│       ├──▶ SoundManager                  │                  │
│       ├──▶ PowerUpManager                ▼                  │
│       └──▶ ActiveEffectManager     IComponent               │
│                                          │                   │
│                                          ├─ TransformComp    │
│                                          ├─ MovementComp     │
│                                          ├─ RenderComp       │
│                                          └─ CollisionComp    │
│                                                               │
│  ┌──────────────────────────┐                               │
│  │ IGameEntityFactory       │                               │
│  └──────────────────────────┘                               │
│              △                                               │
│              │                                               │
│  ┌──────────┴──────────────┐                               │
│  │ GameEntityFactory        │                               │
│  └──────────────────────────┘                               │
└───────────────────────────────────────────────────────────────┘
```

## Core Entities

```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│      Ball       │     │     Paddle      │     │      Wall       │
├─────────────────┤     ├─────────────────┤     ├─────────────────┤
│+ X: float       │     │+ X: float       │     │+ X: float       │
│+ Y: float       │     │+ Y: float       │     │+ Y: float       │
│+ Size: int      │     │+ Width: int     │     │+ Width: int     │
│+ Velocity       │     │+ Height: int    │     │+ Height: int    │
│+ Speed: float   │     │+ Speed: float   │     │+ Speed: float   │
│+ Color          │     │+ Color          │     │+ Color          │
├─────────────────┤     ├─────────────────┤     ├─────────────────┤
│+ Move()         │     │+ MoveUp()       │     │+ Move()         │
│+ Bounce()       │     │+ MoveDown()     │     │+ Draw()         │
│+ ResetPosition()│     │+ ResetSpeed()   │     │+ Update()       │
│+ Update()       │     │+ ResetPosition()│     └─────────────────┘
│+ Draw()         │     │+ Update()       │
└─────────────────┘     │+ Draw()         │
                        └─────────────────┘

┌─────────────────┐     ┌─────────────────┐
│  Scoreboard     │     │   Vector2D      │
├─────────────────┤     ├─────────────────┤
│+ LeftScore      │     │+ X: float       │
│+ RightScore     │     │+ Y: float       │
├─────────────────┤     ├─────────────────┤
│+ IncrementLeft()│     │+ Magnitude      │
│+ IncrementRight()│    │+ Normalize()    │
│+ GetTotal()     │     │+ DotProduct()   │
│+ Reset()        │     │+ Copy()         │
└─────────────────┘     │+ Multiply()     │
                        │+ Subtract()     │
                        └─────────────────┘
```

## Services & Managers

```
┌──────────────────────┐   ┌──────────────────────┐
│   SoundManager       │   │  PowerUpManager      │
├──────────────────────┤   ├──────────────────────┤
│+ PlayBounce()        │   │+ Update()            │
│+ PlayScore()         │   │+ Draw()              │
│+ PlayPowerUp()       │   │+ SpawnPowerUp()      │
│+ PlayGameOver()      │   │+ CheckCollision()    │
└──────────────────────┘   └──────────────────────┘

┌──────────────────────┐   ┌──────────────────────┐
│ ActiveEffectManager  │   │   InputHandler       │
├──────────────────────┤   ├──────────────────────┤
│+ ApplyEffect()       │   │+ HandleInput()       │
│+ Update()            │   │+ ProcessCommands()   │
│+ HasActiveEffect()   │   └──────────────────────┘
└──────────────────────┘

┌──────────────────────┐   ┌──────────────────────┐
│  CollisionHandler    │   │    UIRenderer        │
├──────────────────────┤   ├──────────────────────┤
│+ CheckBallPaddle()   │   │+ RenderScore()       │
│+ CheckBallWall()     │   │+ RenderMenu()        │
│+ HandleCollision()   │   │+ RenderGameOver()    │
└──────────────────────┘   └──────────────────────┘
```

## Design Pattern Summary

| Pattern | Purpose | Classes Involved |
|---------|---------|------------------|
| **Singleton** | Ensure single game manager instance | GameManager |
| **State** | Manage game states (Menu, Play, GameOver) | IGameState, StateMachine, MenuState, PlayState, GameOverState |
| **Component** | Compose game objects from reusable components | IComponent, GameObject, TransformComponent, MovementComponent, RenderComponent |
| **Decorator** | Add temporary effects to entities | IGameEntity, EntityDecorator, SpeedBoostDecorator, SizeBoostDecorator |
| **Factory** | Create game entities | IGameEntityFactory, GameEntityFactory |
| **Observer** | Notify score changes | IObserver, ISubject, ScoreSubject, SoundScoreObserver, WallScoreObserver |
| **Command** | Encapsulate paddle movements | ICommand, MoveUpCommand, MoveDownCommand |

---

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                         Program                             │
│                           │                                 │
│                           ▼                                 │
│                    ┌─────────────┐                          │
│                    │GameManager  │ (Singleton)              │
│                    │(Entry Point)│                          │
│                    └──────┬──────┘                          │
│                           │                                 │
│          ┌────────────────┼────────────────┐               │
│          ▼                ▼                ▼               │
│   ┌─────────────┐  ┌───────────┐   ┌────────────┐        │
│   │StateMachine │  │GameContext│   │   GameUI   │        │
│   └─────────────┘  └───────────┘   └────────────┘        │
│          │                │                                │
│          ▼                ▼                                │
│   ┌──────────┐     ┌───────────┐                         │
│   │  States  │     │ Entities  │                         │
│   │          │     │           │                         │
│   │- Menu    │     │- Ball     │                         │
│   │- Play    │     │- Paddles  │                         │
│   │- GameOver│     │- Walls    │                         │
│   └──────────┘     └───────────┘                         │
│                           │                                │
│                           ▼                                │
│                    ┌────────────┐                         │
│                    │ Components │                         │
│                    │            │                         │
│                    │- Transform │                         │
│                    │- Movement  │                         │
│                    │- Render    │                         │
│                    │- Collision │                         │
│                    └────────────┘                         │
└─────────────────────────────────────────────────────────────┘
```

## Sequence Diagram - Game Flow

```
Player    GameManager   StateMachine   IGameState    GameContext
  │            │             │              │             │
  │──Start──▶  │             │              │             │
  │            │──Init──▶    │              │             │
  │            │             │──Change──▶   │             │
  │            │             │          MenuState         │
  │            │             │              │             │
  │──Input──▶  │             │              │             │
  │            │──Update──▶  │              │             │
  │            │             │──Update──▶   │             │
  │            │             │              │             │
  │──Start──▶  │             │              │             │
  │            │             │──Change──▶ PlayState       │
  │            │             │              │             │
  │            │             │              │──Access──▶  │
  │            │             │              │   Ball,     │
  │            │             │              │   Paddles   │
  │            │◀────────────────────────────────────────│
  │            │             Game Loop                    │
  │            │                                          │
```

---
**Generated**: October 10, 2025  
**Project**: C# Pong Game with Design Patterns  
**Framework**: .NET 8.0 with SplashKit
