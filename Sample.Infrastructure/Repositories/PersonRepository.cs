using Microsoft.EntityFrameworkCore;
using Sample.Domain.Persons;

namespace Sample.Infrastructure.Repositories;

public sealed class PersonRepository : IPersonRepository
{
    private readonly ApplicationDbContext _context;

    public PersonRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddRange(ICollection<Person> persons)
    {
        _context.Persons.AddRange(persons);
    }

    public async Task<IEnumerable<Person>> GetPersons(IEnumerable<PersonId> ids)
    {
        return await _context.Persons
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
    }
}
