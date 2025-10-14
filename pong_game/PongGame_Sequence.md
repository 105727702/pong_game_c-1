# Pong Game - Sequence Diagrams

## 1. Game Initialization Sequence

```mermaid
sequenceDiagram
    participant Main as Program.Main
    participant GM as GameManager
    participant SM as StateMachine
    participant Factory as GameEntityFactory
    participant Context as GameContext
    participant MS as MenuState
    participant PS as PlayState
    participant GOS as GameOverState

    Main->>GM: Instance (Singleton)
    GM->>GM: Constructor (private)
    GM->>Factory: new GameEntityFactory()
    GM->>Context: new GameContext()
    GM->>SM: new StateMachine()
    
    Main->>GM: InitializeGame()
    GM->>MS: new MenuState(context)
    GM->>PS: new PlayState(context)
    GM->>GOS: new GameOverState(context)
    GM->>SM: AddState("Menu", menuState)
    GM->>SM: AddState("Play", playState)
    GM->>SM: AddState("GameOver", gameOverState)
    GM->>SM: ChangeState("Menu")
    SM->>MS: Enter()
    
    Main->>GM: StartGame()
    activate GM
    loop Game Loop
        GM->>SM: Update()
        SM->>MS: Update()
        MS-->>SM: Current State Logic
    end
    deactivate GM
```

## 2. Start New Game Sequence

```mermaid
sequenceDiagram
    participant User
    participant UI as GameUI
    participant MS as MenuState
    participant SM as StateMachine
    participant PS as PlayState
    participant Factory as GameEntityFactory
    participant Context as GameContext
    participant Entities as GameEntities
    participant ScoreSubject as ScoreSubject
    participant Sound as SoundManager

    User->>UI: Click "Start Game"
    UI->>MS: StartNewGame()
    MS->>SM: ChangeState("Play")
    SM->>MS: Exit()
    MS->>Sound: StopMusic()
    
    SM->>PS: Enter()
    activate PS
    
    PS->>Factory: CreateBall(width, height)
    Factory-->>PS: ball
    PS->>Factory: CreatePaddle(x, y, width, height, isLeft)
    Factory-->>PS: leftPaddle
    PS->>Factory: CreatePaddle(x, y, width, height, isLeft)
    Factory-->>PS: rightPaddle
    PS->>Factory: CreateWalls(width, height, wallCount)
    Factory-->>PS: walls[]
    PS->>Factory: CreateScoreboard()
    Factory-->>PS: scoreboard
    
    PS->>Context: Set Entities (ball, paddles, walls)
    PS->>Entities: Store entities
    
    PS->>ScoreSubject: new ScoreSubject(scoreboard)
    PS->>ScoreSubject: Attach(UIScoreObserver)
    PS->>ScoreSubject: Attach(ConsoleScoreObserver)
    PS->>ScoreSubject: InjectDependencies(ball, soundManager, activeEffectManager)
    
    PS->>PS: Initialize InputHandler
    PS->>PS: Initialize CollisionHandler
    
    deactivate PS
```

## 3. Gameplay Loop Sequence

