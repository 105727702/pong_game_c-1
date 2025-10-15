# Command Pattern - Input Handling

## Mô tả
Command Pattern đóng gói requests thành objects, cho phép parameterize clients với queues, requests, và hỗ trợ undo operations.

## UML Diagram

```mermaid
classDiagram
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
    
    class Paddle {
        +float Y
        +float Speed
        +MoveUp()
        +MoveDown()
        +ResetSpeed()
    }
    style Paddle fill:#fff3e0,stroke:#ff9800,stroke-width:2px

    %% Relationships
    ICommand <|.. MoveUpCommand : implements
    ICommand <|.. MoveDownCommand : implements
    ICommand <|.. StopPaddleCommand : implements
    
    MoveUpCommand --> Paddle : controls
    MoveDownCommand --> Paddle : controls
    StopPaddleCommand --> Paddle : controls
    
    InputHandler o-- ICommand : stores commands
    InputHandler --> MoveUpCommand : uses
    InputHandler --> MoveDownCommand : uses
    InputHandler --> StopPaddleCommand : uses

    note for ICommand "Command Interface\n- Execute(): Perform action\n- Undo(): Reverse action"
    
    note for InputHandler "Invoker\n- Maps keys to commands\n- Executes commands\n- Decouples input from action"
```

## Command Pattern Flow

```mermaid
sequenceDiagram
    participant User
    participant InputHandler
    participant MoveUpCmd
    participant Paddle
    
    User->>InputHandler: Press W Key
    InputHandler->>InputHandler: Lookup key binding
    InputHandler->>MoveUpCmd: Execute()
    MoveUpCmd->>MoveUpCmd: Store _previousY
    MoveUpCmd->>Paddle: MoveUp()
    Paddle->>Paddle: Y -= Speed
    
    Note over User,Paddle: Undo Operation
    
    User->>InputHandler: Request Undo
    InputHandler->>MoveUpCmd: Undo()
    MoveUpCmd->>Paddle: Y = _previousY
```

## Key Bindings

```mermaid
graph LR
    A[W Key] --> B[MoveUpCommand<br/>Left Paddle]
    C[S Key] --> D[MoveDownCommand<br/>Left Paddle]
    E[Up Arrow] --> F[MoveUpCommand<br/>Right Paddle]
    G[Down Arrow] --> H[MoveDownCommand<br/>Right Paddle]
    
    I[Release W/S] --> J[StopPaddleCommand<br/>Left Paddle]
    K[Release Arrows] --> L[StopPaddleCommand<br/>Right Paddle]
    
    style B fill:#c8e6c9
    style D fill:#c8e6c9
    style F fill:#c8e6c9
    style H fill:#c8e6c9
    style J fill:#ffccbc
    style L fill:#ffccbc
```

## Implementation Details

### Command Interface:
```csharp
public interface ICommand
{
    void Execute();
    void Undo();
}
```

### Concrete Commands:

#### MoveUpCommand:
```csharp
public class MoveUpCommand : ICommand
{
    private Paddle _paddle;
    private float _previousY;
    
    public MoveUpCommand(Paddle paddle)
    {
        _paddle = paddle;
    }
    
    public void Execute()
    {
        _previousY = _paddle.Y;
        _paddle.MoveUp();
    }
    
    public void Undo()
    {
        _paddle.Y = _previousY;
    }
}
```

#### MoveDownCommand:
```csharp
public class MoveDownCommand : ICommand
{
    private Paddle _paddle;
    private float _previousY;
    
    public MoveDownCommand(Paddle paddle)
    {
        _paddle = paddle;
    }
    
    public void Execute()
    {
        _previousY = _paddle.Y;
        _paddle.MoveDown();
    }
    
    public void Undo()
    {
        _paddle.Y = _previousY;
    }
}
```

#### StopPaddleCommand:
```csharp
public class StopPaddleCommand : ICommand
{
    private Paddle _paddle;
    private float _previousSpeed;
    
    public StopPaddleCommand(Paddle paddle)
    {
        _paddle = paddle;
    }
    
    public void Execute()
    {
        _previousSpeed = _paddle.Speed;
        _paddle.ResetSpeed();
    }
    
    public void Undo()
    {
        _paddle.Speed = _previousSpeed;
    }
}
```

### InputHandler (Invoker):
```csharp
public class InputHandler
{
    private Dictionary<KeyCode, ICommand> _keyBindings;
    private ICommand _stopLeftPaddleCommand;
    private ICommand _stopRightPaddleCommand;
    
    public InputHandler(Paddle leftPaddle, Paddle rightPaddle)
    {
        _keyBindings = new Dictionary<KeyCode, ICommand>
        {
            { KeyCode.W, new MoveUpCommand(leftPaddle) },
            { KeyCode.S, new MoveDownCommand(leftPaddle) },
            { KeyCode.UpKey, new MoveUpCommand(rightPaddle) },
            { KeyCode.DownKey, new MoveDownCommand(rightPaddle) }
        };
        
        _stopLeftPaddleCommand = new StopPaddleCommand(leftPaddle);
        _stopRightPaddleCommand = new StopPaddleCommand(rightPaddle);
    }
    
    public void HandleKeyInput()
    {
        foreach (var binding in _keyBindings)
        {
            if (SplashKit.KeyDown(binding.Key))
            {
                binding.Value.Execute();
            }
        }
    }
    
    public void UpdatePaddleMovement()
    {
        if (!SplashKit.KeyDown(KeyCode.W) && !SplashKit.KeyDown(KeyCode.S))
            _stopLeftPaddleCommand.Execute();
            
        if (!SplashKit.KeyDown(KeyCode.UpKey) && !SplashKit.KeyDown(KeyCode.DownKey))
            _stopRightPaddleCommand.Execute();
    }
}
```

## Benefits:
1. ✅ **Decoupling**: Tách input detection khỏi action execution
2. ✅ **Undo/Redo**: Dễ dàng implement undo functionality
3. ✅ **Command Queuing**: Có thể queue commands để execute sau
4. ✅ **Macro Commands**: Combine nhiều commands thành 1
5. ✅ **Configurable**: Dễ dàng thay đổi key bindings
6. ✅ **Testability**: Test commands độc lập với input system

## Use Cases:
- ⚙️ Input handling (keyboard, gamepad)
- 🔄 Undo/Redo systems
- 📝 Macro recording
- 🎮 AI scripting
- 📊 Transaction systems
