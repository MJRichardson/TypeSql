tree grammar RawSqlTransform;

options {
	output=template;
	rewrite=true;
    	language=CSharp3;
    	tokenVocab=TypeSql;
    	ASTLabelType=CommonTree;
}

@namespace { TypeSql.Parsing }
@modifier{internal}

public typeSql 

	:	^(TYPESQL 
			usingNamespace*
			^(SQL (	outputToken | inputToken | . )*	) 
		)
	;

usingNamespace 	:	^(USING ^(NAMESPACE ID ('.' ID)*)) -> template() ""
;

outputToken 
	:	^(OUTPUT_TOKEN id=ID type ) -> template(id={$id.text}) "[<id>]" 
	;
	
inputToken 
	:	^(INPUT_TOKEN id=ID type ) -> template(id={$id.text}) "@<id>" 
	;
	
type returns [string typeName] 	
	:	^(TYPE ID { $typeName=$ID.text; } ('?' {$typeName += '?'; })? )	;
