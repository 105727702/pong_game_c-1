# Factory Pattern - Entity Creation

## Mô tả
Factory Pattern tập trung việc tạo objects vào một nơi, giảm coupling và tăng tính mở rộng. Sử dụng Dependency Injection để inject validators.

## UML Diagram

```mermaid
classDiagram
    class IGameEntityFactory {
        <<interface>>
        +CreateBall(windowWidth, windowHeight) Ball
        +CreatePaddle(x, y, windowHeight, isLeft) Paddle
        +CreateWall(y, windowWidth, windowHeight) Wall
        +CreateScoreboard() Scoreboard
        +CreateWalls(windowWidth, windowHeight) List~Wall~
        +CalculateWallCount(windowHeight) int
    }
    style IGameEntityFactory fill:#e1f5ff,stroke:#0066cc,stroke-width:3px
    
    class GameEntityFactory {
        -Random _random
        -IWallValidator _wallValidator
        +GameEntityFactory(IWallValidator validator)
        +CreateBall(windowWidth, windowHeight) Ball
        +CreatePaddle(x, y, windowHeight, isLeft) Paddle
        +CreateWall(y, windowWidth, windowHeight) Wall
        +CreateScoreboard() Scoreboard
        +CreateWalls(windowWidth, windowHeight) List~Wall~
        +CalculateWallCount(windowHeight) int
    }
    style GameEntityFactory fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class IWallValidator {
        <<interface>>
        +IsValidPosition(y, existingWalls, minGap) bool
    }
    style IWallValidator fill:#e1f5ff,stroke:#0066cc,stroke-width:3px
    
    class WallValidator {
        +IsValidPosition(y, existingWalls, minGap) bool
    }
    style WallValidator fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class Ball {
        +Ball(x, y, windowWidth, windowHeight)
    }
    style Ball fill:#fff3e0,stroke:#ff9800,stroke-width:2px
    
    class Paddle {
        +Paddle(x, y, windowHeight, isLeft)
    }
    style Paddle fill:#fff3e0,stroke:#ff9800,stroke-width:2px
    
    class Wall {
        +Wall(y, windowWidth, windowHeight)
    }
    style Wall fill:#fff3e0,stroke:#ff9800,stroke-width:2px
    
    class Scoreboard {
        +Scoreboard()
    }
    style Scoreboard fill:#fff3e0,stroke:#ff9800,stroke-width:2px

    %% Relationships
    IGameEntityFactory <|.. GameEntityFactory : implements
    IWallValidator <|.. WallValidator : implements
    
    GameEntityFactory --> IWallValidator : depends on
    GameEntityFactory ..> Ball : creates
    GameEntityFactory ..> Paddle : creates
    GameEntityFactory ..> Wall : creates
    GameEntityFactory ..> Scoreboard : creates
    
    WallValidator --> Wall : validates

    note for IGameEntityFactory "Factory Interface\n- Defines creation methods\n- Abstracts construction logic"
    
    note for GameEntityFactory "Concrete Factory\n- Constructor Injection\n- Encapsulates creation\n- Uses validator"
    
    note for IWallValidator "Validator Interface\n- Single Responsibility\n- Dependency Inversion"
```

## Factory Pattern with DI

```mermaid
graph TD
    A[GameManager] -->|injects| B[IWallValidator]
    B -->|implements| C[WallValidator]
    A -->|creates| D[GameEntityFactory]
    D -->|uses| B
    D -->|creates| E[Ball]
    D -->|creates| F[Paddle]
    D -->|creates| G[Wall]
    D -->|creates| H[Scoreboard]
    
    style A fill:#e3f2fd
    style D fill:#c8e6c9
    style B fill:#fff9c4
    style C fill:#fff9c4
    style E fill:#ffccbc
    style F fill:#ffccbc
    style G fill:#ffccbc
    style H fill:#ffccbc
```

## Implementation Details

### Factory Interface:
```csharp
public interface IGameEntityFactory
{
    Ball CreateBall(int windowWidth, int windowHeight);
    Paddle CreatePaddle(float x, float y, int windowHeight, bool isLeft);
    Wall CreateWall(float y, int windowWidth, int windowHeight);
    Scoreboard CreateScoreboard();
    List<Wall> CreateWalls(int windowWidth, int windowHeight);
    int CalculateWallCount(int windowHeight);
}
```

