namespace Beast.Targets;

public interface ITarget
{
    object? Add(object? left, object? right);
    
    bool IsEquals(object? left, object? right);
    bool NotEquals(object? left, object? right);
    bool LessThan(object? left, object? right);
}