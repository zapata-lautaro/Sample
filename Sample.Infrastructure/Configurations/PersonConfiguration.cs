using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Domain.Persons;

namespace Sample.Infrastructure.Configurations;

internal sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("persons");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => new PersonId(value));

        builder.OwnsOne(p => p.Name);

        builder.Property(p => p.Language)
            .HasMaxLength(100)
            .HasConversion(language => language.Value, value => new Language(value));

        builder.Property(p => p.Bio)
            .HasMaxLength(500)
            .HasConversion(bio => bio.Value, value => new Bio(value));

        builder.Property(p => p.Version)
            .HasConversion(version => version.Value, value => new PersonVersion(value));
    }
}
