namespace Sample.Domain.Persons;

public sealed class Person
{
    private Person(
        PersonId id,
        Name name,
        Language language,
        Bio bio,
        PersonVersion version)
    {
        Id = id;
        Name = name;
        Language = language;
        Bio = bio;
        Version = version;
    }

    private Person()
    {
    }

    public PersonId Id { get; private set; }
    public Name Name { get; private set; }
    public Language Language { get; private set; }
    public Bio Bio { get; private set; }
    public PersonVersion Version { get; private set; }

    public static Person Create(
        PersonId id,
        Name name,
        Language language,
        Bio bio,
        PersonVersion version)
    {
        return new Person(id, name, language, bio, version);
    }
}
