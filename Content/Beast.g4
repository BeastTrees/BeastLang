grammar Beast;

program: instruction* EOF;

instruction: statement;

statement: assignment ';';

assignment: IDENTIFIER '=' value;



value: constant;

constant: INTEGER | FLOAT | BOOL;


INTEGER: [0-9]*;
FLOAT: [0-9.]*;
BOOL: 'true' | 'false';



IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;

WS: [ \t\r\n]+ -> skip;