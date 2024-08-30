using Sample.Application.Abstractions.Messaging;

namespace Sample.Application.Persons.GetPersons;

public class GetPersonsQuery : IQuery<IReadOnlyCollection<PersonResponse>>
{
}
