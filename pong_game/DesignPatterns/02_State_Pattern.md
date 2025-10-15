# State Pattern - Game States

## Mô tả
State Pattern cho phép object thay đổi hành vi khi internal state thay đổi. Game có 3 trạng thái chính: Menu, Play, và GameOver.

## UML Diagram

```mermaid
classDiagram
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
        -ICollisionHandler _collisionHandler
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
    
    class StateMachine {
        -IGameState _currentState
        -Dictionary~string, IGameState~ _states
        +AddState(string name, IGameState state)
        +ChangeState(string stateName)
        +Update()
        +GetCurrentState() IGameState
    }
    style StateMachine fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class GameContext {
        +GameEntities Entities
        +GameServices Services
        +ScoreSubject ScoreSubject
        +int WindowWidth
        +int WindowHeight
    }
    style GameContext fill:#fff3e0,stroke:#ff9800,stroke-width:2px

    %% Relationships
    IGameState <|.. MenuState : implements
    IGameState <|.. PlayState : implements
    IGameState <|.. GameOverState : implements
    
    StateMachine o-- IGameState : manages
    StateMachine --> IGameState : current state
    
    MenuState --> GameContext : uses
    PlayState --> GameContext : uses
    GameOverState --> GameContext : uses

    note for IGameState "State Interface\n- Enter(): Initialize state\n- Update(): State logic\n- Exit(): Cleanup"
    
    note for StateMachine "Context Class\n- Stores registered states\n- Manages current state\n- Delegates to current state"
```

## State Transitions

```mermaid
stateDiagram-v2
    [*] --> MenuState
    MenuState --> PlayState : StartNewGame()
    PlayState --> GameOverState : Game ends
    GameOverState --> MenuState : ReturnToMenu()
    GameOverState --> PlayState : RestartGame()
```

## Implementation Details

### State Interface (IGameState):
```csharp
public interface IGameState
{
    void Enter();    // Called when entering this state
    void Update();   // Called every frame
    void Exit();     // Called when leaving this state
}
```

### State Machine:
```csharp
public class StateMachine
{
    private IGameState _currentState;
    private Dictionary<string, IGameState> _states;
    
    public void ChangeState(string stateName)
    {
        _currentState?.Exit();
        _currentState = _states[stateName];
        _currentState?.Enter();
    }
}
```

### Concrete States:

#### MenuState:
- Enter: Hiển thị menu, phát nhạc nền
- Update: Xử lý input menu
- Exit: Dừng nhạc menu

#### PlayState:
- Enter: Khởi tạo game entities, spawn power-ups
- Update: Game loop chính (input, physics, collision)
- Exit: Dừng game, cleanup

#### GameOverState:
- Enter: Hiển thị kết quả, phát nhạc game over
- Update: Xử lý input restart/menu
- Exit: Cleanup UI

## Benefits:
1. ✅ Tách biệt logic của từng state
2. ✅ Dễ dàng thêm states mới
3. ✅ Avoid massive if/switch statements
4. ✅ State transitions rõ ràng
