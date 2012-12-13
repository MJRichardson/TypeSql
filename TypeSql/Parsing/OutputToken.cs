namespace TypeSql.Parsing
{
internal class OutputToken
    {
        public string Id { get; private set; }
        public string Type { get; private set; }

        public OutputToken(string id, string type )
        {
            Id = id;
            Type = type;
        }
    }}