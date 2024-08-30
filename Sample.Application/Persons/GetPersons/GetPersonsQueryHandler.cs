using Dapper;
using Sample.Application.Abstractions.Data;
using Sample.Application.Abstractions.Messaging;
using Sample.Domain.Abstractions;

namespace Sample.Application.Persons.GetPersons;

internal class GetPersonsQueryHandler : IQueryHandler<GetPersonsQuery, IReadOnlyCollection<PersonResponse>>
{
    private readonly ISqlConnectionFactory sqlConnectionFactory;

    public GetPersonsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        this.sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyCollection<PersonResponse>>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
    {
        using System.Data.Common.DbConnection connection = sqlConnectionFactory.CreateConnection();

        const string query = """
            SELECT 
                id as Id, 
                name_first as FirstName, 
                name_last as LastName, 
                language as Language, 
                bio as Bio, 
                version as Version
            FROM persons
            ORDER BY name_first, name_last
            """;

        List<PersonResponse> persons = (await connection.QueryAsync<PersonResponse>(query)).AsList();

        return persons;
    }
}
