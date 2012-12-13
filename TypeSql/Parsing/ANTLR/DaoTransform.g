tree grammar DaoTransform;

options {
	output=template;
    	language=CSharp3;
    	tokenVocab=TypeSql;
    	ASTLabelType=CommonTree;
}

@header {
using System;
using System.Linq;
using TypeSql.Parsing;
}

@namespace { TypeSql.Parsing }
@modifier{internal}

public sql [string name, string rawSql]
@init {
	var outputTokens = new List<OutputToken>();
	var inputTokens = new List<InputToken>();
}
	:	^(SQL (
			outputToken { outputTokens.Add($outputToken.token); } 
			| inputToken { inputTokens.Add($inputToken.token); }
			| .
			)*
		) 
		-> dao(name={name}, outputTokens={outputTokens}, inputTokens={inputTokens}, rawSql={rawSql} )
	;
	

outputToken returns [OutputToken token]
	:	^(OUTPUT_TOKEN id=ID type=ID )  { $token= new OutputToken($id.text, $type.text); }
	;

inputToken returns [InputToken token]
	:	^(INPUT_TOKEN id=ID type=ID )  { $token= new InputToken($id.text, $type.text); }
	;