```mermaid
sequenceDiagram
    participant SM as StateMachine
    participant PS as PlayState
    participant IH as InputHandler
    participant Ball
    participant Paddle
    participant Wall
    participant CH as CollisionHandler
    participant PUM as PowerUpManager
    participant AEM as ActiveEffectManager
    participant SS as ScoreSubject
    participant Sound as SoundManager

    loop Every Frame
        SM->>PS: Update()
        activate PS
        
        PS->>IH: HandleKeyInput()
        IH->>IH: UpdatePaddleMovement()
        alt W Key Pressed
            IH->>Paddle: MoveUpCommand.Execute()
        else S Key Pressed
            IH->>Paddle: MoveDownCommand.Execute()
        else Up Arrow Pressed
            IH->>Paddle: MoveUpCommand.Execute()
        else Down Arrow Pressed
            IH->>Paddle: MoveDownCommand.Execute()
        end
        
        PS->>Ball: Move()
        Ball->>Ball: Update velocity
        
        loop For each Wall
            PS->>Wall: Move()
            Wall->>Wall: Update position
        end
        
        PS->>CH: HandleCollisions(ball, paddles, walls)
        CH->>CH: CheckCollision(ball, paddle)
        alt Ball hits Paddle
            CH->>Ball: Bounce()
            CH->>Sound: PlayEffect(PaddleHit)
            CH->>PUM: SpawnRandomPowerUp()
        end
        
        CH->>CH: CheckCollision(ball, wall)
        alt Ball hits Wall
            CH->>Ball: Bounce()
            CH->>Sound: PlayEffect(WallHit)
        end
        
        PS->>PUM: Update()
        PUM->>PUM: Remove expired power-ups
        PUM->>PUM: CheckCollisions(ball)
        alt Ball collects PowerUp
            PUM->>AEM: ApplyEffect(type, paddle)
            AEM->>AEM: ActivateEffect()
            PUM->>Sound: PlayEffect(PotionEffect)
        end
        
        PS->>AEM: Update()
        AEM->>AEM: Check expired effects
        alt Effect expired
            AEM->>AEM: DeactivateEffect()
        end
        
        PS->>SS: CheckBallOutOfBounds()
        alt Ball out of bounds (left)
            SS->>SS: RightPoint()
            SS->>SS: Notify()
            SS->>Sound: PlayEffect(BallOut)
            SS->>Ball: ResetPosition()
            SS->>AEM: ClearAllEffects()
        else Ball out of bounds (right)
            SS->>SS: LeftPoint()
            SS->>SS: Notify()
            SS->>Sound: PlayEffect(BallOut)
            SS->>Ball: ResetPosition()
            SS->>AEM: ClearAllEffects()
        end
        
        alt Score >= 5
            PS->>SM: ChangeState("GameOver")
        end
        
        deactivate PS
    end
```

## 4. Input Command Pattern Sequence

```mermaid
sequenceDiagram
    participant User
    participant SplashKit
    participant IH as InputHandler
    participant CMD as ICommand
    participant MoveUp as MoveUpCommand
    participant MoveDown as MoveDownCommand
    participant Stop as StopPaddleCommand
    participant Paddle

    User->>SplashKit: Press Key (W/S/Up/Down)
    SplashKit->>IH: KeyDown(keyCode)
    
    IH->>IH: HandleKeyInput()
    IH->>IH: Lookup command in _keyBindings
    
    alt W Key (Left Paddle Up)
        IH->>MoveUp: Execute()
        MoveUp->>Paddle: Store previousY
        MoveUp->>Paddle: MoveUp()
    else S Key (Left Paddle Down)
        IH->>MoveDown: Execute()
        MoveDown->>Paddle: Store previousY
        MoveDown->>Paddle: MoveDown()
    else Up Arrow (Right Paddle Up)
        IH->>MoveUp: Execute()
        MoveUp->>Paddle: Store previousY
        MoveUp->>Paddle: MoveUp()
    else Down Arrow (Right Paddle Down)
        IH->>MoveDown: Execute()
        MoveDown->>Paddle: Store previousY
        MoveDown->>Paddle: MoveDown()
    end
    
    User->>SplashKit: Release Key
    SplashKit->>IH: KeyUp(keyCode)
    IH->>IH: UpdatePaddleMovement()
    
    alt No keys pressed for paddle
        IH->>Stop: Execute()
        Stop->>Paddle: Store previousSpeed
        Stop->>Paddle: Y = previousY (reset position)
    end
    
    Note over CMD,Paddle: Undo support (optional)
    opt Undo requested
        alt MoveUp Undo
            MoveUp->>Paddle: Y = previousY
        else MoveDown Undo
            MoveDown->>Paddle: Y = previousY
        else Stop Undo
            Stop->>Paddle: Speed = previousSpeed
        end
    end
```

## 5. Observer Pattern - Score Update Sequence

