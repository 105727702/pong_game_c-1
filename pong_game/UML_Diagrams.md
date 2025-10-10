# Pong Game - UML Diagrams# Pong Game - UML Diagrams# Pong Game - UML Diagrams



## 1. State Pattern - Core Game States



```mermaid## 1. Core Architecture - State Pattern## 1. Core Architecture Overview

classDiagram

    class GameManager {

        -GameManager instance

        -object lock```mermaid```mermaid

        -StateMachine StateMachine

        -MenuState MenuStateclassDiagramclassDiagram

        -PlayState PlayState

        -GameOverState GameOverState    class GameManager {    class GameManager {

        +Instance GameManager

        +InitializeGame()        <<Singleton>>        -GameManager _instance

        +Run()

    }        -static GameManager _instance        -object _lock

    

    class StateMachine {        -static object _lock        -GameContext Context

        -IGameState currentState

        -Dictionary states        -StateMachine StateMachine        -StateMachine StateMachine

        +AddState(name, state)

        +ChangeState(stateName)        -MenuState MenuState        -IGameEntityFactory _factory

        +Update(deltaTime)

    }        -PlayState PlayState        +Instance : GameManager

    

    class IGameState {        -GameOverState GameOverState        +InitializeGame()

        <<interface>>

        +Enter()        +static Instance : GameManager        +Run()

        +Update(deltaTime)

        +Exit()        -GameManager()    }

    }

            +InitializeGame()    

    class MenuState {

        -GameContext context        +Run()    class StateMachine {

        +Enter()

        +Update(deltaTime)    }        -IGameState _currentState

        +Exit()

    }            -Dictionary~string, IGameState~ _states

    

    class PlayState {    class StateMachine {        +AddState(name, state)

        -GameContext context

        +Enter()        -IGameState _currentState        +ChangeState(stateName)

        +Update(deltaTime)

        +Exit()        -Dictionary~string, IGameState~ _states        +Update(deltaTime)

    }

            +AddState(name, state)        +GetCurrentState()

    class GameOverState {

        -GameContext context        +ChangeState(stateName)    }

        -string winner

        +Enter()        +Update(deltaTime)    

        +Update(deltaTime)

        +Exit()    }    class GameContext {

    }

                +Ball Ball

    GameManager --> StateMachine

    GameManager --> MenuState    class IGameState {        +Paddle LeftPaddle

    GameManager --> PlayState

    GameManager --> GameOverState        <<interface>>        +Paddle RightPaddle

    StateMachine --> IGameState

    IGameState <|.. MenuState        +Enter()        +List~Wall~ Walls

    IGameState <|.. PlayState

    IGameState <|.. GameOverState        +Update(deltaTime)        +ScoreSubject ScoreSubject

```

        +Exit()        +SoundManager SoundManager

## 2. Command Pattern - Paddle Controls

    }        +PowerUpManager PowerUpManager

