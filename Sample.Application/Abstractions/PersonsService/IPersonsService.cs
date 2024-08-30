using Sample.Domain.Abstractions;

namespace Sample.Application.Abstractions.PersonService;

public interface IPersonsService
{
    Task<Result<IReadOnlyCollection<PersonDTO>>> GetPersons();
}
