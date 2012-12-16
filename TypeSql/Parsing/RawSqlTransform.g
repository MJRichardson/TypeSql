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

public sql 

	:	^(SQL (
			outputToken 
			| inputToken
			| .
			)*
		) 
	;

outputToken 
	:	^(OUTPUT_TOKEN id=ID type=ID ) -> template(id={$id.text}, type={$type.text}) "[<id>]" 
	;
	
inputToken 
	:	^(INPUT_TOKEN id=ID type=ID ) -> template(id={$id.text}, type={$type.text}) "@<id>" 
	;