```mermaid

classDiagram            +ActiveEffectManager ActiveEffectManager

    class ICommand {

        <<interface>>    class MenuState {        +InputHandler InputHandler

        +Execute()

        +Undo()        -GameContext _context    }

    }

            +Enter()

    class MoveUpCommand {

        -Paddle paddle        +Update(deltaTime)    %% State Pattern

        -float previousY

        +Execute()        +Exit()    class IGameState {

        +Undo()

    }    }        <<interface>>

    

    class MoveDownCommand {            +Enter()

        -Paddle paddle

        -float previousY    class PlayState {        +Update(deltaTime)

        +Execute()

        +Undo()        -GameContext _context        +Exit()

    }

            +Enter()    }

    class StopPaddleCommand {

        -Paddle paddle        +Update(deltaTime)    

        -float previousSpeed

        +Execute()        +Exit()    class MenuState {

        +Undo()

    }    }        -GameContext _context

    

    class InputHandler {            +Enter()

        -Dictionary keyBindings

        -List commandHistory    class GameOverState {        +Update(deltaTime)

        +HandleKeyInput(leftPaddle, rightPaddle)

        +UpdatePaddleMovement(leftPaddle, rightPaddle)        -GameContext _context        +Exit()

        +UndoLastCommand()

    }        -string _winner    }

    

    class Paddle {        +Enter()    

        +float Y

        +float Speed        +Update(deltaTime)    class PlayState {

        +MoveUp()

        +MoveDown()        +Exit()        -GameContext _context

        +ResetSpeed()

    }    }        +Enter()

    

    ICommand <|.. MoveUpCommand            +Update(deltaTime)

    ICommand <|.. MoveDownCommand

    ICommand <|.. StopPaddleCommand    GameManager --> StateMachine        +Exit()

    InputHandler --> ICommand

    MoveUpCommand --> Paddle    GameManager --> MenuState    }

    MoveDownCommand --> Paddle

    StopPaddleCommand --> Paddle    GameManager --> PlayState    

```

    GameManager --> GameOverState    class GameOverState {

## 3. Component Pattern - Game Objects

    StateMachine --> IGameState        -GameContext _context

```mermaid

classDiagram    IGameState <|.. MenuState        +Enter()

    class IComponent {

        <<interface>>    IGameState <|.. PlayState        +Update(deltaTime)

        +Update(deltaTime)

    }    IGameState <|.. GameOverState        +Exit()

    

    class GameObject {```    }

        <<abstract>>

        -List components    

        +AddComponent(component)

        +RemoveComponent(component)## 2. Command Pattern    GameManager --> StateMachine

        +GetComponent()

        +Update(deltaTime)    GameManager --> GameContext

        +Draw()

    }```mermaid    StateMachine --> IGameState

    

    class TransformComponent {classDiagram    IGameState <|.. MenuState

        +float X

        +float Y    class ICommand {    IGameState <|.. PlayState

        +float Width

        +float Height        <<interface>>    IGameState <|.. GameOverState

        +GetBounds()

        +Update(deltaTime)        +Execute()```

    }

            +Undo()

    class MovementComponent {

        -TransformComponent transform    }    %% Component Pattern

        +Vector2D Velocity

        +float Speed        class IComponent {

        +SetVelocity(x, y)

        +Update(deltaTime)    class MoveUpCommand {        <<interface>>

    }

            -Paddle _paddle        +Update(deltaTime)

    class RenderComponent {

        -TransformComponent transform        -float _previousY    }

        +Color Color

        +bool IsCircle        +MoveUpCommand(paddle)    

        +Draw()

        +Update(deltaTime)        +Execute()    class GameObject {

    }

            +Undo()        <<abstract>>

    class Ball {

        -TransformComponent transform    }        #List~IComponent~ _components

        -MovementComponent movement

        -RenderComponent render            +AddComponent(component)

        +Draw()

    }    class MoveDownCommand {        +RemoveComponent(component)

    

    class Paddle {        -Paddle _paddle        +GetComponent~T~()

        -TransformComponent transform

        -RenderComponent render        -float _previousY        +Update(deltaTime)

        +Draw()

    }        +MoveDownCommand(paddle)        +Draw()*

    

    class Wall {        +Execute()    }

        -TransformComponent transform

        -MovementComponent movement        +Undo()    

        -RenderComponent render

        +Draw()    }    class TransformComponent {

    }

                +float X

    IComponent <|.. TransformComponent

    IComponent <|.. MovementComponent    class StopPaddleCommand {        +float Y

    IComponent <|.. RenderComponent

    GameObject <|-- Ball        -Paddle _paddle        +float Width

    GameObject <|-- Paddle

    GameObject <|-- Wall        -float _previousSpeed        +float Height

    GameObject --> IComponent

    Ball --> TransformComponent        +StopPaddleCommand(paddle)        +GetBounds() Rectangle

    Ball --> MovementComponent

    Ball --> RenderComponent        +Execute()        +Update(deltaTime)

    Paddle --> TransformComponent

    Paddle --> RenderComponent        +Undo()    }

    Wall --> TransformComponent

    Wall --> MovementComponent    }    

    Wall --> RenderComponent

```        class MovementComponent {



## 4. Observer Pattern - Score System    class InputHandler {        -TransformComponent _transform



```mermaid        <<Invoker>>        +Vector2D Velocity

