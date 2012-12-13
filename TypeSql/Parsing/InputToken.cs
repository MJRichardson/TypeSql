namespace TypeSql.Parsing
{
    internal class InputToken
    {
        public string Id { get; private set; }
        public string Type { get; private set; }

        public InputToken(string id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}