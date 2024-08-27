namespace Sample.Domain.Persons
{
    public sealed class Person
    {
        private Person(
            PersonId id,
            Name name,
            Language language,
            Bio bio,
            Version version)
        {
            Id = id;
            Name = name;
            Language = language;
            Bio = bio;
            Version = version;
        }

        public PersonId Id { get; private set; }
        public Name Name { get; private set; }
        public Language Language { get; private set; }
        public Bio Bio { get; private set; }
        public Version Version { get; private set; }

        public static Person Create(
            PersonId id,
            Name name,
            Language language,
            Bio bio,
            Version version)
        {
            return new Person(id, name, language, bio, version);
        }
    }
}
