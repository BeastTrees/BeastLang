using Antlr4.Runtime;

namespace Beast;

public class BeastErrorHandler : BaseErrorListener
{
    public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
        var sourceName = recognizer.InputStream.SourceName;
        Console.WriteLine(sourceName);
        sourceName = String.Format("{0:s}:{1:d}:{2:d}", sourceName, line, charPositionInLine);

        Console.Error.WriteLine(sourceName + " - " + msg + " - " + offendingSymbol.StartIndex);
    }
}