```mermaid
sequenceDiagram
    participant Ball
    participant SS as ScoreSubject
    participant Scoreboard
    participant UIObserver as UIScoreObserver
    participant ConsoleObserver as ConsoleScoreObserver
    participant Sound as SoundManager
    participant AEM as ActiveEffectManager

    Ball->>SS: CheckBallOutOfBounds()
    
    alt Ball.X < 0 (Right player scores)
        SS->>SS: RightPoint()
        SS->>Scoreboard: RightPoint()
        Scoreboard->>Scoreboard: RightScore++
        
        SS->>SS: Notify()
        activate SS
        
        par Notify all observers
            SS->>UIObserver: Update(scoreSubject)
            UIObserver->>SS: Get LeftScore, RightScore
            UIObserver->>UIObserver: Update local state
            UIObserver->>UIObserver: DrawScore() next frame
        and
            SS->>ConsoleObserver: Update(scoreSubject)
            ConsoleObserver->>SS: Get LeftScore, RightScore
            ConsoleObserver->>ConsoleObserver: Console.WriteLine(score)
        end
        
        deactivate SS
        
        SS->>Sound: PlayEffect(BallOut)
        SS->>Ball: ResetPosition()
        SS->>AEM: ClearAllEffects()
        
    else Ball.X > WindowWidth (Left player scores)
        SS->>SS: LeftPoint()
        SS->>Scoreboard: LeftPoint()
        Scoreboard->>Scoreboard: LeftScore++
        
        SS->>SS: Notify()
        activate SS
        
        par Notify all observers
            SS->>UIObserver: Update(scoreSubject)
            UIObserver->>SS: Get LeftScore, RightScore
            UIObserver->>UIObserver: Update local state
        and
            SS->>ConsoleObserver: Update(scoreSubject)
            ConsoleObserver->>SS: Get LeftScore, RightScore
            ConsoleObserver->>ConsoleObserver: Console.WriteLine(score)
        end
        
        deactivate SS
        
        SS->>Sound: PlayEffect(BallOut)
        SS->>Ball: ResetPosition()
        SS->>AEM: ClearAllEffects()
    end
```

## 6. Factory Pattern - Entity Creation Sequence

```mermaid
sequenceDiagram
    participant Client as PlayState
    participant Factory as GameEntityFactory
    participant Validator as WallValidator
    participant Ball
    participant Paddle
    participant Wall
    participant Transform as TransformComponent
    participant Movement as MovementComponent
    participant Render as RenderComponent

    Client->>Factory: CreateBall(windowWidth, windowHeight)
    activate Factory
    Factory->>Ball: new Ball(width, height)
    Ball->>Transform: new TransformComponent()
    Ball->>Movement: new MovementComponent()
    Ball->>Render: new RenderComponent()
    Ball->>Ball: AddComponent(transform)
    Ball->>Ball: AddComponent(movement)
    Ball->>Ball: AddComponent(render)
    Factory-->>Client: ball
    deactivate Factory
    
    Client->>Factory: CreatePaddle(x, y, width, height, isLeft)
    activate Factory
    Factory->>Paddle: new Paddle(x, y, width, height)
    Paddle->>Transform: new TransformComponent()
    Paddle->>Render: new RenderComponent()
    Paddle->>Paddle: AddComponent(transform)
    Paddle->>Paddle: AddComponent(render)
    Factory-->>Client: paddle
    deactivate Factory
    
    Client->>Factory: CreateWalls(windowWidth, windowHeight, count)
    activate Factory
    Factory->>Factory: CalculateWallCount()
    Factory->>Validator: new WallValidator()
    
    loop For wallCount times
        Factory->>Factory: Generate random position
        Factory->>Validator: IsValidPosition(walls, x, y, width, height)
        Validator->>Validator: Check collision with existing walls
        
        alt Valid position
            Factory->>Wall: new Wall(x, y, width, height, speed)
            Wall->>Transform: new TransformComponent()
            Wall->>Movement: new MovementComponent()
            Wall->>Render: new RenderComponent()
            Wall->>Wall: AddComponent(transform)
            Wall->>Wall: AddComponent(movement)
            Wall->>Wall: AddComponent(render)
            Factory->>Factory: Add to walls list
        else Invalid position
            Factory->>Factory: Retry with new position
        end
    end
    
    Factory-->>Client: walls[]
    deactivate Factory
```

## 7. Strategy Pattern - Power-Up Effect Application Sequence