classDiagram

    class ISubject {        -Dictionary~KeyCode, ICommand~ _keyBindings        +float Speed

        <<interface>>

        +Attach(observer)        -List~ICommand~ _commandHistory        +SetVelocity(x, y)

        +Detach(observer)

        +Notify()        +InputHandler(leftPaddle, rightPaddle)        +Update(deltaTime)

    }

            +HandleKeyInput(leftPaddle, rightPaddle)    }

    class IObserver {

        <<interface>>        +UpdatePaddleMovement(leftPaddle, rightPaddle)    

        +Update(subject)

    }        +UndoLastCommand()    class RenderComponent {

    

    class ScoreSubject {    }        -TransformComponent _transform

        -List observers

        -Scoreboard scoreboard            +Color Color

        -Ball ball

        -SoundManager soundManager    class Paddle {        +bool IsCircle

        +int LeftScore

        +int RightScore        <<Receiver>>        +Draw()

        +Attach(observer)

        +Detach(observer)        +float Y        +Update(deltaTime)

        +Notify()

        +LeftPoint()        +float Speed    }

        +RightPoint()

        +Start()        +MoveUp()

        +Reset()

    }        +MoveDown()    %% Entities

    

    class UIScoreObserver {        +ResetSpeed()    class Ball {

        -int leftScore

        -int rightScore    }        -TransformComponent _transform

        -bool gameStarted

        +Update(subject)            -MovementComponent _movement

        +DrawScore(windowWidth, windowHeight)

    }    ICommand <|.. MoveUpCommand        -RenderComponent _render

    

    class ConsoleScoreObserver {    ICommand <|.. MoveDownCommand        -float _baseSpeed

        +Update(subject)

    }    ICommand <|.. StopPaddleCommand        -Vector2D _velocity

    

    class Scoreboard {    InputHandler --> ICommand        +float X

        -int leftScore

        -int rightScore    MoveUpCommand --> Paddle        +float Y

        -bool gameStarted

        +LeftPoint()    MoveDownCommand --> Paddle        +int Size

        +RightPoint()

        +Start()    StopPaddleCommand --> Paddle        +Color Color

        +Reset()

    }```        +float Speed

    

    ISubject <|.. ScoreSubject        +Move()

    IObserver <|.. UIScoreObserver

    IObserver <|.. ConsoleScoreObserver## 3. Component Pattern        +Bounce(surfaceNormal)

    ScoreSubject --> IObserver

    ScoreSubject --> Scoreboard        +ResetPosition()

```

```mermaid        +Draw()

## 5. Decorator Pattern - Power-ups

classDiagram    }

```mermaid

