using Antlr4.Runtime;
using Beast;
using Beast.Content;
using Beast.Utils;

ConsoleErrorWriterDecorator.SetToConsole();

var fileName = args[0];

Console.WriteLine("[beastc] Start compiling on '" + fileName + "'...");

var fileContents = File.ReadAllText(fileName);
var inputStream = new AntlrInputStream(fileContents);
var lexer = new BeastLangLexer(inputStream);
var commonTokenStream = new CommonTokenStream(lexer);
var parser = new BeastLangParser(commonTokenStream);
parser.AddErrorListener(new BeastErrorHandler());
var beastContext = parser.program();
var visitor = new BeastVisitor();

visitor.Visit(beastContext);