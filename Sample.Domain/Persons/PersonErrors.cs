using Sample.Domain.Abstractions;

namespace Sample.Domain.Persons;

public static class PersonErrors
{
    public static Error EmptyName => new Error(
        "Person.Name.Empty",
        "Person name could not be empty");

    public static Error InvaludName => new Error(
        "Person.Name.Ivalid",
        "Person name should have at least one first name and one last name");
}
