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

public typeSql [string name, string rawSql]
@init {
	var outputTokens = new List<OutputToken>();
	var inputTokens = new List<InputToken>();
	var usingNamespaces = new List<string>();
}
	:	^(TYPESQL 
			(^(USING nameSpace) { usingNamespaces.Add($nameSpace.ns); } )* 
			(^(SQL (
				outputToken { outputTokens.Add($outputToken.token); } 
				| inputToken { inputTokens.Add($inputToken.token); }
				| .
			)*
		))		 )
		-> dao(name={name}, usingNamespaces={usingNamespaces}, outputTokens={outputTokens}, inputTokens={inputTokens}, rawSql={rawSql} )
	;

//usingStatement	: ^(USING NAMESPACE) { usingNamespaces.Add($NAMESPACE.text); }	
//	;

nameSpace returns [string ns]
	:	^(NAMESPACE part1=ID {$ns = $part1.text; } ('.' partN=ID { $ns+= ("." + $partN.text ); })* )
	;

outputToken returns [OutputToken token]
	:	^(OUTPUT_TOKEN id=ID type )  { $token= new OutputToken($id.text, $type.typeName); }
	;

inputToken returns [InputToken token]
	:	^(INPUT_TOKEN id=ID type )  { $token= new InputToken($id.text, $type.typeName); }
	;

type returns [string typeName] 	
	:	^(TYPE ID { $typeName=$ID.text; } ('?' {$typeName += '?'; })? )	;