classDiagram    class IComponent {    

    class IGameEntity {

        <<interface>>        <<interface>>    class Paddle {

        +GetSpeed()

        +GetSize()        +Update(deltaTime)        -TransformComponent _transform

        +Update(deltaTime)

    }    }        -RenderComponent _render

    

    class Ball {            -const float BASE_SPEED

        +GetSpeed()

        +GetSize()    class GameObject {        -const float MAX_SPEED

        +Update(deltaTime)

    }        <<abstract>>        +float X

    

    class Paddle {        #List~IComponent~ _components        +float Y

        +GetSpeed()

        +GetSize()        +AddComponent(component)        +int Width

        +Update(deltaTime)

    }        +RemoveComponent(component)        +int Height

    

    class EntityDecorator {        +GetComponent~T~()        +float Speed

        <<abstract>>

        -IGameEntity wrappedEntity        +Update(deltaTime)        +MoveUp()

        +GetSpeed()

        +GetSize()        +Draw()*        +MoveDown()

        +Update(deltaTime)

    }    }        +ResetSpeed()

    

    class SpeedBoostDecorator {            +Draw()

        -float speedMultiplier

        -DateTime startTime    class TransformComponent {    }

        -double duration

        +GetSpeed()        +float X    

        +IsActive()

    }        +float Y    class Wall {

    

    class SpeedReductionDecorator {        +float Width        -TransformComponent _transform

        -float speedMultiplier

        -DateTime startTime        +float Height        -MovementComponent _movement

        -double duration

        +GetSpeed()        +TransformComponent(x, y, width, height)        -RenderComponent _render

        +IsActive()

    }        +GetBounds() Rectangle        +const int WALL_WIDTH

    

    class SizeBoostDecorator {        +Update(deltaTime)        +const int WALL_HEIGHT

        -float sizeMultiplier

        -DateTime startTime    }        +float X

        -double duration

        +GetSize()            +float Y

        +IsActive()

    }    class MovementComponent {        +float YSpeed

    

    IGameEntity <|.. Ball        -TransformComponent _transform        +Move()

    IGameEntity <|.. Paddle

    IGameEntity <|.. EntityDecorator        +Vector2D Velocity        +Draw()

    EntityDecorator <|-- SpeedBoostDecorator

    EntityDecorator <|-- SpeedReductionDecorator        +float Speed    }

    EntityDecorator <|-- SizeBoostDecorator

    EntityDecorator --> IGameEntity        +MovementComponent(transform, speed)    

```

        +SetVelocity(x, y)    class Scoreboard {

## 6. Factory Pattern - Object Creation

        +Update(deltaTime)        -int _leftScore

```mermaid

classDiagram    }        -int _rightScore

    class IGameEntityFactory {

        <<interface>>            -bool _gameStarted

        +CreateBall(width, height)

        +CreatePaddle(x, y, height)    class RenderComponent {        +int LeftScore

        +CreateWall(x, y, height, speed)

        +CreateScoreboard()        -TransformComponent _transform        +int RightScore

        +CreateWalls(num, minDist, height)

        +CalculateWallCount(totalScore, baseWalls)        +Color Color        +LeftPoint()

    }

            +bool IsCircle        +RightPoint()

    class GameEntityFactory {

        -Random random        +RenderComponent(transform, color, isCircle)        +Start()

        +CreateBall(width, height)

        +CreatePaddle(x, y, height)        +Draw()        +Reset()

        +CreateWall(x, y, height, speed)

        +CreateScoreboard()        +Update(deltaTime)    }

        +CreateWalls(num, minDist, height)

        +CalculateWallCount(totalScore, baseWalls)    }

    }

            %% Decorator Pattern

    class EffectFactory {

        +ApplyEffect(type, ball, leftPaddle, rightPaddle)    class Ball {    class IGameEntity {

        +RemoveEffect(type, ball, leftPaddle, rightPaddle, height, speed)

        +ResetAllEffects(ball, leftPaddle, rightPaddle, height)        -TransformComponent _transform        <<interface>>

    }

            -MovementComponent _movement        +GetSpeed() float

    class ActiveEffectManager {

        -List activeEffects        -RenderComponent _render        +GetSize() float

        -EffectFactory effectFactory

        +ApplyEffect(type, duration)        +Ball(windowWidth, windowHeight)        +Update(deltaTime)

        +Update()

        +ClearAllEffects()        +Draw()    }

    }

        }    

    IGameEntityFactory <|.. GameEntityFactory

    ActiveEffectManager --> EffectFactory        class EntityDecorator {

```

    class Paddle {        <<abstract>>

## 7. Complete System Architecture

        -TransformComponent _transform        #IGameEntity _wrappedEntity

```mermaid

classDiagram        -RenderComponent _render        +GetSpeed() float

    class GameManager {

        -GameManager instance        +Paddle(x, y, windowHeight)        +GetSize() float

        -GameContext Context

        -StateMachine StateMachine        +Draw()        +Update(deltaTime)

        +Instance GameManager

        +InitializeGame()    }    }

        +Run()

    }        

    

    class GameContext {    class Wall {    class SpeedBoostDecorator {

        +Ball Ball

        +Paddle LeftPaddle        -TransformComponent _transform        -float _speedMultiplier

        +Paddle RightPaddle

        +List Walls        -MovementComponent _movement        -DateTime _startTime

        +ScoreSubject ScoreSubject

        +SoundManager SoundManager        -RenderComponent _render        -double _duration

        +PowerUpManager PowerUpManager

        +ActiveEffectManager ActiveEffectManager        +Wall(x, y, windowHeight, speedMultiplier)        +GetSpeed() float

        +InputHandler InputHandler

    }        +Draw()        +IsActive() bool

    

    class StateMachine {    }    }

        -IGameState currentState

        -Dictionary states        

        +AddState(name, state)

        +ChangeState(stateName)    IComponent <|.. TransformComponent    class SpeedReductionDecorator {

        +Update(deltaTime)

    }    IComponent <|.. MovementComponent        -float _speedMultiplier

    

    class IGameState {    IComponent <|.. RenderComponent        -DateTime _startTime

        <<interface>>

        +Enter()    GameObject <|-- Ball        -double _duration

        +Update(deltaTime)

        +Exit()    GameObject <|-- Paddle        +GetSpeed() float

    }

        GameObject <|-- Wall        +IsActive() bool

    class Ball {

        +Move()    GameObject --> IComponent    }

        +Bounce()

        +Draw()    Ball --> TransformComponent    

    }

        Ball --> MovementComponent    class SizeBoostDecorator {

    class Paddle {

        +MoveUp()    Ball --> RenderComponent        -float _sizeMultiplier

        +MoveDown()

        +Draw()    Paddle --> TransformComponent        -DateTime _startTime

    }

        Paddle --> RenderComponent        -double _duration

    class Wall {

        +Move()    Wall --> TransformComponent        +GetSize() float

        +Draw()

    }    Wall --> MovementComponent        +IsActive() bool

    

    class ScoreSubject {    Wall --> RenderComponent    }

        +LeftPoint()

        +RightPoint()```

        +Notify()

    }    %% Observer Pattern

    

    class InputHandler {## 4. Observer Pattern    class ISubject {

        +HandleKeyInput()

    }        <<interface>>

    

    class ActiveEffectManager {```mermaid        +Attach(observer)

        +ApplyEffect()

        +Update()classDiagram        +Detach(observer)

    }

        class ISubject {        +Notify()

    GameManager --> GameContext

    GameManager --> StateMachine        <<interface>>    }

    GameContext --> Ball

    GameContext --> Paddle        +Attach(observer)    

    GameContext --> Wall

    GameContext --> ScoreSubject        +Detach(observer)    class IObserver {

    GameContext --> InputHandler

    GameContext --> ActiveEffectManager        +Notify()        <<interface>>

    StateMachine --> IGameState

```    }        +Update(subject)


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

