# Component Pattern - Game Entities

## Mô tả
Component Pattern cho phép entities được compose từ các components độc lập, tái sử dụng được. Thay vì inheritance hierarchy, sử dụng composition.

## UML Diagram

```mermaid
classDiagram
    class IComponent {
        <<interface>>
        +Update()
    }
    style IComponent fill:#e1f5ff,stroke:#0066cc,stroke-width:3px
    
    class GameObject {
        <<abstract>>
        #List~IComponent~ _components
        +AddComponent(IComponent component)
        +RemoveComponent(IComponent component)
        +GetComponent~T~() T
        +Update()
        +Draw()*
    }
    style GameObject fill:#fff4e1,stroke:#ff9800,stroke-width:3px
    
    class TransformComponent {
        +float X
        +float Y
        +float Width
        +float Height
        +GetBounds() Rectangle
        +Update()
    }
    style TransformComponent fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class MovementComponent {
        -TransformComponent _transform
        +Vector2D Velocity
        +float Speed
        +Update()
        +SetVelocity(Vector2D velocity)
    }
    style MovementComponent fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class RenderComponent {
        -TransformComponent _transform
        -IRenderer _renderer
        +Color Color
        +bool IsCircle
        +Update()
        +Draw()
    }
    style RenderComponent fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class CollisionComponent {
        -TransformComponent _transform
        +bool IsSolid
        +Update()
        +GetBounds() Rectangle
        +CheckCollision(CollisionComponent other) bool
    }
    style CollisionComponent fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class Ball {
        -TransformComponent _transform
        -MovementComponent _movement
        -RenderComponent _render
        +Move()
        +Bounce()
        +ResetPosition()
        +Draw()
    }
    style Ball fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class Paddle {
        -TransformComponent _transform
        -RenderComponent _render
        +MoveUp()
        +MoveDown()
        +Draw()
    }
    style Paddle fill:#e8f5e9,stroke:#4caf50,stroke-width:2px
    
    class Wall {
        -TransformComponent _transform
        -MovementComponent _movement
        -RenderComponent _render
        +Move()
        +Draw()
    }
    style Wall fill:#e8f5e9,stroke:#4caf50,stroke-width:2px

    %% Relationships
    IComponent <|.. TransformComponent : implements
    IComponent <|.. MovementComponent : implements
    IComponent <|.. RenderComponent : implements
    IComponent <|.. CollisionComponent : implements
    
    GameObject o-- IComponent : contains
    GameObject <|-- Ball : extends
    GameObject <|-- Paddle : extends
    GameObject <|-- Wall : extends
    
    Ball --> TransformComponent : uses
    Ball --> MovementComponent : uses
    Ball --> RenderComponent : uses
    
    Paddle --> TransformComponent : uses
    Paddle --> RenderComponent : uses
    
    Wall --> TransformComponent : uses
    Wall --> MovementComponent : uses
    Wall --> RenderComponent : uses
    
    MovementComponent --> TransformComponent : depends on
    RenderComponent --> TransformComponent : depends on
    CollisionComponent --> TransformComponent : depends on

    note for GameObject "Base Entity\n- Manages components\n- Update() all components\n- Abstract Draw()"
    
    note for IComponent "Component Interface\n- Update() lifecycle\n- Reusable behavior"
```

## Component Types

### Core Components:

#### 1. **TransformComponent**
- **Purpose**: Position, size, bounds
- **Data**: X, Y, Width, Height
- **Methods**: GetBounds()

#### 2. **MovementComponent**
- **Purpose**: Velocity, speed logic
- **Dependencies**: TransformComponent
- **Methods**: SetVelocity(), Update() position

#### 3. **RenderComponent**
- **Purpose**: Visual representation
- **Dependencies**: TransformComponent, IRenderer
- **Methods**: Draw()

#### 4. **CollisionComponent**
- **Purpose**: Collision detection
- **Dependencies**: TransformComponent
- **Methods**: CheckCollision(), GetBounds()

## Entity Composition

```mermaid
graph TD
    A[Ball] --> T1[TransformComponent]
    A --> M1[MovementComponent]
    A --> R1[RenderComponent]
    
    B[Paddle] --> T2[TransformComponent]
    B --> R2[RenderComponent]
    
    C[Wall] --> T3[TransformComponent]
    C --> M2[MovementComponent]
    C --> R3[RenderComponent]
    
    style A fill:#e3f2fd
    style B fill:#e3f2fd
    style C fill:#e3f2fd
    style T1 fill:#c8e6c9
    style T2 fill:#c8e6c9
    style T3 fill:#c8e6c9
    style M1 fill:#fff9c4
    style M2 fill:#fff9c4
    style R1 fill:#f8bbd0
    style R2 fill:#f8bbd0
    style R3 fill:#f8bbd0
```

## Implementation Details

### GameObject Base Class:
```csharp
public abstract class GameObject
{
    protected List<IComponent> _components = new List<IComponent>();
    
    public void AddComponent(IComponent component)
    {
        _components.Add(component);
    }
    
    public T GetComponent<T>() where T : IComponent
    {
        return _components.OfType<T>().FirstOrDefault();
    }
    
    public void Update()
    {
        foreach (var component in _components)
        {
            component.Update();
        }
    }
    
    public abstract void Draw();
}
```

### Component Implementation:
```csharp
public class TransformComponent : IComponent
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    
    public Rectangle GetBounds()
    {
        return new Rectangle(X, Y, Width, Height);
    }
    
    public void Update() { }
}

public class MovementComponent : IComponent
{
    private TransformComponent _transform;
    public Vector2D Velocity { get; set; }
    public float Speed { get; set; }
    
    public void Update()
    {
        _transform.X += Velocity.X * Speed;
        _transform.Y += Velocity.Y * Speed;
    }
}
```

### Entity Usage:
```csharp
public class Ball : GameObject
{
    private TransformComponent _transform;
    private MovementComponent _movement;
    
    public Ball()
    {
        _transform = new TransformComponent();
        _movement = new MovementComponent { _transform };
        
        AddComponent(_transform);
        AddComponent(_movement);
    }
    
    public void Move()
    {
        Update(); // Updates all components
    }
}
```

## Benefits:
1. ✅ **Composition over Inheritance**: Linh hoạt hơn inheritance
2. ✅ **Reusability**: Components tái sử dụng cho nhiều entities
3. ✅ **Separation of Concerns**: Mỗi component có 1 responsibility
4. ✅ **Easy Extension**: Thêm components mới không ảnh hưởng code cũ
5. ✅ **Flexibility**: Entities có thể add/remove components runtime
