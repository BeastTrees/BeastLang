grammar BeastLang;

program: packageDeclaration line* EOF;

line: statement | ifBlock | whileBlock;

statement: (assignment | functionCall | importStatement) ';';

ifBlock: 'if' '(' expression ')' block ('else' elseIfBlock)?;

elseIfBlock: block | ifBlock;

whileBlock: WHILE '(' expression ')' block ('else' elseIfBlock)?;

WHILE: 'while' | 'until';

assignment: ('var' | 'const')? IDENTIFIER '=' expression;

functionCall:
	IDENTIFIER '(' (expression (',' expression)*)? ')';

importStatement: 'import' PACKAGE_IMPORT;

packageDeclaration: 'package' PACKAGE ';';

expression:
	constant							#constantExpression
	| IDENTIFIER						#identifierExpression
	| functionCall						#functionCallExpression
	| '(' expression ')'				#parenthesizedExpression
	| '!' expression					#notExpression
    | expression multOp expression		#multiplicativeExpression
    | expression addOp expression		#additiveExpression
    | expression compareOp expression	#comparisonExpression
    | expression boolOp expression		#booleanExpression
    ;

multOp: '*' | '/' | '%';
addOp: '+' | '-';
compareOp: '==' | '!=' | '>' | '<' | '>=' | '<=';
boolOp: BOOL_OPERATOR;

BOOL_OPERATOR: '&&' | '||' | 'xor';

constant: INTEGER | FLOAT | STRING | BOOL | NULL;

INTEGER: [0-9]+;
FLOAT: [0-9]+ '.' [0-9]+;
STRING: ('"' ~'"'* '"') | ('\'' ~'\''* '\'');
BOOL: 'true' | 'false';
NULL: 'null';

block: '{' line* '}';

WS: [ \t\r\n]+ -> skip;
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;
PACKAGE: [a-zA-Z._*]+;
PACKAGE_IMPORT: PACKAGE ':' [a-zA-Z._*]+;

COMMENT: '/*' .*? '*/' -> skip;
LINE_COMMENT: '//' ~[\r\n]* -> skip;