```mermaid
sequenceDiagram
    participant Ball
    participant PUM as PowerUpManager
    participant PowerUp
    participant AEM as ActiveEffectManager
    participant EF as EffectFactory
    participant Strategy as IEffect
    participant SpeedBoost as SpeedBoostEffect
    participant SizeBoost as SizeBoostEffect
    participant Paddle
    participant Sound as SoundManager

    Ball->>PUM: CheckCollisions(ball)
    PUM->>PowerUp: IsColliding(ball)
    PowerUp-->>PUM: true
    
    PUM->>PUM: ApplyPowerUpEffect(powerUp, ball)
    PUM->>AEM: ApplyEffect(powerUpType, paddle, duration)
    
    activate AEM
    AEM->>EF: GetEffect(powerUpType)
    
    alt SpeedBoost type
        EF-->>AEM: SpeedBoostEffect
        AEM->>SpeedBoost: Apply(ball)
        SpeedBoost->>Ball: Speed *= 1.5
        SpeedBoost->>Ball: Accelerate()
    else SpeedReduction type
        EF-->>AEM: SpeedReductionEffect
        AEM->>Strategy: Apply(ball)
        Strategy->>Ball: Speed *= 0.7
    else SizeBoost type
        EF-->>AEM: SizeBoostEffect
        AEM->>SizeBoost: Apply(paddle)
        SizeBoost->>Paddle: Height *= 1.5
    end
    
    AEM->>AEM: Store ActiveEffect(type, startTime, duration)
    deactivate AEM
    
    PUM->>Sound: PlayEffect(PotionEffect)
    PUM->>PUM: Remove powerUp from active list
    
    Note over AEM: Later, when effect expires...
    
    AEM->>AEM: Update() - check expired effects
    alt Effect expired
        AEM->>AEM: DeactivateEffect(activeEffect)
        AEM->>EF: GetEffect(powerUpType)
        
        alt SpeedBoost expired
            EF-->>AEM: SpeedBoostEffect
            AEM->>SpeedBoost: Remove(ball)
            SpeedBoost->>Ball: ResetSpeed()
        else SizeBoost expired
            EF-->>AEM: SizeBoostEffect
            AEM->>SizeBoost: Remove(paddle)
            SizeBoost->>Paddle: Height = originalHeight
        end
        
        AEM->>AEM: Remove from _activeEffects list
    end
```

## 8. Collision Detection Sequence

```mermaid
sequenceDiagram
    participant PS as PlayState
    participant CH as CollisionHandler
    participant Ball
    participant Paddle
    participant Wall
    participant Collision as CollisionComponent
    participant Transform as TransformComponent
    participant Sound as SoundManager
    participant PUM as PowerUpManager

    PS->>CH: HandleCollisions(ball, leftPaddle, rightPaddle, walls)
    activate CH
    
    CH->>CH: CheckCollision(ball, leftPaddle)
    CH->>Ball: GetBounds()
    Ball->>Collision: GetBounds()
    Collision->>Transform: Get X, Y, Width, Height
    Transform-->>Collision: Rectangle
    Collision-->>Ball: ballBounds
    
    CH->>Paddle: GetBounds()
    Paddle->>Transform: Get X, Y, Width, Height
    Transform-->>Paddle: paddleBounds
    
    CH->>CH: ballBounds.Intersects(paddleBounds)
    
    alt Collision detected
        CH->>CH: ResolveCollision(ball, paddle)
        CH->>Ball: Bounce(paddle)
        Ball->>Ball: Calculate new velocity
        Ball->>Ball: Accelerate slightly
        CH->>Sound: PlayEffect(PaddleHit)
        CH->>PUM: SpawnRandomPowerUp()
    end
    
    CH->>CH: CheckCollision(ball, rightPaddle)
    alt Collision with right paddle
        CH->>CH: ResolveCollision(ball, paddle)
        CH->>Ball: Bounce(paddle)
        CH->>Sound: PlayEffect(PaddleHit)
        CH->>PUM: SpawnRandomPowerUp()
    end
    
    loop For each wall
        CH->>CH: CheckCollision(ball, wall)
        CH->>Wall: GetBounds()
        Wall->>Transform: Get X, Y, Width, Height
        Transform-->>Wall: wallBounds
        
        CH->>CH: ballBounds.Intersects(wallBounds)
        alt Collision with wall
            CH->>Ball: Bounce(wall)
            Ball->>Ball: Reverse Y velocity
            CH->>Sound: PlayEffect(WallHit)
        end
    end
    
    deactivate CH
```

## 9. State Transition - Game Over Sequence

