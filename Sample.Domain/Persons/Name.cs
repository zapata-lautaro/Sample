using Sample.Domain.Abstractions;

namespace Sample.Domain.Persons;

public record Name
{
    private Name(string first, string last)
    {
        First = first;
        Last = last;
    }

    public string First { get; private set; }
    public string Last { get; private set; }

    public static Name Create(string first, string last)
    {
        return new Name(first, last);
    }

    public static Result<Name> FromFullNameString(string fullName)
    {
        if (string.IsNullOrEmpty(fullName))
        {
            return Result<Name>.ValidationFailure(PersonErrors.EmptyName);
        }

        string[] nameChunks = fullName.Trim().Split(" ");
        if (nameChunks.Count() < 2)
        {
            return Result<Name>.ValidationFailure(PersonErrors.InvaludName);
        }

        return new Name(nameChunks.First(), nameChunks.Last());
    }
}
