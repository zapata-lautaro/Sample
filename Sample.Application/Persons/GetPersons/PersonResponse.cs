namespace Sample.Application.Persons;

public sealed record PersonResponse(
    string Id,
    string FirstName,
    string LastName,
    string Language,
    string Bio,
    double Version)
{


    public override string ToString()
    {
        return $"""
            First name: {FirstName}
            Last name: {LastName}
            Language: {Language}
            Id: {Id}
            Bio: {Bio}
            Version: {Version}
            """;
    }
}
