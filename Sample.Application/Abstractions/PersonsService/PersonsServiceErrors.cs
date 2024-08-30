using Sample.Domain.Abstractions;

namespace Sample.Application.Abstractions.PersonService;

public static class PersonsServiceErrors
{
    public static Error ConnectionError => new Error(
        "PersonsServiceErrors.ConnectionError",
        "Error trying to connect to the service");
}
