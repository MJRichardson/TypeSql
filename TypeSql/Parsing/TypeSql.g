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
}

@modifier{internal}
@lexer::modifier{internal}
@namespace { TypeSql.Parsing }

public typesql	:	usingNamespace* sql -> ^(TYPESQL usingNamespace* sql)
//public typesql	:	 token* -> ^(TYPESQL ^(SQL token*))
	;

sql	:	token* -> ^(SQL token*)
	;

usingNamespace	: USING NAMESPACE -> ^(USING NAMESPACE)
	;

//nameSpace : NAMESPACE_SEGMENT ('.' NAMESPACE_SEGMENT)* 
	//;
	
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
USING	: ('U'|'u')('S'|'s')('I'|'i')('N'|'n')('G'|'g') 	
	;
ID  :	
	('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|DIGIT|'_')*
    ;

NAMESPACE  
	: (NAMESPACE_SEGMENT) ('.' NAMESPACE_SEGMENT)*	
	;

fragment NAMESPACE_SEGMENT 
	:	('a'..'z'|'A'..'Z'|DIGIT)*
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