```mermaid
sequenceDiagram
    participant PS as PlayState
    participant SS as ScoreSubject
    participant Scoreboard
    participant SM as StateMachine
    participant GOS as GameOverState
    participant UI as GameUI
    participant Sound as SoundManager
    participant Context as GameContext

    PS->>SS: CheckBallOutOfBounds()
    SS->>Scoreboard: Get scores
    Scoreboard-->>SS: LeftScore=5, RightScore=3
    
    alt LeftScore >= 5 OR RightScore >= 5
        PS->>PS: Check if game should end
        PS->>Context: Set GameOver = true
        PS->>SM: ChangeState("GameOver")
        
        SM->>PS: Exit()
        PS->>Sound: StopMusic()
        PS->>PS: Cleanup resources
        
        SM->>GOS: Enter()
        activate GOS
        GOS->>Context: Get Scoreboard
        GOS->>Scoreboard: Get LeftScore, RightScore
        
        alt LeftScore > RightScore
            GOS->>UI: Winner = 1 (Left player)
        else RightScore > LeftScore
            GOS->>UI: Winner = 2 (Right player)
        end
        
        GOS->>UI: CurrentState = GameState.GameOver
        GOS->>Sound: PlayMusic(GameOverMusic)
        deactivate GOS
        
        Note over User,GOS: User clicks "Play Again" or "Main Menu"
        
        alt Play Again clicked
            UI->>GOS: RestartGame()
            GOS->>Context: Reset()
            GOS->>Scoreboard: Reset()
            GOS->>SM: ChangeState("Play")
        else Main Menu clicked
            UI->>GOS: ReturnToMenu()
            GOS->>Context: Reset()
            GOS->>SM: ChangeState("Menu")
        end
    end
```

## 10. Component Pattern - Entity Update Sequence

```mermaid
sequenceDiagram
    participant Ball
    participant Transform as TransformComponent
    participant Movement as MovementComponent
    participant Render as RenderComponent
    participant Vector2D
    participant Renderer as IRenderer
    participant SplashKit as SplashKitRenderer

    Note over Ball: Ball.Update() called every frame
    
    Ball->>Ball: Update()
    activate Ball
    
    loop For each component
        Ball->>Transform: Update()
        Transform->>Transform: Update bounds
        
        Ball->>Movement: Update()
        activate Movement
        Movement->>Transform: Get current position
        Transform-->>Movement: X, Y
        Movement->>Vector2D: Add(position, velocity)
        Vector2D-->>Movement: newPosition
        Movement->>Transform: Set X, Y (newPosition)
        deactivate Movement
        
        Ball->>Render: Update()
        Render->>Render: Prepare rendering data
    end
    
    deactivate Ball
    
    Note over Ball: Ball.Draw() called every frame
    
    Ball->>Ball: Draw()
    activate Ball
    Ball->>Render: Draw()
    activate Render
    Render->>Transform: Get X, Y, Width, Height
    Transform-->>Render: bounds
    Render->>Renderer: DrawCircle(x, y, radius, color)
    Renderer->>SplashKit: FillCircle(color, x, y, radius)
    SplashKit-->>Renderer: Rendered
    deactivate Render
    deactivate Ball
```

## Key Patterns Demonstrated

### 1. **Singleton Pattern** - GameManager ensures single instance
### 2. **State Pattern** - Clean state transitions (Menu → Play → GameOver)
### 3. **Observer Pattern** - Score updates notify multiple observers
### 4. **Command Pattern** - Input handling with undo support
### 5. **Factory Pattern** - Centralized entity creation with validation
### 6. **Strategy Pattern** - Flexible power-up effects with effect strategies
### 7. **Component Pattern** - Modular entity behavior (Transform, Movement, Render)
### 8. **Dependency Injection** - Services injected into managers
### 9. **Adapter Pattern** - IRenderer abstracts SplashKit implementation
### 10. **Immutable Value Objects** - Vector2D ensures thread-safe operations

---

## Luồng chính của Game

1. **Initialization**: Program → GameManager → StateMachine → MenuState
2. **Start Game**: MenuState → PlayState → Factory creates entities → Initialize services
3. **Gameplay Loop**: InputHandler → Update entities → CollisionHandler → PowerUpManager → ScoreSubject
4. **Score Update**: ScoreSubject notifies observers → UI updates → Check win condition
5. **Game Over**: Transition to GameOverState → Display winner → Restart or Menu
6. **Commands**: User input → Command objects → Execute on entities
7. **Effects**: PowerUp collision → ActiveEffectManager → Strategy pattern applies effect → Auto-remove when expired
8. **Components**: Entity.Update() → Update all components → Render components draw to screen
