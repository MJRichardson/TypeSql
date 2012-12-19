#TypeSql

TypeSql is a compiler. It compiles annotated SQL into strongly-typed data-access-objects.

Do you believe that a strongly-typed, object-orientated programming language is the best way to write applications?
Do you believe that SQL is *still* the best language for querying relational databases?

Good for you. Here at TypeSql HQ, we feel the same.

Bridging the O-O relational-databases divide has been attempted many times, in many ways. Frankly, we don't like any of them.
Here's our wish-list:
- We want to use SQL. Not a vaguely SQLish language that we have to reverse-engineeer to produce the SQL we want (e.g. Linq, Hibernate Query Language, etc).
- We want the data returned to be typed. Call us old-fashioned if you will, but we like types. It's a little piece of certainty in an uncertain world.
- We want getting our data to be minimum friction. We don't want to repeat ourselves. The typical data-retrieval ceremony (connection, command, reader, casting, etc) is tedious, and surface-area for bugs.

So that's where we're coming from.

##Availability
Currently, the only TypeSql flavor available is C#.

##Show me some code! 
Here's how it works. We have some data in our relational DB. We want to query it. We write our parameterised SQL ( let's suppose its in a file called 'TurtlesByColor.sql' ).

	SELECT Id, Name 
	FROM TeenageMutantNinjaTurtles
	WHERE SashColor= @Color 
 
Now we annotate it with types.

	SELECT Id:int, Name:string 
	FROM TeenageMutantNinjaTurtles
	WHERE SashColor= @Color:string

Compile with TypeSql. This produces a class called `TurtlesByColor`. `TurtlesByColor` has two constructors:

	TurtlesByColor(string connectionStringName)

	TurtlesByColor(IDbConnection connection, IDbTransaction transaction=null) 

And a method:
	IEnumerable<TurtlesByColorResult> Execute(string color, bool buffered=true); 

The 'TurtlesByColorResult' class looks like:
	public partial class TurtlesByColorResult
	{
		public TurtlesByColorResult(int id, string name)
		{
			public int Id { get; private set; }	

			public string Name{ get; private set; }
		}
	}

##Usage

