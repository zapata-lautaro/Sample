namespace Sample.Domain.Persons
{
    public sealed class Person
    {
        private Person(
        string id,
        string name,
        string language,
        string bio,
        decimal version)
        {
            Id = id;
            Name = name;
            Language = language;
            Bio = bio;
            Version = version;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Language { get; private set; }
        public string Bio { get; private set; }
        public decimal Version { get; private set; }
    }
}
