# Observer Pattern - Score System

## Mô tả
Observer Pattern cho phép objects subscribe và nhận thông báo khi có thay đổi về điểm số. Tách biệt logic scoring khỏi UI rendering.

## UML Diagram

```mermaid
classDiagram
    class IObserver {
        <<interface>>
        +Update(ScoreSubject subject)
    }
    style IObserver fill:#e1f5ff,stroke:#0066cc,stroke-width:3px
    
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
        +Attach(IObserver observer)
        +Detach(IObserver observer)
        +Notify()
        +LeftPoint()
        +RightPoint()
        +Start()
        +Reset()
        +CheckBallOutOfBounds()
    }
    style ScoreSubject fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class UIScoreObserver {
        -int _leftScore
        -int _rightScore
        -bool _gameStarted
        +int LeftScore
        +int RightScore
        +bool GameStarted
        +Update(ScoreSubject subject)
        +DrawScore()
    }
    style UIScoreObserver fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class ConsoleScoreObserver {
        +Update(ScoreSubject subject)
    }
    style ConsoleScoreObserver fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class Scoreboard {
        +int LeftScore
        +int RightScore
        +bool GameStarted
        +LeftPoint()
        +RightPoint()
        +Start()
        +Reset()
    }
    style Scoreboard fill:#fff3e0,stroke:#ff9800,stroke-width:2px

    %% Relationships
    IObserver <|.. UIScoreObserver : implements
    IObserver <|.. ConsoleScoreObserver : implements
    
    ScoreSubject o-- IObserver : maintains list
    ScoreSubject --> Scoreboard : delegates to
    ScoreSubject --> Ball : checks position
    ScoreSubject --> SoundManager : plays sound
    ScoreSubject --> ActiveEffectManager : clears effects

    note for ScoreSubject "Subject (Publisher)\n- Maintains observer list\n- Notifies on score change\n- Delegates to Scoreboard"
    
    note for IObserver "Observer Interface\n- Update() called when\n  score changes"
```

## Observer Pattern Flow

```mermaid
sequenceDiagram
    participant Ball
    participant ScoreSubject
    participant Scoreboard
    participant UIObserver
    participant ConsoleObserver
    participant SoundManager
    
    Ball->>ScoreSubject: CheckBallOutOfBounds()
    ScoreSubject->>Scoreboard: LeftPoint() / RightPoint()
    ScoreSubject->>SoundManager: PlayEffect(BallOut)
    ScoreSubject->>ScoreSubject: Notify()
    ScoreSubject->>UIObserver: Update(this)
    UIObserver->>UIObserver: Update local state
    ScoreSubject->>ConsoleObserver: Update(this)
    ConsoleObserver->>ConsoleObserver: Log to console
```

## Implementation Details

### Observer Interface:
```csharp
public interface IObserver
{
    void Update(ScoreSubject subject);
}
```

### Subject (ScoreSubject):
```csharp
public class ScoreSubject
{
    private List<IObserver> _observers = new List<IObserver>();
    private Scoreboard _scoreboard;
    
    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }
    
    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }
    
    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(this);
        }
    }
    
    public void LeftPoint()
    {
        _scoreboard.LeftPoint();
        _soundManager.PlayEffect(SoundType.BallOut);
        Notify(); // Notify all observers
    }
}
```

### Concrete Observers:

#### UIScoreObserver:
- Cập nhật UI score display
- Render điểm số lên màn hình
- Pull data từ ScoreSubject

#### ConsoleScoreObserver:
- Log điểm số ra console
- Debug và monitoring

## Key Features:

### Subject Responsibilities:
1. ✅ Maintain observer list (Attach/Detach)
2. ✅ Notify observers on state change
3. ✅ Delegate scoring logic to Scoreboard
4. ✅ Coordinate với SoundManager, EffectManager

### Observer Responsibilities:
1. ✅ Register với Subject
2. ✅ Update local state khi notified
3. ✅ Pull data từ Subject (không push)

## Benefits:
1. ✅ Loose coupling: Subject không biết concrete observers
2. ✅ Dễ thêm observers mới (Open/Closed Principle)
3. ✅ Tách biệt business logic khỏi presentation
4. ✅ Multiple views của cùng data
