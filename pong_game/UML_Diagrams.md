# Pong Game - UML Diagrams# Pong Game - UML Diagrams



## 1. Core Architecture - State Pattern## 1. Core Architecture Overview



```mermaid```mermaid

classDiagramclassDiagram

    class GameManager {    class GameManager {

        <<Singleton>>        -GameManager _instance

        -static GameManager _instance        -object _lock

        -static object _lock        -GameContext Context

        -StateMachine StateMachine        -StateMachine StateMachine

        -MenuState MenuState        -IGameEntityFactory _factory

        -PlayState PlayState        +Instance : GameManager

        -GameOverState GameOverState        +InitializeGame()

        +static Instance : GameManager        +Run()

        -GameManager()    }

        +InitializeGame()    

        +Run()    class StateMachine {

    }        -IGameState _currentState

            -Dictionary~string, IGameState~ _states

    class StateMachine {        +AddState(name, state)

        -IGameState _currentState        +ChangeState(stateName)

        -Dictionary~string, IGameState~ _states        +Update(deltaTime)

        +AddState(name, state)        +GetCurrentState()

        +ChangeState(stateName)    }

        +Update(deltaTime)    

    }    class GameContext {

            +Ball Ball

    class IGameState {        +Paddle LeftPaddle

        <<interface>>        +Paddle RightPaddle

        +Enter()        +List~Wall~ Walls

        +Update(deltaTime)        +ScoreSubject ScoreSubject

        +Exit()        +SoundManager SoundManager

    }        +PowerUpManager PowerUpManager

            +ActiveEffectManager ActiveEffectManager

    class MenuState {        +InputHandler InputHandler

        -GameContext _context    }

        +Enter()

        +Update(deltaTime)    %% State Pattern

        +Exit()    class IGameState {

    }        <<interface>>

            +Enter()

    class PlayState {        +Update(deltaTime)

        -GameContext _context        +Exit()

        +Enter()    }

        +Update(deltaTime)    

        +Exit()    class MenuState {

    }        -GameContext _context

            +Enter()

    class GameOverState {        +Update(deltaTime)

        -GameContext _context        +Exit()

        -string _winner    }

        +Enter()    

        +Update(deltaTime)    class PlayState {

        +Exit()        -GameContext _context

    }        +Enter()

            +Update(deltaTime)

    GameManager --> StateMachine        +Exit()

    GameManager --> MenuState    }

    GameManager --> PlayState    

    GameManager --> GameOverState    class GameOverState {

    StateMachine --> IGameState        -GameContext _context

    IGameState <|.. MenuState        +Enter()

    IGameState <|.. PlayState        +Update(deltaTime)

    IGameState <|.. GameOverState        +Exit()

```    }

    

## 2. Command Pattern    GameManager --> StateMachine

    GameManager --> GameContext

```mermaid    StateMachine --> IGameState

classDiagram    IGameState <|.. MenuState

    class ICommand {    IGameState <|.. PlayState

        <<interface>>    IGameState <|.. GameOverState

        +Execute()```

        +Undo()

    }    %% Component Pattern

        class IComponent {

    class MoveUpCommand {        <<interface>>

        -Paddle _paddle        +Update(deltaTime)

        -float _previousY    }

        +MoveUpCommand(paddle)    

        +Execute()    class GameObject {

        +Undo()        <<abstract>>

    }        #List~IComponent~ _components

            +AddComponent(component)

    class MoveDownCommand {        +RemoveComponent(component)

        -Paddle _paddle        +GetComponent~T~()

        -float _previousY        +Update(deltaTime)

        +MoveDownCommand(paddle)        +Draw()*

        +Execute()    }

        +Undo()    

    }    class TransformComponent {

            +float X

    class StopPaddleCommand {        +float Y

        -Paddle _paddle        +float Width

        -float _previousSpeed        +float Height

        +StopPaddleCommand(paddle)        +GetBounds() Rectangle

        +Execute()        +Update(deltaTime)

        +Undo()    }

    }    

        class MovementComponent {

    class InputHandler {        -TransformComponent _transform

        <<Invoker>>        +Vector2D Velocity

        -Dictionary~KeyCode, ICommand~ _keyBindings        +float Speed

        -List~ICommand~ _commandHistory        +SetVelocity(x, y)

        +InputHandler(leftPaddle, rightPaddle)        +Update(deltaTime)

        +HandleKeyInput(leftPaddle, rightPaddle)    }

        +UpdatePaddleMovement(leftPaddle, rightPaddle)    

        +UndoLastCommand()    class RenderComponent {

    }        -TransformComponent _transform

            +Color Color

    class Paddle {        +bool IsCircle

        <<Receiver>>        +Draw()

        +float Y        +Update(deltaTime)

        +float Speed    }

        +MoveUp()

        +MoveDown()    %% Entities

        +ResetSpeed()    class Ball {

    }        -TransformComponent _transform

            -MovementComponent _movement

    ICommand <|.. MoveUpCommand        -RenderComponent _render

    ICommand <|.. MoveDownCommand        -float _baseSpeed

    ICommand <|.. StopPaddleCommand        -Vector2D _velocity

    InputHandler --> ICommand        +float X

    MoveUpCommand --> Paddle        +float Y

    MoveDownCommand --> Paddle        +int Size

    StopPaddleCommand --> Paddle        +Color Color

```        +float Speed

        +Move()

## 3. Component Pattern        +Bounce(surfaceNormal)

        +ResetPosition()

```mermaid        +Draw()

classDiagram    }

    class IComponent {    

        <<interface>>    class Paddle {

        +Update(deltaTime)        -TransformComponent _transform

    }        -RenderComponent _render

            -const float BASE_SPEED

    class GameObject {        -const float MAX_SPEED

        <<abstract>>        +float X

        #List~IComponent~ _components        +float Y

        +AddComponent(component)        +int Width

        +RemoveComponent(component)        +int Height

        +GetComponent~T~()        +float Speed

        +Update(deltaTime)        +MoveUp()

        +Draw()*        +MoveDown()

    }        +ResetSpeed()

            +Draw()

    class TransformComponent {    }

        +float X    

        +float Y    class Wall {

        +float Width        -TransformComponent _transform

        +float Height        -MovementComponent _movement

        +TransformComponent(x, y, width, height)        -RenderComponent _render

        +GetBounds() Rectangle        +const int WALL_WIDTH

        +Update(deltaTime)        +const int WALL_HEIGHT

    }        +float X

            +float Y

    class MovementComponent {        +float YSpeed

        -TransformComponent _transform        +Move()

        +Vector2D Velocity        +Draw()

        +float Speed    }

        +MovementComponent(transform, speed)    

        +SetVelocity(x, y)    class Scoreboard {

        +Update(deltaTime)        -int _leftScore

    }        -int _rightScore

            -bool _gameStarted

    class RenderComponent {        +int LeftScore

        -TransformComponent _transform        +int RightScore

        +Color Color        +LeftPoint()

        +bool IsCircle        +RightPoint()

        +RenderComponent(transform, color, isCircle)        +Start()

        +Draw()        +Reset()

        +Update(deltaTime)    }

    }

        %% Decorator Pattern

    class Ball {    class IGameEntity {

        -TransformComponent _transform        <<interface>>

        -MovementComponent _movement        +GetSpeed() float

        -RenderComponent _render        +GetSize() float

        +Ball(windowWidth, windowHeight)        +Update(deltaTime)

        +Draw()    }

    }    

        class EntityDecorator {

    class Paddle {        <<abstract>>

        -TransformComponent _transform        #IGameEntity _wrappedEntity

        -RenderComponent _render        +GetSpeed() float

        +Paddle(x, y, windowHeight)        +GetSize() float

        +Draw()        +Update(deltaTime)

    }    }

        

    class Wall {    class SpeedBoostDecorator {

        -TransformComponent _transform        -float _speedMultiplier

        -MovementComponent _movement        -DateTime _startTime

        -RenderComponent _render        -double _duration

        +Wall(x, y, windowHeight, speedMultiplier)        +GetSpeed() float

        +Draw()        +IsActive() bool

    }    }

        

    IComponent <|.. TransformComponent    class SpeedReductionDecorator {

    IComponent <|.. MovementComponent        -float _speedMultiplier

    IComponent <|.. RenderComponent        -DateTime _startTime

    GameObject <|-- Ball        -double _duration

    GameObject <|-- Paddle        +GetSpeed() float

    GameObject <|-- Wall        +IsActive() bool

    GameObject --> IComponent    }

    Ball --> TransformComponent    

    Ball --> MovementComponent    class SizeBoostDecorator {

    Ball --> RenderComponent        -float _sizeMultiplier

    Paddle --> TransformComponent        -DateTime _startTime

    Paddle --> RenderComponent        -double _duration

    Wall --> TransformComponent        +GetSize() float

    Wall --> MovementComponent        +IsActive() bool

    Wall --> RenderComponent    }

```

    %% Observer Pattern

## 4. Observer Pattern    class ISubject {

        <<interface>>

```mermaid        +Attach(observer)

classDiagram        +Detach(observer)

    class ISubject {        +Notify()

        <<interface>>    }

        +Attach(observer)    

        +Detach(observer)    class IObserver {

        +Notify()        <<interface>>

    }        +Update(subject)

        }

    class IObserver {    

        <<interface>>    class ScoreSubject {

        +Update(subject)        -List~IObserver~ _observers

    }        -Scoreboard _scoreboard

            -Ball _ball

    class ScoreSubject {        -const int WINNING_SCORE

        -List~IObserver~ _observers        +int LeftScore

        -Scoreboard _scoreboard        +int RightScore

        -Ball _ball        +Attach(observer)

        -SoundManager _soundManager        +Detach(observer)

        +int LeftScore        +Notify()

        +int RightScore        +LeftPoint()

        +Attach(observer)        +RightPoint()

        +Detach(observer)    }

        +Notify()    

        +LeftPoint()    class UIScoreObserver {

        +RightPoint()        -int _leftScore

        +Start()        -int _rightScore

        +Reset()        +Update(subject)

    }        +DrawScore(width, height)

        }

    class UIScoreObserver {    

        -int _leftScore    class ConsoleScoreObserver {

        -int _rightScore        +Update(subject)

        -bool _gameStarted    }

        +Update(subject)

        +DrawScore(windowWidth, windowHeight)    %% Command Pattern

    }    class ICommand {

            <<interface>>

    class ConsoleScoreObserver {        +Execute()

        +Update(subject)        +Undo()

    }    }

        

    class Scoreboard {    class MoveUpCommand {

        -int _leftScore        -Paddle _paddle

        -int _rightScore        -float _previousY

        -bool _gameStarted        +Execute()

        +LeftPoint()        +Undo()

        +RightPoint()    }

        +Start()    

        +Reset()    class MoveDownCommand {

    }        -Paddle _paddle

            -float _previousY

    ISubject <|.. ScoreSubject        +Execute()

    IObserver <|.. UIScoreObserver        +Undo()

    IObserver <|.. ConsoleScoreObserver    }

    ScoreSubject --> IObserver    

    ScoreSubject --> Scoreboard    class StopPaddleCommand {

```        -Paddle _paddle

        -float _previousSpeed

## 5. Decorator Pattern        +Execute()

        +Undo()

```mermaid    }

classDiagram    

    class IGameEntity {    class InputHandler {

        <<interface>>        -Dictionary~KeyCode, ICommand~ _keyBindings

        +GetSpeed() float        -List~ICommand~ _commandHistory

        +GetSize() float        +HandleKeyInput(leftPaddle, rightPaddle)

        +Update(deltaTime)        +UpdatePaddleMovement(leftPaddle, rightPaddle)

    }        +UndoLastCommand()

        }

    class Ball {

        +GetSpeed() float    %% Factory Pattern

        +GetSize() float    class IGameEntityFactory {

        +Update(deltaTime)        <<interface>>

    }        +CreateBall(width, height) Ball

            +CreatePaddle(x, y, height) Paddle

    class Paddle {        +CreateWall(x, y, height, speed) Wall

        +GetSpeed() float        +CreateScoreboard() Scoreboard

        +GetSize() float        +CreateWalls(num, minDist, height) List~Wall~

        +Update(deltaTime)    }

    }    

        class GameEntityFactory {

    class EntityDecorator {        -Random _random

        <<abstract>>        +CreateBall(width, height) Ball

        #IGameEntity _wrappedEntity        +CreatePaddle(x, y, height) Paddle

        +EntityDecorator(entity)        +CreateWall(x, y, height, speed) Wall

        +GetSpeed() float        +CreateScoreboard() Scoreboard

        +GetSize() float        +CreateWalls(num, minDist, height) List~Wall~

        +Update(deltaTime)        +CalculateWallCount(totalScore, baseWalls) int

    }    }

        

    class SpeedBoostDecorator {    class EffectFactory {

        -float _speedMultiplier        +ApplyEffect(type, ball, leftPaddle, rightPaddle)

        -DateTime _startTime        +RemoveEffect(type, ball, leftPaddle, rightPaddle, height, speed)

        -double _duration        +ResetAllEffects(ball, leftPaddle, rightPaddle, height)

        +SpeedBoostDecorator(entity, multiplier, duration)        -ApplySpeedBoost(ball)

        +GetSpeed() float        -ApplySpeedReduction(ball)

        +IsActive() bool        -ApplySizeBoost(leftPaddle, rightPaddle)

    }    }

        

    class SpeedReductionDecorator {    class ActiveEffectManager {

        -float _speedMultiplier        -List~ActiveEffect~ _activeEffects

        -DateTime _startTime        -Ball _ball

        -double _duration        -Paddle _leftPaddle

        +SpeedReductionDecorator(entity, multiplier, duration)        -Paddle _rightPaddle

        +GetSpeed() float        -EffectFactory _effectFactory

        +IsActive() bool        +ApplyEffect(type, duration)

    }        +Update()

            +ClearAllEffects()

    class SizeBoostDecorator {        +GetActiveEffects() List~ActiveEffect~

        -float _sizeMultiplier    }

        -DateTime _startTime    

        -double _duration    class ActiveEffect {

        +SizeBoostDecorator(entity, multiplier, duration)        +PowerUpType Type

        +GetSize() float        +DateTime StartTime

        +IsActive() bool        +double Duration

    }        +IsExpired() bool

            +GetRemainingTime() double

    IGameEntity <|.. Ball    }

    IGameEntity <|.. Paddle

    IGameEntity <|.. EntityDecorator

    EntityDecorator <|-- SpeedBoostDecoratorclassDiagram

    EntityDecorator <|-- SpeedReductionDecorator    class GameManager {

    EntityDecorator <|-- SizeBoostDecorator        <<Singleton>>

    EntityDecorator --> IGameEntity        -static GameManager _instance

```        -static object _lock

        -StateMachine StateMachine

## 6. Factory Pattern        -MenuState MenuState

        -PlayState PlayState

```mermaid        -GameOverState GameOverState

classDiagram        +static Instance : GameManager

    class IGameEntityFactory {        -GameManager()

        <<interface>>        +InitializeGame()

        +CreateBall(width, height) Ball        +Run()

        +CreatePaddle(x, y, height) Paddle    }

        +CreateWall(x, y, height, speed) Wall    

        +CreateScoreboard() Scoreboard    class StateMachine {

        +CreateWalls(num, minDist, height) List~Wall~        -IGameState _currentState

        +CalculateWallCount(totalScore, baseWalls) int        -Dictionary~string, IGameState~ _states

    }        +AddState(name, state)

            +ChangeState(stateName)

    class GameEntityFactory {        +Update(deltaTime)

        -Random _random    }

        +CreateBall(width, height) Ball    

        +CreatePaddle(x, y, height) Paddle    class IGameState {

        +CreateWall(x, y, height, speed) Wall        <<interface>>

        +CreateScoreboard() Scoreboard        +Enter()

        +CreateWalls(num, minDist, height) List~Wall~        +Update(deltaTime)

        +CalculateWallCount(totalScore, baseWalls) int        +Exit()

    }    }

        

    class EffectFactory {    class MenuState {

        +ApplyEffect(type, ball, leftPaddle, rightPaddle)        -GameContext _context

        +RemoveEffect(type, ball, leftPaddle, rightPaddle, height, speed)        +Enter()

        +ResetAllEffects(ball, leftPaddle, rightPaddle, height)        +Update(deltaTime)

        -ApplySpeedBoost(ball)        +Exit()

        -ApplySpeedReduction(ball)    }

        -ApplySizeBoost(leftPaddle, rightPaddle)    

        -ResetSpeed(ball)    class PlayState {

        -ResetSize(leftPaddle, rightPaddle, originalHeight)        -GameContext _context

    }        +Enter()

            +Update(deltaTime)

    class ActiveEffectManager {        +Exit()

        -List~ActiveEffect~ _activeEffects    }

        -EffectFactory _effectFactory    

        +ApplyEffect(type, duration)    class GameOverState {

        +Update()        -GameContext _context

        +ClearAllEffects()        -string _winner

    }        +Enter()

            +Update(deltaTime)

    IGameEntityFactory <|.. GameEntityFactory        +Exit()

    ActiveEffectManager --> EffectFactory    }

```    

classDiagram

## 7. Complete System Architecture    class ICommand {

        <<interface>>

```mermaid        +Execute()

classDiagram        +Undo()

    class GameManager {    }

        -GameManager _instance    

        -object _lock    class MoveUpCommand {

        -GameContext Context        -Paddle _paddle

        -StateMachine StateMachine        -float _previousY

        -IGameEntityFactory _factory        +MoveUpCommand(paddle)

        +Instance : GameManager        +Execute()

        +InitializeGame()        +Undo()

        +Run()    }

    }    

        class MoveDownCommand {

    class GameContext {        -Paddle _paddle

        +Ball Ball        -float _previousY

        +Paddle LeftPaddle        +MoveDownCommand(paddle)

        +Paddle RightPaddle        +Execute()

        +List~Wall~ Walls        +Undo()

        +ScoreSubject ScoreSubject    }

        +SoundManager SoundManager    

        +PowerUpManager PowerUpManager    class StopPaddleCommand {

        +ActiveEffectManager ActiveEffectManager        -Paddle _paddle

        +InputHandler InputHandler        -float _previousSpeed

    }        +StopPaddleCommand(paddle)

            +Execute()

    class StateMachine {        +Undo()

        -IGameState _currentState    }

        -Dictionary~string, IGameState~ _states    

        +AddState(name, state)    class InputHandler {

        +ChangeState(stateName)        <<Invoker>>

        +Update(deltaTime)        -Dictionary~KeyCode, ICommand~ _keyBindings

        +GetCurrentState()        -List~ICommand~ _commandHistory

    }        +InputHandler(leftPaddle, rightPaddle)

            +HandleKeyInput(leftPaddle, rightPaddle)

    class IGameState {        +UpdatePaddleMovement(leftPaddle, rightPaddle)

        <<interface>>        +UndoLastCommand()

        +Enter()    }

        +Update(deltaTime)    

        +Exit()    class Paddle {

    }        <<Receiver>>

            +float Y

    class Ball {        +float Speed

        +Move()        +MoveUp()

        +Bounce()        +MoveDown()

        +Draw()        +ResetSpeed()

    }    }

        

    class Paddle {classDiagram

        +MoveUp()    class IComponent {

        +MoveDown()        <<interface>>

        +Draw()        +Update(deltaTime)

    }    }

        

    class Wall {    class GameObject {

        +Move()        <<abstract>>

        +Draw()        #List~IComponent~ _components

    }        +AddComponent(component)

            +RemoveComponent(component)

    class ScoreSubject {        +GetComponent~T~()

        +LeftPoint()        +Update(deltaTime)

        +RightPoint()        +Draw()*

        +Notify()    }

    }    

        class TransformComponent {

    class InputHandler {        +float X

        +HandleKeyInput()        +float Y

    }        +float Width

            +float Height

    class ActiveEffectManager {        +TransformComponent(x, y, width, height)

        +ApplyEffect()        +GetBounds() Rectangle

        +Update()        +Update(deltaTime)

    }    }

        

    GameManager --> GameContext    class MovementComponent {

    GameManager --> StateMachine        -TransformComponent _transform

    GameContext --> Ball        +Vector2D Velocity

    GameContext --> Paddle        +float Speed

    GameContext --> Wall        +MovementComponent(transform, speed)

    GameContext --> ScoreSubject        +SetVelocity(x, y)

    GameContext --> InputHandler        +Update(deltaTime)

    GameContext --> ActiveEffectManager    }

    StateMachine --> IGameState    

```    class RenderComponent {

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

