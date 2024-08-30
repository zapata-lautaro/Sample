using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sample.Application.Abstractions.PersonService;
using Sample.Application.Persons.ImportPersons;
using Sample.Domain.Abstractions;
using Sample.Domain.Persons;

namespace Sample.Application.Tests.Persons;

public class ImportPersonsCommandHandlerTests
{
    private readonly IPersonsService _personsService;
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ImportPersonsCommandHandler> _logger;
    private readonly ImportPersonsCommandHandler _sut;

    public ImportPersonsCommandHandlerTests()
    {
        _personsService = Substitute.For<IPersonsService>();
        _personRepository = Substitute.For<IPersonRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ILogger<ImportPersonsCommandHandler>>();
        _sut = new ImportPersonsCommandHandler(_personsService, _personRepository, _unitOfWork, _logger);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPersonsServiceReturnsFailure()
    {
        // Arrange
        var command = new ImportPersonsCommand();
        _personsService.GetPersons().Returns(Result.Failure<IReadOnlyCollection<PersonDTO>>(PersonsServiceErrors.ConnectionError));

        // Act
        Result result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PersonsServiceErrors.ConnectionError);
    }

    [Fact]
    public async Task Handle_ShouldNotSavePersons_WhenPersonsExistOrIsInvalid()
    {
        // Arrange
        var persons = new List<PersonDTO>
        {
            new PersonDTO("John Doe", "en", "1", "Bio", 1.0),
            new PersonDTO("Invalid name", "en", "2", "Bio", 1.0),
        };
        var command = new ImportPersonsCommand();
        _personsService.GetPersons().Returns(Result.Success<IReadOnlyCollection<PersonDTO>>(persons));
        _personRepository
            .GetPersons(Arg.Any<IEnumerable<PersonId>>())
            .Returns(persons
                .Select(p => Person.Create(
                    new PersonId(p.Id),
                    Name.FromFullNameString(p.Name).Value,
                    new Language(p.Language),
                    new Bio(p.Bio),
                    new PersonVersion(p.Version)))
                .ToList());

        // Act
        Result result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await _unitOfWork.DidNotReceive().SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldSaveNewPersons_WhenSomePersonsDoNotExistAndIsValid()
    {
        // Arrange
        var persons = new List<PersonDTO>
        {
            new PersonDTO("John Doe", "en", "1", "Bio", 1.0),
            new PersonDTO("Jane Doe", "fr", "2", "Bio", 1.0)
        };
        var command = new ImportPersonsCommand();
        _personsService.GetPersons().Returns(Result.Success<IReadOnlyCollection<PersonDTO>>(persons));
        _personRepository
            .GetPersons(Arg.Any<IEnumerable<PersonId>>())
            .Returns(new List<Person> {
                Person.Create(
                    new PersonId("1"),
                    Name.FromFullNameString("John Doe").Value,
                    new Language("en"),
                    new Bio("Bio"),
                    new PersonVersion(1.0))
            });

        // Act
        Result result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _personRepository.Received(1).AddRange(Arg.Is<ICollection<Person>>(x => x.Count() == 1 && x.First().Id == new PersonId("2")));
        await _unitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldLogWarning_WhenPersonExistsOrHasInvalidName()
    {
        // Arrange
        var person = new PersonDTO("Invalid", "en", "1", "Bio", 1.0);
        var persons = new List<PersonDTO> { person };
        var command = new ImportPersonsCommand();
        _personsService.GetPersons().Returns(Result.Success<IReadOnlyCollection<PersonDTO>>(persons));
        _personRepository.GetPersons(Arg.Any<IEnumerable<PersonId>>()).Returns(Enumerable.Empty<Person>());

        // Act
        Result result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _logger.Received(1);
        _personRepository.DidNotReceive().AddRange(Arg.Any<ICollection<Person>>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(CancellationToken.None);
    }
}