### Concrete Factory:
```csharp
public class GameEntityFactory : IGameEntityFactory
{
    private readonly Random _random;
    private readonly IWallValidator _wallValidator;
    
    // Constructor Injection
    public GameEntityFactory(IWallValidator wallValidator)
    {
        _random = new Random();
        _wallValidator = wallValidator;
    }
    
    public Ball CreateBall(int windowWidth, int windowHeight)
    {
        float x = windowWidth / 2;
        float y = windowHeight / 2;
        return new Ball(x, y, windowWidth, windowHeight);
    }
    
    public Paddle CreatePaddle(float x, float y, int windowHeight, bool isLeft)
    {
        return new Paddle(x, y, windowHeight, isLeft);
    }
    
    public Wall CreateWall(float y, int windowWidth, int windowHeight)
    {
        return new Wall(y, windowWidth, windowHeight);
    }
    
    public Scoreboard CreateScoreboard()
    {
        return new Scoreboard();
    }
    
    public List<Wall> CreateWalls(int windowWidth, int windowHeight)
    {
        List<Wall> walls = new List<Wall>();
        int wallCount = CalculateWallCount(windowHeight);
        int minGap = 150;
        
        for (int i = 0; i < wallCount; i++)
        {
            int maxAttempts = 50;
            int attempts = 0;
            
            while (attempts < maxAttempts)
            {
                float y = _random.Next(50, windowHeight - 50);
                
                // Use injected validator
                if (_wallValidator.IsValidPosition(y, walls, minGap))
                {
                    walls.Add(CreateWall(y, windowWidth, windowHeight));
                    break;
                }
                
                attempts++;
            }
        }
        
        return walls;
    }
    
    public int CalculateWallCount(int windowHeight)
    {
        return windowHeight / 200;
    }
}
```

### Wall Validator (Dependency Inversion):
```csharp
public interface IWallValidator
{
    bool IsValidPosition(float y, List<Wall> existingWalls, int minGap);
}

public class WallValidator : IWallValidator
{
    public bool IsValidPosition(float y, List<Wall> existingWalls, int minGap)
    {
        foreach (var wall in existingWalls)
        {
            if (Math.Abs(wall.Y - y) < minGap)
            {
                return false;
            }
        }
        return true;
    }
}
```

### Factory Usage:
```csharp
// Setup Dependency Injection
IWallValidator validator = new WallValidator();
IGameEntityFactory factory = new GameEntityFactory(validator);

// Create entities
Ball ball = factory.CreateBall(800, 600);
Paddle leftPaddle = factory.CreatePaddle(50, 250, 600, true);
Paddle rightPaddle = factory.CreatePaddle(730, 250, 600, false);
List<Wall> walls = factory.CreateWalls(800, 600);
Scoreboard scoreboard = factory.CreateScoreboard();
```

## SOLID Principles Applied

### Single Responsibility:
- ✅ **GameEntityFactory**: Chỉ lo creation logic
- ✅ **WallValidator**: Chỉ lo validation logic
- ✅ Tách Wall validation khỏi Wall entity

### Open/Closed:
- ✅ Mở rộng bằng cách thêm factory methods mới
- ✅ Không cần sửa code khi thêm entity types

### Liskov Substitution:
- ✅ Có thể swap IGameEntityFactory implementations

### Interface Segregation:
- ✅ IWallValidator là focused interface
- ✅ Không force unused methods

### Dependency Inversion:
- ✅ Factory depends on IWallValidator abstraction
- ✅ High-level không depend vào low-level details

## Benefits:
1. ✅ **Centralized Creation**: Tất cả creation logic ở 1 nơi
2. ✅ **Reduced Coupling**: Clients không cần biết constructors
3. ✅ **Testability**: Dễ mock factory cho testing
4. ✅ **Consistency**: Đảm bảo entities được tạo đúng cách
5. ✅ **Dependency Injection**: Inject validators, renderers
6. ✅ **Single Responsibility**: Tách validation logic ra khỏi entities
