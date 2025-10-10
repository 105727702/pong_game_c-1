
    class GameManager {
        -GameManager _instance
        -object _lock
        -GameContext Context
        -StateMachine StateMachine
        -IGameEntityFactory _factory
        +Instance : GameManager
        +InitializeGame()
        +Run()
    }
    
    class StateMachine {
        -IGameState _currentState
        -Dictionary~string, IGameState~ _states
        +AddState(name, state)
        +ChangeState(stateName)
        +Update(deltaTime)
        +GetCurrentState()
    }
    
    class GameContext {
        +Ball Ball
        +Paddle LeftPaddle
        +Paddle RightPaddle
        +List~Wall~ Walls
        +ScoreSubject ScoreSubject
        +SoundManager SoundManager
        +PowerUpManager PowerUpManager
        +ActiveEffectManager ActiveEffectManager
        +InputHandler InputHandler
    }

    %% State Pattern
    class IGameState {
        <<interface>>
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    class MenuState {
        -GameContext _context
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    class PlayState {
        -GameContext _context
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    class GameOverState {
        -GameContext _context
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
        #List~IComponent~ _components
        +AddComponent(component)
        +RemoveComponent(component)
        +GetComponent~T~()
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
        -TransformComponent _transform
        +Vector2D Velocity
        +float Speed
        +SetVelocity(x, y)
        +Update(deltaTime)
    }
    
    class RenderComponent {
        -TransformComponent _transform
        +Color Color
        +bool IsCircle
        +Draw()
        +Update(deltaTime)
    }

    %% Entities
    class Ball {
        -TransformComponent _transform
        -MovementComponent _movement
        -RenderComponent _render
        -float _baseSpeed
        -Vector2D _velocity
        +float X
        +float Y
        +int Size
        +Color Color
        +float Speed
        +Move()
        +Bounce(surfaceNormal)
        +ResetPosition()
        +Draw()
    }
    
    class Paddle {
        -TransformComponent _transform
        -RenderComponent _render
        -const float BASE_SPEED
        -const float MAX_SPEED
        +float X
        +float Y
        +int Width
        +int Height
        +float Speed
        +MoveUp()
        +MoveDown()
        +ResetSpeed()
        +Draw()
    }
    
    class Wall {
        -TransformComponent _transform
        -MovementComponent _movement
        -RenderComponent _render
        +const int WALL_WIDTH
        +const int WALL_HEIGHT
        +float X
        +float Y
        +float YSpeed
        +Move()
        +Draw()
    }
    
    class Scoreboard {
        -int _leftScore
        -int _rightScore
        -bool _gameStarted
        +int LeftScore
        +int RightScore
        +LeftPoint()
        +RightPoint()
        +Start()
        +Reset()
    }

    %% Decorator Pattern
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

    %% Observer Pattern
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
        -Ball _ball
        -const int WINNING_SCORE
        +int LeftScore
        +int RightScore
        +Attach(observer)
        +Detach(observer)
        +Notify()
        +LeftPoint()
        +RightPoint()
    }
    
    class UIScoreObserver {
        -int _leftScore
        -int _rightScore
        +Update(subject)
        +DrawScore(width, height)
    }
    
    class ConsoleScoreObserver {
        +Update(subject)
    }

    %% Command Pattern
    class ICommand {
        <<interface>>
        +Execute()
        +Undo()
    }
    
    class MoveUpCommand {
        -Paddle _paddle
        -float _previousY
        +Execute()
        +Undo()
    }
    
    class MoveDownCommand {
        -Paddle _paddle
        -float _previousY
        +Execute()
        +Undo()
    }
    
    class StopPaddleCommand {
        -Paddle _paddle
        -float _previousSpeed
        +Execute()
        +Undo()
    }
    
    class InputHandler {
        -Dictionary~KeyCode, ICommand~ _keyBindings
        -List~ICommand~ _commandHistory
        +HandleKeyInput(leftPaddle, rightPaddle)
        +UpdatePaddleMovement(leftPaddle, rightPaddle)
        +UndoLastCommand()
    }

    %% Factory Pattern
    class IGameEntityFactory {
        <<interface>>
        +CreateBall(width, height) Ball
        +CreatePaddle(x, y, height) Paddle
        +CreateWall(x, y, height, speed) Wall
        +CreateScoreboard() Scoreboard
        +CreateWalls(num, minDist, height) List~Wall~
    }
    
    class GameEntityFactory {
        -Random _random
        +CreateBall(width, height) Ball
        +CreatePaddle(x, y, height) Paddle
        +CreateWall(x, y, height, speed) Wall
        +CreateScoreboard() Scoreboard
        +CreateWalls(num, minDist, height) List~Wall~
        +CalculateWallCount(totalScore, baseWalls) int
    }
    
    class EffectFactory {
        +ApplyEffect(type, ball, leftPaddle, rightPaddle)
        +RemoveEffect(type, ball, leftPaddle, rightPaddle, height, speed)
        +ResetAllEffects(ball, leftPaddle, rightPaddle, height)
        -ApplySpeedBoost(ball)
        -ApplySpeedReduction(ball)
        -ApplySizeBoost(leftPaddle, rightPaddle)
    }
    
    class ActiveEffectManager {
        -List~ActiveEffect~ _activeEffects
        -Ball _ball
        -Paddle _leftPaddle
        -Paddle _rightPaddle
        -EffectFactory _effectFactory
        +ApplyEffect(type, duration)
        +Update()
        +ClearAllEffects()
        +GetActiveEffects() List~ActiveEffect~
    }
    
    class ActiveEffect {
        +PowerUpType Type
        +DateTime StartTime
        +double Duration
        +IsExpired() bool
        +GetRemainingTime() double
    }


classDiagram
    class GameManager {
        <<Singleton>>
        -static GameManager _instance
        -static object _lock
        -StateMachine StateMachine
        -MenuState MenuState
        -PlayState PlayState
        -GameOverState GameOverState
        +static Instance : GameManager
        -GameManager()
        +InitializeGame()
        +Run()
    }
    
    class StateMachine {
        -IGameState _currentState
        -Dictionary~string, IGameState~ _states
        +AddState(name, state)
        +ChangeState(stateName)
        +Update(deltaTime)
    }
    
    class IGameState {
        <<interface>>
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    class MenuState {
        -GameContext _context
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    class PlayState {
        -GameContext _context
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
    class GameOverState {
        -GameContext _context
        -string _winner
        +Enter()
        +Update(deltaTime)
        +Exit()
    }
    
classDiagram
    class ICommand {
        <<interface>>
        +Execute()
        +Undo()
    }
    
    class MoveUpCommand {
        -Paddle _paddle
        -float _previousY
        +MoveUpCommand(paddle)
        +Execute()
        +Undo()
    }
    
    class MoveDownCommand {
        -Paddle _paddle
        -float _previousY
        +MoveDownCommand(paddle)
        +Execute()
        +Undo()
    }
    
    class StopPaddleCommand {
        -Paddle _paddle
        -float _previousSpeed
        +StopPaddleCommand(paddle)
        +Execute()
        +Undo()
    }
    
    class InputHandler {
        <<Invoker>>
        -Dictionary~KeyCode, ICommand~ _keyBindings
        -List~ICommand~ _commandHistory
        +InputHandler(leftPaddle, rightPaddle)
        +HandleKeyInput(leftPaddle, rightPaddle)
        +UpdatePaddleMovement(leftPaddle, rightPaddle)
        +UndoLastCommand()
    }
    
    class Paddle {
        <<Receiver>>
        +float Y
        +float Speed
        +MoveUp()
        +MoveDown()
        +ResetSpeed()
    }
    
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
        +GetComponent~T~()
        +Update(deltaTime)
        +Draw()*
    }
    
    class TransformComponent {
        +float X
        +float Y
        +float Width
        +float Height
        +TransformComponent(x, y, width, height)
        +GetBounds() Rectangle
        +Update(deltaTime)
    }
    
    class MovementComponent {
        -TransformComponent _transform
        +Vector2D Velocity
        +float Speed
        +MovementComponent(transform, speed)
        +SetVelocity(x, y)
        +Update(deltaTime)
    }
    
    class RenderComponent {
        -TransformComponent _transform
        +Color Color
        +bool IsCircle
        +RenderComponent(transform, color, isCircle)
        +Draw()
        +Update(deltaTime)
    }
    
    class Ball {
        -TransformComponent _transform
        -MovementComponent _movement
        -RenderComponent _render
        +Ball(windowWidth, windowHeight)
        +Draw()
    }
    
    class Paddle {
        -TransformComponent _transform
        -RenderComponent _render
        +Paddle(x, y, windowHeight)
        +Draw()
    }
    
    class Wall {
        -TransformComponent _transform
        -MovementComponent _movement
        -RenderComponent _render
        +Wall(x, y, windowHeight, speedMultiplier)
        +Draw()
    }
    
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
        -Ball _ball
        -SoundManager _soundManager
        +int LeftScore
        +int RightScore
        +Attach(observer)
        +Detach(observer)
        +Notify()
        +LeftPoint()
        +RightPoint()
        +Start()
        +Reset()
    }
    
    class UIScoreObserver {
        -int _leftScore
        -int _rightScore
        -bool _gameStarted
        +Update(subject)
        +DrawScore(windowWidth, windowHeight)
    }
    
    class ConsoleScoreObserver {
        +Update(subject)
    }
    
    class Scoreboard {
        -int _leftScore
        -int _rightScore
        -bool _gameStarted
        +LeftPoint()
        +RightPoint()
        +Start()
        +Reset()
    }
    
classDiagram
    class IGameEntity {
        <<interface>>
        +GetSpeed() float
        +GetSize() float
        +Update(deltaTime)
    }
    
    class Ball {
        +GetSpeed() float
        +GetSize() float
        +Update(deltaTime)
    }
    
    class Paddle {
        +GetSpeed() float
        +GetSize() float
        +Update(deltaTime)
    }
    
    class EntityDecorator {
        <<abstract>>
        #IGameEntity _wrappedEntity
        +EntityDecorator(entity)
        +GetSpeed() float
        +GetSize() float
        +Update(deltaTime)
    }
    
    class SpeedBoostDecorator {
        -float _speedMultiplier
        -DateTime _startTime
        -double _duration
        +SpeedBoostDecorator(entity, multiplier, duration)
        +GetSpeed() float
        +IsActive() bool
    }
    
    class SpeedReductionDecorator {
        -float _speedMultiplier
        -DateTime _startTime
        -double _duration
        +SpeedReductionDecorator(entity, multiplier, duration)
        +GetSpeed() float
        +IsActive() bool
    }
    
    class SizeBoostDecorator {
        -float _sizeMultiplier
        -DateTime _startTime
        -double _duration
        +SizeBoostDecorator(entity, multiplier, duration)
        +GetSize() float
        +IsActive() bool
    }
    
classDiagram
    class IGameEntityFactory {
        <<interface>>
        +CreateBall(width, height) Ball
        +CreatePaddle(x, y, height) Paddle
        +CreateWall(x, y, height, speed) Wall
        +CreateScoreboard() Scoreboard
        +CreateWalls(num, minDist, height) List~Wall~
        +CalculateWallCount(totalScore, baseWalls) int
    }
    
    class GameEntityFactory {
        -Random _random
        +CreateBall(width, height) Ball
        +CreatePaddle(x, y, height) Paddle
        +CreateWall(x, y, height, speed) Wall
        +CreateScoreboard() Scoreboard
        +CreateWalls(num, minDist, height) List~Wall~
        +CalculateWallCount(totalScore, baseWalls) int
    }
    
    class EffectFactory {
        +ApplyEffect(type, ball, leftPaddle, rightPaddle)
        +RemoveEffect(type, ball, leftPaddle, rightPaddle, height, speed)
        +ResetAllEffects(ball, leftPaddle, rightPaddle, height)
        -ApplySpeedBoost(ball)
        -ApplySpeedReduction(ball)
        -ApplySizeBoost(leftPaddle, rightPaddle)
        -ResetSpeed(ball)
        -ResetSize(leftPaddle, rightPaddle, originalHeight)
    }
    
    class ActiveEffectManager {
        -List~ActiveEffect~ _activeEffects
        -EffectFactory _effectFactory
        +ApplyEffect(type, duration)
        +Update()
        +ClearAllEffects()
    }

