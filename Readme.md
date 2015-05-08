#TypeSql

TypeSql is a compiler. It compiles annotated SQL into strongly-typed data-access-objects. 

The inspiration was [TypeScript](http://www.typescriptlang.org/). But don't try to take the analogy too far. TypeScript takes annotated javascript and compiles it into plain javascript. Its primary purpose is verification. TypeSQL takes annotated SQL and compiles it into a different, general-purpose programming language (C# or VB.NET). Our primary purpose is for you to write less code. Less code means less bugs and more functionality. It means you can ask for more money and go home earlier. Your family will love you more, and you will live longer. 

##Are you talking to me?
Do you believe that a strongly-typed, object-orientated programming language is the best way to write applications? 

Do you believe that SQL is *still* the best language for querying relational databases?

Good for you. Here at TypeSql HQ, we feel the same.

Bridging the O-O relational-databases divide has been attempted many times, in many ways. Frankly, we don't love any of them.
Here's our wish-list:
- We want to use SQL. Not Linq, not Hibernate Query Language. SQL. 
- We want the data returned to be typed. Call us old-fashioned if you will, but we like types. It's a little piece of certainty in an uncertain world.
- We want getting our data to be minimum friction. We don't want to repeat ourselves. The typical data-retrieval ceremony (connection, command, reader, casting, etc) is tedious, and surface-area for bugs.

The closest we've found are micro-ORMs (eg. [Dapper](http://code.google.com/p/dapper-dot-net/)). Our issue with using these is how to manage the classes created to hold query results (we like types, remember). In a large application, it is not uncommon for many queries to differ only slightly in the results they return. Which tends to lead to either:

###Class explosion
	User
	UserWithAddress
	UserWithImage
	UserAccounts

**or**

###Super DTO!
One class that contains all possible properties that can be returned. Consumers of the class accessing, say `user.Address.Street` then play Russian-Roulette, hoping that the data-access operation they just performed resulted in the Address property being populated. Otherwise, its that most dreaded of exceptions.

We have found that both approaches are messy to maintain. With TypeSql, you just the write and maintain the SQL. The data-access (DAO) and data-transfer (DTO) classes take care of themselves.

So that's where we're coming from.

##I already have [insert ORM of choice], why do I need TypeSql?
Maybe you don't. If your team is happy and productive using [ORM](http://en.wikipedia.org/wiki/Object-relational_mapping) X, then carry on. 
Full-service ORMs like [Entity Framework](http://msdn.microsoft.com/en-us/data/ef.aspx) and [NHibernate](http://nhforge.org/) are cruise-ships; TypeSQL is a surfboard. We just want to make travelling through the water a little easier and more fun. They do their best to hide the fact the ocean even exists (until the abstraction leaks, and sinks the ship...forgive me). But seriously, we approach the problem from different angles. TypeSql knows nothing about your database schema, and why should it? We care about the shape of the data you are *retrieving*, not the shape the data is *stored*. That is the fundamental difference. EF & friends map things that live in the database (like tables, views, and stored-procedures) to objects. They then attempt to translate interactions with those objects (ie. accessing properties and calling methods) into the appropriate SQL (this often happens at runtime). With TypeSQL, *you* write your SQL (plus a few annotations), and at *compile time* the SQL is translated to data-access objects that represent your *query* (not stuff that lives in the database). 
EF & friends also deal with concerns like caching and object-identity; TypeSql does not. 
 

##Show me some code! 
Here's how it works. We have some data in our relational DB. We want to query it. We write our parameterised SQL ( let's suppose its in a file called 'TurtlesByColor.tsql' ).

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

Accessing your data then looks like:

	IEnumerable<TurtlesByColorResult> redTurtles = new TurtlesByColor("turtlesDb").Execute("Red");

##Availability
Currently, TypeSQL is available in C# and VB.NET flavors. 

##Installation
Currently, the supported method for using TypeSql is via a VisualStudio extension in Visual Studio 2010 or Visual Studio 2012. 
- Install the [TypeSql Visual Studio extension](http://visualstudiogallery.msdn.microsoft.com/4e2dbc67-a429-4120-b56f-3a93a1003905). This can be done via Tools -> Extensions and Updates in Visual Studio.
- Install the [TypeSql nuget package](https://nuget.org/packages/TypeSql).

###Type that SQL baby!
- Add a new text file to your project; you can give it whatever extension you like (eg: OverdueBooks.tsql).
- Enter your type-annotated SQL.
- Right click on the file in the Visual Studio Solution Explorer, click 'Properties', and set the 'Custom Tool' property to 'TypeSql'.
- Two dependent files should be added under your TypeSql file: One will contain the source code of your generated data-access class; the other will contain the raw, unadorned SQL (this is just for convenience if you want to execute the SQL directly). 

##Its not you, its me
This is a work-in-progress. There are an infinite number of scenarios we have not thought of. We welcome your feedback. But please be gentle, our egos are fragile and we cry easily.

##Performance
Internally, TypeSql uses [Dapper](https://github.com/StackExchange/dapper-dot-net) for data-access. Its performance is effectively identical, which is itself effectively identical to raw ADO.NET. Checkout the [benchmarks](https://github.com/StackExchange/dapper-dot-net#performance).

##Credits
TypeSql stands on the shoulders of two brilliant projects.

###ANTLR
If you need to parse text, [ANTLR](http://www.antlr.org/) is your tool. [Terence Parr](http://www.cs.usfca.edu/~parrt/) has made language-recognition possible for mere mortals. And thanks to Sam Harwell for the C# version.

###Dapper
[Dapper](https://github.com/StackExchange/dapper-dot-net) has been called a micro-ORM. Whatever you call it, [Sam Saffron](http://samsaffron.com/) and [Marc Gravell](http://marcgravell.blogspot.com.au/) have created a super-useful component. 

##USAGE
###Outputs
Just append the items in your SELECT list with the types you expect them to be. e.g

	SELECT Id:int, Name:string
	FROM Smurfs

#### Aggregates
Yeah, you can do:

	SELECT COUNT(*) AS SmurfCount:int
	FROM Smurfs

###Inputs
Type your input parameters like so:

	SELECT Name:string
	FROM Smurfs
	WHERE Id= @SmurfId:int

###Nullable types
Sure.

	SELECT BirthDate:DateTime?
	FROM Smurfs

###Namespaces
The USING statement is used to import namespaces.
See Enums section below for an example.

###Enums
Yup. Say you have:

	namespace SmurfSpace {
		
		public enum SmurfSex {
			Female,
			Male
		}
	}

Then you can do:

	USING SmurfSpace	
	SELECT Id:int, Gender:SmurfSex


