namespace Sample.Application.Abstractions.PersonService;

public sealed record PersonDTO(
    string Name,
    string Language,
    string Id,
    string Bio,
    double Version);
