using Antlr4.Runtime;
using Beast.Content;
using Beast.Frontends;

var fileName = args[0];

Console.WriteLine("[beastc] Start compiling on '" + fileName + "'...");

var fileContents = File.ReadAllText(fileName);
var inputStream = new AntlrInputStream(fileContents);
var lexer = new BeastLexer(inputStream);
var commonTokenStream = new CommonTokenStream(lexer);
var parser = new BeastParser(commonTokenStream);
var beastContext = parser.program();
var visitor = new ScriptFrontend();

visitor.Visit(beastContext);