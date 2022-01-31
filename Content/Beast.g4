grammar Beast;

program: instruction* EOF;

instruction: statement;

statement: assignment ';';

assignment: IDENTIFIER '=' value;



value: BOOL;


BOOL: 'true' | 'false';


IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;

WS: [ \t\r\n]+ -> skip;