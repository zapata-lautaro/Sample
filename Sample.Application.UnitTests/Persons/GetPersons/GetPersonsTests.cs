using System.Data;
using Dapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sample.Application.Abstractions.Data;

namespace Sample.Application.Persons.GetPersons.Tests;


public class GetPersonsTests
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IDbConnection _dbConnection;
    private readonly GetPersonsQueryHandler _handler;

    public GetPersonsTests()
    {
        _sqlConnectionFactory = Substitute.For<ISqlConnectionFactory>();
        _dbConnection = Substitute.For<IDbConnection>();
        _handler = new GetPersonsQueryHandler(_sqlConnectionFactory);
    }

    [Fact]
    public async Task Handle_ShouldReturnPersonsOrderedByName_WhenQueryIsSuccessful()
    {
        // Arrange
        var query = new GetPersonsQuery();
        CancellationToken cancellationToken = CancellationToken.None;

        var personResponses = new List<PersonResponse>
        {
            new PersonResponse( "1", "John", "Doe", "English", "Bio", 1.0),
            new PersonResponse( "2", "John", "Doe", "English", "Bio", 1.0),
        };

        _dbConnection.QueryAsync<PersonResponse>(Arg.Any<string>()).Returns(personResponses);
        _sqlConnectionFactory.CreateConnection().Returns(_dbConnection);

        // Act
        Domain.Abstractions.Result<IReadOnlyCollection<PersonResponse>> result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(personResponses);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyCollection_WhenNoPersonsAreFound()
    {
        // Arrange
        var query = new GetPersonsQuery();
        CancellationToken cancellationToken = CancellationToken.None;

        var personResponses = new List<PersonResponse>();

        _dbConnection.QueryAsync<PersonResponse>(Arg.Any<string>()).Returns(personResponses);

        // Act
        Domain.Abstractions.Result<IReadOnlyCollection<PersonResponse>> result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenQueryFails()
    {
        // Arrange
        var query = new GetPersonsQuery();
        CancellationToken cancellationToken = CancellationToken.None;

        _dbConnection.QueryAsync<PersonResponse>(Arg.Any<string>()).Throws(new DataException("Database error"));

        // Act
        Domain.Abstractions.Result<IReadOnlyCollection<PersonResponse>> result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Database error");
    }
}
