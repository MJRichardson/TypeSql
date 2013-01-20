grammar TypeSql;

options {
    language=CSharp3;
    output=AST;
}

tokens {
TYPESQL;
SQL;
OUTPUT_TOKEN;
INPUT_TOKEN;
TYPE;
NAMESPACE;
}

@modifier{internal}
@lexer::modifier{internal}
@namespace { TypeSql.Parsing }

public typesql	:	usingNamespace* sql -> ^(TYPESQL usingNamespace* sql)
//public typesql	:	 token* -> ^(TYPESQL ^(SQL token*))
	;

sql	:	token* -> ^(SQL token*)
	;

usingNamespace	: USING nameSpace -> ^(USING nameSpace )
	;

nameSpace 	: ID  ( '.' ID )* -> ^(NAMESPACE ID  ( '.' ID )*)
	;
	
token 	:	 outputToken
		|	('@' ID ':') => inputToken
		|	.
	;
	
outputToken  
	:	(ID ':')=> id=ID ':' type -> ^(OUTPUT_TOKEN $id type)
	|	('[' ID ']')=>'[' id=ID ']' ':' type -> ^(OUTPUT_TOKEN $id type)
	;
	
inputToken
	: 	'@' id=ID ':' type -> ^(INPUT_TOKEN $id type)
	;
	
type
	: ID '?'? -> ^(TYPE ID '?'?)
	;


//OUTPUT_TOKEN  : 
//	id=ID ':' type=ID 
	// {TypeSqlTokenId=$id.text; TypeSqlTokenType=$type.text;} 
	//-> ^(OUTPUT_TOKEN $id $type)
//    ; 	
    
//INPUT_TOKEN :
//	'@' ID ':' ID
//	;	
USING	: ('U'|'u')('S'|'s')('I'|'i')('N'|'n')('G'|'g') 	
	;
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

