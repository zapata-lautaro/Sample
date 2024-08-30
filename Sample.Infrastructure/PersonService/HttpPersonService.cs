using System.Net.Http.Json;
using Sample.Application.Abstractions.PersonService;
using Sample.Domain.Abstractions;

namespace Sample.Infrastructure.PersonService;

public sealed class HttpPersonService : IPersonsService
{
    private readonly HttpClient _httpClient;

    public HttpPersonService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IReadOnlyCollection<PersonDTO>>> GetPersons()
    {
        try
        {
            return await _httpClient
                .GetFromJsonAsAsyncEnumerable<PersonDTO>("")
                .Where(person => person != null)
                .Select(person => person!)
                .ToListAsync();
        }
        catch (Exception e)
        {
            return Result<IReadOnlyCollection<PersonDTO>>.ValidationFailure(PersonsServiceErrors.ConnectionError);
        }

    }
}
