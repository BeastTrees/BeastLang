using Antlr4.Runtime.Misc;
using Beast.Content;

namespace Beast.Frontends;
public class ScriptFrontend : BeastBaseVisitor<string>
{
    IDictionary<string, object?> variables = new Dictionary<string, object?>();

    public override string VisitInstruction([NotNull] BeastParser.InstructionContext context)
    {
        Console.WriteLine(Visit(context.statement()));
        return "";
    }

    public override string VisitStatement([NotNull] BeastParser.StatementContext context)
    {
        return Visit(context.assignment());
    }

    public override string VisitAssignment([NotNull] BeastParser.AssignmentContext context)
    {
        variables.Add(context.IDENTIFIER().GetText(), Visit(context.value()));
        return $"Setting '{context.IDENTIFIER().GetText()}' to '{Visit(context.value())}'";
    }

    public override string VisitValue([NotNull] BeastParser.ValueContext context)
    {
        if(context.IDENTIFIER() != null)
        {
            return variables[context.IDENTIFIER().GetText()].ToString();
        }
        return context.GetText();
    }
}
