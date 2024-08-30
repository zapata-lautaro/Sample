using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using Sample.Application.Abstractions.Messaging;
using Sample.Application.Abstractions.PersonService;
using Sample.Domain.Abstractions;
using Sample.Domain.Persons;

namespace Sample.Application.Persons.ImportPersons;

public sealed class ImportPersonsCommandHandler : ICommandHandler<ImportPersonsCommand>
{
    private readonly IPersonsService _personsService;
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ImportPersonsCommandHandler> _logger;

    public ImportPersonsCommandHandler(
        IPersonsService personsService,
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork,
        ILogger<ImportPersonsCommandHandler> logger)
    {
        _personsService = personsService;
        _personRepository = personRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(ImportPersonsCommand request, CancellationToken cancellationToken)
    {
        Result<IReadOnlyCollection<PersonDTO>> result = await _personsService.GetPersons();
        if (result.IsFailure)
        {
            return result;
        }
        IReadOnlyCollection<PersonDTO> personsFromService = result.Value.Where(p => p.Id is not null).ToList();

        var existingPersonIds = (await _personRepository
            .GetPersons(personsFromService.Select(p => new PersonId(p.Id))))
            .Select(p => p.Id)
            .ToImmutableHashSet();

        IList<Person> mappedPersons = MapPersons(personsFromService, existingPersonIds);
        if (mappedPersons.Count() > 0)
        {
            _personRepository.AddRange(mappedPersons);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }

    private IList<Person> MapPersons(IReadOnlyCollection<PersonDTO> personsFromService, ImmutableHashSet<PersonId> existingPersonIds)
    {
        IList<Person> persons = [];

        foreach (PersonDTO personDTO in personsFromService)
        {
            if (existingPersonIds.Contains(new PersonId(personDTO.Id)))
            {
                _logger.LogWarning("Person with id {id} already exists and was skiped from import", personDTO.Id);
                continue;
            }

            Result<Name> nameResult = Name.FromFullNameString(personDTO.Name);
            if (nameResult.IsFailure)
            {
                _logger.LogWarning("Person with id {id} was skiped from import. Error: {error}", personDTO.Id, nameResult.Error.Name);
                continue;
            }

            persons.Add(
                Person.Create(
                    new PersonId(personDTO.Id),
                    nameResult.Value,
                    new Language(personDTO.Language),
                    new Bio(personDTO.Bio),
                    new PersonVersion(personDTO.Version)
                )
            );
        }

        return persons;
    }
}
