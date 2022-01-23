using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Targets;

public class ScriptTarget : ITarget
{
    object? ITarget.Add(object? left, object? right)
    {
        if (left is int l && right is int r)
            return l + r;

        if (left is float lf && right is float rf)
            return lf + rf;

        if (left is int lInt && right is float rFloat)
            return lInt + rFloat;

        if (left is float lFloat && right is int rInt)
            return lFloat + rInt;
        if (left is string || right is string)
            return $"{left}{right}";

        throw new Exception($"Cannot add values of types {left?.GetType()} and {right?.GetType()}.");
    }

    bool ITarget.IsEquals(object? left, object? right)
    {
        return Object.Equals(left, right);
    }

    bool ITarget.NotEquals(object? left, object? right)
    {
        return left != right;
    }

    bool ITarget.LessThan(object? left, object? right)
{
        if (left is int l && right is int r)
            return l < r;
        if (left is float lf && right is float rf)
            return lf < rf;
        if (left is int lInt && right is float rFloat)
            return lInt < rFloat;
        if (left is float lFloat && right is int rInt)
            return lFloat < rInt;

        throw new Exception($"Cannot compare values of types {left?.GetType()} and {right?.GetType()}.");
    }
}