#TypeSql

TypeSql is a compiler. It compiles annotated SQL into strongly-typed data-access-objects. The inspiration was [TypeScript](http://www.typescriptlang.org/).

Do you believe that a strongly-typed, object-orientated programming language is the best way to write applications?
Do you believe that SQL is *still* the best language for querying relational databases?

Good for you. Here at TypeSql HQ, we feel the same.

Bridging the O-O relational-databases divide has been attempted many times, in many ways. Frankly, we don't like any of them.
Here's our wish-list:
- We want to use SQL. Not a vaguely SQLish language that we have to reverse-engineeer to produce the SQL we want (e.g. Linq, Hibernate Query Language, etc).
- We want the data returned to be typed. Call us old-fashioned if you will, but we like types. It's a little piece of certainty in an uncertain world.
- We want getting our data to be minimum friction. We don't want to repeat ourselves. The typical data-retrieval ceremony (connection, command, reader, casting, etc) is tedious, and surface-area for bugs.

So that's where we're coming from.

##Show me some code! 
Here's how it works. We have some data in our relational DB. We want to query it. We write our parameterised SQL ( let's suppose its in a file called 'TurtlesByColor.sql' ).

	SELECT Id, Name 
	FROM TeenageMutantNinjaTurtles
	WHERE SashColor= @Color 
 
Now we annotate it with types.

	SELECT Id:int, Name:string 
	FROM TeenageMutantNinjaTurtles
	WHERE SashColor= @Color:string

Compile with TypeSql. This produces a class called `TurtlesByColor`. `TurtlesByColor` has two constructors (in C#):

	TurtlesByColor(string connectionStringName)

	TurtlesByColor(IDbConnection connection, IDbTransaction transaction=null) 

And a method:

	IEnumerable<TurtlesByColorResult> Execute(string color, bool buffered=true); 

The 'TurtlesByColorResult' class looks like:

	public partial class TurtlesByColorResult
	{
		public TurtlesByColorResult(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public int Id { get; private set; }	

		public string Name{ get; private set; }
		
	}

##Availability
Currently, TypeSQL is available in C# flavor only. Hopefully, we will be coming soon to a language near you.

##Usage
Currently, the only supported method for using TypeSql is via a VisualStudio extension in Visual Studio 2010 or Visual Studio 2012. 

###Installation
- Install the [TypeSql Visual Studio extension](http://visualstudiogallery.msdn.microsoft.com/4e2dbc67-a429-4120-b56f-3a93a1003905). This can be done via Tools -> Extensions and Updates in Visual Studio.
- Install the [TypeSql nuget package](https://nuget.org/packages/TypeSql).

###Type that SQL baby!
- Add a new text file to your project; you can give it whatever extension you like (eg: OverdueBooks.tsql).
- Enter your type-annotated SQL.
- Right click on the file in the Visual Studio Solution Explorer, click 'Properties', and set the 'Custom Tool' property to 'TypeSql'.
- Two dependent files should be added under your TypeSql file: One will contain the source code of your generated data-access class; the other will contain the raw, unadorned SQL (this is just for convenience if you want to execute the SQL directly). 

##Credits
Internally, TypeSql stands on the shoulders of two brilliant projects.

###ANTLR
If you need to parse text, [ANTLR](http://www.antlr.org/) is your tool. [Terence Parr](http://www.cs.usfca.edu/~parrt/) has made language-recognition possible for mere mortals. And thanks to Sam Harwell for the C# version.

###Dapper
[Dapper](http://code.google.com/p/dapper-dot-net/) has been called a micro-ORM. Whatever you call it, [Sam Saffron](http://samsaffron.com/) and [Marc Gravell](http://marcgravell.blogspot.com.au/) have created a super-useful component. 
