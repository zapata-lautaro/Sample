namespace Sample.Domain.Persons;

public interface IPersonRepository
{
    void AddRange(ICollection<Person> persons);

    Task<IEnumerable<Person>> GetPersons(IEnumerable<PersonId> ids);
}
