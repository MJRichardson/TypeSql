grammar TypeSql;

options {
    language=CSharp3;
    output=AST;
}

tokens {
SQL;
OUTPUT_TOKEN;
INPUT_TOKEN;
}

@modifier{internal}
@lexer::modifier{internal}
@namespace { TypeSql.Parsing }

public sql	:	token* -> ^(SQL token*)
	;

token 	:	 outputToken
		|	('@' ID ':') => inputToken
		|	.
	;
	
outputToken  
	:	(ID ':')=> id=ID ':' type=ID -> ^(OUTPUT_TOKEN $id $type)
	|	('[' ID ']')=>'[' id=ID ']' ':' type=ID -> ^(OUTPUT_TOKEN $id $type)
	;
	
inputToken
	: 	'@' id=ID ':' type=ID -> ^(INPUT_TOKEN $id $type)
	;

//OUTPUT_TOKEN  : 
//	id=ID ':' type=ID 
	// {TypeSqlTokenId=$id.text; TypeSqlTokenType=$type.text;} 
	//-> ^(OUTPUT_TOKEN $id $type)
//    ; 	
    
//INPUT_TOKEN :
//	'@' ID ':' ID
//	;	

ID  :	
	('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|DIGIT|'_')*
    ;

fragment DIGIT 	: 
	'0'..'9'
    ;

NEWLINE :
	'\r'? '\n'{ $channel = Hidden; }
    ;

WHITESPACE  :   (' '|'\t')+ {$channel = Hidden;} 
	;

ANY 	:	.
	;
