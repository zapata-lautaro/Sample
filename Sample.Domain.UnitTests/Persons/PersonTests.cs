using FluentAssertions;
using Sample.Domain.Abstractions;
using Sample.Domain.Persons;

namespace Sample.Domain.UnitTests.Persons;

public class PersonTests
{
    [Theory]
    [MemberData(nameof(ValidNameCases))]
    public void NameFromFullNameString_WhenStringIsValid_ShouldReturnNameWithExpectedValues(
        string caseDescription,
        string fullNameString,
        string expectedFirstName,
        string expectedLastName)
    {
        //act
        Result<Name> result = Name.FromFullNameString(fullNameString);

        //assert
        caseDescription.Should().NotBeEmpty();
        result.IsSuccess.Should().BeTrue();
        result.Value.First.Should().Be(expectedFirstName);
        result.Value.Last.Should().Be(expectedLastName);
    }

    public static IEnumerable<object[]> ValidNameCases =>
    new List<object[]>
    {
        new object[]
        {
            "Full name string with one first name and one last name",
            "Lautaro Zapata",
            "Lautaro",
            "Zapata"
        },
       new object[]
        {
            "Full name string with multiple names and one last name",
            "Lautaro Nicolas Zapata",
            "Lautaro",
            "Zapata"
        },
    };

    [Fact]
    public void NameFromFullNameString_WhenStringIsEmpty_ShouldReturnExpectedError()
    {
        //act
        Result<Name> result = Name.FromFullNameString(string.Empty);

        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PersonErrors.EmptyName);
    }
}
