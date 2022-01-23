using Antlr4.Runtime.Misc;
using Beast.Content;

namespace Beast;

public class BeastVisitor : BeastLangBaseVisitor<object?>
{
    private Dictionary<string, object?> Variables { get; } = new();

    public BeastVisitor() {
        
    }

    private object? Write(object?[] args)
    {
        foreach(var arg in args)
        {
            Console.Write(arg);
        }
        Console.WriteLine();

        return null;
    }

    public override object? VisitImportStatement([NotNull] BeastLangParser.ImportStatementContext context)
    {
        var package = context.PACKAGE_IMPORT().GetText();
        Console.WriteLine("Importing package: " + package);
        switch (package)
        {
            case "beast.stdio:*":
                Variables["Write"] = new Func<object?[], object?>(Write);
                break;
        }
        return null;
    }

    public override object? VisitFunctionCall([NotNull] BeastLangParser.FunctionCallContext context)
    {
        var name = context.IDENTIFIER().ToString();
        var args = context.expression().Select(Visit).ToArray();

        if(!Variables.TryGetValue(name, out var value))
        {
            throw new Exception($"Function {name} is not defined.");
        }

        if (Variables[name] is not Func<object?[], object?> func)
            throw new Exception($"Variable {name} is not a function.");
        
        return func(args);
    }

    public override object? VisitAssignment([NotNull] BeastLangParser.AssignmentContext context)
    {
        var varName = context.IDENTIFIER().GetText();

        var value = Visit(context.expression());

        Variables[varName] = value;

        return null;
    }

    public override object? VisitIdentifierExpression([NotNull] BeastLangParser.IdentifierExpressionContext context)
    {
        var varName = context.IDENTIFIER().GetText();

        if(!Variables.ContainsKey(varName))
        {
            throw new Exception($"Variable {varName} is not defined yet.");
        }

        return Variables[varName];
    }

    public override object? VisitConstant([NotNull] BeastLangParser.ConstantContext context)
    {
        if(context.INTEGER() is { } i)
            return int.Parse(i.GetText());
        if (context.FLOAT() is { } f)
            return float.Parse(f.GetText());
        if (context.STRING() is { } s)
            return s.GetText()[1..^1];
        if (context.BOOL() is { } b)
            return b.GetText() == "true";
        if (context.NULL() is { })
            return null;

        throw new NotImplementedException();
    }

    public override object? VisitNotExpression([NotNull] BeastLangParser.NotExpressionContext context)
    {
        return IsFalse(Visit(context.expression()));
    }

    public override object? VisitAdditiveExpression([NotNull] BeastLangParser.AdditiveExpressionContext context)
    {
        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

        var op = context.addOp().GetText();

        return op switch
        {
            "+" => Add(left, right),
            // "-" => Subtract(left, right),
            _ => throw new NotImplementedException()
        };
    }

    private object? Add(object? left, object? right)
    {
        if(left is int l && right is int r)
            return l + r;

        if(left is float lf && right is float rf)
            return lf + rf;

        if(left is int lInt && right is float rFloat)
            return lInt + rFloat;

        if(left is float lFloat && right is int rInt)
            return lFloat + rInt;
        if(left is string || right is string)
            return $"{left}{right}";

        throw new Exception($"Cannot add values of types {left?.GetType()} and {right?.GetType()}.");
    }

    public override object? VisitWhileBlock([NotNull] BeastLangParser.WhileBlockContext context)
    {
        Func<object?, bool> condition = context.WHILE().GetText() == "while"
            ? IsTrue
            : IsFalse;

        if(condition(Visit(context.expression())))
        {
            do
            {
                Visit(context.block());
            } while (condition(Visit(context.expression())));
        } else if(context.elseIfBlock() != null)
        {
            Visit(context.elseIfBlock());
        }

        return null;
    }

    public override object? VisitIfBlock([NotNull] BeastLangParser.IfBlockContext context)
    {
        if (IsTrue(Visit(context.expression())))
        {
            Visit(context.block());
        } else if (context.elseIfBlock() != null)
        {
            Visit(context.elseIfBlock());
        }

        return null;
    }

    public override object? VisitComparisonExpression([NotNull] BeastLangParser.ComparisonExpressionContext context)
    {
        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

        var op = context.compareOp().GetText();

        return op switch
        {
            "==" => IsEquals(left, right),
            "!=" => NotEquals(left, right),
            // ">" => GreaterThan(left, right),
            "<" => LessThan(left, right),
            // ">=" => GreaterOrEqualThan(left, right),
            // "<=" => LessOrEqualThan(left, right),
            _ => throw new NotImplementedException()
        };
    }

    private bool IsEquals(object? left, object? right)
    {
        return Object.Equals(left, right);
    }

    private bool NotEquals(object? left, object? right)
    {
        return left != right;
    }

    private bool LessThan(object? left, object? right)
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

    private bool IsTrue(object? value)
    {
        if(value is bool b)
            return b;

        throw new Exception("Value is not boolean");
    }

    private bool IsFalse(object? value) => !IsTrue(value);
}