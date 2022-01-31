using Antlr4.Runtime.Misc;
using Beast.Content;

namespace Beast.Frontends;
public class ScriptFrontend : BeastBaseVisitor<string>
{
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
        return $"Setting '{context.IDENTIFIER().GetText()}' to '{context.value().GetText()}'";
    }
}
